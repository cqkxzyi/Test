// ======================================================================
// 
//           Copyright (C) 2019-2020 ����������Ϣ�Ƽ����޹�˾
//           All rights reserved
// 
//           filename : LocalizableComboboxItemSourceDto.cs
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

using System.Collections.ObjectModel;

namespace Magicodes.Admin.Application.Editions.Dto
{
    //Mapped in CustomDtoMapper
    public class LocalizableComboboxItemSourceDto
    {
        public Collection<LocalizableComboboxItemDto> Items { get; set; }
    }
}