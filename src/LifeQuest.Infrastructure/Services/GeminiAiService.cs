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
    
    private const string BalanceTable = @"
    Таблиця балансу (СУВОРО дотримуйся):
    - Easy (легке, до 30 хв): XP 10-40, Gold 5-20
    - Medium (середнє, 30хв-2год): XP 41-80, Gold 21-40
    - Hard (важке, 2-8 год): XP 81-150, Gold 41-70
    - Epic (епічне, кілька днів): XP 151-200, Gold 71-100";

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
    - Не будь занадто суворим — гравець міг обрати вищу складність

    Відповідай ТІЛЬКИ JSON:
    {{
        ""IsFair"": true/false,
        ""Feedback"": ""Коротке пояснення якщо відхилено""
    }}";

        try
        {
            var response = await _client.Models.GenerateContentAsync(
                "gemini-3.1-flash-lite-preview",
                prompt
            );
            var jsonText = CleanJson(response.Text);
            return JsonSerializer.Deserialize<FairnessVerdict>(jsonText, _jsonOptions)
                   ?? new FairnessVerdict { IsFair = true };
        }
        catch
        {
            return new FairnessVerdict { IsFair = true };
        }
    }

    public async Task<string> GetNpcResponse(string userMessage, List<ChatMessage> history)
    {
        var prompt = $@"Ти мудрий NPC Елдор у таверні фентезійної RPG гри. 
    Відповідай ТІЛЬКИ простим текстом БЕЗ markdown розмітки (без **, ##, *, - та інших символів).
    Повідомлення гравця: {userMessage}";

        try
        {
            var response = await _client.Models.GenerateContentAsync(
                "gemini-3.1-flash-lite-preview", prompt);
            return response.Text ?? "Мої магічні канали забиті.";
        }
        catch
        {
            return "Ех... Темна магія перебиває мій зв'язок із сервером.";
        }
    }

    public async Task<string> GenerateMotivationMessage(string questTitle)
    {
        var prompt = $"Напиши коротку мотивацію (1 речення) для виконання квесту '{questTitle}'. Тільки простий текст без markdown.";
        try
        {
            var response = await _client.Models.GenerateContentAsync("using LifeQuest.Application.Interfaces;\nusing LifeQuest.Domain.Entities;\nusing LifeQuest.Infrastructure.Data;\nusing System.Collections.Generic;\nusing System.Linq;\n\nnamespace LifeQuest.Infrastructure.Repositories;\n\npublic class QuestRepository : IQuestRepository\n{\n    private readonly LifeQuestDbContext _context;\n\n    public QuestRepository(LifeQuestDbContext context)\n    {\n        _context = context;\n    }\n\n    public IEnumerable<Quest> GetActiveQuests(int userId)\n    {\n        return _context.Quests\n            .Where(q => !q.IsCompleted && q.UserId == userId)\n            .ToList();\n    }\n\n    public Quest? GetQuestById(string id)\n    {\n        return _context.Quests.Find(id);\n    }\n\n    public void UpdateQuest(Quest quest)\n    {\n        // Find перевіряє спочатку локальний кеш EF, потім БД\n        var existing = _context.Quests.Find(quest.Id);\n\n        if (existing == null)\n        {\n            // Новий квест — додаємо\n            _context.Quests.Add(quest);\n        }\n        else\n        {\n            // Існуючий — копіюємо нові значення в відстежуваний об'єкт\n            _context.Entry(existing).CurrentValues.SetValues(quest);\n        }\n\n        _context.SaveChanges();\n    }\n}", prompt);
            return response.Text ?? "Ти молодець!";
        }
        catch { return "Чудова робота!"; }
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