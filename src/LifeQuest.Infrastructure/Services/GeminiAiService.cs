using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.GenAI;
using LifeQuest.Application.Interfaces;

namespace LifeQuest.Infrastructure.Services;

public class GeminiAiService : IAiService
{
    private readonly string _apiKey;
    private readonly Client _client;

    private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public GeminiAiService(string apiKey)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new ArgumentException("Gemini API key is not configured.");
        _apiKey = apiKey;
        _client = new Client(apiKey: _apiKey);
    }

    public async Task<AiQuestProposal> AnalyzeAndBalanceQuest(string userInput)
    {
        var prompt = $@"
        Ти гейм-дизайнер. Користувач хоче виконати задачу: '{userInput}'.
        Оціни складність (Easy, Medium, Hard) і призначи XP (від 10 до 200) та Gold (від 5 до 100).
        Формат (ТІЛЬКИ JSON, без жодного тексту):
        {{
            ""Title"": ""Назва"",
            ""Difficulty"": ""Medium"",
            ""RewardXp"": 50,
            ""RewardGold"": 15
        }}";

        try
        {
            var response = await _client.Models.GenerateContentAsync(
                "gemini-3.1-flash-lite-preview",
                prompt
            );
            var jsonText = CleanJson(response.Text);
            return JsonSerializer.Deserialize<AiQuestProposal>(jsonText, _jsonOptions)
                   ?? GetFallbackQuest(userInput);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка AnalyzeAndBalanceQuest: {ex.Message}");
            return GetFallbackQuest(userInput);
        }
    }

    public async Task<FairnessVerdict> ValidateQuestFairness(AiQuestProposal userEditedQuest)
    {
        var prompt = $@"
        Ти суворий Гейм-Майстер. Перевір квест: '{userEditedQuest.Title}'.
        Нагорода: {userEditedQuest.RewardXp} XP, {userEditedQuest.RewardGold} Gold.
        Якщо нагорода занадто велика - IsFair: false. Якщо адекватна - true.
        Формат (ТІЛЬКИ JSON, без жодного тексту):
        {{
            ""IsFair"": true,
            ""Feedback"": ""Твій коментар""
        }}";

        try
        {
            var response = await _client.Models.GenerateContentAsync(
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
        try
        {
            var response = await _client.Models.GenerateContentAsync(
                "gemini-2.0-flash-lite",
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
        var prompt = $"Коротка мотивація (1 речення) для квесту: {questTitle}";
        try
        {
            var response = await _client.Models.GenerateContentAsync(
                "gemini-2.0-flash-lite",
                prompt
            );
            return response.Text ?? "Ти молодець!";
        }
        catch
        {
            return "Чудова робота!";
        }
    }

    private string CleanJson(string? text)
    {
        return string.IsNullOrEmpty(text)
            ? ""
            : text.Replace("```json", "").Replace("```", "").Trim();
    }

    private AiQuestProposal GetFallbackQuest(string input)
    {
        return new AiQuestProposal { Title = input, Difficulty = "Easy", RewardXp = 10, RewardGold = 5 };
    }
}