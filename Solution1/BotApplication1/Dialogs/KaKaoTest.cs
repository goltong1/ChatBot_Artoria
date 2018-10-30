using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Bot.Builder.Luis.Models;
using System.Net;
using System.IO;

namespace BotApplication1.Dialogs
{
    [Serializable]
    public class KaKaoTest : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as IMessageActivity;
            await this.GetLUISIntentAsync(context, result);
        }
        private async Task GetLUISIntentAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as IMessageActivity;
            string intent = "테스트";
            var client = new HttpClient();
            var url = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/a0b1e3e1-4212-4251-8abc-249fa212f45c?subscription-key=efe57775900d488196a432a0e93304d5&timezoneOffset=-360&q="+ activity.Text;
            var response = await client.GetAsync(url);
            var strResponseContent = await response.Content.ReadAsStringAsync();
            LuisResult objResult = JsonConvert.DeserializeObject<LuisResult>(strResponseContent);
            intent = objResult.TopScoringIntent.Intent;
            await context.PostAsync($"사용자 최적화된 의도는 {objResult.TopScoringIntent.Intent}이고 적합도는 {objResult.TopScoringIntent.Score}입니다.");
            switch (intent)
            {
                case "급식":
                    string sURL;
                    sURL = "https://schoolmenukr.ml/api/high/E100001812?year=2018&month=10";
                    WebRequest wrGETURL;
                    wrGETURL = WebRequest.Create(sURL);
                    Stream objStream;
                    objStream = wrGETURL.GetResponse().GetResponseStream();
                    StreamReader objReader = new StreamReader(objStream);
                    await context.PostAsync(objReader.ReadLine());
                    break;
                case "이스터에그":
                    await context.PostAsync($"서번트 세이버, 마스터 지시를.");
                    break;
                case "인삿말":
                    await context.PostAsync($"안녕하십니까, 학생용비서 아르토리아입니다. 지시를 내려주십시오.");
                    break;
                case "개발자":
                    await context.PostAsync($"총괄은 공진우님, 기획은 김나현,박준영,이경민님, 프로그래밍은 안인균님이 하셨습니다.");
                    break;
                case "숙제":
                    await context.PostAsync($"숙제 기능은 준비중입니다.");
                    break;
                case "메뉴":
                    await context.PostAsync($"기능 메뉴는 준비중입니다.");
                    break;
                case "일정":
                    await context.PostAsync($"준비중인 기능입니다.");
                    break;
                case "None":
                    await context.PostAsync($"잘 모르겠습니다...다시 한번 말씀해 주십시오.");
                    break;
                default:
                    await context.PostAsync($"준비중인 기능입니다.");
                    break;
            }
        }
    }
    
}