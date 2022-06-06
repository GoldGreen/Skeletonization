using Skeletonization.BusinessLayer.Data;
using System.Threading.Tasks;

namespace Skeletonization.BusinessLayer.Abstractions
{
    public interface IReportService
    {
        Task SendReport(Report report);
    }
}
