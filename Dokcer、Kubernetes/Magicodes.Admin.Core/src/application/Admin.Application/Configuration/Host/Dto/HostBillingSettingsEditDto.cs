// ======================================================================
// 
//           Copyright (C) 2019-2020 ����������Ϣ�Ƽ����޹�˾
//           All rights reserved
// 
//           filename : HostBillingSettingsEditDto.cs
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

namespace Magicodes.Admin.Application.Configuration.Host.Dto
{
    public class HostBillingSettingsEditDto
    {
        /// <summary>
        ///     ̧ͷ����
        /// </summary>
        public string LegalName { get; set; }

        /// <summary>
        ///     ��ַ
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        ///     ˰��
        /// </summary>
        public string TaxNumber { get; set; }

        /// <summary>
        ///     ��ϵ��ʽ
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        ///     �����˻�
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        ///     ������
        /// </summary>
        public string Bank { get; set; }
    }
}