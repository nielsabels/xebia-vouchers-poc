using Newtonsoft.Json.Converters;

namespace Xebia.Vouchers.API.Dto
{
    public class VoucherTypeDto
    {
        [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
        public VoucherTypeEnum VoucherType { get; set; } = VoucherTypeEnum.FreeShipping;
    }
}