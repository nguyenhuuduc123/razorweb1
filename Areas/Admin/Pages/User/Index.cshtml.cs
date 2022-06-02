using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razorweb.models;
namespace App.Admin.User
{
    public class IndexModel : PageModel
    {
      private readonly UserManager<AppUser> _usermanager ;
      
        public IndexModel(UserManager<AppUser> usermanager) 
        {
          _usermanager = usermanager;

        }
        [TempData]
        public string StatusMessage {set;get;}
        public class UserAndRole : AppUser {
          public string RoleNames {get;set;}
        }
        public List<UserAndRole> users{set;get;}

        public const int Items_per_page = 10;
        [BindProperty(SupportsGet =true, Name = "p")]
        public int currentPage {get;set;}
        public int countPages {get;set;}
        public int totalUsers {get;set;}
        public async Task OnGet()
        {
          //users = await  _usermanager.Users.OrderByDescending(u => u.Id ).ToListAsync();
          var qr =  _usermanager.Users.OrderBy(u => u.UserName);

             totalUsers = await qr.CountAsync();
                countPages =(int)Math.Ceiling((double)totalUsers/Items_per_page);
                if(currentPage <1 ){
                    currentPage = 1;
                }
                if(currentPage > countPages){
                    currentPage = countPages;
                }
                var qr1 = qr.Skip((currentPage-1)*Items_per_page).Take(Items_per_page).Select(u => new UserAndRole(){
                  Id = u.Id,
                  UserName = u.UserName,
                });

                users = await qr1.ToListAsync();
              foreach (var user in users)
              {
                var roles = await _usermanager.GetRolesAsync(user);
                  user.RoleNames = string.Join(",",roles);
              }
          }

        public void OnPost() => RedirectToPage();

        
    }
}
