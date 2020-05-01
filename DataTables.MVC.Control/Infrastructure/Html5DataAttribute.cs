using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication5.HtmlExtensions.Infrastructure
{
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public class Html5DataAttribute : Attribute
    {
        public string Html5DataAttributeName { get; private set; }

        public Html5DataAttribute(string name)
        {
            Html5DataAttributeName = name;
        }
    }
}