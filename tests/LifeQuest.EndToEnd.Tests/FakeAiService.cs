using LifeQuest.Application.Interfaces;

namespace LifeQuest.E2E.Tests
{
    public class FakeAiService : IAiService
    {
        public Task<AiQuestProposal> AnalyzeAndBalanceQuest(string userInput)
        {
            var proposal = new AiQuestProposal();
            proposal.Title = "[AI] " + userInput;
            proposal.Difficulty = "Medium";
            proposal.RewardXp = 60;
            proposal.RewardGold = 30;
            return Task.FromResult(proposal);
        }

        public Task<FairnessVerdict> ValidateQuestFairness(AiQuestProposal quest)
        {
            var verdict = new FairnessVerdict();
            verdict.IsFair = true;
            verdict.Feedback = "OK";
            return Task.FromResult(verdict);
        }

        public Task<string> GetNpcResponse(string userMessage, List<ChatMessage> history)
            => Task.FromResult("Молодець!");

        public Task<string> GenerateMotivationMessage(string questTitle)
            => Task.FromResult($"Ти впорався із {questTitle}!");
    }
}