namespace LibraProgramming.Windows.Games.Engine
{
    public interface ILookupGroup
    {
        int[] RequiredComponents
        {
            get;
        }

        int[] ExcludedComponents
        {
            get;
        }
    }
}