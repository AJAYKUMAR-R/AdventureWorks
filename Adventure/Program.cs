using Adventure.Data;
using Adventure.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

//This is for the IdentityConfig
builder.Services.AddDbContext<IdentityCoreDBContext>(options =>
{
    var connString = builder.Configuration["ConnectionStrings:Default"];
    options.UseSqlServer(connString);
});

//This is for the user,role config you want to add the extra properties 
//I should inherit to add the property
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<IdentityCoreDBContext>();

// Add services to the container.

//enabling the service for the controller
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//This will help you validate it in the api level for access token
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true, // Ensures the token has not expired
        ValidateIssuerSigningKey = true, // Validates the signing key
        ValidIssuer = "YourIssuer",
        ValidAudience = "YourAudience",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecretKeyHere"))
    };
});

builder.Services.Configure<IdentityOptions>(options =>
{
        options.Password.RequiredLength = 5;
        options.Password.RequireDigit = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Lockout.MaxFailedAccessAttempts = 3;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(30);
        options.SignIn.RequireConfirmedEmail = false;
});



var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();


// Enable routing
app.UseRouting();


//map the controller 
app.MapControllers();

app.Run();
