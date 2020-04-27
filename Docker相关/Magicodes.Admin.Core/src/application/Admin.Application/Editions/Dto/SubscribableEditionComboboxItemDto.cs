// ======================================================================
// 
//           Copyright (C) 2019-2020 ����������Ϣ�Ƽ����޹�˾
//           All rights reserved
// 
//           filename : SubscribableEditionComboboxItemDto.cs
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

using Abp.Application.Services.Dto;

namespace Magicodes.Admin.Application.Editions.Dto
{
    public class SubscribableEditionComboboxItemDto : ComboboxItemDto
    {
        public SubscribableEditionComboboxItemDto(string value, string displayText, bool? isFree) : base(value,
            displayText)
        {
            IsFree = isFree;
        }

        public bool? IsFree { get; set; }
    }
}