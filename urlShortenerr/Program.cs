using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using urlShortener.Data;

var builder = WebApplication.CreateBuilder(args);

// Додайте контекст бази даних (UrlShortenerDbContext)
builder.Services.AddDbContext<UrlShortenerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Налаштування Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<UrlShortenerDbContext>()
    .AddDefaultTokenProviders();  // Додає підтримку токенів (наприклад, для відновлення пароля)

// Додайте контролери з поданнями (MVC)
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

// Middleware для автентифікації та авторизації
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
