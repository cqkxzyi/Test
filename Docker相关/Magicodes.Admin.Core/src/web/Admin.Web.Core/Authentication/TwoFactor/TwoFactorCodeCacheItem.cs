﻿// ======================================================================
// 
//           Copyright (C) 2019-2020 湖南心莱信息科技有限公司
//           All rights reserved
// 
//           filename : TwoFactorCodeCacheItem.cs
//           description :
// 
//           created by 雪雁 at  2019-06-14 11:22
//           开发文档: docs.xin-lai.com
//           公众号教程：magiccodes
//           QQ群：85318032（编程交流）
//           Blog：http://www.cnblogs.com/codelove/
//           Home：http://xin-lai.com
// 
// ======================================================================

using System;

namespace Magicodes.Admin.Web.Core.Authentication.TwoFactor
{
    [Serializable]
    public class TwoFactorCodeCacheItem
    {
        public const string CacheName = "AppTwoFactorCodeCache";

        public TwoFactorCodeCacheItem()
        {
        }

        public TwoFactorCodeCacheItem(string code)
        {
            Code = code;
        }

        public string Code { get; set; }
    }
}