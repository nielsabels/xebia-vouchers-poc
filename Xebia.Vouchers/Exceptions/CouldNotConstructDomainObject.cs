using System;

namespace Xebia.Vouchers.Exceptions
{
    public class CouldNotConstructDomainObject : Exception
    {
        public CouldNotConstructDomainObject(string message) : base(message) 
        {
        }
    }
}