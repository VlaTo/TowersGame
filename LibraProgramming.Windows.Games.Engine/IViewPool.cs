using LibraProgramming.Windows.Games.Engine.Infrastructure;

namespace LibraProgramming.Windows.Games.Engine
{
    public interface IViewPool : IPool<object>
    {
        void PreAllocate(int allocationCount);

        void DeAllocate(int dellocationCount);

        void EmptyPool();
    }
}