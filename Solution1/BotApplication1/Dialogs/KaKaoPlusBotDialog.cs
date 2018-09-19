using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BotApplication1.Dialogs
{
    [Serializable]
    public class KaKaoDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            // Return our reply to the user
            await context.PostAsync($"안녕하십니까, 학생용 비서 Artoria(ver.Kakao)입니다.");

            context.Wait(SecondMessageReceivedAsync);
        }
        private async Task SecondMessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            // Return our reply to the user
            await context.PostAsync($"입력하신 메세지는 {activity.Text}입니다.");

        }
    }
}
