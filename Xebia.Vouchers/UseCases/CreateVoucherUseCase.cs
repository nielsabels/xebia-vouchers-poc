using System;
using Xebia.Vouchers.Domain;
using Xebia.Vouchers.Exceptions;

namespace Xebia.Vouchers.UseCases
{
    public class CreateVoucherUseCase
    {
        private readonly ICreateVouchers _voucherCreator;

        public CreateVoucherUseCase(ICreateVouchers voucherCreator)
        {
            _voucherCreator = voucherCreator;
        }
        
        public NewVoucher Create(VoucherType voucherType)
        {
            try
            {
                var voucher = _voucherCreator.Create(voucherType);
                return voucher;
            }
            catch (CouldNotConstructDomainObject e)
            {
                throw new CouldNotCreateAVoucher(
                    "Exception occurred creating domain object for a new voucher", 
                    e);
            }
            catch (Exception e)
            {
                throw new CouldNotCreateAVoucher(
                    "Generic exception occurred while creating a new voucher", 
                    e);
            }            
        }
    }
}