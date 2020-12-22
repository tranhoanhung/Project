using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookstoreMVC5.MomoAPI
{
    public class momoInfo
    {
        public static readonly string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
        public static readonly string partnerCode = "MOMOMVSF20201101";
        public static readonly string accessKey = "EkhGHeEcPIq0yOin";
        public static readonly string serectkey = "OZGMRaBlWly28f1LwD8EuuTXBjUdkPyq";
        public static readonly string orderInfo = "Bookstore test momo";
        public static readonly string returnUrl = "https://localhost:44307/confirm-orderPaymentOnline-momo";
        public static readonly string notifyurl = "https://localhost:44307/cancel-order-momo";
    }



}