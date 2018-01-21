using AuditReport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace AuditReport.Util
{

    public class CustomAuthorize : AuthorizeAttribute
    {
        private EPARSDbContext db = new EPARSDbContext();

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            string idParam = "";
            var descriptor = filterContext.ActionDescriptor;
            var actionName = descriptor.ActionName;
            var controllerName = descriptor.ControllerDescriptor.ControllerName;

            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                //if not logged, it will work as normal Authorize and redirect to the Login
                base.HandleUnauthorizedRequest(filterContext);
                return;

            }
            else if (!this.Roles.Split(',').Select(x => x.Trim()).Distinct().ToArray().Any(filterContext.HttpContext.User.IsInRole))
            {
                // The user is not in any of the listed roles => 
                // show the unauthorized view
                filterContext.Result = filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
            }

            try
            {
                idParam = filterContext.Controller.ValueProvider.GetValue("id").AttemptedValue;
            }
            catch (NullReferenceException ex)
            {
                return;

            }

            int id;
            string username = FormsAuthentication.Decrypt(HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            User appuser = db.User.SingleOrDefault(u => u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
            UsersRole loggedinUser = new SessionAttributeRetreival().getStoredUserPermission();

            if (int.TryParse(idParam, out id))
            {
                if (System.Web.HttpContext.Current.User.IsInRole("Bonder"))
                {
                    if (id != appuser.ID && controllerName.Equals("AppUser"))
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
                    }
                    if (controllerName.Equals("ExBond"))
                    {
                        if (loggedinUser == null)
                        {
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
                        }
                        else
                        {
                            EXBOND exbond = db.EXBONDs.SingleOrDefault(u => u.ID == id);
                            if (exbond != null && exbond.BONDERID != loggedinUser.BONDERID)
                            {
                                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
                            }
                        }
                    }
                    if (controllerName.Equals("InBond"))
                    {
                        if (loggedinUser == null)
                        {
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
                        }
                        else
                        {
                            INBOND inbond = db.INBONDs.SingleOrDefault(u => u.ID == id);
                            if (inbond != null && inbond.BONDERID != loggedinUser.BONDERID)
                            {
                                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
                            }
                        }
                    }
                    
                    if (controllerName.Equals("UP"))
                    {
                        if (loggedinUser == null)
                        {
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
                        }

                    }
                    if (controllerName.Equals("UPRequest"))
                    {
                        if (loggedinUser == null)
                        {
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
                        }
                        else
                        {
                            UPREQUEST uprequest = db.UPREQUESTs.SingleOrDefault(u => u.ID == id);
                            if (uprequest != null && uprequest.BONDERID != loggedinUser.BONDERID)
                            {
                                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
                            }
                        }
                    }

                }
            }


        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {

            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                //if not logged, it will work as normal Authorize and redirect to the Login
                base.HandleUnauthorizedRequest(filterContext);

            }
            else
            {
                //logged and wihout the role to access it - redirect to the custom controller action
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
            }
        }

    }

}