using System.Collections.Generic;
using System.Numerics;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    public interface ITargetProvider
    {
        IList<Vector2> GetWaypoints(Vector2 fromPosition);
    }
}