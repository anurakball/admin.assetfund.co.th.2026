using AngleSharp.Io;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;
using Serilog.Events;

// UseSerilog() below replaces the default logging providers, console sink included.
// Without one the app writes nothing to stdout, so `dotnet watch run` never sees
// Kestrel's "Now listening on: <url>" line -- the line it parses to learn which URL
// to open -- and the browser is never launched (launchBrowser in launchSettings.json
// alone is not enough). Console output only in Development; on the server the file
// sink stays the single destination.
var isDevelopment = string.Equals(
    Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
    "Development",
    StringComparison.OrdinalIgnoreCase);

var loggerConfig = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    // "Now listening on:" / "Application started" live under this category at
    // Information, so it has to opt out of the "Microsoft" -> Warning override above.
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .WriteTo.File(
        path: "Logs/error-.txt",
        restrictedToMinimumLevel: LogEventLevel.Error,
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}");

if (isDevelopment)
{
    loggerConfig = loggerConfig.WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}");
}

Log.Logger = loggerConfig.CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();
// Headroom above the elFinder per-file cap (50 MB, see FileSystemController.GetConnector)
// because elFinder can POST several files plus multipart overhead in one request.
builder.WebHost.ConfigureKestrel(o => o.Limits.MaxRequestBodySize = 80 * 1024 * 1024);
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddMemoryCache();
builder.Services.AddMvc().AddRazorRuntimeCompilation();
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();
builder.Services.AddSession(options =>
{
    // Distinct cookie name so the admin session does not collide with the public
    // front-end app. Cookies are scoped by host only (not port, per RFC 6265), so a
    // shared default name (.AspNetCore.Session) on the same localhost host lets the
    // front-end overwrite the admin session cookie and silently log the admin out.
    options.Cookie.Name = "SAM.Admin.Session";
    options.IdleTimeout = TimeSpan.FromSeconds(60 * 60 * 24);//sec*min*hr
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();

/*
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Admin/Error");
}
*/

app.UseDeveloperExceptionPage();
app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();
app.Use(async (context, next) =>
{
    //context.Response.Headers.Add("X-Frame-Options", "deny");
    context.Response.Headers.Add("X-Xss-Protection", "1;mode = block");
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    //context.Response.Headers.Add("Strict-Transport-Security", "maxage=31536000; includeSubdomains;");
    context.Response.Headers.Add("Referrer-Policy", "no-referrer");
    //context.Response.Headers.Add("Content-Security-Policy","default-src 'self'; object-src 'none'");

    context.Response.Headers.Remove("Server");
    context.Response.Headers.Remove("X-AspNet-Version");
    context.Response.Headers.Remove("X-AspNetMvc-Version");

    context.Response.Headers.Add("Server", "");
    context.Response.Headers.Add("X-AspNet-Version", "");
    context.Response.Headers.Add("X-AspNetMvc-Version", "");

    await next();
});
app.MapControllerRoute(
    name: "AdminArea",
    pattern: "Admin/{controller=User}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
