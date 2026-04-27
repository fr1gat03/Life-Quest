using System.Threading.Tasks;

namespace LifeQuest.Application.Interfaces;

// Для пропозиції квесту
public class AiQuestProposal
{
    public string Title { get; set; } = "";
    public string Difficulty { get; set; } = "Medium";
    public int RewardXp { get; set; }
    public int RewardGold { get; set; }
}

// Для анти-чіта
public class FairnessVerdict
{
    public bool IsFair { get; set; }
    public string Feedback { get; set; } = "";
    public AiQuestProposal? SuggestedCorrection { get; set; }
}

// Для чату з NPC
public class ChatMessage
{
    public string Role { get; set; } = "user";
    public string Text { get; set; } = "";
}

// Інтерфейс
public interface IAiService
{
    Task<AiQuestProposal> AnalyzeAndBalanceQuest(string userInput);
    Task<FairnessVerdict> ValidateQuestFairness(AiQuestProposal userEditedQuest);
    Task<string> GetNpcResponse(string userMessage, List<ChatMessage> history);
    Task<string> GenerateMotivationMessage(string questTitle);
}