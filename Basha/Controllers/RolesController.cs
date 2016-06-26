using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Basha.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data;
using System.Data.Entity;
using System.Net;

namespace Basha.Controllers
{
    public class RolesController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public RolesController()
        {
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
        }
        public RolesController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }
        public UserManager<ApplicationUser> UserManager { get; private set; }
        public RoleManager<IdentityRole> RoleManager { get; private set; }
        /// /////////// users //////////////////
        public ActionResult UserList()
        {
            var userlist = UserManager.Users.ToList();
            return View(userlist);
        }

        // GET: /Users/Create
        public ActionResult CreateUser()
        {
            //Get the list of Roles
            ViewBag.RoleId = new SelectList(RoleManager.Roles.ToList(), "Id", "Name");
            return View();
        }
        //
        // POST: /Users/Create
        [HttpPost]
        //public  ActionResult  CreateUser(RegisterViewModel userViewModel, string RoleId)
        public ActionResult CreateUser(RegisterViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser();
                user.UserName = userViewModel.Email;
                user.Email = user.UserName;
                //user.HomeTown = userViewModel.HomeTown;
                var adminresult = UserManager.Create(user, userViewModel.Password);
                //Add User Admin to Role Admin
                if (adminresult.Succeeded)
                {
                //    if (!String.IsNullOrEmpty(RoleId))
                //    {
                //        //Find Role Admin
                //        var role = RoleManager.FindById(RoleId);
                //        var result = UserManager.AddToRole(user.Id, role.Name);
                //        if (!result.Succeeded)
                //        {
                //            ModelState.AddModelError("", result.Errors.First().ToString());
                //            ViewBag.RoleId = new SelectList(RoleManager.Roles.ToList(), "Id", "Name");
                //            return View();
                //        }
                //    }
                }
                else
                {
                    ModelState.AddModelError("", adminresult.Errors.First().ToString());
                    //ViewBag.RoleId = new SelectList(RoleManager.Roles, "Id", "Name");
                    return View();

                }
                return RedirectToAction("UserList");
            }
            else
            {
                //ViewBag.RoleId = new SelectList(RoleManager.Roles, "Id", "Name");
                return View();
            }
        }



        // GET: /Users/Edit/1
        public ActionResult EditUser(string usern)
        {

            if (string.IsNullOrWhiteSpace(usern))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = context.Users.Where(u => u.UserName.Equals(usern, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            string Id = user.Id;

            ////ViewBag.RoleId = new SelectList(RoleManager.Roles, "Id", "Name");

            var usernow = UserManager.FindById(Id);
            if (usernow == null)
            {
                return HttpNotFound();
            }

            Session["usernn"] = user.Email ;
            
            
            return View(user);
        
        }


        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult EditUser([Bind(Include = "Email,Id")] ApplicationUser formuser, string id, string RoleId)
        public ActionResult EditUser([Bind(Include = "Email,Id")] ApplicationUser formuser, string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string olduser = Convert.ToString(Session["usernn"]);
            if (olduser == formuser.Email)
            {
                return RedirectToAction("UserList");
            }
   
           ////////////ViewBag.RoleId = new SelectList(RoleManager.Roles, "Id", "Name");
            var user = UserManager.FindById(id);
            user.UserName = formuser.Email;
            user.Email = formuser.Email;
            
            if (ModelState.IsValid)
            {
                //Update the user details
                UserManager.Update(user);

                //If user has existing Role then remove the user from the role
                // This also accounts for the case when the Admin selected Empty from the drop-down and
                // this means that all roles for the user must be removed
                var rolesForUser = UserManager.GetRoles(id);
                if (rolesForUser.Count() > 0)
                {
                    foreach (var item in rolesForUser)
                    {
                        var result = UserManager.RemoveFromRole(id, item);
                    }
                }

                //if (!String.IsNullOrEmpty(RoleId))
                //{
                //    //Find Role
                //    var role = RoleManager.FindById(RoleId);
                //    //Add user to new role
                //    var result = UserManager.AddToRole(id, role.Name);
                //    if (!result.Succeeded)
                //    {
                //        ModelState.AddModelError("", result.Errors.First().ToString());
                //        ViewBag.RoleId = new SelectList(RoleManager.Roles, "Id", "Name");
                //        return View();
                //    }
                //}
                return RedirectToAction("UserList");
            }
            else
            {
                //ViewBag.RoleId = new SelectList(RoleManager.Roles, "Id", "Name");
                return View();
            }
        }
        /////////////////////////////////
        // GET: /Users/Delete/5
        [HttpGet]
        public ActionResult DeleteUser(string usern)
        {
            if (string.IsNullOrWhiteSpace(usern))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = context.Users.Where(u => u.UserName.Equals(usern, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            return View(user);
        }

        // POST: /Users/Delete/5
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUserconf(string usern)
        {
            ApplicationUser user = context.Users.Where(u => u.UserName.Equals(usern, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(usern))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //var logins = user.Logins;
                //foreach (var login in logins)
                //{
                //    context.UserLogins.Remove(login);
                //}
                var userrols = UserManager.GetRoles(user.Id);
                if (userrols.Count() > 0)
                {
                    foreach (var item in userrols)
                    {
                        UserManager.RemoveFromRole(user.Id, item);
                        context.SaveChanges();
                    }
                }
                context.Users.Remove(user);
                context.SaveChanges();
                return RedirectToAction("UserList");
            }
            else
            {
                return View(user);
            }
        }
        /////////////////////////////////////////////////////////////
        // GET: Roles
        public ActionResult Index()
        {
            var roles = context.Roles.ToList();
            return View(roles);
        }
        // GET: Roles/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: Roles/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            string Nameraw = collection["RoleName"];
            if (string.IsNullOrEmpty(Nameraw))
            {
                ViewBag.ResultMessage = "Role Is empty  !";
                return RedirectToAction("Index");
            }
            try
            {
                context.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole()
                {
                    Name = collection["RoleName"]
                });

              
                context.SaveChanges();
                ViewBag.ResultMessage = "Role created successfully !";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Roles/Edit/5
        public ActionResult Edit(string roleName)
        {
            var thisRole = context.Roles.Where(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            return View(thisRole);
        }
        //
        // POST: /Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Microsoft.AspNet.Identity.EntityFramework.IdentityRole role)
        {

            string Nameraw = role.Name;
            if (string.IsNullOrEmpty(Nameraw))
            {
                ViewBag.ResultMessage = "Role Is empty  !";
                return RedirectToAction("Index");
            }
            try
            {
                context.Entry(role).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Roles/Delete/5
        public ActionResult Delete(string roleName)
        {
            var thisRole = context.Roles.Where(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            return View(thisRole);
        }
        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string roleName)
        {
            var thisRole = context.Roles.Where(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            context.Roles.Remove(thisRole);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        /// /////////////////
        public ActionResult ManageUserRoles(string usern)
        {
            // prepopulat roles for the view dropdown
            if (string.IsNullOrWhiteSpace(usern))
            {

                var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
                new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                ViewBag.Roles = list;
            }
            else
            {
                ApplicationUser user = context.Users.Where(u => u.UserName.Equals(usern, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                ViewBag.username = user.UserName.ToString();

                var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                ViewBag.Roles = list;

            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAddToUser(string UserName, string RoleName)
        {
            if (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(RoleName))
            {

                ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(UserName))
                {
                    UserManager.AddToRole(user.Id, RoleName);
                    context.SaveChanges();
                    ViewBag.ResultMessage = "Role created successfully !";
                    // prepopulat roles for the view dropdown
                    var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                    ViewBag.Roles = list;
                }
                else
                {
                    ViewBag.ResultMessage = "User Not Found  !";
                    var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                    ViewBag.Roles = list;

                }

            }
            else
            {
                var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
                new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                ViewBag.Roles = list;

            }
            return View("ManageUserRoles");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetRoles(string UserName)
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                ViewBag.RolesForThisUser = UserManager.GetRoles(user.Id);
                // prepopulat roles for the view dropdown
                var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                ViewBag.Roles = list;
            }
            else
            {
                var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
                new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                ViewBag.Roles = list;
            }
            return View("ManageUserRoles");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleForUser(string UserName, string RoleName)
        {
             var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
              new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            if (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(RoleName))
            {
                ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                if (string.IsNullOrWhiteSpace(RoleName))
                {
                    ViewBag.ResultMessage = "Select Role";
                    return View("ManageUserRoles");
                }
                if (UserManager.IsInRole(user.Id, RoleName))
                {
                    //account.UserManager.RemoveFromRole(user.Id, RoleName);
                    UserManager.RemoveFromRole(user.Id, RoleName);
                    context.SaveChanges();
                    ViewBag.ResultMessage = "Role removed from this user successfully !";
                }
                else
                {
                    ViewBag.ResultMessage = "This user doesn't belong to selected role.";
                  }
            }
            return View("ManageUserRoles");
        }
        /////////////////////////////////////////////////
    }
}
