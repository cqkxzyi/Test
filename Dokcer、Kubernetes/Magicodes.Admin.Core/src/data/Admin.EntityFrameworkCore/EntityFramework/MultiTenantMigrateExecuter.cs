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
using Castle.Core.Logging;
using Magicodes.Admin.Core.MultiTenancy;
using Magicodes.Admin.EntityFrameworkCore.Migrations.Seed;

namespace Magicodes.Admin.EntityFrameworkCore.EntityFramework
{
    /// <summary>
    ///     ���ݿ�Ǩ��
    /// </summary>
    public class MultiTenantMigrateExecuter : ITransientDependency
    {
        private readonly IDbPerTenantConnectionStringResolver _connectionStringResolver;
        private readonly AbpZeroDbMigrator _migrator;
        private readonly IRepository<Tenant> _tenantRepository;

        public MultiTenantMigrateExecuter(
            AbpZeroDbMigrator migrator,
            IRepository<Tenant> tenantRepository,
            ILogger logger,
            IDbPerTenantConnectionStringResolver connectionStringResolver)
        {
            Logger = logger;
            _migrator = migrator;
            _tenantRepository = tenantRepository;
            _connectionStringResolver = connectionStringResolver;
        }

        public ILogger Logger { get; }

        /// <summary>
        ///     ִ��Ǩ��
        /// </summary>
        public void Run()
        {
            var hostConnStr =
                _connectionStringResolver.GetNameOrConnectionString(
                    new ConnectionStringResolveArgs(MultiTenancySides.Host));
            if (hostConnStr.IsNullOrWhiteSpace())
            {
                Logger.Error("û���ҵ�����Ϊ'Default'�������ַ���!");
                return;
            }

            Logger.Info("--------------------------------------------------------");
            Logger.Info("�����ݿ�: " + ConnectionStringHelper.GetConnectionString(hostConnStr));
            Logger.Info("�����ݿ��Զ�Ǩ��������...");

            try
            {
                _migrator.CreateOrMigrateForHost(SeedHelper.SeedHostDb);
            }
            catch (Exception ex)
            {
                Logger.Info("Ǩ�������ݿ�ʱ��������:");
                Logger.Error(ex.Message, ex);
                Logger.Info("Ǩ����ȡ��!");
                return;
            }

            Logger.Info("�����ݿ�Ǩ�����.");
            Logger.Info("--------------------------------------------------------");

            var migratedDatabases = new HashSet<string>();
            var tenants = _tenantRepository.GetAllList(t => t.ConnectionString != null && t.ConnectionString != "");
            for (var i = 0; i < tenants.Count; i++)
            {
                var tenant = tenants[i];
                Logger.Info(string.Format("�⻧���ݿ�Ǩ��������... ({0} / {1})", i + 1, tenants.Count));
                Logger.Info("���� �� " + tenant.Name);
                Logger.Info("�⻧���� �� " + tenant.TenancyName);
                Logger.Info("�⻧Id �� " + tenant.Id);
                Logger.Info("�����ַ��� �� " + SimpleStringCipher.Instance.Decrypt(tenant.ConnectionString));

                if (!migratedDatabases.Contains(tenant.ConnectionString))
                {
                    try
                    {
                        _migrator.CreateOrMigrateForTenant(tenant);
                    }
                    catch (Exception ex)
                    {
                        Logger.Info("Ǩ���⻧���ݿ�ʱ��������:");
                        Logger.Info(ex.Message, ex);
                        Logger.Info("��������ǰǨ�Ƽ�����һ���⻧���ݿ�Ǩ��...");
                    }

                    migratedDatabases.Add(tenant.ConnectionString);
                }
                else
                {
                    Logger.Info("��ǰ���ݿ���Ǩ��. ������....");
                }

                Logger.Info(string.Format("�⻧���ݿ�Ǩ�����. ({0} / {1})", i + 1, tenants.Count));
                Logger.Info("--------------------------------------------------------");
            }

            Logger.Info("�������ݿ�������Ǩ��.");
        }
    }
}