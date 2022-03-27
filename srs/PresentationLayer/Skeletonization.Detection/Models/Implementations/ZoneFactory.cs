using Skeletonization.CrossLayer.Data;
using Skeletonization.PresentationLayer.Detection.Models.Abstractions;
using Skeletonization.PresentationLayer.Shared.Data;
using System;
using System.Linq;

namespace Skeletonization.PresentationLayer.Detection.Models.Implementations
{
    internal class ZoneFactory : IZoneFactory
    {
        private int _id;

        public Zone Create()
        {
            return new(0.4, 0.4, 0.1, 0.1)
            {
                Name = $"Зона {_id++}",
                Color = "Red",
                Opacity = 0.5,
                MinCount = 0,
                Delay = 0,
                CheckInside = true,
                BodyParts = typeof(BodyPart).GetEnumValues()
                                            .Cast<BodyPart>()
                                            .Select(x => new Selectable<BodyPart>(x)
                                            {
                                                IsSelected = true
                                            })
                                            .ToList()
            };
        }
    }
}
