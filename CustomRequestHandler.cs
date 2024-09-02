using System.Text;
using System.Threading.Tasks;

namespace LootValueEX
{
	public class CustomRequestHandler
	{
		public static async Task<string> PostJsonAsync(string path, string json)
		{
			byte[] responseBytes = await SPT.Common.Http.RequestHandler.HttpClient.PostAsync(path, Encoding.UTF8.GetBytes(json));
			string responseString = Encoding.UTF8.GetString(responseBytes);
			if (string.IsNullOrWhiteSpace(responseString))
			{
				Mod.Log.LogError($"[REQUEST FAILED] {path}");
                return "";
			}
			Mod.Log.LogInfo($"[REQUEST SUCCESSFUL] {path}");
            return responseString;
		}
	}
}
