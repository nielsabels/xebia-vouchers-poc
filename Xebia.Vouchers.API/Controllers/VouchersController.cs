using System;
using System.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
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
        private readonly ILogger _logger;

        public VouchersController(
            CreateVoucherUseCase createVoucherUseCase, 
            ILogger logger)
        {
            _createVoucherUseCase = createVoucherUseCase;
            _logger = logger;
        }
        
        /// <summary>
        /// Create a new voucher
        /// </summary>
        /// <param name="voucherTypeDto">Specify which type of voucher to create</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(NewVoucherDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<NewVoucher> Post([FromBody]VoucherTypeDto voucherTypeDto)
        {
            VoucherType voucherType = (VoucherType)((int)voucherTypeDto.VoucherType);

            try
            {
                var newVoucherDto = NewVoucherDto.FromDomain(_createVoucherUseCase.Create(voucherType));
                return StatusCode((int) HttpStatusCode.Created, newVoucherDto);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Unable to create a voucher code.");
                
                return StatusCode(500, "Could not create a voucher code.");
            }
        }
    }
}