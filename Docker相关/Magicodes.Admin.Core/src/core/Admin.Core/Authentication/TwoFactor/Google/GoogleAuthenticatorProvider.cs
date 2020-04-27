// ======================================================================
// 
//           Copyright (C) 2019-2020 ����������Ϣ�Ƽ����޹�˾
//           All rights reserved
// 
//           filename : GoogleAuthenticatorProvider.cs
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

using System.Threading.Tasks;
using Abp.Dependency;
using Abp.UI;
using Magicodes.Admin.Core.Authorization.Users;
using Microsoft.AspNetCore.Identity;

namespace Magicodes.Admin.Core.Authentication.TwoFactor.Google
{
    public class GoogleAuthenticatorProvider : AdminServiceBase, IUserTwoFactorTokenProvider<User>, ITransientDependency
    {
        public const string Name = "GoogleAuthenticator";
        private readonly GoogleTwoFactorAuthenticateService _googleTwoFactorAuthenticateService;

        public GoogleAuthenticatorProvider(GoogleTwoFactorAuthenticateService googleTwoFactorAuthenticateService)
        {
            _googleTwoFactorAuthenticateService = googleTwoFactorAuthenticateService;
        }

        public Task<string> GenerateAsync(string purpose, UserManager<User> userManager, User user)
        {
            CheckIfGoogleAuthenticatorIsEnabled(user);

            var setupInfo = _googleTwoFactorAuthenticateService.GenerateSetupCode("Magicodes.Admin", user.EmailAddress,
                user.GoogleAuthenticatorKey, 300, 300);

            return Task.FromResult(setupInfo.QrCodeSetupImageUrl);
        }

        public Task<bool> ValidateAsync(string purpose, string token, UserManager<User> userManager, User user)
        {
            CheckIfGoogleAuthenticatorIsEnabled(user);

            return Task.FromResult(
                _googleTwoFactorAuthenticateService.ValidateTwoFactorPin(user.GoogleAuthenticatorKey, token));
        }

        public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<User> userManager, User user)
        {
            return Task.FromResult(user.IsTwoFactorEnabled && user.GoogleAuthenticatorKey != null);
        }

        private void CheckIfGoogleAuthenticatorIsEnabled(User user)
        {
            if (user.GoogleAuthenticatorKey == null)
                throw new UserFriendlyException(L("GoogleAuthenticatorIsNotEnabled"));
        }
    }
}