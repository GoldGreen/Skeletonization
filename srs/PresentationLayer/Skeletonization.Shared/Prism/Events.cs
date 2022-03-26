using Emgu.CV;
using Prism.Events;
using Skeletonization.PresentationLayer.Shared.Data;
using System.Collections.Specialized;

namespace Skeletonization.PresentationLayer.Shared.Prism
{
    #region ZonesEvents
    public class ZonesChanged
        : PubSubEvent<(NotifyCollectionChangedAction action, Zone zone)>
    { }

    public class ZoneSelected
     : PubSubEvent<Zone>
    { }
    #endregion

    #region Frame
    public class FrameChanged : PubSubEvent<Mat>
    { }
    #endregion
}

