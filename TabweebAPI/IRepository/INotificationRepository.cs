using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Tabweeb_Model.Common.commonclass;
using Tabweeb_Model;
using TabweebAPI.Common;
using Microsoft.AspNetCore.Mvc;
namespace TabweebAPI.IRepository
{
    interface INotificationRepository
    {
        //Task<MethodResult<NotificationsOutModel>> SendNotification(NotificationsInModel notificationsInModel);
        Task<MethodResult<saveStatus>> SendNotification(QuotationMailSend obj, string AppPath);
    }
}
