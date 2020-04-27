// ======================================================================
// 
//           Copyright (C) 2019-2020 ����������Ϣ�Ƽ����޹�˾
//           All rights reserved
// 
//           filename : PayPalCreatePaymentRequest.cs
//           description :
// 
//           created by ѩ�� at  2019-06-17 10:17
//           �����ĵ�: docs.xin-lai.com
//           ���ںŽ̳̣�magiccodes
//           QQȺ��85318032����̽�����
//           Blog��http://www.cnblogs.com/codelove/
//           Home��http://xin-lai.com
// 
// ======================================================================

using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;

namespace Magicodes.Admin.Core.MultiTenancy.Payments.Paypal
{
    public class PayPalCreatePaymentRequest
    {
        [JsonProperty("intent")] public string Intent { get; set; }

        [JsonProperty("transactions")] public List<PayPalTransaction> Transactions { get; set; }

        [JsonProperty("payer")] public Payer Payer { get; set; }

        [JsonProperty("redirect_urls")] public PayPalRedirectUrls PayPalRedirectUrls { get; set; }

        public static PayPalCreatePaymentRequest Create(string description, decimal amount)
        {
            return new PayPalCreatePaymentRequest
            {
                Intent = "sale",
                Payer = new Payer
                {
                    PaymentMethod = "paypal"
                },
                Transactions = new List<PayPalTransaction>
                {
                    new PayPalTransaction
                    {
                        Description = description,
                        Amount = new PayPalAmount
                        {
                            Total = amount.ToString("F", CultureInfo.InvariantCulture),
                            Currency = "USD"
                        }
                    }
                },
                PayPalRedirectUrls = new PayPalRedirectUrls
                {
                    //These are required by Paypal but actually haven't used by our application
                    ReturnUrl = "http://www.paypal.com/return",
                    CancelUrl = "http://www.paypal.com/cancel"
                }
            };
        }
    }
}