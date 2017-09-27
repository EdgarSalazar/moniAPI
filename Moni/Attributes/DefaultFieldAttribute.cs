using System;

namespace Moni.Attributes
{
    public class DefaultFieldAttribute : Attribute
    {
        public DefaultFieldAttribute(int order)
        {
            Order = order;
        }

        public int Order { get; set; }
    }
}
