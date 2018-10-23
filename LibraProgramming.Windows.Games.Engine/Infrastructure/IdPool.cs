using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraProgramming.Windows.Games.Engine.Infrastructure
{
    public class IdPool : IIdPool
    {
        private readonly int increaseSize;
        private int lastMax;

        public int IncrementSize => increaseSize;

        public readonly List<int> AvailableIds;

        public IdPool(int increaseSize = 1000, int startingSize = 10000)
        {
            this.increaseSize = increaseSize;
            lastMax = startingSize;

            AvailableIds = Enumerable.Range(1, lastMax).ToList();
        }

        public int AllocateInstance()
        {
            if (0 == AvailableIds.Count)
            {
                Expand();
            }

            var id = AvailableIds[0];

            AvailableIds.RemoveAt(0);

            return id;
        }

        public bool IsAvailable(int id) => id > lastMax || AvailableIds.Contains(id);

        public void AllocateSpecificId(int id)
        {
            if (id > lastMax)
            {
                Expand(id);
            }

            AvailableIds.Remove(id);
        }

        public void ReleaseInstance(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id has to be >= 1");
            }

            if (id > lastMax)
            {
                Expand(id);
            }

            AvailableIds.Add(id);
        }

        public void Expand(int? newId = null)
        {
            var increaseBy = newId - lastMax ?? increaseSize;

            AvailableIds.AddRange(Enumerable.Range(lastMax + 1, increaseBy));
            lastMax += increaseBy + 1;
        }
    }
}