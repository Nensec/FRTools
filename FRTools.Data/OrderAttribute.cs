using System;

namespace FRTools.Data
{
    public class OrderAttribute : Attribute
    {
        public int Order { get; }

        public OrderAttribute(int order)
        {
            Order = order;
        }
    }
}
