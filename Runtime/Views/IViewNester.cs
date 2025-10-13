namespace Tellory.StackableUI.Views
{
    public interface IViewNester
    {
        public IView NestedView { get; }
        public void OnNestedViewClose();
    }
}