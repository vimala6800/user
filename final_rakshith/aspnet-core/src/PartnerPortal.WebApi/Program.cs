
using Microsoft.Extensions.DependencyInjection;
using PartnerPortal.Infrastructure.Persistence;
using PartnerPortal.WebApi.Services;
using PartnerPortal.WebApi.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PartnerPortal.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using PartnerPortal.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PartnerPortal.Domain.Entities.Models;
using System.Text;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// For Entity Framework
var configuration = builder.Configuration;
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("Default")));

// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
//Add Config for Required Email
builder.Services.Configure<IdentityOptions>(
    opts => opts.SignIn.RequireConfirmedEmail = true
    );

// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    //options.TokenValidationParameters = new TokenValidationParameters()
    //{
    //    ValidateIssuer = true,
    //    ValidateAudience = true,
    //    ValidAudience = configuration["JWT:ValidAudience"],
    //    ValidIssuer = configuration["JWT:ValidIssuer"],
    //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    //};
});


//Add Email Configs
var emailConfig1 = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig1);

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddControllers();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebApiServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
var emailConfig = builder.Configuration.GetSection("MailSettings").Get<MailSettings>();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<PartnerPortal.WebApi.Services.IMailService, MailService>();
builder.Services.AddScoped<MailRequest, MailRequest>();
builder.Services.AddControllers();
builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
    opt.TokenLifespan = TimeSpan.FromDays(1));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();

    // Initialise and seed database
    using (var scope = app.Services.CreateScope())
    {
        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
        await initialiser.InitialiseAsync();
        await initialiser.SeedAsync();
    }
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors(policy=>policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseOpenApi();
app.UseSwaggerUi3();

app.UseRouting();
app.UseAuthentication();
app.UseIdentityServer();
app.UseAuthorization();
//app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
