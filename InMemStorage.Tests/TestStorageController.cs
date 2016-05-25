using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InMemStorage.Controllers;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using System.Net.Http.Formatting;

namespace InMemStorage.Tests
{
	[TestClass]
	public class TestStorageController
	{
		private StorageController GetController()
		{
			var controller = new StorageController();
			controller.Request = new HttpRequestMessage()
			{
				Content = new ObjectContent(typeof(string), "value", new JsonMediaTypeFormatter(), JsonMediaTypeFormatter.DefaultMediaType)
			};
			controller.Set("key");

			return controller;
		}

		[TestMethod]
		public void SetThenGet_ShouldReturnCorrectValue()
		{
			var controller = GetController();
			
			var value = controller.Get("key");

			Assert.AreEqual("value", value);
		}

		[TestMethod]
		public void SetThenGetKeys_ShouldReturnCorrectKeys()
		{
			var controller = GetController();

			var keys = controller.GetKeys();

			CollectionAssert.AreEqual(new List<string>{ "key" }, keys.ToList());
		}

		[TestMethod]
		public void SetThenDelete_ShouldRemoveKeyAndValue()
		{
			var controller = GetController();

			var result = controller.Delete("key");
			var keys = controller.GetKeys();
			var value = controller.Get("key");

			Assert.AreEqual(true, result);
			Assert.AreEqual(0, keys.Count());
			Assert.AreEqual(null, value);
		}
	}
}
