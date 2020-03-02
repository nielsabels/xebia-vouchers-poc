using System;

namespace Xebia.Vouchers.Exceptions
{
    public class VoucherAlreadyClaimed : Exception
    {
        public VoucherAlreadyClaimed(string message) : base(message) 
        {
        }
    }
}