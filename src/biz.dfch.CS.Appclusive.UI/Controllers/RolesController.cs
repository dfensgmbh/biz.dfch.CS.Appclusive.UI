using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;
using System.Data.Services.Client;
using System.Diagnostics.Contracts;
using biz.dfch.CS.Appclusive.UI.App_LocalResources;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class RolesController : CoreControllerBase<Api.Core.Role, Models.Core.Role, object>
    {
        protected override DataServiceQuery<Api.Core.Role> BaseQuery { get { return CoreRepository.Roles; } }

        #region Role

        // GET: Roles/Details/5
        public ActionResult Details(long id, string rId = "0", string rAction = null, string rController = null)
        {
            ViewBag.ReturnId = rId;
            ViewBag.ReturnAction = rAction;
            ViewBag.ReturnController = rController;
            try
            {
                var item = CoreRepository.Roles.Expand("Permissions").Expand("Users").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.Role>(item));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.Role());
            }
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            return View(new Models.Core.Role());
        }

        // POST: Roles/Create
        [HttpPost]
        public ActionResult Create(Models.Core.Role role)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(role);
                }
                else
                {
                    var apiItem = AutoMapper.Mapper.Map<Api.Core.Role>(role);

                    CoreRepository.AddToRoles(apiItem);
                    CoreRepository.SaveChanges();

                    return RedirectToAction("Details", new { id = apiItem.Id });
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(role);
            }
        }

        // GET: Roles/Edit/5
        public ActionResult Edit(long id)
        {
            try
            {
                var apiItem = CoreRepository.Roles.Expand("Permissions").Expand("Users").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.Role>(apiItem));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.Role());
            }
        }

        // POST: Roles/Edit/5
        [HttpPost]
        public ActionResult Edit(long id, Models.Core.Role role)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(role);
                }
                else
                {
                    var apiItem = CoreRepository.Roles.Expand("Permissions").Expand("Users").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();

                    #region copy all edited properties

                    apiItem.Name = role.Name;
                    apiItem.Description = role.Description;
                    apiItem.MailAddress = role.MailAddress;
                    apiItem.RoleType = role.RoleType;

                    #endregion
                    CoreRepository.UpdateObject(apiItem);
                    CoreRepository.SaveChanges();
                    ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, GeneralResources.SuccessfullySaved));
                    return View(AutoMapper.Mapper.Map<Models.Core.Role>(apiItem));
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(role);
            }
        }

        // GET: Roles/Delete/5
        public ActionResult Delete(long id)
        {
            Api.Core.Role apiItem = null;
            try
            {
                apiItem = CoreRepository.Roles.Expand("Permissions").Expand("Users").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Index", new { d = id });
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View("Details", AutoMapper.Mapper.Map<Models.Core.Role>(apiItem));
            }
        }


        #endregion

        #region Permissions

        public PartialViewResult RemovePermission(long roleId, int permissionId)
        {
            Contract.Requires(roleId > 0);
            Contract.Requires(permissionId > 0);
            ViewBag.AjaxCall = true;
            ViewBag.Id = roleId;
            Api.Core.Role role = null;
            try
            {
                role = CoreRepository.Roles.Expand("Permissions").Where(c => c.Id == roleId).FirstOrDefault();
                Contract.Assert(null != role);
                Contract.Assert(null != role.Permissions);
                Contract.Assert(role.Permissions.Where(p => p.Id == permissionId).Count() == 1, ErrorResources.userNotExist);

                var user = CoreRepository.Permissions.Where(c => c.Id == permissionId).FirstOrDefault();
                Contract.Assert(null != user);

                role.Permissions.Remove(user);
                this.CoreRepository.DeleteLink(role, "Permissions", user);
                this.CoreRepository.SaveChanges();
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, ErrorResources.userRemoved));

                return PartialView("PermissionList", AutoMapper.Mapper.Map<List<Models.Core.Permission>>(role.Permissions));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                if (role != null)
                {
                    return PartialView("PermissionList", AutoMapper.Mapper.Map<List<Models.Core.Permission>>(role.Permissions));
                }
                else
                {
                    return PartialView("PermissionList", AutoMapper.Mapper.Map<List<Models.Core.User>>(new List<Models.Core.Permission>()));
                }
            }
        }

        public PartialViewResult AddPermission(long roleId, int permissionId)
        {
            Contract.Requires(roleId > 0);
            Contract.Requires(permissionId > 0);
            ViewBag.AjaxCall = true;
            ViewBag.Id = roleId;
            Api.Core.Role role = null;
            try
            {
                role = CoreRepository.Roles.Expand("Permissions").Where(c => c.Id == roleId).FirstOrDefault();
                Contract.Assert(null != role);
                Contract.Assert(null != role.Permissions);
                Contract.Assert(role.Permissions.Where(p => p.Id == permissionId).Count() == 0, ErrorResources.permissionAlreadyAdded);

                var permission = CoreRepository.Permissions.Where(c => c.Id == permissionId).FirstOrDefault();
                Contract.Assert(null != permission);

                role.Permissions.Add(permission); 
                this.CoreRepository.AddLink(role, "Permissions", permission);
                this.CoreRepository.SaveChanges();
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, ErrorResources.permissionAdded));

                System.Web.HttpContext.Current.Cache.Remove("role_" + roleId);
                return PartialView("PermissionList", AutoMapper.Mapper.Map<List<Models.Core.Permission>>(role.Permissions));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                if (role != null)
                {
                    return PartialView("PermissionList", AutoMapper.Mapper.Map<List<Models.Core.Permission>>(role.Permissions));
                }
                else
                {
                    return PartialView("PermissionList", AutoMapper.Mapper.Map<List<Models.Core.User>>(new List<Models.Core.Permission>()));
                }
            }
        }

        public ActionResult PermissionSearch(long roleId, string term)
        {
            string cacheKeyPermissions = "permission_options_role_" + roleId;
            List<AjaxOption> options = (List<AjaxOption>)System.Web.HttpContext.Current.Cache.Get(cacheKeyPermissions);
            if (null == options)
            {
                options = new List<AjaxOption>();

                string cacheKeyRole = "role_" + roleId;
                Api.Core.Role role = GetCachedRole(roleId);
                foreach (var perm in CoreRepository.Permissions)
                {
                    if (role == null || role.Permissions == null || role.Permissions.Count == 0 
                        || role.Permissions.Where(p => p.Id == perm.Id).Count() == 0)
                    {
                        options.Add(new AjaxOption(perm.Id, perm.Name));
                    }
                }
                System.Web.HttpContext.Current.Cache.Add(cacheKeyPermissions, options, null, DateTime.Now.AddSeconds(30), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
            }
            return this.Json(options.Where(t => t.value.ToLower().Contains(term.ToLower())), JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Users

        public PartialViewResult RemoveUser(long roleId, int userId)
        {
            Contract.Requires(roleId > 0);
            Contract.Requires(userId > 0);
            ViewBag.AjaxCall = true;
            ViewBag.Id = roleId;
            Api.Core.Role role = null;
            try
            {
                role = CoreRepository.Roles.Expand("Users").Where(c => c.Id == roleId).FirstOrDefault();
                Contract.Assert(null != role);
                Contract.Assert(null != role.Users);
                Contract.Assert(role.Users.Where(p => p.Id == userId).Count() == 1, ErrorResources.userNotExist);

                var user = CoreRepository.Users.Where(c => c.Id == userId).FirstOrDefault();
                Contract.Assert(null != user);

                role.Users.Remove(user); 
                this.CoreRepository.DeleteLink(role, "Users", user);
                this.CoreRepository.SaveChanges();
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, ErrorResources.userRemoved));

                return PartialView("UserList", AutoMapper.Mapper.Map<List<Models.Core.User>>(role.Users));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                if (role != null)
                {
                    return PartialView("UserList", AutoMapper.Mapper.Map<List<Models.Core.User>>(role.Users));
                }
                else
                {
                    return PartialView("UserList", AutoMapper.Mapper.Map<List<Models.Core.User>>(new List<Models.Core.User>()));
                }
            }
        }

        public PartialViewResult AddUser(long roleId, int userId)
        {
            Contract.Requires(roleId > 0);
            Contract.Requires(userId > 0);
            ViewBag.AjaxCall = true;
            ViewBag.Id = roleId;

            Api.Core.Role role = null;
            try
            {
                role = CoreRepository.Roles.Expand("Users").Where(c => c.Id == roleId).FirstOrDefault();
                Contract.Assert(null != role);
                Contract.Assert(null != role.Users);
                Contract.Assert(role.Users.Where(p => p.Id == userId).Count() == 0, ErrorResources.userAlreadyAdded);

                var user = CoreRepository.Users.Where(c => c.Id == userId).FirstOrDefault();
                Contract.Assert(null != user);

                role.Users.Add(user); // because auf caching bug in ServiceContext
                this.CoreRepository.AddLink(role, "Users", user);
                this.CoreRepository.SaveChanges();
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, ErrorResources.userAdded));

                System.Web.HttpContext.Current.Cache.Remove("role_" + roleId);
                return PartialView("UserList", AutoMapper.Mapper.Map<List<Models.Core.User>>(role.Users));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                if (role != null)
                {
                    return PartialView("UserList", AutoMapper.Mapper.Map<List<Models.Core.User>>(role.Users));
                }
                else
                {
                    return PartialView("UserList", AutoMapper.Mapper.Map<List<Models.Core.User>>(new List<Models.Core.User>()));
                }
            }
        }

        public ActionResult UserSearch(long roleId, string term)
        {
            string cacheKeyUsers = "user_options_role_" + roleId;
            List<AjaxOption> options = (List<AjaxOption>)System.Web.HttpContext.Current.Cache.Get(cacheKeyUsers);
            if (null == options)
            {
                options = new List<AjaxOption>();
                Api.Core.Role role = GetCachedRole(roleId);
                foreach (var user in CoreRepository.Users)
                {
                    if (role == null || role.Users == null || role.Users.Count == 0
                        || role.Users.Where(p => p.Id == user.Id).Count() == 0)
                    {
                        options.Add(new AjaxOption(user.Id, user.Name));
                    }
                }
                System.Web.HttpContext.Current.Cache.Add(cacheKeyUsers, options, null, DateTime.Now.AddSeconds(30), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
            }
            return this.Json(options.Where(t => t.value.ToLower().Contains(term.ToLower())), JsonRequestBehavior.AllowGet);

        }

        #endregion

        private Api.Core.Role GetCachedRole(long roleId)
        {
            string cacheKeyRole = "role_" + roleId;
            Api.Core.Role role = (Api.Core.Role)System.Web.HttpContext.Current.Cache.Get(cacheKeyRole);
            if (null == role)
            {
                role = CoreRepository.Roles.Expand("Permissions").Expand("Users").Where(c => c.Id == roleId).FirstOrDefault();
                System.Web.HttpContext.Current.Cache.Add(cacheKeyRole, role, null, DateTime.Now.AddSeconds(30), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
            }
            return role;
        }

    }
}
