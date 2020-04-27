// ======================================================================
// 
//           Copyright (C) 2019-2020 ����������Ϣ�Ƽ����޹�˾
//           All rights reserved
// 
//           filename : DemoUiComponentsController.cs
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

using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using Magicodes.Admin.Application.DemoUiComponents.Dto;
using Magicodes.Admin.Core.Storage;
using Microsoft.AspNetCore.Mvc;

namespace Magicodes.Admin.Web.Host.Controllers
{
    [AbpMvcAuthorize]
    public class DemoUiComponentsController : AdminControllerBase
    {
        private readonly IBinaryObjectManager _binaryObjectManager;

        public DemoUiComponentsController(IBinaryObjectManager binaryObjectManager)
        {
            _binaryObjectManager = binaryObjectManager;
        }

        [HttpPost]
        public async Task<JsonResult> UploadFiles()
        {
            try
            {
                var files = Request.Form.Files;

                //Check input
                if (files == null) throw new UserFriendlyException(L("File_Empty_Error"));

                var filesOutput = new List<UploadFileOutput>();

                foreach (var file in files)
                {
                    if (file.Length > 1048576) //1MB
                        throw new UserFriendlyException(L("File_SizeLimit_Error"));

                    byte[] fileBytes;
                    using (var stream = file.OpenReadStream())
                    {
                        fileBytes = stream.GetAllBytes();
                    }

                    var fileObject = new BinaryObject(null, fileBytes);
                    await _binaryObjectManager.SaveAsync(fileObject);

                    filesOutput.Add(new UploadFileOutput
                    {
                        Id = fileObject.Id,
                        FileName = file.FileName
                    });
                }

                return Json(new AjaxResponse(filesOutput));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }
    }
}