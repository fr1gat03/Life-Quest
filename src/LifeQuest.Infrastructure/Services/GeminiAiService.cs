using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.GenAI; 
using LifeQuest.Application.Interfaces; 

namespace LifeQuest.Infrastructure.Services;

public class GeminiAiService : IAiService
{
    // ТИМЧАСОВО встав свій ключ сюди
    private readonly string _apiKey = "AIza...";
    
    private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions 
    { 
        PropertyNameCaseInsensitive = true 
    };

    public async Task<AiQuestProposal> AnalyzeAndBalanceQuest(string userInput)
    {
        var client = new Client(apiKey: _apiKey); 
        var prompt = $@"
        Ти гейм-дизайнер. Користувач хоче виконати задачу: '{userInput}'.
        Оціни складність (Easy, Medium, Hard) і призначи XP (від 10 до 200) та Gold (від 5 до 100).
        Формат (ТІЛЬКИ JSON):
        {{
            ""Title"": ""Назва"",
            ""Difficulty"": ""Medium"",
            ""RewardXp"": 50,
            ""RewardGold"": 15
        }}";

        try
        {
            var response = await client.Models.GenerateContentAsync(
                "gemini-3.1-flash-lite-preview",
                prompt
            );

            var jsonText = CleanJson(response.Text);
            var questProposal = JsonSerializer.Deserialize<AiQuestProposal>(jsonText, _jsonOptions);
            return questProposal ?? GetFallbackQuest(userInput);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
            return GetFallbackQuest(userInput);
        }
    }

    public async Task<FairnessVerdict> ValidateQuestFairness(AiQuestProposal userEditedQuest)
    {
        var client = new Client(apiKey: _apiKey);
        var prompt = $@"
        Ти суворий Гейм-Майстер. Перевір квест: '{userEditedQuest.Title}'.
        Нагорода: {userEditedQuest.RewardXp} XP, {userEditedQuest.RewardGold} Gold.
        Якщо нагорода занадто велика для цієї дії - IsFair: false. Якщо адекватна - true.
        Формат (ТІЛЬКИ JSON):
        {{
            ""IsFair"": true,
            ""Feedback"": ""Твій коментар""
        }}";

        try
        {
            var response = await client.Models.GenerateContentAsync(
                "gemini-3.1-flash-lite-preview", 
                prompt
            );
            
            var jsonText = CleanJson(response.Text);
            return JsonSerializer.Deserialize<FairnessVerdict>(jsonText, _jsonOptions) 
                   ?? new FairnessVerdict { IsFair = false, Feedback = "Помилка парсингу" };
        }
        catch
        {
            return new FairnessVerdict { IsFair = false, Feedback = "Магічна аномалія з'єднання." };
        }
    }

    public async Task<string> GetNpcResponse(string userMessage, List<ChatMessage> history)
    {
        var client = new Client(apiKey: _apiKey);
        try 
        {
            var response = await client.Models.GenerateContentAsync(
                "gemini-3.1-flash-lite-preview", 
                userMessage
            );
            return response.Text ?? "Мої магічні канали забиті.";
        }
        catch
        {
            return "Ех... Темна магія перебиває мій зв'язок із сервером. Запитай трохи пізніше.";
        }
    }

    public async Task<string> GenerateMotivationMessage(string questTitle)
    {
        var client = new Client(apiKey: _apiKey);
        var prompt = $"Коротка мотивація для квесту: {questTitle}";
        try
        {
            var response = await client.Models.GenerateContentAsync("gemini-3.1-flash-lite-preview", prompt);
            return response.Text ?? "Ти молодець!";
        }
        catch { return "Чудова робота!"; }
    }

    private string CleanJson(string text)
    {
        return string.IsNullOrEmpty(text) ? "" : text.Replace("```json", "").Replace("```", "").Trim();
    }

    private AiQuestProposal GetFallbackQuest(string input)
    {
        return new AiQuestProposal { Title = input, Difficulty = "Easy", RewardXp = 10, RewardGold = 5 };
    }
}