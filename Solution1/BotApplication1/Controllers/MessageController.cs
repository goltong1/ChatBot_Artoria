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
        private string directLineSecret = "skT3b21vMcI.cwA.GGQ.7R9m8JC7zR84mfwwQUVNma7WPkGPppSCeyMcPkxBc8Y";
        private string botId = "Artoria";
        private string fromUser = "IngyunAhn";

        private Conversation Conversation = null;
        DirectLineClient Client = null;
        // GET: Message
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
                From = new ChannelAccount(fromUser),
                Type=ActivityTypes.Message,
                Text = content

        };
            await Client.Conversations.PostActivityAsync(this.Conversation.ConversationId, userMessage);

            string watermark = null;
            while (true)
            {
                var activitySet = await Client.Conversations.GetActivitiesAsync(Conversation.ConversationId, watermark);
                watermark = activitySet?.Watermark;

                var activities = from x in activitySet.Activities
                                 where x.From.Id == botId
                                 select x;
                Message message = new Message();
                messageResponse messageResponse = new messageResponse();
                messageResponse.message = message;

                foreach (Activity activity in activities)
                {
                    message.text = activity.Text;
                }

                return Json(messageResponse, JsonRequestBehavior.AllowGet);

            }
                

        }
        }
    

    public class messageResponse
    {
        public Message message { get; set; }
    }
    public class Message
    {
        public string text { get; set; }

    }
}