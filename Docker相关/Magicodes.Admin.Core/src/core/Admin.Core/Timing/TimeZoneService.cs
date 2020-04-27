// ======================================================================
// 
//           Copyright (C) 2019-2020 ����������Ϣ�Ƽ����޹�˾
//           All rights reserved
// 
//           filename : TimeZoneService.cs
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
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Timing;
using TimeZoneConverter;

namespace Magicodes.Admin.Core.Timing
{
    public class TimeZoneService : ITimeZoneService, ITransientDependency
    {
        private readonly ISettingDefinitionManager _settingDefinitionManager;
        private readonly ISettingManager _settingManager;

        public TimeZoneService(
            ISettingManager settingManager,
            ISettingDefinitionManager settingDefinitionManager)
        {
            _settingManager = settingManager;
            _settingDefinitionManager = settingDefinitionManager;
        }

        public async Task<string> GetDefaultTimezoneAsync(SettingScopes scope, int? tenantId)
        {
            if (scope == SettingScopes.User)
            {
                if (tenantId.HasValue)
                    return await _settingManager.GetSettingValueForTenantAsync(TimingSettingNames.TimeZone,
                        tenantId.Value);

                return await _settingManager.GetSettingValueForApplicationAsync(TimingSettingNames.TimeZone);
            }

            if (scope == SettingScopes.Tenant)
                return await _settingManager.GetSettingValueForApplicationAsync(TimingSettingNames.TimeZone);

            if (scope == SettingScopes.Application)
            {
                var timezoneSettingDefinition =
                    _settingDefinitionManager.GetSettingDefinition(TimingSettingNames.TimeZone);
                return timezoneSettingDefinition.DefaultValue;
            }

            throw new Exception("Unknown scope for default timezone setting.");
        }

        public TimeZoneInfo FindTimeZoneById(string timezoneId)
        {
            return TZConvert.GetTimeZoneInfo(timezoneId);
        }

        public List<NameValueDto> GetWindowsTimezones()
        {
            return TZConvert.KnownWindowsTimeZoneIds.OrderBy(tz => tz)
                .Select(tz => new NameValueDto
                {
                    Value = tz,
                    Name = tz
                }).ToList();
        }
    }
}