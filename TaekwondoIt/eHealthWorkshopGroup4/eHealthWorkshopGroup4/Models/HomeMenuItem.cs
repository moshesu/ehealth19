using System;
using System.Collections.Generic;
using System.Text;

namespace eHealthWorkshopGroup4.Models
{
    // these describe the down menu.
    public enum MenuItemType
    {
        Home,
        Profile,
        Info,
        Train,
        Messages
    }
    public class HomeMenuItem
    {
        //TODO deal with it
        public MenuItemType Id { get; set; }
        public String Title { get; set; }
    }
}
