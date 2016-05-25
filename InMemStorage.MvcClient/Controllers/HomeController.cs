using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using InMemStorage.ClientLib;

namespace InMemStorage.MvcClient.Controllers
{
	[RoutePrefix("")]
	public class HomeController : Controller
	{
		[HttpGet]
		[Route("")]
		public async Task<ActionResult> Index()
		{
			var storageClient = GetInMemStorageClient();

			var keys = await storageClient.GetKeys();

			return View(keys);
		}

		[HttpGet]
		[Route("Edit/{key?}")]
		public async Task<ActionResult> GetEdit(string key = "")
		{
			string value;

			if (!string.IsNullOrWhiteSpace(key))
			{
				var storageClient = GetInMemStorageClient();

				value = await storageClient.Get(key);
			}
			else
			{
				value = "";
			}

			return View("Edit", new KeyValuePair<string, string>(key, value));
		}

		[HttpPost]
		[Route("Edit")]
		public async Task<ActionResult> PostEdit(string key, string value)
		{
			var storageClient = GetInMemStorageClient();

			await storageClient.Set(key, value);

			return RedirectToAction("Index", new { @key = key });
		}

		[HttpGet]
		[Route("Remove")]
		public async Task<ActionResult> Remove(string key)
		{
			var storageClient = GetInMemStorageClient();

			await storageClient.Remove(key);

			return RedirectToAction("Index");
		}

		private InMemStorageClient GetInMemStorageClient()
		{
			return new InMemStorageClient(ConfigurationManager.AppSettings["inMemStorageBaseUrl"]);
		}
	}
}