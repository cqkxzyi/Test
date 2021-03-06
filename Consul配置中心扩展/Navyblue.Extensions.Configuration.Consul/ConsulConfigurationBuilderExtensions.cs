﻿// *****************************************************************************************************************
// Project          : Navyblue
// File             : ConsulConfigurationBuilderExtensions.cs
// Created          : 2019-05-23  19:30
//
// Last Modified By : (jstsmaxx@163.com)
// Last Modified On : 2019-05-24  11:42
// *****************************************************************************************************************
// <copyright file="ConsulConfigurationBuilderExtensions.cs" company="Shanghai Future Mdt InfoTech Ltd.">
//     Copyright ©  2012-2019 Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// *****************************************************************************************************************

using Microsoft.Extensions.Configuration;
using System;
using System.Threading;

namespace Extension.Configuration.Consul
{
    /// <summary>
    /// 1外部调用总入口
    /// </summary>
    public static class ConsulConfigurationBuilderExtensions
    {
        /// <summary>
        /// 1添加console资源
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="serviceKey"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddConsul(this IConfigurationBuilder builder, string serviceKey, CancellationToken cancellationToken, Action<IConsulConfigurationSource> options)
        {
            ConsulConfigurationSource consulConfigSource = new ConsulConfigurationSource(serviceKey, cancellationToken);
            options(consulConfigSource);
            return builder.Add(consulConfigSource);
        }
    }
}