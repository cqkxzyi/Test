// ======================================================================
// 
//           Copyright (C) 2019-2020 ����������Ϣ�Ƽ����޹�˾
//           All rights reserved
// 
//           filename : PayPalPaymentGatewayPaymentStatusConverter.cs
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

namespace Magicodes.Admin.Core.MultiTenancy.Payments.Paypal
{
    public class PayPalPaymentGatewayPaymentStatusConverter : IPaymentGatewayPaymentStatusConverter
    {
        public SubscriptionPaymentStatus ConvertToSubscriptionPaymentStatus(string externalStatus)
        {
            if (externalStatus == "approved") return SubscriptionPaymentStatus.Completed;

            if (externalStatus == "created") return SubscriptionPaymentStatus.Processing;

            return SubscriptionPaymentStatus.Failed;
        }
    }
}