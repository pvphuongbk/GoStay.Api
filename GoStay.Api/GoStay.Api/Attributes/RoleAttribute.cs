using GoStay.Api.Providers;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using GoStay.Common.Configuration;

namespace GoStay.Api.Attributes
{
    public class AuthorizeAttribute : Attribute, Microsoft.AspNetCore.Mvc.Filters.IAuthorizationFilter
    {
        //public RoleEnum[] Role { get; set; }
        //public ModuleType? ModuleType { get; set; }
        //public RoleAttribute(params RoleEnum[] roles)
        //{
        //    Role = roles;
        //}
        //public RoleAttribute(ModuleType moduleType, params RoleEnum[] roles)
        //{
        //    Role = roles;
        //    ModuleType = moduleType;
        //}
        /// <summary>  
        /// This will Authorize User  
        /// </summary>  
        /// <returns></returns>  
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (filterContext != null)
            {
                #region Hanld token
                Microsoft.Extensions.Primitives.StringValues authTokens;
                filterContext.HttpContext.Request.Headers.TryGetValue("apk", out authTokens);
                var _token = authTokens.FirstOrDefault();
                //var handler = new JwtSecurityTokenHandler();
                //var jwtSecurityToken = handler.ReadJwtToken(_token);
                //filterContext.HttpContext.Request.Headers["token_"] = "ta la phuong";
                string value = AppConfigs.ApiKey;
                if (value != _token)
                {
                    filterContext.Result = new JsonResult("NotAuthorized")
                    {
                        StatusCode = 405,
                        Value = new
                        {
                            Status = "Error",
                            Message = "Sorry, You are not authorized to perform this action."
                        },
                    };
                }
                #endregion
            }

        }
    }
}
