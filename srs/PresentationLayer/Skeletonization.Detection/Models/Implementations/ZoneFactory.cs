using Skeletonization.CrossfulLayer.Abstractions;
using Skeletonization.CrossLayer.Data;
using Skeletonization.PresentationLayer.Shared.Data;
using System;
using System.Linq;

namespace Skeletonization.PresentationLayer.Detection.Models.Implementations
{
    internal class ZoneFactory : IFactory<Zone>
    {
        private int _id;

        public Zone Create()
        {
            return new(0.4, 0.4, 0.1, 0.1, Guid.NewGuid())
            {
                Name = $"Зона {_id++}",
                Color = "Red",
                Opacity = 0.65,
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
