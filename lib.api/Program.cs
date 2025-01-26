using lib.api.Data;
using lib.api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// builder.Services.AddSingleton<IEmailService, EmailService>();
// builder.Services.AddHostedService<NotificationService>();
builder.Services.AddScoped<ILibService, LibService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
    // .AddJsonOptions(options => {
    //     options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve; });

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();