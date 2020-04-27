// ======================================================================
// 
//           Copyright (C) 2019-2020 ����������Ϣ�Ƽ����޹�˾
//           All rights reserved
// 
//           filename : GetExpiringTenantsOutput.cs
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

namespace Magicodes.Admin.Application.MultiTenancy.HostDashboard.Dto
{
    public class GetExpiringTenantsOutput
    {
        public GetExpiringTenantsOutput(List<ExpiringTenant> expiringTenants)
        {
            ExpiringTenants = expiringTenants;
        }

        public List<ExpiringTenant> ExpiringTenants { get; set; }
    }
}