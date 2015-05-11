using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TypeLite;

namespace MvcTypeScript.Models.Home.Container
{
    public class UserEntry
    {
        public string FullName { get; set; }
        public string BirthDate { get; set; }
        public DateTime Birth { get; set; }
    }
}