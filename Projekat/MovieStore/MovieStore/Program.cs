using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieStore.Areas.Identity.Data;

void CreateRoles([FromServices] IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    Task<IdentityResult> roleResult;

    Task<bool> hasAdminRole = roleManager.RoleExistsAsync("Administrator");
    hasAdminRole.Wait();

    Task<bool> hasEmployeeRole = roleManager.RoleExistsAsync("Employee");
    hasEmployeeRole.Wait();

    Task<bool> hasUserRole = roleManager.RoleExistsAsync("User");
    hasUserRole.Wait();

    // Creating Roles if they don't exist
    if (!hasAdminRole.Result)
    {
        var admin = new IdentityRole("Administrator");
        roleResult = roleManager.CreateAsync(admin);
        roleResult.Wait();
    }

    if (!hasEmployeeRole.Result)
    {
        var employee = new IdentityRole("Employee");
        roleResult = roleManager.CreateAsync(employee);
        roleResult.Wait();
    }

    if (!hasUserRole.Result)
    {
        var user = new IdentityRole("User");
        roleResult = roleManager.CreateAsync(user);
        roleResult.Wait();
    }
}

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorization(options => 
{
    options.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("Administrator"));
    options.AddPolicy("EmployeeRolePolicy", policy => policy.RequireRole("Employee"));
    options.AddPolicy("UserRolePolicy", policy => policy.RequireRole("User"));

    options.AddPolicy("AddDeleteRolePolicy", policy => policy.RequireClaim("Add Role").RequireClaim("Delete Role"));
});

// Add services to the container.
builder.Services.AddControllersWithViews();
var sc = builder.Services.AddHttpContextAccessor();
IServiceProvider serviceProvider = sc.BuildServiceProvider();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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

CreateRoles(serviceProvider);
app.MapRazorPages();
app.Run();
