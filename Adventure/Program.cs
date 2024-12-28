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
       ValidateIssuer = true,
       ValidateAudience = true,
       ValidateLifetime = true, // Ensures the token has not expired
       ValidateIssuerSigningKey = true, // Validates the signing key
       ClockSkew = TimeSpan.FromSeconds(30), //This sets the time tolerence between the server and client if client is 30 sec ahead it will adjust it
       ValidIssuer = "http://localhost:5289",
       ValidAudience = "http://localhost:5289", //this will be update once the front-end has been designed
       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecretKeyHere"))
    };

    //This is for loggin the whether the jwt token get failed or not to debug the exception message
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.Clear();
            Console.WriteLine($"Authentication failed: {context.Exception.Message}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.Clear();
            Console.WriteLine($"Token validated: {context.SecurityToken}");
            return Task.CompletedTask;
        }
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

//Note : if you did not add the authentcation and Authorizaton middleware [Authorize] attribute would not work
app.UseAuthentication();

// Enable routing
app.UseRouting();

//This should be placed here to validate the authorization token
app.UseAuthorization();



//map the controller 
app.MapControllers();

app.Run();
