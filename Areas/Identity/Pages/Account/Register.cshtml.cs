// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using razorweb.models;

namespace apprazor.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserStore<AppUser> _userStore;
        private readonly IUserEmailStore<AppUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<AppUser> userManager,
            IUserStore<AppUser> userStore,
            SignInManager<AppUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>

            [Required(ErrorMessage = "phải nhập {0}")]
            [EmailAddress(ErrorMessage = "sai định dạng email")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "mật khẩu")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "nhập lại mật khẩu")]
            [Compare("Password", ErrorMessage = "mật khẩu lặp lại không chính xác")]
            public string ConfirmPassword { get; set; }
            [Display(Name = "tên tài khoản")]
            [Required(ErrorMessage = "phải nhập {0}")]
            [DataType(DataType.Text)]
            [StringLength(100, ErrorMessage = " {0} ohair dài từ {2}cho đến {1} ký tự.", MinimumLength = 3)]
            public string UserName { set; get; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            // kiểm tra có dịch vụ ngoài không
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            foreach(var provider in ExternalLogins){
                _logger.LogInformation(provider.Name);
            }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            // trang chủ
            returnUrl ??= Url.Content("~/");
            // dịch vụ ngoài như là google , facebook
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
           // nếu tất cả dữ liệu đăng ký hợp lệ
            if (ModelState.IsValid)
            {
                
                //var user = CreateUser();
              //  await _userStore.SetUserNameAsync(user, Input.UserName, CancellationToken.None);
              //  await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
             
             // tạo ra user với tên đăng nhập và email đã submit
               var user = new AppUser(){
                   UserName = Input.UserName,
                   Email = Input.Email
               };
               // tạo ra user với mật khẩu  ( trả về true nếu thành công)
                var result = await _userManager.CreateAsync(user, Input.Password);
                    // nếu tạo thành công
                if (result.Succeeded)
                {
                    _logger.LogInformation("đã tạo user mới");
                    // lấy id của User
                    var userId = await _userManager.GetUserIdAsync(user);
                    // phát sinh token để xác nhận email
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    // encode để đính kèm mã trên url
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    // https://7171/identity/account/confirmEmail/?Userid=dsffd&code=dsfdg&
                    var callbackUrl = Url.Page(
                       pageName: "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);
                    // gửi email , gọi đến trang từ đường dẫn call back url 
                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private AppUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<AppUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(AppUser)}'. " +
                    $"Ensure that '{nameof(AppUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<AppUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<AppUser>)_userStore;
        }
    }
}
