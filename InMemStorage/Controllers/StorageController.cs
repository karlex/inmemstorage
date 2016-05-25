using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace InMemStorage.Controllers
{
	[RoutePrefix("api/storage")]
	public class StorageController : ApiController
	{
		private static readonly ConcurrentDictionary<string, string> storage = new ConcurrentDictionary<string, string>();

		// GET api/storage
		[HttpGet]
		[Route("")]
		public IEnumerable<string> GetKeys()
		{
			return storage.Keys;
		}

		// GET api/storage/5
		[HttpGet]
		[Route("{key}")]
		public string Get(string key)
		{
			string value;

			if (storage.TryGetValue(key, out value))
			{
				return value;
			}

			return null;
		}

		// POST api/storage/5
		[HttpPost]
		[Route("{key}")]
		public async void Set(string key)
		{
			var value = await Request.Content.ReadAsAsync<string>();
			storage[key] = value;
		}

		// DELETE api/storage/5
		[HttpDelete]
		[Route("{key}")]
		public bool Delete(string key)
		{
			string value;
			return storage.TryRemove(key, out value);
		}
	}
}
