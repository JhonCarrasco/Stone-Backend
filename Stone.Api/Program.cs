using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Stone.Api.Endpoints;
using Stone.Api.Filters;
using Stone.Entities;
using Stone.Persistence;
using Stone.Repositories;
using Stone.Repositories.Implementation;
using Stone.Repositories.Implements;
using Stone.Repositories.Interface;
using Stone.Services.Implementation;
using Stone.Services.Interface;
using Stone.Services.Profiles;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

//Options pattern register
builder.Services.Configure<AppSettings>(builder.Configuration);

//bool StorageAzure = builder.Configuration.GetValue<bool>("Flags.StorageAzure");


// CORS
var corsConfiguration = "StoneCors";
builder.Services.AddCors(setuo =>
{
    setuo.AddPolicy(corsConfiguration, policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyHeader().WithExposedHeaders(new string[] { "TotalRecordsQuantity" });
        policy.AllowAnyMethod();
    });
});

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(FilterExceptions));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configuring Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"));
});

//Identity
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:JWTKey"] ?? throw new InvalidOperationException("JWT Key not configured"));
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});
builder.Services.AddIdentity<User, IdentityRole>(policies =>
    {
        policies.Password.RequireDigit = true;
        policies.Password.RequiredLength = 6;
        policies.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddHttpContextAccessor();

//Registering services
builder.Services.AddTransient<IGenreRepository, GenreRepository>();
builder.Services.AddTransient<IConcertRepository, ConcertRepository>();
builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
builder.Services.AddTransient<ISaleRepository, SaleRepository>();
builder.Services.AddTransient<IPersonRepository, PersonRepository>();
builder.Services.AddTransient<IContactRepository, ContactRepository>();
builder.Services.AddTransient<ILocationRepository, LocationRepository>();
builder.Services.AddTransient<IBankAccountRepository, BankAccountRepository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<IManufacturerRepository, ManufacturerRepository>();
builder.Services.AddTransient<IBudgetRepository, BudgetRepository>();
builder.Services.AddTransient<IItemizedProductRepository, ItemizedProductRepository>();
builder.Services.AddTransient<IItemizedServiceRepository, ItemizedServiceRepository>();
builder.Services.AddTransient<IBankRepository, BankRepository>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<ICommuneRepository, CommuneRepository>();
builder.Services.AddTransient<IManufacturerRepository, ManufacturerRepository>();
builder.Services.AddTransient<IRegionRepository, RegionRepository>();
builder.Services.AddTransient<ITypeAccountRepository, TypeAccountRepository>();
builder.Services.AddTransient<IUnitMeasurementRepository, UnitMeasurementRepository>();
builder.Services.AddTransient<IProviderRepository, ProviderRepository>();
builder.Services.AddTransient<IMaterialRepository, MaterialRepository>();
builder.Services.AddTransient<IMaterialVoucherRepository, MaterialVoucherRepository>();
builder.Services.AddTransient<IReceptionGuideRepository, ReceptionGuideRepository>();
builder.Services.AddTransient<IDispatchGuideRepository, DispatchGuideRepository>();

builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<IConcertService, ConcertService>();
builder.Services.AddTransient<IGenreService, GenreService>();
builder.Services.AddTransient<ISaleService, SaleService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IFileStorage, FileStorageLocal>();
builder.Services.AddTransient<IPersonService, PersonService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IBudgetService, BudgetService>();
builder.Services.AddTransient<IProviderService, ProviderService>();
builder.Services.AddTransient<ISharedService, SharedService>();
builder.Services.AddTransient<IMaterialService, MaterialService>();
builder.Services.AddTransient<IVoucherService, VoucherService>();
builder.Services.AddTransient<IReceptionGuideService, ReceptionGuideService>();
builder.Services.AddTransient<IDispatchGuideService, DispatchGuideService>();


//if (StorageAzure)
//{
//    builder.Services.AddTransient<IFileStorage, FileStorageAzure>();
//}
//else
//{
//    builder.Services.AddTransient<IFileStorage, FileStorageLocal>();
//}

//Registering healthchecks
builder.Services.AddHealthChecks()
    .AddCheck("selfcheck", () => HealthCheckResult.Healthy())
    .AddDbContextCheck<ApplicationDbContext>();

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<ConcertProfile>();
    config.AddProfile<GenreProfile>();
    config.AddProfile<SaleProfile>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseCors(corsConfiguration);

app.UseStaticFiles();
app.UseDirectoryBrowser();

app.MapReports();
app.MapHomeEndpoints();

app.MapControllers();

//SCOPE
using (var scope = app.Services.CreateScope())
{
    //Auto-migrations
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.MigrateAsync();

    //seed data for the admin default user
    await UserDataSeeder.Seed(scope.ServiceProvider);
}

//Configuring health checks
app.MapHealthChecks("/healthcheck", new()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
