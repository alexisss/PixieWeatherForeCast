using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WeatherForecast.Mvc.Models;
using WeatherForecast.Data;
using WeatherForecast.Models;
using System.IO;
using Kendo.Mvc.Extensions;

namespace WeatherForecast.Mvc.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        const string DefaultProfilePictureUrl = "default.jpg";
        UowData Data;
        public AccountController() 
        {
            this.Data = new UowData();
            IdentityManager = new AuthenticationIdentityManager(new IdentityStore(new DataContext()));
            
        }

        public AccountController(AuthenticationIdentityManager manager)
        {
            this.Data = new UowData();
            IdentityManager = manager;

        }

        public AuthenticationIdentityManager IdentityManager { get; private set; }

        private Microsoft.Owin.Security.IAuthenticationManager AuthenticationManager {
            get {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                // Validate the password
                IdentityResult result = await IdentityManager.Authentication.CheckPasswordAndSignInAsync(AuthenticationManager, model.UserName, model.Password, model.RememberMe);
                if (result.Success)
                {
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    AddErrors(result);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(ExtendedRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Create a local login before signing in the user
                var user = new ExtendedUser 
                {
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Description = model.Description,
                    PictureUrl = DefaultProfilePictureUrl
                };
                var result = await IdentityManager.Users.CreateLocalUserAsync(user, model.Password);
                if (result.Success)
                {
                    await IdentityManager.Authentication.SignInAsync(AuthenticationManager, user.Id, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    AddErrors(result);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            string message = null;
            IdentityResult result = await IdentityManager.Logins.RemoveLoginAsync(User.Identity.GetUserId(), loginProvider, providerKey);
            if (result.Success)
            {
                message = "The external login was removed.";
            }
            else
            {
                message = result.Errors.FirstOrDefault();
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public async Task<ActionResult> Manage(string message)
        {
            ViewBag.StatusMessage = message ?? "";
            ViewBag.HasLocalPassword = await IdentityManager.Logins.HasLocalLoginAsync(User.Identity.GetUserId());
            ViewBag.ReturnUrl = Url.Action("Manage");
            string userId = User.Identity.GetUserId();
            PopulateFields(userId);

            bool isAuthor = Data.Users.All().FirstOrDefault(u=>u.Id == userId).Roles.Any(r=>r.Role.Name == "Author");
            ManageUserViewModel model = new ManageUserViewModel
            {
                IsAuthor = isAuthor,
                Username = User.Identity.GetUserName()
            };
            return View(model);
        }

        
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {           
            string userId = User.Identity.GetUserId();
            
            bool hasLocalLogin = await IdentityManager.Logins.HasLocalLoginAsync(userId);
            ViewBag.HasLocalPassword = hasLocalLogin;

            ExtendedUser user = Data.Users.All().Where(u => u.Id == userId).FirstOrDefault();
            ViewBag.UserId = userId;
            user.FirstName  = model.FirstName;         
            user.LastName  = model.LastName;
            user.Description = model.Description;   

            Data.SaveChanges();
            //if (hasLocalLogin)
            //{               
            //    if (ModelState.IsValid)
            //    {
            //        IdentityResult result = await IdentityManager.Passwords.ChangePasswordAsync(User.Identity.GetUserName(), model.OldPassword, model.NewPassword);
            //        if (result.Success)
            //        {
            //            return RedirectToAction("Manage", new { Message = "Your password has been changed." });
            //        }
            //        else
            //        {
            //            AddErrors(result);
            //        }
            //    }
            //}
            //else
            //{
            //    // User does not have a local password so remove any validation errors caused by a missing OldPassword field
            //    ModelState state = ModelState["OldPassword"];
            //    if (state != null)
            //    {
            //        state.Errors.Clear();
            //    }

            //    if (ModelState.IsValid)
            //    {
            //        // Create the local login info and link it to the user
            //        IdentityResult result = await IdentityManager.Logins.AddLocalLoginAsync(userId, User.Identity.GetUserName(), model.NewPassword);
            //        if (result.Success)
            //        {
            //            return RedirectToAction("Manage", new { Message = "Your password has been set." });
            //        }
            //        else
            //        {
            //            AddErrors(result);
            //        }
            //    }
            //}

            // If we got this far, something failed, redisplay form
           
            return View(model);
        }

        private void PopulateFields(string id)
        {
            var user = this.Data.Users.All().Where(u => u.Id == id).FirstOrDefault();

            var firstName = user.FirstName;
            var lastName = user.LastName;
            var description = user.Description;
            var picture = user.PictureUrl;

            ViewBag.firstName = firstName;
            ViewBag.lastName = lastName;
            ViewBag.description = description;
            ViewBag.picture = picture;
        }

        //private string GetUserById()
        //{
        //    string userId = User.Identity.GetUserId();

        //    ExtendedUser user = this.context.Users.All().Where(u => u.Id == userId).FirstOrDefault();
        //    return user.PictureUrl;
        //}
        public ActionResult UploadedFiles(IEnumerable<HttpPostedFileBase> upload)
        {
            if (upload != null)
            {
                var currentUserName = User.Identity.GetUserName();
                foreach (var file in upload)
                {
                    var fileName = currentUserName + Path.GetExtension(file.FileName);
                    var physicalPath = Path.Combine(Server.MapPath("~/img/Account"), fileName);
                   
                    string userName = User.Identity.GetUserName();
                    ExtendedUser user = this.Data.Users.All().Where(u => u.UserName == userName).FirstOrDefault();
                    user.PictureUrl = fileName;
                    ViewBag.picture = fileName;   
                    Data.SaveChanges();
                    file.SaveAs(physicalPath);
                }
            }

            return Content("");
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { loginProvider = provider, ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string loginProvider, string returnUrl)
        {
            ClaimsIdentity id = await IdentityManager.Authentication.GetExternalIdentityAsync(AuthenticationManager);
            // Sign in this external identity if its already linked
            IdentityResult result = await IdentityManager.Authentication.SignInExternalIdentityAsync(AuthenticationManager, id);
            if (result.Success) 
            {
                return RedirectToLocal(returnUrl);
            }
            else if (User.Identity.IsAuthenticated)
            {
                // Try to link if the user is already signed in
                result = await IdentityManager.Authentication.LinkExternalIdentityAsync(id, User.Identity.GetUserId());
                if (result.Success)
                {
                    return RedirectToLocal(returnUrl);
                }
                else 
                {
                    return View("ExternalLoginFailure");
                }
            }
            else
            {
                // Otherwise prompt to create a local user
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = id.Name });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }
            
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                IdentityResult result = await IdentityManager.Authentication.CreateAndSignInExternalUserAsync(AuthenticationManager, new User(model.UserName));
                if (result.Success)
                {
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    AddErrors(result);
                }
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return (ActionResult)PartialView("_ExternalLoginsListPartial", new List<AuthenticationDescription>(AuthenticationManager.GetExternalAuthenticationTypes()));
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            return Task.Run(async () =>
            {
                var linkedAccounts = new List<IUserLogin>(await IdentityManager.Logins.GetLoginsAsync(User.Identity.GetUserId()));
                ViewBag.ShowRemoveButton = linkedAccounts.Count > 1;
                return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
            }).Result;
        }

        protected override void Dispose(bool disposing) {
            if (disposing && IdentityManager != null) {
                IdentityManager.Dispose();
                IdentityManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        private void AddErrors(IdentityResult result) {
            foreach (var error in result.Errors) {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUrl)
            {
                LoginProvider = provider;
                RedirectUrl = redirectUrl;
            }

            public string LoginProvider { get; set; }
            public string RedirectUrl { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                context.HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties() { RedirectUrl = RedirectUrl }, LoginProvider);
            }
        }
        #endregion
    }
}