namespace Tellory.StackableUI.Views
{
    public interface IAnimatedView
    {
        public bool IsUsingScaledTime { get; }

        public float ShowAnimationDuration { get; }
        public float HideAnimationDuration { get; }

        public void ShowView();
        public void HideView();
    }
}