using System;
using Microsoft.AspNetCore.Mvc;
using Xebia.Vouchers.API.Dto;
using Xebia.Vouchers.Domain;
using Xebia.Vouchers.UseCases;

namespace Xebia.Vouchers.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VouchersController : ControllerBase
    {
        private readonly CreateVoucherUseCase _createVoucherUseCase;

        public VouchersController(CreateVoucherUseCase createVoucherUseCase)
        {
            _createVoucherUseCase = createVoucherUseCase;
        }
        
        [HttpPost]
        public ActionResult<NewVoucher> Post([FromBody]VoucherTypeDto voucherTypeDto)
        {
            VoucherType voucherType = (VoucherType)((int)voucherTypeDto.VoucherType);

            var newVoucher = _createVoucherUseCase.Create(voucherType);
            return Ok(newVoucher);
        }
    }
}