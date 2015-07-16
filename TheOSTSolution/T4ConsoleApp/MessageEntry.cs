using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T4ConsoleApp
{
    public class MessageEntry
    {
        public string Id { get; private set; }
        public string Value { get; private set; }
        public string Category { get; private set; }

        public MessageEntry(string id, string value, string category)
        {
            this.Id = id;
            this.Value = value;
            this.Category = category;
        }
        public string Format(params object[] args)
        {
            return string.Format(this.Value, args);
        }
    }
}
