// ======================================================================
// 
//           Copyright (C) 2019-2020 ����������Ϣ�Ƽ����޹�˾
//           All rights reserved
// 
//           filename : AdminDbContextConfigurer.cs
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

using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Magicodes.Admin.EntityFrameworkCore.EntityFramework
{
    public static class AdminDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<AdminDbContext> builder, string connectionString,
            bool isUseRowNumber = true)
        {
            if (isUseRowNumber)
                //��֧��SQL Server 2012�������ݿ�
                builder.UseSqlServer(connectionString, p => p.UseRowNumberForPaging());
            else
                builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<AdminDbContext> builder, DbConnection connection,
            bool isUseRowNumber = true)
        {
            if (isUseRowNumber)
                //��֧��SQL Server 2012�������ݿ�
                builder.UseSqlServer(connection, p => p.UseRowNumberForPaging());
            else
                builder.UseSqlServer(connection);
        }
    }
}