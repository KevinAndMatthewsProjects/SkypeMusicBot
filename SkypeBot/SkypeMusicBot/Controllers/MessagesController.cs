using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.ConnectorEx;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Resource;
using System;
using System.Text;
namespace SkypeMusicBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
  
        public static async Task<Microsoft.Bot.Connector.ConversationResourceResponse>
          CreateDirectConversation(Microsoft.Bot.Connector.IConversations
          operations, string botAddress, string userAccount,
          Microsoft.Bot.Connector.Activity activity = null)
        {

            var connector = new ConnectorClient(new Uri(activity.ServiceUrl));

            IMessageActivity message = Activity.CreateMessageActivity();

            message.Text = "Hello!";


            await connector.Conversations.SendToConversationAsync((Activity)message);
            var response = new Microsoft.Bot.Connector.ConversationResourceResponse();
            return response;

        }


        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
           
            bool boolAskedForUserName = false;
            string strUserName = "";

            string usrInput = activity.Text;
            if (activity.Type == ActivityTypes.Message)
            {
               
                StateClient sc = activity.GetStateClient();
                BotData userData = sc.BotState.GetPrivateConversationData(
                    activity.ChannelId, activity.Conversation.Id, activity.From.Id);
                boolAskedForUserName = userData.GetProperty<bool>("AskedForUserName");
                strUserName = userData.GetProperty<string>("UserName") ?? "";   
                StringBuilder strReplyMessage = new StringBuilder();
                if (boolAskedForUserName == false) 
                {
                    strReplyMessage.Append($"Hello, I am a Bot!");
                    strReplyMessage.Append($"\n\n\n\n");
                    strReplyMessage.Append($"\n\n\n");
                    strReplyMessage.Append($"\n\n\n");

                    strReplyMessage.Append($"What is your name?");

                    userData.SetProperty<bool>("AskedForUserName", true);

                }
                else 
                {
                    if (strUserName == "") 
                    {
                        
                        strReplyMessage.Append($"Hello {activity.Text}!");
                        strReplyMessage.Append($"\n\n\n");
                        strReplyMessage.Append($"Type /help for more options!");
                        strReplyMessage.Append($"\n\n\n");
                        userData.SetProperty<string>("UserName", activity.Text);
                    }

                    else
                    {
                        //strReplyMessage.Append($"{strUserName}, You said: {activity.Text}");
                    }
                }

                if (usrInput == "/help")
                {
                    strReplyMessage.Append($"Here are your help options:");
                    strReplyMessage.Append($"\n\n\n");
                    strReplyMessage.Append($"/play: Play a song");
                    strReplyMessage.Append($"\n\n\n");
                    strReplyMessage.Append($"Other Commands tbd");

                }

                sc.BotState.SetPrivateConversationData(
                    activity.ChannelId, activity.Conversation.Id, activity.From.Id, userData);

                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                Activity replyMessage = activity.CreateReply(strReplyMessage.ToString());
                await connector.Conversations.ReplyToActivityAsync(replyMessage);
            }
            else
            {
                Activity replyMessage = HandleSystemMessage(activity);
                if (replyMessage != null)
                {
                    ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                    await connector.Conversations.ReplyToActivityAsync(replyMessage);
                }
            }
            
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
               
            }
            else if (message.Type == ActivityTypes.Typing)
            {
               
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }




    [Serializable]
    public class EchoDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            await context.PostAsync("You said: " + message.Text);
            context.Wait(MessageReceivedAsync);
        }
    }


}