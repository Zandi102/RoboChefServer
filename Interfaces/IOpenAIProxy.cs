using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;

namespace RoboChefServer
{
    public interface IOpenAIProxy
    {
        Task<ChatCompletionMessage[]> SendChatMessage(string message);
    }
}
