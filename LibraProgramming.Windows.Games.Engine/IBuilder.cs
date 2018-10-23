namespace LibraProgramming.Windows.Games.Engine
{
    public interface IBuilder<out TTarget>
    {
        TTarget Build();
    }
}