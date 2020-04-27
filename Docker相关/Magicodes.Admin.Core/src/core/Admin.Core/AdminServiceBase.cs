// ======================================================================
// 
//           Copyright (C) 2019-2020 ����������Ϣ�Ƽ����޹�˾
//           All rights reserved
// 
//           filename : AdminServiceBase.cs
//           description :
// 
//           created by ѩ�� at  2019-06-14 11:22
//           �����ĵ�: docs.xin-lai.com
//           ���ںŽ̳̣�magiccodes
//           QQȺ��85318032����̽�����
//           Blog��http://www.cnblogs.com/codelove/
//           Home��http://xin-lai.com
// 
// ======================================================================

using Abp;
using Magicodes.Admin.Localization;

namespace Magicodes.Admin.Core
{
    /// <summary>
    ///     This class can be used as a base class for services in this application.
    ///     It has some useful objects property-injected and has some basic methods most of services may need to.
    ///     It's suitable for non domain nor application service classes.
    ///     For domain services inherit <see cref="AdminDomainServiceBase" />.
    ///     For application services inherit AdminAppServiceBase.
    /// </summary>
    public abstract class AdminServiceBase : AbpServiceBase
    {
        protected AdminServiceBase()
        {
            LocalizationSourceName = LocalizationConsts.LocalizationSourceName;
        }
    }
}