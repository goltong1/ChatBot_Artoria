using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BotApplication1.Dialogs
{
    [Serializable]
    public class MainDialogs : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as IMessageActivity;
            await context.PostAsync($"학생용 비서 챗봇 아르토리아 입니다. 무엇을 도와드릴까요?");
            context.Wait(MainHelpReceiveAsync);

        }
        private async Task MainHelpReceiveAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            int length = (activity.Text ?? string.Empty).Length;
            await context.PostAsync($"You sent {activity.Text} which was {length} characters");

        }
    }
}