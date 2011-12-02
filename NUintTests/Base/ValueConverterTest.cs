﻿using System;
using NUnit.Framework;
using SharpKml.Base;
using SharpKml.Dom;

namespace NUintTests.Base
{
    [TestFixture]
    public class ValueConverterTest
    {
        [Test]
        public void TestBool()
        {
            // The Xml specification states:
            // ·boolean· can have the following legal literals {true, false, 1, 0}.
            object value;
            ValueConverter.TryGetValue(typeof(bool), "true", out value);
            Assert.AreEqual(true, value);
            ValueConverter.TryGetValue(typeof(bool), "1", out value);
            Assert.AreEqual(true, value);

            ValueConverter.TryGetValue(typeof(bool), "false", out value);
            Assert.AreEqual(false, value);
            ValueConverter.TryGetValue(typeof(bool), "0", out value);
            Assert.AreEqual(false, value);

            // Invalid data
            ValueConverter.TryGetValue(typeof(bool), "10", out value);
            Assert.IsNull(value);

            ValueConverter.TryGetValue(typeof(bool), string.Empty, out value);
            Assert.IsNull(value);
        }

        [Test]
        public void TestDateTime()
        {
            string[] ValidDateTimes =
            {
                "1997",
                "1997-07",
                "1997-07-16",
                "1997-07-16T07:30:15Z",
                "1997-07-16T10:30:15+03:00",
                "1997-07-16T14:30:15Z" // Check 24hour value
            };

            foreach (var date in ValidDateTimes)
            {
                object value;
                ValueConverter.TryGetValue(typeof(DateTime), date, out value);

                DateTime dateTime = (DateTime)value;
                Assert.AreEqual(1997, dateTime.Year);
                Assert.AreEqual(DateTimeKind.Utc, dateTime.Kind);
            }
        }

        [Test]
        public void TestTypes()
        {
            // The type converter must be able to parse these types, even if the
            // passed in string is not valid.
            object value;
            Assert.IsTrue(ValueConverter.TryGetValue(typeof(AltitudeMode), string.Empty, out value));
            Assert.IsTrue(ValueConverter.TryGetValue(typeof(bool), string.Empty, out value));
            Assert.IsTrue(ValueConverter.TryGetValue(typeof(Color32), string.Empty, out value));
            Assert.IsTrue(ValueConverter.TryGetValue(typeof(DateTime), string.Empty, out value));
            Assert.IsTrue(ValueConverter.TryGetValue(typeof(double), string.Empty, out value));
            Assert.IsTrue(ValueConverter.TryGetValue(typeof(int), string.Empty, out value));
            Assert.IsTrue(ValueConverter.TryGetValue(typeof(string), string.Empty, out value));
            Assert.IsTrue(ValueConverter.TryGetValue(typeof(Uri), string.Empty, out value));

            // Make sure it's just not always returning true and doesn't throw an exception
            Assert.IsFalse(ValueConverter.TryGetValue(this.GetType(), string.Empty, out value));
        }
    }
}