// ======================================================================
// 
//           Copyright (C) 2019-2020 ����������Ϣ�Ƽ����޹�˾
//           All rights reserved
// 
//           filename : MultiTenantMigrateExecuter.cs
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
using System.Collections.Generic;
using Abp.Data;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using Magicodes.Admin.Core.MultiTenancy;
using Magicodes.Admin.EntityFrameworkCore.EntityFramework;
using Magicodes.Admin.EntityFrameworkCore.Migrations.Seed;

namespace Magicodes.Admin.Migrator
{
    public class MultiTenantMigrateExecuter : ITransientDependency
    {
        private readonly IDbPerTenantConnectionStringResolver _connectionStringResolver;

        private readonly AbpZeroDbMigrator _migrator;
        private readonly IRepository<Tenant> _tenantRepository;

        public MultiTenantMigrateExecuter(
            AbpZeroDbMigrator migrator,
            IRepository<Tenant> tenantRepository,
            Log log,
            IDbPerTenantConnectionStringResolver connectionStringResolver)
        {
            Log = log;

            _migrator = migrator;
            _tenantRepository = tenantRepository;
            _connectionStringResolver = connectionStringResolver;
        }

        public Log Log { get; }

        public void Run(bool skipConnVerification)
        {
            var hostConnStr =
                _connectionStringResolver.GetNameOrConnectionString(
                    new ConnectionStringResolveArgs(MultiTenancySides.Host));
            if (hostConnStr.IsNullOrWhiteSpace())
            {
                Log.Write("���������ļ�����Ĭ�ϵ������ַ�����Default����");
                return;
            }

            Log.Write("�����ݿ⣺ " + ConnectionStringHelper.GetConnectionString(hostConnStr));
            if (!skipConnVerification)
            {
                Log.Write("����Ǩ���������ݿ⣬�Ƿ����..? (Y/N): ");
                var command = Console.ReadLine();
                if (!command.IsIn("Y", "y"))
                {
                    Log.Write("Ǩ����ȡ����");
                    return;
                }
            }

            Log.Write("��ʼִ�������ݿ�Ǩ��...");

            try
            {
                _migrator.CreateOrMigrateForHost(SeedHelper.SeedHostDb);
            }
            catch (Exception ex)
            {
                Log.Write("An error occured during migration of host database:");
                Log.Write(ex.ToString());
                Log.Write("Canceled migrations.");
                return;
            }

            Log.Write("�����ݿ�Ǩ����ɡ�");
            Log.Write("--------------------------------------------------------");

            var migratedDatabases = new HashSet<string>();
            var tenants = _tenantRepository.GetAllList(t => t.ConnectionString != null && t.ConnectionString != "");
            for (var i = 0; i < tenants.Count; i++)
            {
                var tenant = tenants[i];
                Log.Write(string.Format("��ʼǨ���⻧���ݿ�... ({0} / {1})", i + 1, tenants.Count));
                Log.Write("Name              : " + tenant.Name);
                Log.Write("TenancyName       : " + tenant.TenancyName);
                Log.Write("Tenant Id         : " + tenant.Id);
                Log.Write("Connection string : " + SimpleStringCipher.Instance.Decrypt(tenant.ConnectionString));

                if (!migratedDatabases.Contains(tenant.ConnectionString))
                {
                    try
                    {
                        _migrator.CreateOrMigrateForTenant(tenant);
                    }
                    catch (Exception ex)
                    {
                        Log.Write("An error occured during migration of tenant database:");
                        Log.Write(ex.ToString());
                        Log.Write("Skipped this tenant and will continue for others...");
                    }

                    migratedDatabases.Add(tenant.ConnectionString);
                }
                else
                {
                    Log.Write(
                        "This database has already migrated before (you have more than one tenant in same database). Skipping it....");
                }

                Log.Write(string.Format("�⻧���ݿ�Ǩ�����. ({0} / {1})", i + 1, tenants.Count));
                Log.Write("--------------------------------------------------------");
            }

            Log.Write("�������ݿ������Ǩ�ơ�");
        }
    }
}