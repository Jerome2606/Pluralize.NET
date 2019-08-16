﻿using NUnit.Framework;
using System;
using System.Text.RegularExpressions;

namespace Pluralize.NET.Tests
{
    [TestFixture]
    public class PluralizerTests
    {
        private Pluralizer _pluralizer;

        [SetUp]
        public void SetUp()
        {
            _pluralizer = new Pluralizer();
        }

        [Test]
        public void InputData()
        {
            var input = Resources.InputData.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in input)
            {
                var singular = line.Split(',')[0];
                var plural = line.Split(',')[1];
                Assert.AreEqual(plural, _pluralizer.Pluralize(singular));
                Assert.AreEqual(plural, _pluralizer.Pluralize(plural));
                Assert.AreEqual(singular, _pluralizer.Singularize(plural));
                Assert.AreEqual(singular, _pluralizer.Singularize(singular));
            }
        }

        [Test]
        public void ExceptionPluralToSingularException()
        {
            var input = Resources.PluralToSingularExceptions.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in input)
            {
                var singular = line.Split(',')[0];
                var plural = line.Split(',')[1];
                Assert.AreEqual(singular, _pluralizer.Singularize(plural));
                Assert.AreEqual(singular, _pluralizer.Singularize(singular));
            }
        }

        [Test]
        public void ExceptionSingularToPluralException()
        {
            var input = Resources.SingularToPluralExceptions.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in input)
            {
                var singular = line.Split(',')[0];
                var plural = line.Split(',')[1];
                Assert.AreEqual(plural, _pluralizer.Pluralize(singular));
                Assert.AreEqual(plural, _pluralizer.Pluralize(plural));
            }
        }

        [Test]
        public void IsSingular_ReturnsValidResponse()
        {
            var input = Resources.InputData.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in input)
            {
                var singular = line.Split(',')[0];

                Assert.IsTrue(_pluralizer.IsSingular(singular));
            }
        }

        [Test]
        public void IsPlural_ReturnsValidResponse()
        {
            var input = Resources.InputData.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in input)
            {
                var plural = line.Split(',')[1];

                Assert.IsTrue(_pluralizer.IsPlural(plural));
            }
        }

        [Test]
        public void Format_Singular_DoesNotIncludeCount()
        {
            var count = 1;
            var word = "dogs";

            Assert.AreEqual("dog", _pluralizer.Format(word, count));
        }

        [Test]
        public void Format_Singular_IncludesCount()
        {
            var count = 1;
            var word = "dog";

            Assert.AreEqual("1 dog", _pluralizer.Format(word, count, true));
        }

        [Test]
        public void Format_Plural_DoesNotIncludeCount()
        {
            var count = 2;
            var word = "dog";

            Assert.AreEqual("dogs", _pluralizer.Format(word, count));
        }

        [Test]
        public void Format_Plural_IncludesCount()
        {
            var count = 2;
            var word = "dog";

            Assert.AreEqual("2 dogs", _pluralizer.Format(word, count, true));
        }

        [Test]
        public void Format_Zero_IsPlural_DoesNotIncludeCount()
        {
            var count = 0;
            var word = "dog";

            Assert.AreEqual("dogs", _pluralizer.Format(word, count));
        }

        [Test]
        public void Format_Zero_IsPlural_IncludesCount()
        {
            var count = 0;
            var word = "dog";

            Assert.AreEqual("0 dogs", _pluralizer.Format(word, count, true));
        }

        [Test]
        public void AddPluralRule_Regex()
        {
            const string singular = "regex";
            const string plural = "regexii";

            _pluralizer.AddPluralRule(new Regex("gex$"), "gexii");

            Assert.AreEqual(plural, _pluralizer.Pluralize(singular));
        }

        [Test]
        public void AddPluralRule_String()
        {
            const string singular = "person";
            const string plural = "peeps";

            _pluralizer.AddPluralRule(singular, plural);

            Assert.AreEqual(plural, _pluralizer.Pluralize(singular));
        }

        [Test]
        public void AddSingularRule_Regex()
        {
            const string singular = "regex";
            const string plural = "regexii";

            _pluralizer.AddSingularRule(new Regex("gexii$"), "gex");

            Assert.AreEqual(singular, _pluralizer.Singularize(plural));
        }

        [Test]
        public void AddSingularRule_String()
        {
            const string singular = "suck";
            const string plural = "mornings";

            _pluralizer.AddSingularRule(plural, singular);

            Assert.AreEqual(singular, _pluralizer.Singularize(plural));
        }

        [Test]
        public void AddUncountableRule()
        {
            Assert.AreEqual("papers", _pluralizer.Pluralize("paper"));

            _pluralizer.AddUncountableRule("paper");

            Assert.AreEqual("paper", _pluralizer.Pluralize("paper"));
        }

        [Test]
        public void AddUncountableRule_Regex()
        {
            Assert.AreEqual("blahs", _pluralizer.Pluralize("blah"));

            _pluralizer.AddUncountableRule(new Regex("blah"));

            Assert.AreEqual("blah", _pluralizer.Pluralize("blah"));
        }
    }
}
