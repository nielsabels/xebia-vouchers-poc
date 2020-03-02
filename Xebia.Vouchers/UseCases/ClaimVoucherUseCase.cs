using System;
using Xebia.Vouchers.Domain;
using Xebia.Vouchers.Exceptions;

namespace Xebia.Vouchers.UseCases
{
    public class ClaimVoucherUseCase
    {
        private readonly IClaimVouchers _voucherClaimer;

        public ClaimVoucherUseCase(IClaimVouchers voucherClaimer)
        {
            _voucherClaimer = voucherClaimer;
        }
        
        public ClaimedVoucher Claim(Guid voucherId)
        {
            try
            {
                var voucher = _voucherClaimer.Claim(voucherId);
                return voucher;
            }
            catch (VoucherAlreadyClaimed e)
            {
                throw;
            }
            catch (VoucherDoesNotExist e)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CouldNotClaimVoucher(
                    "Generic exception occurred while creating a new voucher", 
                    e);
            }            
        }
    }
}