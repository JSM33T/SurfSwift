namespace SurfSwift.Engine.Helpers
{
    public class KeyValMapper
    {
        public static string ReplacePlaceholdersInJson(string json, Dictionary<string, string> keyValues)
        {
            if (string.IsNullOrEmpty(json)) return json;

            foreach (var kvp in keyValues)
            {
                json = json.Replace($"{{{{{kvp.Key}}}}}", kvp.Value);
            }

            return json;
        }
    }
}
