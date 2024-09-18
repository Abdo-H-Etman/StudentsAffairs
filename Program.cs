using BlazorAppServer;
using BlazorAppServer.UnitOfWork;

WebApplicationBuilder? webApplicationBuilder = WebApplication.CreateBuilder(args);
webApplicationBuilder.Services.AddLocalization(options => {
                                                options.ResourcesPath = "Resources";
                                               });
webApplicationBuilder.Services
                     .AddRazorComponents()
                     .AddInteractiveServerComponents();

IConfigurationRoot configuration = new ConfigurationBuilder()
                                        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                        .AddJsonFile("appsettings.json")
                                        .Build();
webApplicationBuilder.Services.AddDbContext<StudentsAffairsDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                .EnableServiceProviderCaching()
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .EnableThreadSafetyChecks());

webApplicationBuilder.Services.AddScoped<IStudentsUnitOfWork, StudentsUnitOfWork>();
webApplicationBuilder.Services.AddScoped<IStudentsRepository, StudentsRepository>();
webApplicationBuilder.Services.AddScoped<IApplicantsRepository, ApplicantsRepository>();
webApplicationBuilder.Services.AddScoped<ITeachersRepository, TeachersRepository>();
webApplicationBuilder.Services.AddScoped<ITeachersUnitOfWork, TeachersUnitOfWork>();


WebApplication? webApplication = webApplicationBuilder.Build();


// Configure the HTTP request pipeline.
if (!webApplication.Environment.IsDevelopment())
{
    webApplication.UseExceptionHandler("/Error", createScopeForErrors: true);

    webApplication.UseHsts();
}

webApplication.UseHttpsRedirection();

webApplication.UseStaticFiles();
webApplication.UseAntiforgery();

webApplication.MapRazorComponents<App>()
              .AddInteractiveServerRenderMode();

webApplication.Run();
