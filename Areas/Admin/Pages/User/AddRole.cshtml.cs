// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using razorweb.models;

namespace App.Admin.User
{
    public class AddRoleModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly  RoleManager<IdentityRole> _rolemanager;

        public AddRoleModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> rolemanager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _rolemanager = rolemanager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
       
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public AppUser user{set;get;}
        
        [BindProperty]
        [DisplayName("các role gán cho user")]

        public string[]  RoleNames {get;set;}
        public SelectList allRoles {get;set;}
         public async Task<IActionResult> OnGetAsync(string id)
        {
            if(string.IsNullOrEmpty(id)){
                return NotFound("không tìm thấy user");
            }

            var user = await _userManager.FindByIdAsync(id);
            ViewData["GetUserNameById"] = user.UserName;
            if (user == null)
            {
                return NotFound($"không thấy user với id bằng  '{id}'.");
            }
            
         RoleNames =   (await _userManager.GetRolesAsync(user)).ToArray<string>();

        // lấy danh sách tên của các role
        List<string> roleNames = await _rolemanager.Roles.Select(r => r.Name).ToListAsync();
         allRoles = new SelectList(roleNames);
        return Page();
            
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if(string.IsNullOrEmpty(id)){
                return NotFound("không tìm thấy user");
            }

            var user = await _userManager.FindByIdAsync(id);
            ViewData["GetUserNameById"] = user.UserName;
            if (user == null)
            {
                return NotFound($"không thấy user với id bằng  '{id}'.");
            }
            // rolename
            var OldRoleNames = (await _userManager.GetRolesAsync(user)).ToArray();
            var deleteRoles  = OldRoleNames.Where(r => !RoleNames.Contains(r));
            var addRoles = RoleNames.Where(r => !OldRoleNames.Contains(r));
             List<string> roleNames = await _rolemanager.Roles.Select(r => r.Name).ToListAsync();
         allRoles = new SelectList(roleNames);
           var resultDelete =  await _userManager.RemoveFromRolesAsync(user,deleteRoles);
         if(!resultDelete.Succeeded){
                resultDelete.Errors.ToList().ForEach(error => {
                    ModelState.AddModelError(string.Empty,error.Description);
                });
                return Page();
         }
          var resultAdd =  await _userManager.AddToRolesAsync(user,addRoles);
         if(!resultAdd.Succeeded){
                resultDelete.Errors.ToList().ForEach(error => {
                    ModelState.AddModelError(string.Empty,error.Description);
                });
                return Page();
          }
            StatusMessage = $"vừa cập nhập role cho use {user.UserName}";

            return RedirectToPage("./Index");
        }
    }
}
