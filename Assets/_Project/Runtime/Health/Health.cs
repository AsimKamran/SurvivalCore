using System;

namespace SurvivalCore.Health
{
    /// <summary>Pure C# health model. No UnityEngine dependency, so it is trivially unit-testable.</summary>
    public sealed class Health
    {
        public int Max { get; }
        public int Current { get; private set; }
        public bool IsDead => Current <= 0;

        public event Action<int> Damaged;
        public event Action<int> Healed;
        public event Action Died;

        public Health(int max)
        {
            if (max <= 0) throw new ArgumentOutOfRangeException(nameof(max), "Max health must be positive.");
            Max = max;
            Current = max;
        }

        public void TakeDamage(int amount)
        {
            if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
            if (IsDead || amount == 0) return;

            Current = Math.Max(0, Current - amount);
            Damaged?.Invoke(amount);
            if (IsDead) Died?.Invoke();
        }

        public void Heal(int amount)
        {
            if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
            if (IsDead) return;

            int gained = Math.Min(Max, Current + amount) - Current;
            if (gained == 0) return;
            Current += gained;
            Healed?.Invoke(gained);
        }
    }
}
