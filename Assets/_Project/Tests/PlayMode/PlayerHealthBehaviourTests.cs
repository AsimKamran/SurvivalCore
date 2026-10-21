using System.Collections;
using NUnit.Framework;
using SurvivalCore.Health;
using UnityEngine;
using UnityEngine.TestTools;

namespace SurvivalCore.Tests
{
    public class PlayerHealthBehaviourTests
    {
        private GameObject _go;

        [TearDown]
        public void TearDown()
        {
            if (_go != null) Object.Destroy(_go);
        }

        [UnityTest]
        public IEnumerator Awake_CreatesModel_WithSerializedMax()
        {
            _go = new GameObject("Player");
            var player = _go.AddComponent<PlayerHealthBehaviour>();
            yield return null;

            Assert.IsNotNull(player.Model);
            Assert.AreEqual(100, player.Model.Max);
        }

        [UnityTest]
        public IEnumerator LethalDamage_RaisesEvent_AndDeactivatesObject()
        {
            _go = new GameObject("Player");
            var player = _go.AddComponent<PlayerHealthBehaviour>();
            bool died = false;
            player.OnDied.AddListener(() => died = true);
            yield return null;

            player.TakeDamage(1000);
            yield return null;

            Assert.IsTrue(died);
            Assert.IsFalse(_go.activeSelf);
        }
    }
}
