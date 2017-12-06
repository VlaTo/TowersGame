using System.Numerics;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    public interface ICoordinatesTransformer
    {
        Vector2 GetPoint(CellPosition position);

        CellPosition GetPosition(Vector2 point);
    }
}