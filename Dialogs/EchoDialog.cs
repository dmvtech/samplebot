using System;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;


namespace Microsoft.Bot.Sample.SimpleEchoBot
{
    [Serializable]
    public class EchoDialog : IDialog<object>
    {
        protected int count = 1;

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;

            if (message.Text == "reset")
            {
                PromptDialog.Confirm(
                    context,
                    AfterResetAsync,
                    "I'm going to reset the count, are you sure you're okay with that!?",
                    "Didn't get that!",
                    promptStyle: PromptStyle.Auto);
            }
            else
            {
                int myCountInt = this.count;
                string myNumSuffix = "";
    
                switch (myCountInt)
                {
                    case 1: myNumSuffix = "st";
                        break;
                    case 2:  myNumSuffix = "nd";
                        break;
                    case 3: myNumSuffix = "rd";
                        break;
                    default:
                        myNumSuffix = "th";
                        break;
                }
                
                await context.PostAsync($"This is the {this.count++}{myNumSuffix} question you've asked. I'm still a learning bot and not sure on your question, but perhaps you can try bing:  https://www.bing.com/search?q={message.Text}");
                context.Wait(MessageReceivedAsync);
            }
        }

        public async Task AfterResetAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                this.count = 1;
                await context.PostAsync("Reset count.");
            }
            else
            {
                await context.PostAsync("Did not reset count.");
            }
            context.Wait(MessageReceivedAsync);
        }

    }
}