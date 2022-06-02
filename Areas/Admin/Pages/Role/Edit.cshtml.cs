using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using razorweb.models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.Admin.Role
{
    public class EditModel : RolePageModels
    {
        public EditModel(RoleManager<IdentityRole> rolemanager, MyBlogContext context) : base(rolemanager, context)
        {
        }
        public class InputModel{
            [DisplayName("tên của role")]
            [StringLength(256,ErrorMessage ="ten nhap dai qua")]
            [Required(ErrorMessage = " phải nhập {0}")]
            public string Name {get;set;}

        }
        [BindProperty]
        public InputModel input {get;set;}
        public IdentityRole role {get;set;}
        public async Task<IActionResult> OnGetAsync(string roleid)
        {
            if(roleid == null) { 
              return NotFound("không tìm thấy role");}

        var role =  await _rolemanager.FindByIdAsync(roleid);
        if(role != null){
            input = new InputModel(){
                Name = role.Name,

            };
            return Page();
        }
              return NotFound("không tìm thấy role");
        }
        
       public  async Task<IActionResult> OnPostAsync(string roleid){
        if(roleid == null) return NotFound("không tìm thấy role");
        role = await _rolemanager.FindByIdAsync(roleid);
        if(role == null) return NotFound("không tìm thấy role");
        if(!ModelState.IsValid){
            return Page();
        }
        role.Name = input.Name;
     var result =  await _rolemanager.UpdateAsync(role);
        if(result.Succeeded){
            StatusMessage = $"bạn vừa đổi tên {input.Name}";
            return RedirectToPage("./Index");
        }
        else {
            result.Errors.ToList().ForEach(e => ModelState.AddModelError(string.Empty,e.Description));  
        }
               return Page();
     }
     
    }
}
