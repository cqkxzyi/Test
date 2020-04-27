// ======================================================================
// 
//           Copyright (C) 2019-2020 ����������Ϣ�Ƽ����޹�˾
//           All rights reserved
// 
//           filename : UserLinkAppService.cs
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
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using Magicodes.Admin.Application.Authorization.Users.Dto;
using Magicodes.Admin.Application.Core;
using Magicodes.Admin.Core.Authorization.Users;
using Magicodes.Admin.Core.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Magicodes.Admin.Application.Authorization.Users
{
    [AbpAuthorize]
    public class UserLinkAppService : AdminAppServiceBase, IUserLinkAppService
    {
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;
        private readonly LogInManager _logInManager;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IRepository<UserAccount, long> _userAccountRepository;
        private readonly IUserLinkManager _userLinkManager;

        public UserLinkAppService(
            AbpLoginResultTypeHelper abpLoginResultTypeHelper,
            IUserLinkManager userLinkManager,
            IRepository<Tenant> tenantRepository,
            IRepository<UserAccount, long> userAccountRepository,
            LogInManager logInManager)
        {
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            _userLinkManager = userLinkManager;
            _tenantRepository = tenantRepository;
            _userAccountRepository = userAccountRepository;
            _logInManager = logInManager;
        }

        public async Task LinkToUser(LinkToUserInput input)
        {
            var loginResult =
                await _logInManager.LoginAsync(input.UsernameOrEmailAddress, input.Password, input.TenancyName);

            if (loginResult.Result != AbpLoginResultType.Success)
                throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result,
                    input.UsernameOrEmailAddress, input.TenancyName);

            if (AbpSession.IsUser(loginResult.User)) throw new UserFriendlyException(L("YouCannotLinkToSameAccount"));

            if (loginResult.User.ShouldChangePasswordOnNextLogin)
                throw new UserFriendlyException(L("ChangePasswordBeforeLinkToAnAccount"));

            await _userLinkManager.Link(GetCurrentUser(), loginResult.User);
        }

        public async Task<PagedResultDto<LinkedUserDto>> GetLinkedUsers(GetLinkedUsersInput input)
        {
            var currentUserAccount = await _userLinkManager.GetUserAccountAsync(AbpSession.ToUserIdentifier());
            if (currentUserAccount == null) return new PagedResultDto<LinkedUserDto>(0, new List<LinkedUserDto>());

            var query = CreateLinkedUsersQuery(currentUserAccount, input.Sorting);

            var totalCount = await query.CountAsync();

            var linkedUsers = await query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToListAsync();

            return new PagedResultDto<LinkedUserDto>(
                totalCount,
                linkedUsers
            );
        }

        [DisableAuditing]
        public async Task<ListResultDto<LinkedUserDto>> GetRecentlyUsedLinkedUsers()
        {
            var currentUserAccount = await _userLinkManager.GetUserAccountAsync(AbpSession.ToUserIdentifier());
            if (currentUserAccount == null) return new ListResultDto<LinkedUserDto>();

            var query = CreateLinkedUsersQuery(currentUserAccount, "TenancyName, Username");
            var recentlyUsedlinkedUsers = await query.Take(3).ToListAsync();

            return new ListResultDto<LinkedUserDto>(recentlyUsedlinkedUsers);
        }

        public async Task UnlinkUser(UnlinkUserInput input)
        {
            var currentUserAccount = await _userLinkManager.GetUserAccountAsync(AbpSession.ToUserIdentifier());

            if (!currentUserAccount.UserLinkId.HasValue) throw new Exception(L("YouAreNotLinkedToAnyAccount"));

            if (!await _userLinkManager.AreUsersLinked(AbpSession.ToUserIdentifier(), input.ToUserIdentifier())) return;

            await _userLinkManager.Unlink(input.ToUserIdentifier());
        }

        private IQueryable<LinkedUserDto> CreateLinkedUsersQuery(UserAccount currentUserAccount, string sorting)
        {
            var currentUserIdentifier = AbpSession.ToUserIdentifier();

            return (from userAccount in _userAccountRepository.GetAll()
                join tenant in _tenantRepository.GetAll() on userAccount.TenantId equals tenant.Id into tenantJoined
                from tenant in tenantJoined.DefaultIfEmpty()
                where
                    (userAccount.TenantId != currentUserIdentifier.TenantId ||
                     userAccount.UserId != currentUserIdentifier.UserId) &&
                    userAccount.UserLinkId.HasValue &&
                    userAccount.UserLinkId == currentUserAccount.UserLinkId
                select new LinkedUserDto
                {
                    Id = userAccount.UserId,
                    TenantId = userAccount.TenantId,
                    TenancyName = tenant == null ? "." : tenant.TenancyName,
                    Username = userAccount.UserName
                }).OrderBy(sorting);
        }
    }
}