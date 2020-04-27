// ======================================================================
// 
//           Copyright (C) 2019-2020 ����������Ϣ�Ƽ����޹�˾
//           All rights reserved
// 
//           filename : PayPalConfiguration.cs
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

using Abp.Dependency;
using Abp.Extensions;
using Magicodes.Admin.Core.Configuration;
using Microsoft.Extensions.Configuration;

namespace Magicodes.Admin.Core.MultiTenancy.Payments.Paypal
{
    public class PayPalConfiguration : ITransientDependency
    {
        private readonly IConfigurationRoot _appConfiguration;

        public PayPalConfiguration(IAppConfigurationAccessor configurationAccessor)
        {
            _appConfiguration = configurationAccessor.Configuration;
        }

        public string Environment => _appConfiguration["Payment:PayPal:Environment"];

        public string BaseUrl => _appConfiguration["Payment:PayPal:BaseUrl"].EnsureEndsWith('/');

        public string ClientId => _appConfiguration["Payment:PayPal:ClientId"];

        public string ClientSecret => _appConfiguration["Payment:PayPal:ClientSecret"];

        public string DemoUsername => _appConfiguration["Payment:PayPal:DemoUsername"];

        public string DemoPassword => _appConfiguration["Payment:PayPal:DemoPassword"];
    }
}