using System.ComponentModel;
using Newtonsoft.Json.Converters;
using Xebia.Vouchers.Domain;

namespace Xebia.Vouchers.API.Dto
{
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public enum VoucherTypeEnum
    {
        FreeShipping = 0,
    }    
}