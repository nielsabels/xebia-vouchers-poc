using System;

namespace Xebia.Vouchers.Exceptions
{
    public class VoucherDoesNotExist : Exception
    {
        public VoucherDoesNotExist(string message) : base(message) 
        {
        }
    }
}