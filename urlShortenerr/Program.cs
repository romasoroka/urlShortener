using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using urlShortener.Data;

var builder = WebApplication.CreateBuilder(args);

// ������� �������� ���� ����� (UrlShortenerDbContext)
builder.Services.AddDbContext<UrlShortenerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ������������ Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<UrlShortenerDbContext>()
    .AddDefaultTokenProviders();  // ���� �������� ������ (���������, ��� ���������� ������)

// ������� ���������� � ��������� (MVC)
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Middleware ��� �������������� �� �����������
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
