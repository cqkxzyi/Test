// ======================================================================
// 
//           Copyright (C) 2019-2020 ����������Ϣ�Ƽ����޹�˾
//           All rights reserved
// 
//           filename : FeatureMetadata.cs
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
using Abp.Localization;

namespace Magicodes.Admin.Core.Features
{
    public class FeatureMetadata
    {
        public const string CustomFeatureKey = "FeatureMetadata";

        public FeatureMetadata()
        {
            TextHtmlColor = value => "inherit";
            IsVisibleOnPricingTable = false;
        }

        public Func<string, ILocalizableString> ValueTextNormalizer { get; set; }

        public bool IsVisibleOnPricingTable { get; set; }

        public Func<string, string> TextHtmlColor { get; set; }
    }
}