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
    
    private const string ModelName = "gemini-3.1-flash-lite-preview";

    private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    private const string BalanceTable = @"
    Таблиця балансу (СУВОРО дотримуйся):
    - Easy (легке, до 30 хв): XP 10-40, Gold 5-20
    - Medium (середнє, 30хв-2год): XP 41-80, Gold 21-40
    - Hard (важке, 2-8 год): XP 81-150, Gold 41-70
    - Epic (епічне, кілька днів): XP 151-200, Gold 71-100";

    public GeminiAiService(string apiKey)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new ArgumentException("Gemini API key is not configured.");
        _apiKey = apiKey;
        _client = new Client(apiKey: _apiKey);
    }

    public async Task<AiQuestProposal> AnalyzeAndBalanceQuest(string userInput)
    {
        var prompt = $@"Ти гейм-дизайнер RPG гри Life Quest.
    Визнач параметри квесту для реальної задачі: '{userInput}'. 
    {BalanceTable}

    Відповідай ТІЛЬКИ JSON, без жодного тексту навколо:
    {{
        ""Title"": ""Коротка назва квесту"",
        ""Difficulty"": ""Easy/Medium/Hard/Epic"",
        ""RewardXp"": число,
        ""RewardGold"": число
    }}";

        try
        {
            var response = await _client.Models.GenerateContentAsync(ModelName, prompt);
            var jsonText = CleanJson(response.Text);
            return JsonSerializer.Deserialize<AiQuestProposal>(jsonText, _jsonOptions)
                   ?? GetFallbackQuest(userInput);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AI] AnalyzeAndBalanceQuest помилка: {ex.Message}");
            return GetFallbackQuest(userInput);
        }
    }

    public async Task<FairnessVerdict> ValidateQuestFairness(AiQuestProposal quest)
    {
        var prompt = $@"Ти Гейм-Майстер RPG гри Life Quest.
    Перевір чи не читерить гравець зі своїм квестом.
    {BalanceTable}

    Квест: '{quest.Title}'
    Складність: {quest.Difficulty}
    Нагорода: {quest.RewardXp} XP, {quest.RewardGold} Gold

    Правила перевірки:
    - Якщо значення в межах або близько до таблиці (±20%) — IsFair: true
    - Відхиляй тільки якщо СУТТЄВО перевищує межі (більш ніж на 50%)

    Відповідай ТІЛЬКИ JSON:
    {{
        ""IsFair"": true/false,
        ""Feedback"": ""Коротке пояснення якщо відхилено""
    }}";

        try
        {
            var response = await _client.Models.GenerateContentAsync(ModelName, prompt);
            var jsonText = CleanJson(response.Text);
            return JsonSerializer.Deserialize<FairnessVerdict>(jsonText, _jsonOptions)
                   ?? new FairnessVerdict { IsFair = true };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AI] ValidateQuestFairness помилка: {ex.Message}");
            return new FairnessVerdict { IsFair = true };
        }
    }

    public async Task<string> GetNpcResponse(string userMessage, List<ChatMessage> history)
    {
        var prompt = $@"Ти мудрий NPC Елдор у таверні фентезійної RPG гри Life Quest.
    Говори як середньовічний мудрець — коротко, по справі, з легким гумором.
    Відповідай ТІЛЬКИ простим текстом БЕЗ markdown (без **, ##, *, - та інших символів).
    Повідомлення гравця: {userMessage}";

        try
        {
            var response = await _client.Models.GenerateContentAsync(ModelName, prompt);
            var text = response.Text;
            Console.WriteLine($"[AI] NPC відповідь: {text}");
            return text ?? "Мої магічні канали забиті.";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AI] GetNpcResponse помилка: {ex.Message}");
            return "Ех... Темна магія перебиває мій зв'язок. Запитай пізніше.";
        }
    }

    public async Task<string> GenerateMotivationMessage(string questTitle)
    {
        var prompt = $@"Ти мотиваційний тренер у RPG грі Life Quest.
    Напиши ОДНЕ коротке речення мотивації для гравця який щойно виконав квест: '{questTitle}'.
    Звертайся до гравця, будь натхненним і позитивним.
    Тільки простий текст, без markdown, без зайвих слів.";

        try
        {
            var response = await _client.Models.GenerateContentAsync(ModelName, prompt);
            var text = response.Text;
            Console.WriteLine($"[AI] Мотивація: {text}");

            if (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine("[AI] Мотивація: порожня відповідь!");
                return "Чудова робота, герою!";
            }

            return text.Trim();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AI] GenerateMotivationMessage помилка: {ex.Message}");
            return "Чудова робота, герою!";
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
        return new AiQuestProposal
        {
            Title = input,
            Difficulty = "Easy",
            RewardXp = 10,
            RewardGold = 5
        };
    }
}