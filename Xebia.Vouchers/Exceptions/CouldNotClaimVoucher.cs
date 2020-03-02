using System;

namespace Xebia.Vouchers.Exceptions
{
    public class CouldNotClaimVoucher : Exception
    {
        public CouldNotClaimVoucher(string message, Exception innerException) : base(message, innerException) 
        {
        }
    }        
}