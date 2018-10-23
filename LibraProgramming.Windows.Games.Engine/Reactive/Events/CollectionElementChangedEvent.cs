namespace LibraProgramming.Windows.Games.Engine.Reactive.Events
{
    public class CollectionElementChangedEvent<TValue>
    {
        public TValue OldValue
        {
            get;
        }

        public TValue NewValue
        {
            get;
        }

        public int Index
        {
            get;
        }

        public CollectionElementChangedEvent(int index, TValue oldValue, TValue newValue)
        {
            Index = index;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}