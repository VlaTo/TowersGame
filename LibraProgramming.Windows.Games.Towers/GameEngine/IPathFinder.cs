using System.Collections.Generic;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    public interface IPathFinder
    {
        CellPosition[] GetWaypoints(CellPosition position);
    }
}