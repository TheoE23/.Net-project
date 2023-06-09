using Bookshop_Project.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Bookshop_Project.Data;
using Bookshop_Project.Services;

var builder = WebApplication.CreateBuilder(args);

var mvcBuilder = builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<User>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<BookDataEndpointOptions>(builder.Configuration.GetSection(nameof(BookDataEndpointOptions)));

if (builder.Environment.IsDevelopment())
{
    mvcBuilder.AddRazorRuntimeCompilation();
}

builder.Services.AddHttpClient(HttpClientNames.BookProviderClient);

builder.Services.AddHttpClient<OpenLibraryService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
    dbContext.Database.Migrate();

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var adminUser = await userManager.FindByNameAsync("admin");
    if (adminUser == null)
    {
        adminUser = new User
        {
            UserName = "admin",
            Email = "admin@abv.bg",
        };

        var password = "123456";
        adminUser.PasswordHash = userManager.PasswordHasher.HashPassword(adminUser, password);

        await userManager.CreateAsync(adminUser);
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}

app.UseCookiePolicy();
app.UseAuthentication();

app.Run();
