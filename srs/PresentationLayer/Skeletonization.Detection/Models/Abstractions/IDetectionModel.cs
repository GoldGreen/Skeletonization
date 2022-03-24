﻿using ReactiveUI;
using Skeletonization.CrossLayer.Data;
using Skeletonization.PresentationLayer.Shared.Data;
using System.Collections.ObjectModel;

namespace Skeletonization.PresentationLayer.Detection.Models.Abstractions
{
    public interface IDetectionModel : IReactiveObject
    {
        byte[] FrameBytes { get; set; }
        ObservableCollection<Zone> Zones { get; }
    }
}
