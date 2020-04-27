// ======================================================================
// 
//           Copyright (C) 2019-2020 ����������Ϣ�Ƽ����޹�˾
//           All rights reserved
// 
//           filename : UserAppService_Delete_Tests.cs
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
using Abp.Application.Services.Dto;
using Shouldly;
using Xunit;

namespace Magicodes.Admin.Tests.Authorization.Users
{
    public class UserAppService_Delete_Tests : UserAppServiceTestBase
    {
        [Fact]
        public async Task Should_Delete_User()
        {
            //Arrange
            CreateTestUsers();

            var user = await GetUserByUserNameOrNullAsync("artdent");
            user.ShouldNotBe(null);

            //Act
            await UserAppService.DeleteUser(new EntityDto<long>(user.Id));

            //Assert
            user = await GetUserByUserNameOrNullAsync("artdent");
            user.IsDeleted.ShouldBe(true);
        }
    }
}