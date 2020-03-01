using System;

namespace Xebia.Vouchers.Exceptions
{
    public class CouldNotCreateAVoucher : Exception
    {
        public CouldNotCreateAVoucher(string message, Exception innerException) : base(message, innerException) 
        {
        }
    }    
}