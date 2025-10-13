namespace Tellory.StackableUI.Transitions
{
    public enum TransitionState
    {
        Idle,
        Showing,
        Hiding,
    }

    public interface ITransition
    {
        public TransitionState State { get; }

        public float ShowDuration { get; }
        public float HideDuration { get; }

        public void ShowInstantly();
        public void HideInstantly();

        public void Show(bool useScaledTime);
        public void Hide(bool useScaledTime);

        public void ForceStop();
    }
}