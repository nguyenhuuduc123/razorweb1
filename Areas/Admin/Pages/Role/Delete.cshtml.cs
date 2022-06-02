using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using razorweb.models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.Admin.Role
{
    public class DeleteModel : RolePageModels
    {
        public DeleteModel(RoleManager<IdentityRole> rolemanager, MyBlogContext context) : base(rolemanager, context)
        {
        }
        
        [BindProperty(SupportsGet =true)]
        public IdentityRole role {get;set;}
        public async Task<IActionResult> OnGet(string roleid)
        {
            if(roleid == null) { 
              return NotFound("không tìm thấy role");}

        var role =  await _rolemanager.FindByIdAsync(roleid);
         ViewData["rolename"] = role.Name;
        if(role == null){
            return NotFound("không tìm thấy role");
        }
              return Page();
        }
        
       public  async Task<IActionResult> OnPostAsync(string roleid){
        if(roleid == null) return NotFound("không tìm thấy role");
        role = await _rolemanager.FindByIdAsync(roleid);
     var result =  await _rolemanager.DeleteAsync(role);
        if(result.Succeeded){
            StatusMessage = $"bạn vừa xóa tên {role.Name}";
            return RedirectToPage("./Index");
        }
        else {
            result.Errors.ToList().ForEach(e => ModelState.AddModelError(string.Empty,e.Description));  
        }
               return Page();
     }
     
    }
}
