namespace LifeQuest.Application.Interfaces;

public interface IAiService
{
    Task<string> GenerateMotivationMessage(string questTitle);
}