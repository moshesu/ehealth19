using CloudStorage;
using eHealthWorkshopGroup4.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace eHealthWorkshopGroup4.ViewModels
{
    public class MessagesViewModel
    {
        public CloudStorageHandler storage = null;
        public ObservableCollection<UIMessage> Messages { get; set; }
        public UIMessage _prevMessage;
        string uname;
        public MessagesViewModel()
        {
            uname = App.MyUserName;
            storage = new CloudStorageHandler();
            Messages = new ObservableCollection<UIMessage>();
            initialize_Messages();
            

        }

        public async void initialize_Messages()
        {
            List<Message> messages_list = await storage.GetUserMessages(App.MyUserName);
            foreach (Message msg in messages_list)
            {
                Messages.Add(new UIMessage(msg, false));
            }

        }


        public void HideOrShowMessage(UIMessage message)
        {
            if(_prevMessage == message)
            {
                // click twice on the same item will hide it
                message.IsVisible = !message.IsVisible;
                UpdateMessages(message);
            }
            else
            {
                if(_prevMessage != null)
                {
                    // hide previous selected item
                    _prevMessage.IsVisible = false;
                    UpdateMessages(_prevMessage);
                }
                // show selected item
                message.IsVisible = true;
                UpdateMessages(message);
            }
            _prevMessage = message;
        }

        private void UpdateMessages(UIMessage message)
        {
            int index = Messages.IndexOf(message);
            Messages.Remove(message);
            Messages.Insert(index, message);
        }

        /*
        public void AddAppMessages(string title, string date, string content)
        {
            Messages.Insert(0, new UIMessage(title, date, content, true));
        }
        

        public void AddCoachMessages(string title, string date, string content)
        {
            Messages.Insert(0, new UIMessage(title, date, content, false));
        }
        */

        public void AddCoachMessages(Message mesg)
        {
            Messages.Insert(0, new UIMessage(mesg, false));
        }

        public Command<UIMessage> DeleteCommand
        {
            get
            {
                return new Command<UIMessage>((message) =>
                {
                    Messages.Remove(message);
                });
            }
            
        }
    }
}
