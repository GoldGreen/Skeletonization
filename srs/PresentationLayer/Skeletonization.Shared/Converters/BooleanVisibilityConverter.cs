namespace Skeletonization.PresentationLayer.Shared.Converters
{
    public class BooleanVisibilityConverter : BaseVisibilityConverter<bool, BooleanVisibilityConverter>
    {
        public override bool VisibleValue => true;
        public override bool HiddenValue => false;
    }
}
