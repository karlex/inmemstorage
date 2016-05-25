using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace InMemStorage.ClientLib
{
	public class InMemStorageClient: IDisposable
    {
		HttpClient httpClient;

		const string ApiUrlPrefix = "api/storage/";

		public InMemStorageClient(string url)
		{
			httpClient = new HttpClient();
			httpClient.BaseAddress = new Uri(url);
			httpClient.DefaultRequestHeaders.Accept.Clear();
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		public async Task<IEnumerable<string>> GetKeys()
		{
			var response = await httpClient.GetAsync(ApiUrlPrefix);

			return await response.Content.ReadAsAsync<string[]>();
		}

		public async Task<string> Get(string key)
		{
			var response = await httpClient.GetAsync(ApiUrlPrefix + key);

			return await response.Content.ReadAsAsync<string>();
		}

		public async Task<bool> Set(string key, string value)
		{
			var response = await httpClient.PostAsJsonAsync(ApiUrlPrefix + key, value);

			return response.StatusCode == System.Net.HttpStatusCode.OK;
		}

		public async Task<bool> Remove(string key)
		{
			var response = await httpClient.DeleteAsync(ApiUrlPrefix + key);

			return await response.Content.ReadAsAsync<bool>();
		}

		public void Dispose()
		{
			if (httpClient != null)
			{
				httpClient.Dispose();
				httpClient = null;
			}
		}
    }
}
