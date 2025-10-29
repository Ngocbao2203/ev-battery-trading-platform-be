using EBTP.Service.IServices;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.Jobs
{
    public static class HangfireJobs
    {
        public static void RegisterJobs()
        {
            RecurringJob.AddOrUpdate<IListingService>(
    "auto-change-status",
    x => x.AutoChangeStatusWhenListingExpiredAsync(),
    "*/5 * * * *" // cron expression: mỗi 5 phút
);
        }
    }
}
