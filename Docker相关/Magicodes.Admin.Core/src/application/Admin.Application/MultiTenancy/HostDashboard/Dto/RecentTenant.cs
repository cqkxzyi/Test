// ======================================================================
// 
//           Copyright (C) 2019-2020 湖南心莱信息科技有限公司
//           All rights reserved
// 
//           filename : RecentTenant.cs
//           description :
// 
//           created by 雪雁 at  2019-06-17 10:17
//           开发文档: docs.xin-lai.com
//           公众号教程：magiccodes
//           QQ群：85318032（编程交流）
//           Blog：http://www.cnblogs.com/codelove/
//           Home：http://xin-lai.com
// 
// ======================================================================

using System;

namespace Magicodes.Admin.Application.MultiTenancy.HostDashboard.Dto
{
    public class RecentTenant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationTime { get; set; }
    }
}