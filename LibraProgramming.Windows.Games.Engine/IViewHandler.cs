namespace LibraProgramming.Windows.Games.Engine
{
    public interface IViewHandler
    {
        void DestroyView(object view);

        void SetActiveState(object view, bool isActive);

        object CreateView();
    }
}