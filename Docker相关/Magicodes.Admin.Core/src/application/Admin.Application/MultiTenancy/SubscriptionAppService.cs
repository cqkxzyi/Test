// ======================================================================
// 
//           Copyright (C) 2019-2020 ����������Ϣ�Ƽ����޹�˾
//           All rights reserved
// 
//           filename : SubscriptionAppService.cs
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
using System.Threading.Tasks;
using Abp.Runtime.Session;
using Magicodes.Admin.Application.Core;
using Magicodes.Admin.Core.Editions;
using Magicodes.Admin.Core.MultiTenancy;

namespace Magicodes.Admin.Application.MultiTenancy
{
    public class SubscriptionAppService : AdminAppServiceBase, ISubscriptionAppService
    {
        private readonly EditionManager _editionManager;
        private readonly TenantManager _tenantManager;

        public SubscriptionAppService(
            TenantManager tenantManager,
            EditionManager editionManager)
        {
            _tenantManager = tenantManager;
            _editionManager = editionManager;
        }

        public async Task UpgradeTenantToEquivalentEdition(int upgradeEditionId)
        {
            if (await UpgradeIsFree(upgradeEditionId))
                await _tenantManager.UpdateTenantAsync(
                    AbpSession.GetTenantId(), true, false, null,
                    upgradeEditionId,
                    EditionPaymentType.Upgrade
                );
        }

        private async Task<bool> UpgradeIsFree(int upgradeEditionId)
        {
            var tenant = await _tenantManager.GetByIdAsync(AbpSession.GetTenantId());

            if (!tenant.EditionId.HasValue)
                throw new Exception("Tenant must be assigned to an Edition in order to upgrade !");

            var currentEdition = (SubscribableEdition) await _editionManager.GetByIdAsync(tenant.EditionId.Value);
            var targetEdition = (SubscribableEdition) await _editionManager.GetByIdAsync(upgradeEditionId);
            var bothEditionsAreFree = targetEdition.IsFree && currentEdition.IsFree;
            var bothEditionsHasSamePrice = currentEdition.HasSamePrice(targetEdition);
            return bothEditionsAreFree || bothEditionsHasSamePrice;
        }
    }
}