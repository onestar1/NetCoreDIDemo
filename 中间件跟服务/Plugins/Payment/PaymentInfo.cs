using System;
using System.Collections.Generic;

namespace Super.Core.Plugins.Payment
{
    /// <summary>
    /// 支付事件
    /// </summary>
    public class PaymentInfo
    {

        /// <summary>
        /// 支付订单号
        /// </summary>
        public IEnumerable<long> OrderIds { get; set; }

        /// <summary>
        /// 支付流水号
        /// </summary>
        public string TradNo { get; set; }

        /// <summary>
        /// 交易时间
        /// </summary>
        public DateTime? TradeTime { get; set; }

        public  decimal TotalAmount { get; set; }

 

        public string Appid { get; set; }
       
        public string SellerId { get; set; }
    //     <charset>UTF-8</charset>
    //<out_trade_no>201908112253830</out_trade_no>
    //<method>alipay.trade.wap.pay.return</method>
    //<total_amount>0.01</total_amount>
    //<sign>H5lISGSiz9ggfOclCzCWdTrIWbgKjqHoYK/5JPSrRdp0jO+Sy2dg/gcbBAZUtGxDifS15fm7Eh+m6S8TngljSgnP8SwjuTa5Q7RES6Geupwrt6H7IbFgDoIZA3gEbA3SQ6rIw6NE4TA6MdBCGboKrpkmUvwmqz7HxQk0GWkYf5p6asuXQj8Q4Q3DPyedLbDdIn/dPrQOuAYVk2j+DHWwepuSKXmY3hUlZDilmkbLh735SCFfNfbnPhqb2xALYAhYehT3uczLy+6OIJUfqFvapfKF9pZNXOkBwOJzt2kI4B3PsTrF4bnmXTZRN/VjGeY+K6/U6m2VlR2MhiBIaoQ6og==</sign>
    //<trade_no>2019081122001412280504282266</trade_no>
    //<auth_app_id>2019081166214046</auth_app_id>
    //<version>1.0</version>
    //<app_id>2019081166214046</app_id>
    //<sign_type>RSA2</sign_type>
    //<seller_id>2088531788080172</seller_id>
        /// <summary>
        /// 主业务处理完成后响应内容
        /// 即当主程序相关订单状态完成后，需要响应请求的内容
        /// </summary>
        public string ResponseContentWhenFinished { get; set; }

    }
}
