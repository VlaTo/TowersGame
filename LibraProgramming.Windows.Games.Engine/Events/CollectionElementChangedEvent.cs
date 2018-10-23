namespace LibraProgramming.Windows.Games.Engine.Events
{
    public class CollectionElementChangedEvent<TElement>
    {
        public int Index
        {
            get;
            set;
        }

        public TElement OldValue
        {
            get;
            set;
        }

        public TElement NewValue
        {
            get;
            set;
        }
    }
}