using UnityEngine;
using UnityEngine.Events;

namespace SurvivalCore.Health
{
    /// <summary>Thin MonoBehaviour adapter around the pure Health model.</summary>
    public sealed class PlayerHealthBehaviour : MonoBehaviour
    {
        [SerializeField, Min(1)] private int maxHealth = 100;
        [SerializeField] private UnityEvent onDied = new UnityEvent();

        public Health Model { get; private set; }
        public UnityEvent OnDied => onDied;

        private void Awake()
        {
            Model = new Health(maxHealth);
            Model.Died += HandleDied;
        }

        private void OnDestroy()
        {
            if (Model != null) Model.Died -= HandleDied;
        }

        public void TakeDamage(int amount) => Model.TakeDamage(amount);

        private void HandleDied()
        {
            onDied.Invoke();
            gameObject.SetActive(false);
        }
    }
}
