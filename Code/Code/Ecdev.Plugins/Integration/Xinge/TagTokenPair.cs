using System;
using System.Collections.Generic;
using System.Text;

namespace Ecdev.Plugins.Integration.Xinge
{
    public class TagTokenPair
    {
        public TagTokenPair(string tag, string token)
        {
            this.Tag = tag;
            this.Token = token;
        }

        public string Tag { get; set; }
        public string Token { get; set; }
    }
}
