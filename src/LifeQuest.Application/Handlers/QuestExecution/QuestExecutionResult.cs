namespace LifeQuest.Application.Handlers.QuestExecution;

public sealed class QuestExecutionResult
{
    public bool IsSuccess { get; }
    public string? ErrorMessage { get; }
    public string? MotivationMessage { get; }

    private QuestExecutionResult(bool isSuccess, string? error, string? motivation)
    {
        IsSuccess = isSuccess;
        ErrorMessage = error;
        MotivationMessage = motivation;
    }

    public static QuestExecutionResult Success(string motivation) => new(true, null, motivation);
    public static QuestExecutionResult Failure(string error) => new(false, error, null);
}