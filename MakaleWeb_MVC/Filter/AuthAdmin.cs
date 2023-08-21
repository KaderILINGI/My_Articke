using MakaleWeb_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MakaleWeb_MVC.Filter
{
    public class AuthAdmin : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {           
            if (SessionUser.Login!=null && SessionUser.Login.Admin==false)
            {
                filterContext.Result = new RedirectResult("/Home/YetkisizErisim");
            }
        }
    }
}