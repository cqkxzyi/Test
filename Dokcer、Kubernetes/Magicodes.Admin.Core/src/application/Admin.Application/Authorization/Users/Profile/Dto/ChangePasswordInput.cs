// ======================================================================
// 
//           Copyright (C) 2019-2020 ����������Ϣ�Ƽ����޹�˾
//           All rights reserved
// 
//           filename : ChangePasswordInput.cs
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
using Abp.Auditing;

namespace Magicodes.Admin.Application.Authorization.Users.Profile.Dto
{
    public class ChangePasswordInput
    {
        [Required] [DisableAuditing] public string CurrentPassword { get; set; }

        [Required] [DisableAuditing] public string NewPassword { get; set; }
    }
}