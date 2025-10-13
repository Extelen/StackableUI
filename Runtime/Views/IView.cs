namespace Tellory.StackableUI.Views
{
    public interface IView
    {
        public string Identifier { get; }
        public IViewNester ParentView { get; set; }

        public void ShowViewInstantly();
        public void HideViewInstantly();
    }
}