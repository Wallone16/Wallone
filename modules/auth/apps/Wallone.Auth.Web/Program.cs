using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using Wallone.Auth.Services;
using Wallone.Auth.Services.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddControllers();

//builder.Services.AddTransient<ClientSeeder>();

builder.Services
    .AddAuthServices(builder.Configuration);

//builder.Services.Configure<SwaggerClientSettings>(
//    builder.Configuration.GetSection(nameof(SwaggerClientSettings)));
//builder.Services.Configure<WebScopeSettings>(
//builder.Configuration.GetSection(nameof(WebScopeSettings)));
builder.Services.Configure<SaltOptions>(
    builder.Configuration.GetSection(nameof(SaltOptions)));
//builder.Services.Configure<ReactClientSettings>(
//    builder.Configuration.GetSection(nameof(ReactClientSettings)));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddOpenIddict()
    .AddServer(options =>
    {
        options
            .SetAuthorizationEndpointUris("connect/authorize")
            .SetLogoutEndpointUris("connect/logout")
            .SetTokenEndpointUris("connect/token");

        options
            .AllowClientCredentialsFlow()
            .AllowAuthorizationCodeFlow();

        options
            .AddDevelopmentEncryptionCertificate()
            .AddDevelopmentSigningCertificate();

        options.AddEncryptionKey(new SymmetricSecurityKey(
            Convert.FromBase64String(builder.Configuration["OpenIddict:SecurityKey"]!)));

        options
            .UseAspNetCore()
            .EnableLogoutEndpointPassthrough()
            .EnableAuthorizationEndpointPassthrough()
            .EnableTokenEndpointPassthrough();
    })
    .AddValidation(options =>
    {
        options.UseLocalServer();
        options.UseAspNetCore();
    });

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
    });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins("https://localhost:7152", "http://localhost:5173/")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    //var seeder = scope.ServiceProvider.GetRequiredService<ClientSeeder>();
    //seeder.AddScopes().GetAwaiter().GetResult();
    //seeder.AddClient().GetAwaiter().GetResult();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();
app.MapRazorPages();

app.Run();