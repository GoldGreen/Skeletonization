using Emgu.CV;
using Emgu.CV.Util;
using Skeletonization.BusinessLayer.Abstractions;
using Skeletonization.CrossLayer.Extensions;
using Skeletonization.DataLayer.Abstractions;
using Skeletonization.DataLayer.Implementations.DatabaseSending;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Skeletonization.BusinessLayer.Implementation.Services
{
    internal class ReportService : IReportService
    {
        public IEmailSender EmailSender { get; }
        public SkeletonizationContext DbContext { get; }

        public ReportService(IEmailSender emailSender, SkeletonizationContext dbContext)
        {
            EmailSender = emailSender;
            DbContext = dbContext;
        }

        public Task SendReport(Data.Report report)
        {
            var emailTask = SendEmailReport(report);
            var databaseTask = SendDatabaseReport(report);

            return Task.WhenAll(emailTask, databaseTask);
        }

        private Task SendEmailReport(Data.Report report)
        {
            return Task.CompletedTask;

            var buffer = new VectorOfByte();
            CvInvoke.Imencode(".png", report.Mat, buffer);

            return EmailSender.Send(report.Title,
                                    report.Description,
                                    $"report_{DateTime.Now.ToLongDateString()}.png",
                                    buffer.ToArray());
        }

        private async Task SendDatabaseReport(Data.Report report)
        {
            string path = $"report_{DateTime.Now.ToLongDateString()}.png";
            CvInvoke.Imwrite(path, report.Mat);

            var dbReport = DbContext.Reports.Add(new()
            {
                Description = report.Description,
                Path = path
            }).Entity;

            foreach (var human in report.Humans)
            {
                var dbHuman = (await DbContext.Humans.AddAsync
                (
                    new()
                    {
                        Name = human.Name,
                        Pose = DbContext.Poses.First(x => x.Id == 1)
                    }
                )).Entity;

                foreach (var point in human.Points)
                {
                    var dbBodyPart = DbContext.BodyParts.FirstOrDefault(x => x.Id == (int)point.BodyPart + 1);

                    if (dbBodyPart == null)
                    {
                        dbBodyPart = (await DbContext.BodyParts.AddAsync(new()
                        {
                            Id = (int)point.BodyPart + 1,
                            Name = point.BodyPart.ToDescriptionOrString()
                        })).Entity;
                    }

                    var dbPoint = (await DbContext.Points.AddAsync(new()
                    {
                        BodyPart = dbBodyPart,
                        X = point.Point.X,
                        Y = point.Point.Y
                    })).Entity;

                    dbHuman.Points.Add(dbPoint);
                }

                DbContext.Humans.Update(dbHuman);

                dbReport.Humans.Add(dbHuman);
            }

            DbContext.Reports.Update(dbReport);
            await DbContext.SaveChangesAsync();
        }
    }
}
