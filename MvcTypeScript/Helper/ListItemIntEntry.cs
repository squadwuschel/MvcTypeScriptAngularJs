using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcTypeScript.Helper
{
    public class ListItemIntEntry
    {
        public ListItemIntEntry()
        {
            Selected = false;
            Text = string.Empty;
            Value = 0;
        }

        public bool Selected { get; set; }
        public string Text { get; set; }
        public int Value { get; set; }
    }
}