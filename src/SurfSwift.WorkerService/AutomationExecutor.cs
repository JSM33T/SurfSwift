using SurfSwift.Engine;
using SurfSwift.Entities;
using SurfSwift.WorkerService.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SurfSwift.WorkerService
{
    public class AutomationExecutor
    {
        public static async Task RunAllAutomationsAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<SurfSwiftDbContext>();
            var automationEngine = scope.ServiceProvider.GetRequiredService<IAutomationEngine>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Worker>>();

            var users = await dbContext.Users.ToListAsync(cancellationToken);

            foreach (var user in users)
            {
                try
                {
                    var projects = await dbContext.ActionProject
                        .Where(p => p.UserId == user.UserId)
                        .Include(p => p.Template)
                        .ToListAsync(cancellationToken);

                    foreach (var project in projects)
                    {
                        foreach (var template in project.Template)
                        {
                            try
                            {
                                await automationEngine.Initialize(new AutomationConfig
                                {
                                    ActionScript = template.ActionJson
                                });

                                logger.LogInformation(
                                    "Initialized automation for Template: {TemplateName} of Project: {ProjectName} by User: {UserName}",
                                    template.TemplateName, project.ProjectName, user.UserName);
                            }
                            catch (Exception ex)
                            {
                                logger.LogError(ex, "Error initializing automation for Template: {TemplateName} of Project: {ProjectName} by User: {UserName}",
                                    template.TemplateName, project.ProjectName, user.UserName);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error processing projects for User: {UserName}", user.UserName);
                }
            }

        }
    }
}
