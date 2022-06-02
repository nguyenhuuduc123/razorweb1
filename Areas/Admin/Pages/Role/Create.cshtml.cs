using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using razorweb.models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.Admin.Role
{
    public class CreateModel : RolePageModels
    {
        public CreateModel(RoleManager<IdentityRole> rolemanager, MyBlogContext context) : base(rolemanager, context)
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
        public void OnGet()
        {
        }
        
       public  async Task<IActionResult> OnPostAsync(){

         if(!ModelState.IsValid){
            return Page();
         }
          var newRole  = new IdentityRole(input.Name);
     var result  =  await _rolemanager.CreateAsync(newRole);
     if(result.Succeeded){
      StatusMessage = $"bạn vừa tạo role mới {input.Name}";
          return RedirectToPage("./Index");
     }  
    else{
         result.Errors.ToList().ForEach(error => {
                ModelState.AddModelError(string.Empty,error.Description);              
         });         
     }
     return Page();
               
     }
     
    }
}
