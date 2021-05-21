using System;

namespace CoffeeTime.Logics.Infrastructure
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException()
        {
        }
    }
}
