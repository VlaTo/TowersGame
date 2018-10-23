namespace LibraProgramming.Windows.Games.Engine
{
    public class ViewComponent : IComponent
    {
        public bool DestroyWithView
        {
            get;
            set;
        }

        public object View
        {
            get;
            set;
        }
    }
}