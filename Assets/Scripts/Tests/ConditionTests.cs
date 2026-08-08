using System;
using NUnit.Framework;

namespace Tests
{
    public class AndTests
    {
        [Test]
        public void Check_AllTrue_ReturnsTrue()
        {
            AndCondition<int> condition = new(new GreaterThanOrEqualCondition<int>(10), new LessThanOrEqualCondition<int>(50));
            Assert.IsTrue(condition.Check(25));
        }

        [Test]
        public void Check_OneFalse_ReturnsFalse()
        {
            AndCondition<int> condition = new(new GreaterThanOrEqualCondition<int>(10), new LessThanOrEqualCondition<int>(50));
            Assert.IsFalse(condition.Check(75));
        }

        [Test]
        public void Check_NoConditions_ReturnsTrue()
        {
            AndCondition<int> condition = new();
            Assert.IsTrue(condition.Check(0));
        }
    }

    public class OrTests
    {
        [Test]
        public void Check_OneTrue_ReturnsTrue()
        {
            OrCondition<int> condition = new(new LessThanOrEqualCondition<int>(0), new GreaterThanOrEqualCondition<int>(10));
            Assert.IsTrue(condition.Check(15));
        }

        [Test]
        public void Check_AllFalse_ReturnsFalse()
        {
            OrCondition<int> condition = new(new LessThanOrEqualCondition<int>(0), new GreaterThanOrEqualCondition<int>(100));
            Assert.IsFalse(condition.Check(50));
        }

        [Test]
        public void Check_NoConditions_ReturnsFalse()
        {
            OrCondition<int> condition = new();
            Assert.IsFalse(condition.Check(0));
        }
    }

    public class NotTests
    {
        [Test]
        public void Check_InvertsTrueToFalse()
        {
            NotCondition<int> condition = new(new GreaterThanOrEqualCondition<int>(10));
            Assert.IsFalse(condition.Check(20));
        }

        [Test]
        public void Check_InvertsFalseToTrue()
        {
            NotCondition<int> condition = new(new GreaterThanOrEqualCondition<int>(10));
            Assert.IsTrue(condition.Check(5));
        }
    }

    public class ProbabilityTests
    {
        private UnityEngine.Random.State originalState;

        [SetUp]
        public void SaveRandomState()
        {
            originalState = UnityEngine.Random.state;
        }

        [TearDown]
        public void RestoreRandomState()
        {
            UnityEngine.Random.state = originalState;
        }

        [Test]
        public void Check_ZeroProbability_AlwaysFalse()
        {
            ProbabilityCondition<int> condition = new(0f);
            for (int i = 0; i < 100; i++)
                Assert.IsFalse(condition.Check(0));
        }

        [Test]
        public void Check_FullProbability_AlwaysTrue()
        {
            ProbabilityCondition<int> condition = new(1f);
            for (int i = 0; i < 100; i++)
                Assert.IsTrue(condition.Check(0));
        }

        [Test]
        public void Check_WithSameSeed_IsDeterministic()
        {
            ProbabilityCondition<int> condition = new(0.5f);

            UnityEngine.Random.InitState(42);
            bool[] resultsA = new bool[20];
            for (int i = 0; i < resultsA.Length; i++)
                resultsA[i] = condition.Check(0);

            UnityEngine.Random.InitState(42);
            bool[] resultsB = new bool[20];
            for (int i = 0; i < resultsB.Length; i++)
                resultsB[i] = condition.Check(0);

            CollectionAssert.AreEqual(resultsA, resultsB);
        }

        [Test]
        public void Constructor_ProbabilityOutOfRange_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ProbabilityCondition<int>(-0.1f));
            Assert.Throws<ArgumentOutOfRangeException>(() => new ProbabilityCondition<int>(1.1f));
        }
    }

    public class GreaterThanOrEqualConditionTests
    {
        [TestCase(15, 10, ExpectedResult = true)]  // value > threshold
        [TestCase(10, 10, ExpectedResult = true)]  // value == threshold
        [TestCase(5, 10, ExpectedResult = false)]  // value < threshold

        public bool Check_ReturnsExpected(int value, int threshold)
        {
            GreaterThanOrEqualCondition<int> condition = new(threshold);
            return condition.Check(value);
        }
    }

    public class LessThanOrEqualTests
    {
        [TestCase(5, 10, ExpectedResult = true)]   // value < threshold
        [TestCase(10, 10, ExpectedResult = true)]  // value == threshold
        [TestCase(15, 10, ExpectedResult = false)] // value > threshold

        public bool Check_ReturnsExpected(int value, int threshold)
        {
            LessThanOrEqualCondition<int> condition = new(threshold);
            return condition.Check(value);
        }
    }

    public class CompositionTests
    {
        [Test]
        public void NestedCombinators_EvaluateCorrectly()
        {
            // (value >= 20) OR NOT(value <= 5)  ==  value >= 20 OR value > 5
            OrCondition<int> condition = new(
                new GreaterThanOrEqualCondition<int>(20),
                new NotCondition<int>(new LessThanOrEqualCondition<int>(5)));

            Assert.IsTrue(condition.Check(25));  // >= 20
            Assert.IsTrue(condition.Check(10));  // > 5, not >= 20
            Assert.IsFalse(condition.Check(3));  // <= 5 and < 20
        }

        [Test]
        public void And_Of_Or_EvaluatesCorrectly()
        {
            // (value >= 0 OR value <= -100) AND value <= 50
            AndCondition<int> condition = new(
                new OrCondition<int>(new GreaterThanOrEqualCondition<int>(0), new LessThanOrEqualCondition<int>(-100)),
                new LessThanOrEqualCondition<int>(50));

            Assert.IsTrue(condition.Check(30));
            Assert.IsFalse(condition.Check(60));
            Assert.IsFalse(condition.Check(-50));
        }
    }
}