// ======================================================================
// 
//           Copyright (C) 2019-2020 ����������Ϣ�Ƽ����޹�˾
//           All rights reserved
// 
//           filename : IncomeStastistic.cs
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

using System;

namespace Magicodes.Admin.Application.MultiTenancy.HostDashboard.Dto
{
    public class IncomeStastistic
    {
        public IncomeStastistic()
        {
        }

        public IncomeStastistic(DateTime date)
        {
            Date = date;
        }

        public IncomeStastistic(DateTime date, decimal amount)
        {
            Date = date;
            Amount = amount;
        }

        public string Label { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }
}