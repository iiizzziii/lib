using lib.api.Data;
using Microsoft.EntityFrameworkCore;

namespace lib.api.Services;

public class NotificationService(
    IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            // ReSharper disable once ConvertToUsingDeclaration
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                var tomorrow = DateTime.Now.AddDays(1).Date;

                var dueTomorrow = dbContext.Borrowings
                    .Include(b => b.User)
                    .Include(b => b.Book)
                    .Where(b => b.DateDue.Date == tomorrow)
                    .ToList();

                foreach (var due in dueTomorrow)
                {
                    emailService.SendEmail(
                        due.User.Email,
                        "Library reminder",
                        $"Hello customer, return {due.Book.Title} tomorrow.");
                }
            }
            await Task.Delay(TimeSpan.FromDays(1), cancellationToken);
        }
    }
}