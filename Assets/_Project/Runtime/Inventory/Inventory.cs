using System;
using System.Collections.Generic;

namespace SurvivalCore.Inventory
{
    /// <summary>Fixed-capacity, slot-based, stackable inventory. Pure C#.</summary>
    public sealed class Inventory
    {
        private sealed class Slot
        {
            public ItemDefinition Item;
            public int Count;
        }

        private readonly List<Slot> _slots = new List<Slot>();
        public int Capacity { get; }
        public event Action Changed;

        public Inventory(int capacity)
        {
            if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));
            Capacity = capacity;
        }

        public int CountOf(string id)
        {
            int total = 0;
            foreach (var s in _slots) if (s.Item.Id == id) total += s.Count;
            return total;
        }

        /// <summary>Adds items; returns the number that did NOT fit.</summary>
        public int Add(ItemDefinition item, int count)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));

            int remaining = count;

            foreach (var s in _slots)
            {
                if (remaining == 0) break;
                if (s.Item.Id != item.Id) continue;
                int moved = Math.Min(item.MaxStack - s.Count, remaining);
                s.Count += moved;
                remaining -= moved;
            }

            while (remaining > 0 && _slots.Count < Capacity)
            {
                int moved = Math.Min(item.MaxStack, remaining);
                _slots.Add(new Slot { Item = item, Count = moved });
                remaining -= moved;
            }

            if (remaining != count) Changed?.Invoke();
            return remaining;
        }

        /// <summary>Removes items atomically: either all requested are removed or nothing changes.</summary>
        public bool Remove(string id, int count)
        {
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));
            if (CountOf(id) < count) return false;

            int remaining = count;
            for (int i = _slots.Count - 1; i >= 0 && remaining > 0; i--)
            {
                var s = _slots[i];
                if (s.Item.Id != id) continue;
                int taken = Math.Min(s.Count, remaining);
                s.Count -= taken;
                remaining -= taken;
                if (s.Count == 0) _slots.RemoveAt(i);
            }

            Changed?.Invoke();
            return true;
        }
    }
}
