using System;
using System.Collections.Generic;
using System.Text;
using eHealthWorkshopGroup4.Models;

namespace eHealthWorkshopGroup4
{
    public class UIMessage
    {

        public string Title { get; set; }
        public string Date { get; set; }
        public string Content { get; set; }
        public string groupName { get; set; }
        public bool IsVisible { get; set; }

        //public bool IsVisibleDelete { get; set; }
        //public bool IsAppMessage { get; set; }

        public UIMessage(Message msg, bool IsVisible)
        {
            this.Title = msg.Title;
            this.groupName = msg.GroupName;
            this.Content = msg.Content;
            this.Date = msg.Date.ToString("MM/dd/yyyy");
            this.IsVisible = IsVisible;

        }

    }
}
