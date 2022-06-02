using App.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using razorweb.models;


var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;
builder.Services.AddOptions();
var mailsetting = configuration.GetSection("MailSettings");
builder.Services.Configure<MailSettings>(mailsetting);

builder.Services.AddSingleton< IEmailSender,SendMailService>();
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<MyBlogContext>(options =>
{
    string connectstring = configuration.GetConnectionString("MyBlogContext");
    options.UseSqlServer(connectstring);

});
// dang ky identity
builder.Services.AddIdentity<AppUser, IdentityRole>().
AddEntityFrameworkStores<MyBlogContext>().
AddDefaultTokenProviders();
// builder.Services.AddDefaultIdentity<AppUser>().
// AddEntityFrameworkStores<MyBlogContext>();
builder.Services.AddSingleton<IdentityErrorDescriber, AppIdentityErrorDescriber>();
builder.Services.Configure<IdentityOptions> (options => {
    // Thiết lập về Password
    options.Password.RequireDigit = false; // Không bắt phải có số
    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
    options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
    options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt
    // Cấu hình Lockout - khóa user
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes (5); // Khóa 5 phút
    options.Lockout.MaxFailedAccessAttempts = 3; // Thất bại 3 lần thì khóa
    options.Lockout.AllowedForNewUsers = true;

    // Cấu hình về User.
    options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;  // Email là duy nhất
    // Cấu hình đăng nhập.
    options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
    options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
    options.SignIn.RequireConfirmedAccount = false; // xác thực email trước khi đăng nhập
});
builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = "/login/";
    options.LogoutPath = "/logout/";
    options.AccessDeniedPath = "/khongduoctruycap.html";
});
builder.Services.AddAuthentication().
 AddGoogle(options => {
   var  ggcofig =  configuration.GetSection("Authentication:Google");
     options.ClientId = ggcofig["ClientId"];
     options.ClientSecret = ggcofig["ClientSecret"];
     // địa chỉ mặc định của CallbackPath là signin-google
     options.CallbackPath = "/dang-nhap-tu-google";
 });
var app = builder.Build();
// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//{"web":{"client_id":"454602534685-gp7idibfo7arr3pgkulbf4q0ul6jckr9.apps.googleusercontent.com","project_id":"nguyenhuuducasp","auth_uri":"https://accounts.google.com/o/oauth2/auth","token_uri":"https://oauth2.googleapis.com/token","auth_provider_x509_cert_url":"https://www.googleapis.com/oauth2/v1/certs","client_secret":"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","redirect_uris":["https://localhost:7171/dang-nhap-tu-google"]}}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
//  xác định danh tính
app.UseAuthentication();
// xác định quyền truy cập
app.UseAuthorization();
app.MapRazorPages();
app.Run();


/*
dotnet aspnet-codegenerator razorpage -m razorweb.models.Article -dc razorweb.models.MyBlogContext -outDir Areas/Identity/Pages -udl --referenceScriptLibraries
tạo nhanh trang asp : dotnet new page -n Create -o Areas/Admin/Pages/Role -na App.Admin.Role
*/
