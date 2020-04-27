// ======================================================================
// 
//           Copyright (C) 2019-2020 ����������Ϣ�Ƽ����޹�˾
//           All rights reserved
// 
//           filename : PayPalRedirectUrls.cs
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

using Newtonsoft.Json;

namespace Magicodes.Admin.Core.MultiTenancy.Payments.Paypal
{
    public class PayPalRedirectUrls
    {
        [JsonProperty("return_url")] public string ReturnUrl { get; set; }

        [JsonProperty("cancel_url")] public string CancelUrl { get; set; }
    }
}