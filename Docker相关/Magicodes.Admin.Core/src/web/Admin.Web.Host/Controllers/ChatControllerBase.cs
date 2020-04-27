// ======================================================================
// 
//           Copyright (C) 2019-2020 ����������Ϣ�Ƽ����޹�˾
//           All rights reserved
// 
//           filename : ChatControllerBase.cs
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

using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using Magicodes.Admin.Core.Chat;
using Magicodes.Admin.Core.Storage;
using Microsoft.AspNetCore.Mvc;

namespace Magicodes.Admin.Web.Host.Controllers
{
    public class ChatControllerBase : AdminControllerBase
    {
        protected readonly IBinaryObjectManager BinaryObjectManager;
        protected readonly IChatMessageManager ChatMessageManager;

        public ChatControllerBase(IBinaryObjectManager binaryObjectManager, IChatMessageManager chatMessageManager)
        {
            BinaryObjectManager = binaryObjectManager;
            ChatMessageManager = chatMessageManager;
        }

        [HttpPost]
        [AbpMvcAuthorize]
        public async Task<JsonResult> UploadFile()
        {
            try
            {
                var file = Request.Form.Files.First();

                //Check input
                if (file == null) throw new UserFriendlyException(L("File_Empty_Error"));

                if (file.Length > 10000000) //10MB
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));

                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                var fileObject = new BinaryObject(null, fileBytes);
                using (CurrentUnitOfWork.SetTenantId(null))
                {
                    await BinaryObjectManager.SaveAsync(fileObject);
                }

                return Json(new AjaxResponse(new
                {
                    id = fileObject.Id,
                    name = file.FileName,
                    contentType = file.ContentType
                }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }
    }
}