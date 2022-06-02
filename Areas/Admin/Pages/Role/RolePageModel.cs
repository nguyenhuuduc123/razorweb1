using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using razorweb.models;

namespace App.Admin.Role{
    public class RolePageModels:PageModel{
        protected readonly RoleManager<IdentityRole> _rolemanager;
        [TempData]
        public string StatusMessage {get;set;}
        protected readonly MyBlogContext _context;
            public RolePageModels(RoleManager<IdentityRole> rolemanager,MyBlogContext context){
                    _rolemanager = rolemanager;
                    _context = context;
            }
    }
}