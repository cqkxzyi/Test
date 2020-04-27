// ======================================================================
// 
//           Copyright (C) 2019-2020 ����������Ϣ�Ƽ����޹�˾
//           All rights reserved
// 
//           filename : RandomUserGenerator.cs
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

using System.Collections.Generic;
using Abp;
using Abp.Dependency;
using Magicodes.Admin.Core.Authorization.Users;
using Microsoft.AspNetCore.Identity;

namespace Magicodes.Admin.Core.MultiTenancy.Demo
{
    public class RandomUserGenerator : ITransientDependency
    {
        public static string[] Names =
        {
            "Agatha Christie",
            "Albert Einstein",
            "Aldous Huxley",
            "Amin Maalouf",
            "Andrew Andrewus",
            "Arda Turan",
            "Audrey Naulin",
            "Biff Tannen",
            "Bruce Wayne",
            "Butch Coolidge",
            "Carl Sagan",
            "Charles Quint",
            "Christophe Grange",
            "Christopher Nolan",
            "Christopher Lloyd",
            "Clara Clayton",
            "Clarice Starling",
            "Dan Brown",
            "Daniel Radcliffe",
            "Douglas Hall",
            "David Wells",
            "Emmett Brown",
            "Friedrich Hegel",
            "Forrest Gump",
            "Franz Kafka",
            "Gabriel Marquez",
            "Galileo Galilei",
            "Georghe Hagi",
            "Georghe Orwell",
            "Georghe Richards",
            "Gottfried Leibniz",
            "Hannibal Lecter",
            "Hercules Poirot",
            "Isaac Asimov",
            "Jane Fuller",
            "Jean Reno",
            "Jeniffer Parker",
            "Johan Elmander",
            "Jules Winnfield",
            "Kurt Vonnegut",
            "Laurence Fishburne",
            "Leo Tolstoy",
            "Lorraine Baines",
            "Marsellus Wallace",
            "Marty Mcfly",
            "Michael Corleone",
            "Oktay Anar",
            "Omer Hayyam",
            "Paulho Coelho",
            "Quentin Tarantino",
            "Rene Descartes",
            "Robert Lafore",
            "Stanislaw Lem",
            "Stefan Zweig",
            "Stephenie Mayer",
            "Stephen Hawking",
            "Thomas More",
            "Vincent Vega",
            "Vladimir Nabokov",
            "William Faulkner"
        };

        public static string[] EmailProviders =
        {
            "yahoo.com",
            "gmail.com",
            "hotmail.com",
            "outlook.com",
            "live.com",
            "yandex.com",
            "aspnetzero.com",
            "aspnetboilerplate.com"
        };

        private readonly IPasswordHasher<User> _passwordHasher;

        public RandomUserGenerator(IPasswordHasher<User> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public List<User> GetRandomUsers(int userCount, int tenantId)
        {
            var users = new List<User>();

            var randomNames = RandomHelper.GenerateRandomizedList(Names);
            for (var i = 0; i < userCount && i < randomNames.Count; i++)
                users.Add(CreateUser(tenantId, randomNames[i]));

            return users;
        }

        private User CreateUser(int? tenantId, string nameSurname)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = GenerateUsername(nameSurname),
                EmailAddress = GenerateEmail(nameSurname),
                Name = nameSurname.Split(' ')[0],
                Surname = nameSurname.Split(' ')[1],
                ShouldChangePasswordOnNextLogin = false,
                IsActive = RandomHelper.GetRandom(0, 100) < 80, //A user will be active by 80% probability
                IsEmailConfirmed = true
            };

            user.SetNormalizedNames();
            user.Password = _passwordHasher.HashPassword(user, "123456");

            return user;
        }

        private static string GenerateUsername(string nameSurname)
        {
            return nameSurname.Replace(" ", ".").ToLowerInvariant();
        }

        private static string GenerateEmail(string nameSurname)
        {
            return GenerateUsername(nameSurname) + "@" + RandomHelper.GetRandomOf(EmailProviders);
        }
    }
}