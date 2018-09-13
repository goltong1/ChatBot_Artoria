using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using BotApplication1.Extensions;
using Newtonsoft.Json;
using Microsoft.Bot.Builder.Luis.Models;
namespace BotApplication1.Dialogs
{
    
    [Serializable]
    public class Artoria_Talk : IDialog<object>
    {
        public int count = 1;
        Random r = new Random();
        private async Task GetLUISIntentAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            string intent = "테스트";

            var client = new HttpClient();
            var uri = "https://westus.api.cognitive.microsoft.com/luis/v2.0"
               + "/apps/a0b1e3e1-4212-4251-8abc-249fa212f45c?subscription-key=efe57775900d488196a432a0e93304d5&verbose=true&timezoneOffset=0"
               +"&q=" + activity.Text;
            var response = await client.GetAsync(uri);
            var strResponseContent = await response.Content.ReadAsStringAsync();

            LuisResult objResult = JsonConvert.DeserializeObject<LuisResult>(strResponseContent);
            intent = objResult.TopScoringIntent.Intent;
            await context.PostAsync($"사용자 최적화된 의도는 {objResult.TopScoringIntent.Intent}이고 적합도는{objResult.TopScoringIntent.Score}이다.");

            switch (intent)
            {
                case "숙제":
                    await context.PostAsync($"어떤 부분을 도와드릴까요?");
                    await this.HomeWork(context);
                    break;
                case "급식":
                    await context.PostAsync($"어떤 정보를 원하십니까?");
                    await this.Meal(context);
                    break;
                case "일정":
                    await context.PostAsync($"어떤 부분을 도와드릴까요?");
                    await this.schedule(context);
                    break;
                case "일정보기":
                    await context.PostAsync($"일정보기");
                    break;
                case "일정설정":
                    await context.PostAsync($"일정설정");
                    break;
                case "학교 소식":
                    await context.PostAsync($"학교소식");
                    break;
                case "개발자":
                    await context.PostAsync($"Leader:공진우,Programing:안인균, Producing:김나현,이경민,박준영");
                    break;
                case "인삿말":
                    int a=r.Next(1,3);
                    if (a == 1)
                    {
                        await context.PostAsync($"안녕하십니까. 챗봇 서비스 Artoria입니다. 무엇을 도와드릴까요?");
                    }
                    else
                    {
                        await context.PostAsync($"안녕하십니까. 마스터 무엇을 도와드릴까요?");
                    }
                        break;
                case "이스터에그":
                    await context.PostAsync($"세이버 아르토리아 펜드레건, 마스터 지시를.");
                    break;
                case "윈터솔저":
                    await context.PostAsync($"네?");
                    context.Wait(WReceiveAsync);
                    break;
                case "숙제설정":
                    await context.PostAsync($"숙제설정");
                    break;
                case "숙제보기":
                    await context.PostAsync($"숙제보기");
                    break;
                case "아침급식":
                    await context.PostAsync($"아침급식");
                    break;
                case "저녁급식":
                    await context.PostAsync($"저녁급식");
                    break;
                case "메뉴":
                    await this.ConfirmServiceTypeMessageAsync(context);
                    break;
                case "점심급식":
                    await context.PostAsync($"http://ijss.icehs.kr/foodlist.do?m=070106&s=ijss");
                    break;
                case "None":
                    await context.PostAsync($"글쎄요 잘 모르겠습니다, 다시 말씀해 주시겠습니까?");
                    break;
                default:
                    await context.PostAsync($"글쎄요 잘 모르겠습니다, 다시 말씀해 주시겠습니까?");
                    break;
            }
        }
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        /// <summary>
        /// 초기
        /// 
        /// </summary>
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            await context.PostAsync($"무엇을 도와드릴까요? 메뉴가 필요하시면 메뉴라고 말씀해주세요.");
            //수신 대기
            context.Wait(HelpReplyReceiveAsync);
        }

        /// <summary>
        /// 사용자 답변 분석
        /// 
        /// </summary>
        private async Task HelpReplyReceiveAsync(IDialogContext context, IAwaitable<object> result)
        {

            var activity = await result as Activity;
            //if (activity.Text.ToLower().Equals("예") == true || activity.Text.ToLower().Equals("yes") == true)
            //{
            //서비스 유형

            //}
            //else if (activity.Text.ToLower().Equals("아니오") == true)
            //{    //신규 가입
            //context.Call(new MembershipDialog(),ReturnRootDialogAsync);
            //}
            //else {
            //되묻기
            //await this.MessageReceivedAsync(context, null);
            if (count != 1)
            {

                context.Wait(WReceiveAsync);
            }
            else {
                await this.GetLUISIntentAsync(context, result);

            }
                //}
        }
        private async Task WReceiveAsync(IDialogContext context, IAwaitable<object> result)
        {

            
            var activity = await result as Activity;
            switch (count)
            {
                case 1 :
                    if (activity.Text.ToLower().Equals("갈망") == true)
                    {
                        count = count + 1;
                    }
                    else
                    {
                        count = 1;
                        await context.PostAsync($"명령이 취소되었습니다. 다른 주문을.");
                        context.Wait(HelpReplyReceiveAsync);
                        break;

                    }
                    break;
                case 2:
                    if (activity.Text.ToLower().Equals("부식") == true)
                    {
                        count = count + 1;
                    }
                    else
                    {
                        count = 1;
                        await context.PostAsync($"명령이 취소되었습니다. 다른 주문을.");
                        context.Wait(HelpReplyReceiveAsync);
                        break;

                    }
                    break;
                case 3:
                    if (activity.Text.ToLower().Equals("열일곱") == true)
                    {
                        count = count + 1;

                    }
                    else
                    {
                        count = 1;
                        await context.PostAsync($"명령이 취소되었습니다. 다른 주문을.");
                        context.Wait(HelpReplyReceiveAsync);
                        break;

                    }
                    break;
                case 4:
                    if (activity.Text.ToLower().Equals("새벽") == true)
                    {
                        count = count + 1;
                    }
                    else
                    {
                        count = 1;
                        await context.PostAsync($"명령이 취소되었습니다. 다른 주문을.");
                        context.Wait(HelpReplyReceiveAsync);
                        break;

                    }
                    break;
                case 5:
                    if (activity.Text.ToLower().Equals("용광로") == true)
                    {
                        count = count + 1;

                    }
                    else
                    {
                        count = 1;
                        await context.PostAsync($"명령이 취소되었습니다. 다른 주문을.");
                        context.Wait(HelpReplyReceiveAsync);
                        break;

                    }
                    break;
                case 6:
                    if (activity.Text.ToLower().Equals("아홉") == true)
                    {
                        count = count + 1;
                    }
                    else
                    {
                        count = 1;
                        await context.PostAsync($"명령이 취소되었습니다. 다른 주문을.");
                        context.Wait(HelpReplyReceiveAsync);
                        break;

                    }
                    break;
                case 7:
                    if (activity.Text.ToLower().Equals("상냥") == true)
                    {
                        count = count + 1;
                    }
                    else
                    {
                        count = 1;
                        await context.PostAsync($"명령이 취소되었습니다. 다른 주문을.");
                        context.Wait(HelpReplyReceiveAsync);
                        break;

                    }
                    break;
                case 8:
                    if (activity.Text.ToLower().Equals("귀향") == true)
                    {
                        count = count + 1;
                    }
                    else
                    {
                        count = 1;
                        await context.PostAsync($"명령이 취소되었습니다. 다른 주문을.");
                        context.Wait(HelpReplyReceiveAsync);
                        break;

                    }
                    break;
                case 9:
                    if (activity.Text.ToLower().Equals("하나") == true)
                    {
                        count = count + 1;
                    }
                    else
                    {
                        count = 1;
                        await context.PostAsync($"명령이 취소되었습니다. 다른 주문을.");
                        context.Wait(HelpReplyReceiveAsync);
                        break;

                    }
                    break;
                case 10:
                    if (activity.Text.ToLower().Equals("화물칸") == true)
                    {
                        count = count + 1;
                    }
                    else
                    {
                        count = 1;
                        await context.PostAsync($"명령이 취소되었습니다. 다른 주문을.");
                        context.Wait(HelpReplyReceiveAsync);
                        break;

                    }
                    break;
                case 11:
                    if (activity.Text.ToLower().Equals("솔저?") == true)
                    {
                        count = 1;
                        await context.PostAsync($"명령에 따릅니다.");
                        context.Wait(HelpReplyReceiveAsync);
                    }
                    else {
                        count = 1;
                        await context.PostAsync($"명령이 취소되었습니다. 다른 주문을.");
                        context.Wait(HelpReplyReceiveAsync);
                        break;

                    }
                    break;
            }

        }

        private async Task ConfirmServiceTypeMessageAsync(IDialogContext context)
        {
            var reply = context.MakeMessage();

            var options = new[]
            {
                "급식",
                "학교소식",
                "일정",
                "숙제"
            };

            reply.AddHeroCard("서비스선택", "아래 원하시는 서비스를 선택해주세요.", options, new[] { "file:///C:/Users/golto/Desktop/Artoria.png" });
            await context.PostAsync(reply);
        }
        private async Task HomeWork(IDialogContext context)
        {
            var reply = context.MakeMessage();

            var options = new[]
            {
                "숙제보기",
                "숙제설정"
            };

            reply.AddHeroCard("서비스선택", "아래 원하시는 서비스를 선택해주세요.", options, new[] { "file:///C:/Users/golto/Desktop/Artoria.png" });
            await context.PostAsync(reply);
        }
        private async Task Meal(IDialogContext context)
        {
            var reply = context.MakeMessage();

            var options = new[]
            {
                "아침급식",
                "점심급식",
                "저녁급식"
            };

            reply.AddHeroCard("서비스선택", "아래 원하시는 서비스를 선택해주세요.", options, new[] { "file:///C:/Users/golto/Desktop/Artoria.png" });
            await context.PostAsync(reply);
        }
        private async Task schedule(IDialogContext context)
        {
            var reply = context.MakeMessage();

            var options = new[]
            {
                "일정설정",
                "일정보기"
            };

            reply.AddHeroCard("서비스선택", "아래 원하시는 서비스를 선택해주세요.", options, new[] { "file:///C:/Users/golto/Desktop/Artoria.png" });
            await context.PostAsync(reply);
        }
    }
    }