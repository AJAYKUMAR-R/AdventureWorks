using Adventure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//This is for the IdentityConfig
builder.Services.AddDbContext<IdentityCoreDBContext>(options =>
{
    var connString = builder.Configuration["ConnectionStrings:Default"];
    options.UseSqlServer(connString);
});

//This is for the user,role config you want to add the extra properties 
//Ishould inherit to add the property
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<IdentityCoreDBContext>();

// Add services to the container.

//enabling the service for the controller
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
