// ======================================================================
// 
//           Copyright (C) 2019-2020 ����������Ϣ�Ƽ����޹�˾
//           All rights reserved
// 
//           filename : FlatFeatureDto.cs
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

namespace Magicodes.Admin.Application.Editions.Dto
{
    public class FlatFeatureDto
    {
        public string ParentName { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public string DefaultValue { get; set; }

        public FeatureInputTypeDto InputType { get; set; }
    }
}