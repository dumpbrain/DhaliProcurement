using System.Globalization;
using DhaliProcurement.ViewModel;
using DhaliProcurement.Models;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.Owin;
//using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using DhaliProcurement.Util;
using System.Web.Security;
using System.Data.Entity;
using PagedList;
using System.Net;

namespace AuditReport.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {
        private DCPSContext db = new DCPSContext();
        private EncryptionDecryptionUtil encryptionDecryptionUtil = new EncryptionDecryptionUtil();
        private int saltLength = 5;

        public AccountController()
        {
        }

        //    public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        //    {
        //        UserManager = userManager;
        //        SignInManager = signInManager;
        //    }

        //    private ApplicationUserManager _userManager;
        //    public ApplicationUserManager UserManager
        //    {
        //        get
        //        {
        //            return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //        }
        //        private set
        //        {
        //            _userManager = value;
        //        }
        //    }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //    private ApplicationSignInManager _signInManager;

        //    public ApplicationSignInManager SignInManager
        //    {
        //        get
        //        {
        //            return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
        //        }
        //        private set { _signInManager = value; }
        //    }

        //    //
        //    // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //// This doen't count login failures towards lockout only two factor authentication
            //// To enable password failures to trigger lockout, change to shouldLockout: true
            //var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            //switch (result)
            //{
            //    case SignInStatus.Success:
            //        return RedirectToLocal(returnUrl);
            //    case SignInStatus.LockedOut:
            //        return View("Lockout");
            //    case SignInStatus.RequiresVerification:
            //        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
            //    case SignInStatus.Failure:
            //    default:
            //        ModelState.AddModelError("", "Invalid login attempt.");
            //        return View(model);
            //}

            //string str = encryptionDecryptionUtil.CreatePasswordHash("lodi", "91qkmxU=");

