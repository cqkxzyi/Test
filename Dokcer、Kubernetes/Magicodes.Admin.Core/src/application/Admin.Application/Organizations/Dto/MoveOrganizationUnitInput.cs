// ======================================================================
// 
//           Copyright (C) 2019-2020 ����������Ϣ�Ƽ����޹�˾
//           All rights reserved
// 
//           filename : MoveOrganizationUnitInput.cs
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

using System.ComponentModel.DataAnnotations;

namespace Magicodes.Admin.Application.Organizations.Dto
{
    public class MoveOrganizationUnitInput
    {
        [Range(1, long.MaxValue)] public long Id { get; set; }

        public long? NewParentId { get; set; }
    }
}