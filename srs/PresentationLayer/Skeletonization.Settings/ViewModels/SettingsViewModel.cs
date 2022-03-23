using ReactiveUI;
using Skeletonization.Settings.Models.Abstractions;

namespace Skeletonization.Settings.ViewModels
{
    internal class SettingsViewModel : ReactiveObject
    {
        public ISettingsModel Model { get; }
        
        public SettingsViewModel(ISettingsModel model)
        {
            Model = model;
        }
    }
}
