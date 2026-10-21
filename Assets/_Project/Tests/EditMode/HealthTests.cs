using System;
using NUnit.Framework;
using SurvivalCore.Health;

namespace SurvivalCore.Tests
{
    public class HealthTests
    {
        [Test]
        public void New_StartsAtMax()
        {
            var h = new Health.Health(100);
            Assert.AreEqual(100, h.Current);
            Assert.IsFalse(h.IsDead);
        }

        [TestCase(0)]
        [TestCase(-5)]
        public void New_NonPositiveMax_Throws(int max)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Health.Health(max));
        }

        [Test]
        public void TakeDamage_ReducesCurrent_AndRaisesDamaged()
        {
            var h = new Health.Health(100);
            int reported = 0;
            h.Damaged += a => reported = a;

            h.TakeDamage(30);

            Assert.AreEqual(70, h.Current);
            Assert.AreEqual(30, reported);
        }

        [Test]
        public void TakeDamage_Lethal_ClampsToZero_AndFiresDiedOnce()
        {
            var h = new Health.Health(50);
            int deaths = 0;
            h.Died += () => deaths++;

            h.TakeDamage(500);
            h.TakeDamage(10);

            Assert.AreEqual(0, h.Current);
            Assert.IsTrue(h.IsDead);
            Assert.AreEqual(1, deaths);
        }

        [Test]
        public void TakeDamage_Negative_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Health.Health(10).TakeDamage(-1));
        }

        [Test]
        public void Heal_CapsAtMax_AndReportsActualAmount()
        {
            var h = new Health.Health(100);
            h.TakeDamage(20);
            int healed = 0;
            h.Healed += a => healed = a;

            h.Heal(999);

            Assert.AreEqual(100, h.Current);
            Assert.AreEqual(20, healed);
        }

        [Test]
        public void Heal_WhenDead_DoesNothing()
        {
            var h = new Health.Health(10);
            h.TakeDamage(10);
            h.Heal(5);
            Assert.AreEqual(0, h.Current);
        }

        [Test]
        public void Heal_AtFullHealth_DoesNotRaiseEvent()
        {
            var h = new Health.Health(10);
            bool raised = false;
            h.Healed += _ => raised = true;
            h.Heal(5);
            Assert.IsFalse(raised);
        }
    }
}
