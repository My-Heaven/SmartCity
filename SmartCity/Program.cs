using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SmartCity.Entities;
using SmartCity.Models;
using SmartCity.Services.AccountServices;
using SmartCity.Services.BookingdetailService;
using SmartCity.Services.BookingDetailService;
using SmartCity.Services.BookingService;
using SmartCity.Services.CommitService;
using SmartCity.Services.EmployeeServices;
using SmartCity.Services.ServiceServices;
using SmartCity.Services.SkillService;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//add Booking Deatil service
builder.Services.AddTransient<IBookingDetailService, BookingDetailService>();
//add Booking service
builder.Services.AddTransient<IBookingService, BookingService>();
//add Commit service
builder.Services.AddTransient<ICommitService, CommitService>();
//add Skill service
builder.Services.AddTransient<ISkillService, SkillService>();
//add Account service
builder.Services.AddTransient<IAccountService, AccountService>();
//add Employee service
builder.Services.AddTransient<IEmployeeService, EmployeeService>();
//add Service service
builder.Services.AddTransient<IServiceService, ServiceService>();
//add database connection string service
builder.Services.AddDbContext<SmartCityContext>
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("SmartCity")));

//add authentication service
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

//builder.Services.AddSwaggerGenNewtonsoftSupport();

builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    // Set Description Swagger
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SmartCity",
        Version = "v1",
        Contact = new OpenApiContact()
        {
            Name = "SmartCity"
        }

    });
    //c.OrderActionsBy((apiDesc) => $"{apiDesc.RelativePath}");

    c.DescribeAllParametersInCamelCase();
    // Set the comments path for the Swagger JSON and UI.
    /*                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);*/

    // c.SchemaFilter<EnumSchemaFilter>();

    // Set Authorize box to swagger
    var jwtSecuriyScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Put **_ONLY_** your token on textbox below!\n Name = Authentication",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(jwtSecuriyScheme.Reference.Id, jwtSecuriyScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {jwtSecuriyScheme, Array.Empty<string>()}
                });


});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartCity.WebAPI v1");
    c.RoutePrefix = string.Empty;
});
//}

app.UseCors(x => x
                //.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
