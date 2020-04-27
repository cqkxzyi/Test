// ======================================================================
// 
//           Copyright (C) 2019-2020 ����������Ϣ�Ƽ����޹�˾
//           All rights reserved
// 
//           filename : SmsVerificationCodeCacheItem.cs
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

using System;
using Abp.Timing;

namespace Magicodes.Admin.Core.Identity.Cache
{
    /// <summary>
    /// </summary>
    [Serializable]
    public class SmsVerificationCodeCacheItem
    {
        public const string CacheName = "AppSmsVerificationCodeCache";

        /// <summary>
        /// </summary>
        public SmsVerificationCodeCacheItem()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="code"></param>
        public SmsVerificationCodeCacheItem(string code)
        {
            Code = code;
        }

        /// <summary>
        ///     ��֤��
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///     ����ʱ��
        /// </summary>
        public DateTime CreationTime { get; set; } = Clock.Now;

        /// <summary>
        ///     ����ʱ��
        /// </summary>
        public DateTime? ExpiredTime { get; set; }
    }
}