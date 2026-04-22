namespace LifeQuest.Domain.Entities;

public class Quest
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }

    public void Complete() => IsCompleted = true;
}