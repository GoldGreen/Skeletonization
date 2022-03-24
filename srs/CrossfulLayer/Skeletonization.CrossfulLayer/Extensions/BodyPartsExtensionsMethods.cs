using Skeletonization.CrossfulLayer.Data;
using System.Collections.Generic;

namespace Skeletonization.CrossfulLayer.Extensions
{
    public static class BodyPartsExtensionsMethods
    {
        private static IReadOnlyDictionary<BodyPart, IEnumerable<BodyPart>> links = new Dictionary<BodyPart, IEnumerable<BodyPart>>
        {
            { BodyPart.Head, new [] { BodyPart.Neck } },
            { BodyPart.Neck, new[] { BodyPart.RightShoulder, BodyPart.Hip, BodyPart.LeftShoulder } },
            { BodyPart.RightShoulder, new[] { BodyPart.RightElbow, BodyPart.Hip } },
            { BodyPart.RightElbow, new[] { BodyPart.RightWrist } },
            { BodyPart.LeftShoulder, new[] { BodyPart.LeftElbow, BodyPart.Hip } },
            { BodyPart.LeftElbow, new[] { BodyPart.LeftWrist } },
            { BodyPart.RightHip, new[] { BodyPart.RightKnee} },
            { BodyPart.RightKnee, new[] { BodyPart.RightAnkle } },
            { BodyPart.LeftHip, new[] { BodyPart.LeftKnee} },
            { BodyPart.LeftKnee, new[] { BodyPart.LeftAnkle } },
            { BodyPart.Hip, new[] { BodyPart.LeftHip, BodyPart.RightHip} },
        };

        public static bool HasLinks(this BodyPart part)
        {
            return links.ContainsKey(part);
        }

        public static IEnumerable<BodyPart> GetLinkedBodyParts(this BodyPart part)
        {
            return links[part];
        }
    }
}
