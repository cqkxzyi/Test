// ======================================================================
// 
//           Copyright (C) 2019-2020 ����������Ϣ�Ƽ����޹�˾
//           All rights reserved
// 
//           filename : EditionInfoDto.cs
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

using Abp.Application.Services.Dto;

namespace Magicodes.Admin.Application.Sessions.Dto
{
    public class EditionInfoDto : EntityDto
    {
        public string DisplayName { get; set; }

        public int? TrialDayCount { get; set; }

        public decimal? MonthlyPrice { get; set; }

        public decimal? AnnualPrice { get; set; }

        public bool IsHighestEdition { get; set; }

        public bool IsFree { get; set; }
    }
}