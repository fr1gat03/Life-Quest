using LifeQuest.Application.Interfaces;

namespace LifeQuest.Application.Tests.Fakes;

public sealed class FakeAiService : IAiService
{
    public const string FakeMessage = "Молодець! :)";

    public Task<string> GenerateMotivationMessage(string questTitle)
        => Task.FromResult(FakeMessage);

    public Task<AiQuestProposal> AnalyzeAndBalanceQuest(string userInput)
        => Task.FromResult(new AiQuestProposal());

    public Task<FairnessVerdict> ValidateQuestFairness(AiQuestProposal quest)
        => Task.FromResult(new FairnessVerdict { IsFair = true });

    public Task<string> GetNpcResponse(string userMessage, List<ChatMessage> history)
        => Task.FromResult(string.Empty);
}