using System;

namespace SurvivalCore.Inventory
{
    public sealed class ItemDefinition
    {
        public string Id { get; }
        public int MaxStack { get; }

        public ItemDefinition(string id, int maxStack = 1)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Id required.", nameof(id));
            if (maxStack < 1) throw new ArgumentOutOfRangeException(nameof(maxStack));
            Id = id;
            MaxStack = maxStack;
        }
    }
}
