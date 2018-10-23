using System;
using System.Numerics;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    /// <summary>
    /// 
    /// </summary>
    internal class GameCoordinatesSystem : ICoordinatesSystem
    {
        private const float CellHeight = 25.0f;
        private const float CellWidth = 25.0f;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Vector2 GetPoint(Position position)
        {
            var point = new Vector2(CellWidth / 2.0f, CellHeight / 2.0f);

            if (position.Column > 0)
            {
                point.X += CellWidth * (position.Column - 1);
            }

            if (position.Row > 0)
            {
                point.Y += CellHeight * (position.Row - 1);
            }

            return point;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Position GetPosition(Vector2 point)
        {
            var column = (int) (point.X / CellWidth);
            var row = (int) (point.Y / CellHeight);

            if (Math.IEEERemainder(point.X, CellWidth) >= Single.Epsilon)
            {
                column++;
            }

            if (Math.IEEERemainder(point.Y, CellHeight) >= Single.Epsilon)
            {
                row++;
            }

            return new Position(column, row);
        }
    }
}