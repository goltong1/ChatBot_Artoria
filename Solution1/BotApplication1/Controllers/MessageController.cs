using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Bot.Connector.DirectLine;
using System.Threading.Tasks;
using System.Configuration;

namespace BotApplication1.Controllers
{
    public class MessageController : Controller
    {
        private string directLineSecret = "n4MJoitjCR8.cwA.v5U.1doAphdsoMWx1yH-UFxoHciwa-PTVTsSF8pd0aU6MEI";
        private string botld = "Artoria";
        private string fromUser = "Megumin";

        private Conversation Conversation = null;
        DirectLineClient Client = null;
        public async Task<ActionResult> Index(string user_key, string type, string content)
        {
            Client = new DirectLineClient(directLineSecret);
            if (Session["cid"] as string != null)
            { 
                this.Conversation = Client.Conversations.ReconnectToConversation((string)Session["CONVERSATION_ID"]);
            }
            else
            {
                this.Conversation = Client.Conversations.StartConversation();

                Session["cid"] = Conversation.ConversationId;
            }

            Activity userMessage = new Activity
            {
                From=new ChannelAccount(fromUser),
                Type=ActivityTypes.Message,
                Text=content
            };

            await Client.Conversations.PostActivityAsync(this.Conversation.ConversationId, userMessage);

            //메세지 받는 파트
            string watermark = null;

            while (true)
            {
                var activitySet = await Client.Conversations.GetActivitiesAsync(Conversation.ConversationId, watermark);
                watermark = activitySet?.Watermark;
                var activities = from x in activitySet.Activities
                                 where x.From.Id == botld
                                 select x;

                Message message = new Message();
                MessageResponse messageResponse = new MessageResponse();
                messageResponse.message = message;

                foreach (Activity activity in activities)
                {
                    message.text = activity.Text;
                }

                return Json(messageResponse,JsonRequestBehavior.AllowGet);
            }
        }


    }
    public class MessageResponse
    {
        public Message message { get; set; }

    }
    public class Message
    {
        public string text { get; set; }
    }
}
