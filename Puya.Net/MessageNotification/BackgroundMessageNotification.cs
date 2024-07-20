using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Puya.Conversion;
using Puya.Data;
using Puya.Extensions;
using Puya.Mail;
using Puya.Service;
using Puya.Sms;

namespace Puya.MessageNotification
{
    public class BackgroundMessageNotification : BackgroundService
    {
        private readonly ILogger<BackgroundMessageNotification> logger;
        private readonly IDb db;
        private readonly IServiceProvider serviceProvider;
        public BackgroundMessageNotification(NotificationConfig config, ILogger<BackgroundMessageNotification> logger, IDb db, IServiceProvider serviceProvider)
        {
            Config = config;

            this.logger = logger;
            this.db = db;
            this.serviceProvider = serviceProvider;
        }

        public NotificationConfig Config { get; }


        async Task<IList<Notification>> GetTasks()
        {
            var result = await db.ExecuteReaderSqlAsync<Notification>(@"
select * from dbo.Notifications
where IsActive = 1 and RetryCount < @MaxRetry and (Succeeded is null or Succeeded = 0)", new { Config.MaxRetry });

            return result;
        }
        async Task<int> StartTask(int taskId)
        {
            var result = 0;

            if (taskId > 0)
            {
                var args = new
                {
                    Parent = taskId,
                    Id = Puya.Data.CommandParameter.Output(SqlDbType.Int, "SqlDbType")
                };

                await db.ExecuteNonQuerySqlAsync(@"
    begin try
        begin tran

        update dbo.Notifications set RetryCount = RetryCount + 1 where Id = @Parent
        insert into dbo.NotificationLog(Parent) values ((select Id from dbo.Notifications where Id = @Parent))

        set @Id = scope_identity()

        commit
    end try
    begin catch
        if @@trancount > 0
            rollback

        declare @args nvarchar(4000)

        set @args =   'taskId: ' + isnull(cast(@Parent as varchar(20)), '')
    
        exec usp0_Log_error @args
    end catch
    ", args);

                result = SafeClrConvert.ToInt(args.Id.Value);
            }

            return result;
        }
        async Task EndTaskWithError(int taskLogId, Exception e)
        {
            if (taskLogId > 0)
            {
                await db.ExecuteNonQuerySqlAsync(@"
update dbo.NotificationLog set Message = @Message, StackTrace = @StackTrace where Id = @Id
", new { Message = e.ToString("\n"), e.StackTrace, Id = taskLogId });
            }
        }
        async Task EndTask(int taskId, int taskLogId, bool succeeded)
        {
            if (taskLogId > 0)
            {
                await db.ExecuteNonQuerySqlAsync(@"
begin try
    begin tran
    
    update dbo.NotificationLog set EndedAt = getdate() where Id = @LogId
    update dbo.Notifications set Succeeded = @Succeeded where Id = @Parent
    
    commit
end try
begin catch
    if @@trancount > 0
        rollback

    declare @args nvarchar(4000)

    set @args =   'NotificationLogId: ' + isnull(cast(@LogId as varchar(20)), '') +
                ', NotificationId: ' + isnull(cast(@Parent as varchar(20)), '') +
                ' Succeeded: ' + isnull(cast(@Succeeded as varchar(10)), '')
    
    exec usp0_Log_error @args
end catch
", new { Parent = taskId, LogId = taskLogId, Succeeded = succeeded });
            }
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(Config.PollSeconds * 1000);

                    var tasks = await GetTasks();

                    logger.LogDebug($"BackgroundNotification polled at {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ffff")}, task count = {tasks.Count}");

                    if (tasks?.Count > 0)
                    {
                        foreach (var task in tasks)
                        {
                            logger.LogDebug($"Starting task:\n{JsonConvert.SerializeObject(task, Formatting.Indented)}");

                            var taskLogId = await StartTask(task.Id);

                            logger.LogDebug($"task log id = {taskLogId}");

                            try
                            {
                                var succeeded = false;

                                switch (task.NotificationStrongType)
                                {
                                    case NotificationType.Sms:
                                        logger.LogDebug($"sending sms: target: {task.Target}, message: {task.Content}");

                                        var sms = serviceProvider.GetService<ISmsService>();
                                        var sr = await sms.SendAsync(task.Target, task.Content, stoppingToken);

                                        succeeded = sr.IsSucceeded();

                                        break;
                                    case NotificationType.Email:
                                        logger.LogDebug($"sending email:\n\ttarget: {task.Target}\n\tsubject: {task.Subject}\n\tmessage: {task.Content}");

                                        var mailer = serviceProvider.GetService<IMailManager>();

                                        succeeded = await mailer.SendAsync(task.Target, task.Subject, task.Content);

                                        break;
                                    default:
                                        logger.LogDebug($"Invalid notification type {task.NotificationType}");

                                        break;
                                }

                                logger.LogDebug($"Ending task {task.Id}");

                                await EndTask(task.Id, taskLogId, succeeded);
                            }
                            catch (Exception e)
                            {
                                logger.LogError(e, $"Task {task.Id} faulted");

                                await EndTaskWithError(taskLogId, e);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "BackgroundNotification destroyed!");
            }
        }
    }
}
