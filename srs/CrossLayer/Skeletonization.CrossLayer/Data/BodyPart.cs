using System.ComponentModel;

namespace Skeletonization.CrossLayer.Data
{
    public enum BodyPart
    {
        [Description("Голова")] 
        Head,
        [Description("Шея")] 
        Neck,

        [Description("Правое плечо")] 
        RightShoulder,
        [Description("Правый локоть")] 
        RightElbow,
        [Description("Правое запястье")] 
        RightWrist,

        [Description("Левое плечо")] 
        LeftShoulder,
        [Description("Левый локоть")] 
        LeftElbow, 
        [Description("Левое запястье")] 
        LeftWrist,

        [Description("Правое бедро")]
        RightHip,
        [Description("Правое колено")]
        RightKnee, 
        [Description("Правая ступня")]
        RightAnkle,

        [Description("Левое бедро")]
        LeftHip, 
        [Description("Левое колено")]
        LeftKnee, 
        [Description("Левая ступня")]
        LeftAnkle,

        [Description("Таз")]
        Hip
    }
}
