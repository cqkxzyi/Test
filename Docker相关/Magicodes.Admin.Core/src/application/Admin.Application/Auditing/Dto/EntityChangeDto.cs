// ======================================================================
// 
//           Copyright (C) 2019-2020 ����������Ϣ�Ƽ����޹�˾
//           All rights reserved
// 
//           filename : EntityChangeDto.cs
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
using Abp.Application.Services.Dto;
using Abp.Events.Bus.Entities;

namespace Magicodes.Admin.Application.Auditing.Dto
{
    public class EntityChangeDto : EntityDto<long>
    {
        public DateTime ChangeTime { get; set; }

        public EntityChangeType ChangeType { get; set; }

        public long EntityChangeSetId { get; set; }

        public string EntityId { get; set; }

        public string EntityTypeFullName { get; set; }

        public int? TenantId { get; set; }

        public object EntityEntry { get; set; }
    }
}