using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;

namespace UnityTest
{
	[TestFixture]
	[Category("Sample Tests")]
	internal class BasicTests
	{
		[Test]
		public void sampleAssertion()
		{
			Assert.True(true);
		}
	}
}
