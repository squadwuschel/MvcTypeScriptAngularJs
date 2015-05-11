using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcTypeScript.Models.Home.Container;
using TypeLite;

namespace MvcTypeScript.Models.Home
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            Name = string.Empty;
            Age = 21;
            UserEntries = new List<UserEntry>();
        }

        public string Name { get; set; }
        public int Age { get; set; }

        public List<UserEntry> UserEntries { get; set; }
    }
}