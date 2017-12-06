using System;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    public struct CellPosition : IEquatable<CellPosition>
    {
        public int Column
        {
            get;
        }

        public int Row
        {
            get;
        }

        public CellPosition(int column, int row)
        {
            Column = column;
            Row = row;
        }

        public bool Equals(CellPosition other)
        {
            return Column == other.Column && Row == other.Row;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is CellPosition position && Equals(position);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Column * 397) ^ Row;
            }
        }

        public static bool operator ==(CellPosition left, CellPosition right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CellPosition left, CellPosition right)
        {
            return false == left.Equals(right);
        }
    }
}