using System.Threading.Tasks;

namespace Skeletonization.DataLayer.Abstractions
{
    public interface IEmailSender
    {
        Task Send(string title, string description, string imageName, byte[] image);
    }
}