            List<User> appusers = db.User.ToList();
            foreach (var appuser in appusers)
            {
                if (appuser.UserName.Equals(model.UserName) && encryptionDecryptionUtil.VerifyPassword(appuser.Password, model.Password, appuser.Salt))
                {

                    FormsAuthentication.SetAuthCookie(model.UserName, false);

                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {

                        UsersRole userpermission = db.UsersRole.First(u => u.UserId == appuser.Id);
                        if (userpermission == null)
                        {
                            FormsAuthentication.SignOut();
                            return RedirectToAction("AccessDenied", "Error", null);
                        }
                        appuser.LastLogin = DateTime.Now;
                        db.Entry(appuser).State = EntityState.Modified;
                        db.SaveChanges();
                        //return RedirectToAction("Index", "Home");
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ListUser(int? page)
        {
            var user = db.User.ToList();

            int pageSize = 100;
            int pageNumber = (page ?? 1);
            return View(user.ToPagedList(pageNumber, pageSize));
        }

        // GET: Account/UserDetails
        public ActionResult UserDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Account/CreateUser
        [Authorize(Roles = "Admin")]
        public ActionResult CreateUser()
        {

            //var status = new SelectList(new List<SelectListItem> { new SelectListItem { Text = "Active", Value = "A" }, new SelectListItem { Text = "Inactive", Value = "I" }, }, "Value", "Text");
            //ViewBag.Status = status;
            return View();
        }

        // POST: Account/CreateUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateUser([Bind(Include = "UserName,FirstName,LastName,Email,Phone,Address,IsActive,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                user.Salt = encryptionDecryptionUtil.GenerateSalt(saltLength);
                user.Password  = encryptionDecryptionUtil.CreatePasswordHash(user.Password, user.Salt);
                user.IsActive = true;
                User duplicateUser = db.User.SingleOrDefault(u => u.UserName.Equals(user.UserName, StringComparison.OrdinalIgnoreCase));
                if (duplicateUser != null)
                {
                    ModelState.AddModelError("", "User Already Exists");
                    user.Password = "";
                    return View(user);
                }
                db.User.Add(user);
                db.SaveChanges();
                return RedirectToAction("ListUser");
            }
                        
            return View(user);
        }

        //
        // GET: /Account/EditUser/5
        [Authorize(Roles = "Admin")]
        public ActionResult EditUser(short id = 0)
        {
            EditUserVM  appuser = new EditUserVM();
              
            User user = db.User.Find(id);

            appuser.UserName = user.UserName;
            appuser.FirstName = user.FirstName;
            appuser.LastName = user.LastName;
            appuser.Email = user.Email;
            appuser.Phone = user.Phone;
            appuser.Address = user.Address;
            appuser.IsActive = user.IsActive;

           
            //string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
           
            if (appuser == null)
            {
                return HttpNotFound();
            }
            //if (appuser.USERNAME.Equals(username) && isPasswordChange)
            //{
            //    appuser.PASSWARD = "";
            //    return View("EditForBonder", appuser);
            //}
            //else if (appuser.USERNAME.Equals(username))
            //{
            //    return View("EditOwnInfo", appuser);
            //}
            return View(appuser);
        }

        //
        // POST: /Account/EditUser/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult EditUser([Bind(Include = "UserName,FirstName,LastName,Email,Phone,Address,IsActive")] EditUserVM appuser)
        {
            
            if (ModelState.IsValid)
            {
                
                User appuserFound = db.User.SingleOrDefault(u => u.UserName.Equals(appuser.UserName, StringComparison.OrdinalIgnoreCase));
                if (appuserFound != null)
                {
                    appuserFound.FirstName = appuser.FirstName;
                    appuserFound.LastName = appuser.LastName;
                    appuserFound.Email = appuser.Email;
                    appuserFound.Phone = appuser.Phone;
                    appuserFound.Address  = appuser.Address;
                    appuserFound.IsActive  = appuser.IsActive;

                    db.Entry(appuserFound).State = EntityState.Modified;
                    db.SaveChanges();

                }
                            
                return RedirectToAction("ListUser");
            }
           
            return View(appuser);
        }

        [Authorize(Roles = "Admin")]
        public JsonResult DeleteUser(int id)
        {
            string result = "";

            var roleFound = db.UsersRole.Where(x => x.UserId == id).ToList();

            if (roleFound.Count == 0)
            {
                bool flag = false;
                try
                {

                    User user = db.User.Find(id);
                    db.User.Remove(user);
                    db.SaveChanges();
                    flag = true;

                }
                catch (Exception ex)
                {

                }
                return Json(flag, JsonRequestBehavior.AllowGet);

            }
            else
            {
                result = "No";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        //    //
        //    // GET: /Account/Register
        //    [AllowAnonymous]
        //    public ActionResult Register()
        //    {
        //        return View();
        //    }

        //    //
        //    // POST: /Account/Register
        //    [HttpPost]
        //    [AllowAnonymous]
        //    [ValidateAntiForgeryToken]
        //    public async Task<ActionResult> Register(RegisterViewModel model)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        //            var result = await UserManager.CreateAsync(user, model.Password);
        //            if (result.Succeeded)
        //            {
        //                var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
        //                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
        //                await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
        //                ViewBag.Link = callbackUrl;
        //                return View("DisplayEmail");
        //            }
        //            AddErrors(result);
        //        }

        //        // If we got this far, something failed, redisplay form
        //        return View(model);
        //    }




        public ViewResult ChangePass()
        {

            ////var uid = userRepository.Users.FirstOrDefault(x => x.USER_NAME == User.Identity.Name).USER_ID;
            //var user = userRepository.Users.FirstOrDefault(x => x.USER_NAME == User.Identity.Name);
            ////ViewBag.UserId = uid;
            //ChangePassViewModel passChange = new ChangePassViewModel();

            //passChange.USER_ID = user.USER_ID;
            ViewBag.UserName = User.Identity.Name;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult ChangePass(ChangePasswordVM changePassword, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(changePassword);
            }

            //if (changePassword.UserName == null || changePassword.CurrentPassword == null || changePassword.NewPassword == null || changePassword.ConfirmPassword == null || changePassword.UserName.Trim().Equals("") || changePassword.CurrentPassword.Trim().Equals("") || changePassword.NewPassword.Trim().Equals("") || changePassword.ConfirmPassword.Trim().Equals(""))
            //{
            //    ModelState.AddModelError("", "Wrong Username or Password");
            //}

            List<User> appusers = db.User.ToList();
            foreach (var appuser in appusers)
            {
                if (appuser.UserName.Equals(changePassword.UserName) && encryptionDecryptionUtil.VerifyPassword(appuser.Password, changePassword.CurrentPassword, appuser.Salt))
                {
                    appuser.Password = encryptionDecryptionUtil.CreatePasswordHash(changePassword.NewPassword, appuser.Salt);
                    db.Entry(appuser).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
            }

            if (ModelState.IsValid)
            {
                ModelState.AddModelError("", "Wrong Username or Password");
            }

            return View(changePassword);
        }

        [Authorize(Roles = "Admin")]
        public ViewResult ChangePassUser(decimal id)
        {

            ////var uid = userRepository.Users.FirstOrDefault(x => x.USER_NAME == User.Identity.Name).USER_ID;
            //var user = userRepository.Users.FirstOrDefault(x => x.USER_NAME == User.Identity.Name);
            ////ViewBag.UserId = uid;
            //ChangePassViewModel passChange = new ChangePassViewModel();

            //passChange.USER_ID = user.USER_ID;

            User user = db.User.FirstOrDefault(c => c.Id == id);
            ViewBag.UserName = user.UserName;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult ChangePassUser(ChangePasswordUserVM changePassword, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(changePassword);
            }

            //if (changePassword.UserName == null || changePassword.CurrentPassword == null || changePassword.NewPassword == null || changePassword.ConfirmPassword == null || changePassword.UserName.Trim().Equals("") || changePassword.CurrentPassword.Trim().Equals("") || changePassword.NewPassword.Trim().Equals("") || changePassword.ConfirmPassword.Trim().Equals(""))
            //{
            //    ModelState.AddModelError("", "Wrong Username or Password");
            //}

            List<User> appusers = db.User.ToList();
            foreach (var appuser in appusers)
            {
                if (appuser.UserName.Equals(changePassword.UserName))
                {
                    appuser.Password = encryptionDecryptionUtil.CreatePasswordHash(changePassword.NewPassword, appuser.Salt);
                    db.Entry(appuser).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ListUser");
                }
            }

            if (ModelState.IsValid)
            {
                ModelState.AddModelError("", "Wrong Username or Password");
            }

            return View(changePassword);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ListUserRole(int? page)
        {
            var user = db.UsersRole.Include(p => p.User).Include(p => p.Role).ToList();

            int pageSize = 100;
            int pageNumber = (page ?? 1);
            return View(user.ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "Admin")]
        public ViewResult RoleAssign()
        {
            PopulateRoleDropDownList();
            PopulateUserDropDownList();
            return View(new UsersRole());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAssign(UsersRole roleassign)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    db.UsersRole.Add(roleassign);
                    db.SaveChanges();

                    return RedirectToAction("ListUserRole");
                    
                }
            }
            catch (Exception)
            {

                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persist please contact with your system administrator");
            }

            //FLM_ROLE role = RoleRepository.roles.FirstOrDefault(c => c.role_id == roleassign.role_id);
            PopulateRoleDropDownList(roleassign.RoleId);
            PopulateUserDropDownList(roleassign.UserId);
            //context.SaveChanges();
            return View(roleassign);
            //return RedirectToAction("ListUserRole", "Account");

        }

        [Authorize(Roles = "Admin")]
        public ActionResult EditUserRole(decimal id)
        {

            if (id == 0)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userRole = db.UsersRole.Include(p => p.User).FirstOrDefault(c => c.Id == id);

            PopulateRoleDropDownList(userRole.RoleId);

            return View(userRole);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserRole(UsersRole userRole)
        {
            try
            {
                if (ModelState.IsValid)
                {                

                    UsersRole roleFound = db.UsersRole.FirstOrDefault(c => c.Id == userRole.Id);
                    if (roleFound != null)
                    {
                        roleFound.RoleId = userRole.RoleId;                                   
                        db.Entry(roleFound).State = EntityState.Modified;
                        db.SaveChanges();

                    }

                    return RedirectToAction("ListUserRole", "Account");
                }
            }
            catch (Exception)
            {

                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persist please contact with your system administrator");
            }

            PopulateRoleDropDownList(userRole.RoleId);
            return View(userRole);

        }

        [Authorize(Roles = "Admin")]
        public JsonResult DeleteUserRole(int id)
        {
            //string result = "";
            
            bool flag = false;
            try
            {

                UsersRole  roleFound = db.UsersRole.Find(id);
                db.UsersRole.Remove(roleFound);
                db.SaveChanges();
                flag = true;

            }
            catch (Exception ex)
            {

            }

            return Json(flag, JsonRequestBehavior.AllowGet);

            

        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login", "Account");            
        }

        private void PopulateRoleDropDownList(object selectedRoleName = null)
        {
            var roleQuery = from s in db.Role 
                            orderby s.Id 
                            select s;

            ViewBag.RoleName = new SelectList(roleQuery, "Id", "RoleName", selectedRoleName);
        }

        private void PopulateUserDropDownList(object selectedUserName = null)
        {
            var userQuery = from s in db.User
                            orderby s.Id
                            select s;

            ViewBag.UserName = new SelectList(userQuery, "Id", "UserName", selectedUserName);
        }

    }
}