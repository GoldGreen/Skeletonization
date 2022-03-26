using Prism.Services.Dialogs;
using Skeletonization.PresentationLayer.Shared.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Skeletonization.PresentationLayer.Shared.Prism
{
    public static class DialogExtensionsMethods
    {
        public static string ConcatExtensions(this string name, params string[] extensions)
        {
            string ext = string.Join(';', extensions.Select(x => $"*.{x}"));
            return $"{name} ({ext})|{ext}";
        }

        public static Task<(bool ok, VideoDeviceInfo device)> OpenCameraChooseDialog(this IDialogService dialogService)
        {
            TaskCompletionSource<(bool, VideoDeviceInfo)> task = new();

            dialogService.ShowDialog("openCameraDialog", res =>
            {
                bool result = res.Result switch
                {
                    ButtonResult.OK => true,
                    _ => false
                };

                var videoDeviceInfo = result switch
                {
                    true => res.Parameters.GetValue<VideoDeviceInfo>("device"),
                    _ => default
                };

                task.SetResult((result, videoDeviceInfo));
            });

            return task.Task;
        }
    }
}
