using System;
using System.Configuration;
using System.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite.Internal.UrlActions;
using Serilog;
using Xebia.Vouchers.API.Dto;
using Xebia.Vouchers.Domain;
using Xebia.Vouchers.Exceptions;
using Xebia.Vouchers.UseCases;

namespace Xebia.Vouchers.API.Controllers
{
    /// <summary>
    /// API Controller which manages access to vouchers (creating, claiming)
    /// </summary>
    [ApiController]
    public class VouchersController : ControllerBase
    {
        private readonly CreateVoucherUseCase _createVoucherUseCase;
        private readonly ClaimVoucherUseCase _claimVoucherUseCase;
        private readonly ILogger _logger;

        /// <summary>ctor</summary>
        public VouchersController(
            CreateVoucherUseCase createVoucherUseCase,
            ClaimVoucherUseCase claimVoucherUseCase,
            ILogger logger)
        {
            _createVoucherUseCase = createVoucherUseCase;
            _claimVoucherUseCase = claimVoucherUseCase;
            _logger = logger;
        }
        
        /// <summary>
        /// Create a new voucher
        /// </summary>
        /// <param name="voucherTypeDto">Specify which type of voucher to create</param>
        /// <returns></returns>
        [HttpPost("/api/vouchers")]
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
                
                return StatusCode((int)HttpStatusCode.InternalServerError, "Could not create a voucher code.");
            }
        }
        
        /// <summary>
        /// Claim a voucher
        /// </summary>
        /// <param name="id">The unique identifier of the voucher</param>
        /// <returns></returns>
        [HttpPost("/api/vouchers/{id}/claim")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<NewVoucher> Post(string id)
        {
            if (!Guid.TryParse(id, out var voucherId))
                return BadRequest("Invalid voucherId, please specify a valid Guid");

            try
            {
                var claimedVoucherDto = ClaimedVoucherDto.FromDomain(_claimVoucherUseCase.Claim(voucherId));
                return Ok(claimedVoucherDto);
            }
            catch (VoucherAlreadyClaimed e)
            {
                return StatusCode((int) HttpStatusCode.Forbidden, e.Message);
            }
            catch (VoucherDoesNotExist e)
            {
                return NotFound(e.Message);
            }
            catch (CouldNotClaimVoucher e)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, e.Message);
            }
            catch (Exception e)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, e.Message);
            }
        }        
    }
}