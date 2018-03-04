using System;

namespace NorthwindDAL
{
    public class OrderNotFound : Exception
    {
        public OrderNotFound() : base() { }
        public OrderNotFound(string message) : base(message) { }
    }

    public class OrderСantBeModified : Exception
    {
        public OrderСantBeModified() : base() { }
        public OrderСantBeModified(string message) : base(message) { }
    }

    public class OrderСantBeDeleted : Exception
    {
        public OrderСantBeDeleted() : base() { }
        public OrderСantBeDeleted(string message) : base(message) { }
    }
}
