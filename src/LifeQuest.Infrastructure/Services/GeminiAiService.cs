using System.Text.Json;
using System.Threading.Tasks;
using Google.GenAI;
using LifeQuest.Application.Interfaces;

namespace LifeQuest.Infrastructure.Services;

public class GeminiAiService : IAiService
{
    // ТИМЧАСОВО встав свій ключ сюди
    private readonly string _apiKey = "...";

    public async Task<AiQuestProposal> AnalyzeAndBalanceQuest(string userInput)
    {
        var client = new Client(apiKey: _apiKey);

        var prompt = $@"
        Ти розумний гейм-дизайнер RPG гри 'Life Quest'.
        Користувач хоче виконати таку задачу в реальному житті: '{userInput}'.
        
        Твоя мета: придумати для цього епічну назву, оцінити складність (Easy, Medium, Hard), 
        і призначити нагороду в Досвіді (від 10 до 200 XP) та Золоті (від 5 до 100 Gold).
        
        Відповідай ВИКЛЮЧНО у форматі валідного JSON, без жодного іншого тексту чи форматування Markdown.
        Формат:
        {{
            ""Title"": ""Епічна назва задачі (до 5 слів)"",
            ""Difficulty"": ""Medium"",
            ""RewardXp"": 50,
            ""RewardGold"": 15
        }}";

        try
        {
            var response = await client.Models.GenerateContentAsync(
                model: "gemini-3.1-flash-lite",
                contents: prompt
            );

            var jsonText = CleanJson(response.Text);
            var questProposal = JsonSerializer.Deserialize<AiQuestProposal>(jsonText);

            return questProposal ?? GetFallbackQuest(userInput);
        }
        catch
        {
            return GetFallbackQuest(userInput);
        }
    }

    public async Task<FairnessVerdict> ValidateQuestFairness(AiQuestProposal userEditedQuest)
    {
        var client = new Client(apiKey: _apiKey);

        var prompt = $@"
    Ти - суворий і справедливий Гейм-Майстер RPG гри 'Life Quest'. 
    Твоє завдання: виявити, чи не намагається гравець 'накрутити' собі досвід за занадто легкі справи.
    
    Гравець пропонує такий квест:
    Назва: {userEditedQuest.Title}
    Складність: {userEditedQuest.Difficulty}
    Нагорода: {userEditedQuest.RewardXp} XP, {userEditedQuest.RewardGold} Gold
    
    Критерії оцінки:
    - Помити посуд/чашку: Easy, max 15 XP.
    - Тренування в залі (1 год): Hard, ~100-150 XP.
    - Прочитати 10 сторінок: Easy/Medium, ~20-30 XP.
    
    Якщо нагорода занадто велика для такої задачі, постав IsFair: false і поясни чому в Feedback.
    Відповідай ВИКЛЮЧНО JSON форматом:
    {{
        ""IsFair"": true/false,
        ""Feedback"": ""Твій коментар гравцю"",
        ""SuggestedCorrection"": {{
            ""Title"": ""Назва"",
            ""Difficulty"": ""CorrectedDifficulty"",
            ""RewardXp"": 0,
            ""RewardGold"": 0
        }}
    }}";

        try
        {
            var response = await client.Models.GenerateContentAsync(
                model: "gemini-3.1-flash-lite-preview",
                contents: prompt
            );
            var jsonText = CleanJson(response.Text);
            return JsonSerializer.Deserialize<FairnessVerdict>(jsonText) ?? new FairnessVerdict { IsFair = true };
        }
        catch
        {
            return new FairnessVerdict 
            { 
                IsFair = false, 
                Feedback = "Гейм-майстер підозрює магічну аномалію або чітерство. ШІ не зміг підтвердити цей запит." 
            };
        }
    }

    public async Task<string> GetNpcResponse(string userMessage, List<ChatMessage> history)
    {
        var client = new Client(apiKey: _apiKey);
        
        try 
        {
            var response = await client.Models.GenerateContentAsync(
                model: "gemini-3.1-flash-lite-preview", 
                contents: userMessage
            );
            return response.Text ?? "Мої магічні канали забиті.";
        }
        catch (Exception ex)
        {
            return "Ех... Темна магія перебиває мій зв'язок із сервером. Запитай трохи пізніше.";
        }
    }

    public async Task<string> GenerateMotivationMessage(string questTitle)
    {
        var client = new Client(apiKey: _apiKey);
        var prompt = $"Придумай коротку епічну мотиваційну фразу (1 речення) для гравця, який щойно виконав квест: {questTitle}";

        try
        {
            var response = await client.Models.GenerateContentAsync(
                model: "gemini-3.1-flash-lite-preview",
                contents: prompt
            );
            return response.Text ?? "Ти молодець!";
        }
        catch
        {
            return "Чудова робота, продовжуй в тому ж дусі!";
        }
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