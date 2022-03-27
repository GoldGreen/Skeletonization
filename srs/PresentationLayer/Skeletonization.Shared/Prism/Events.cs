using Emgu.CV;
using Prism.Events;
using Skeletonization.CrossLayer.Data;
using Skeletonization.PresentationLayer.Shared.Data;
using System.Collections.Generic;
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

    #region Humans
    public class HumansChanged : PubSubEvent<IEnumerable<Human>>
    { }
    #endregion
   
    #region Notification
    public class NotificationSended : PubSubEvent<string> 
    { }
    #endregion
}

