using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore;
using razorweb.models;
namespace App.Admin.Role
{
   [Authorize(Roles = "Admin")]
    public class IndexModel : RolePageModels
    {
        public IndexModel(RoleManager<IdentityRole> rolemanager, MyBlogContext context) : base(rolemanager, context)
        {
        }
        public List<IdentityRole> roles;

        public async Task OnGet()
        {
          roles = await  _rolemanager.Roles.OrderByDescending(r => r.Name ).ToListAsync();}

        public void OnPost() => RedirectToPage();

        
    }
}
