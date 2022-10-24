using Microsoft.AspNetCore.Authorization;
using WebApp_UnderTheHood.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

const string myCookieAuth = "MyCookieAuth";
const string loginPath = "/Account/Login";
const string accessDeniedPath = "/Account/AccessDenied";
const string mustBelongToHRDepartment = "MustBelongToHRDepartment";
const string adminOnly = "AdminOnly";
const string hRManagerOnly = "HRManagerOnly";

builder.Services.AddAuthentication(myCookieAuth).AddCookie(myCookieAuth, options =>
{
    options.Cookie.Name = myCookieAuth;
    options.LoginPath = loginPath;
    options.AccessDeniedPath = accessDeniedPath;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
});

builder.Services.AddSingleton<IAuthorizationHandler, HRManagerProbationRequirementHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(adminOnly, policy =>
        policy.RequireClaim("Admin")
    );
    options.AddPolicy(mustBelongToHRDepartment, policy =>
        policy.RequireClaim("Department", "HR"));
    options.AddPolicy(hRManagerOnly, policy =>
        policy.RequireClaim("Department", "HR")
              .RequireClaim("Manager")
              .Requirements.Add(new HRManagerProbationRequirement(3)));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
