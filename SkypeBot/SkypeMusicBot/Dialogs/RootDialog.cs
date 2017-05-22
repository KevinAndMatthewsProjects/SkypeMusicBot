using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.ConnectorEx;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Resource;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Http;
using System.Text;
using System.Net;

namespace SkypeMusicBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }


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


        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            // calculate something for us to return
            int length = (activity.Text ?? string.Empty).Length;

            string x = (activity.Text);
            // return our reply to the user
            if (x == "/help")
            {
                await context.PostAsync($"List bot help options here");
            }
            /*else if (x == "Hello")
            {
                await context.PostAsync($"Hello! What is your name?");
                
            }
 */
            // await context.PostAsync($"You sent {activity.Text} which was {length} characters");

            context.Wait(MessageReceivedAsync);
        }




    }







}





