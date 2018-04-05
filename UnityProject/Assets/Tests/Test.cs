using System;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

namespace IntegrationTests {
	public class Test : TestAttribute {
		public string Requisite;
		public Test(string description, string requisite = null) { Description = description; Requisite = requisite; }
	}
}
