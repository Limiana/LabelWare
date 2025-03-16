using LabelWare.DataFiles;
using LabelWare.Screens;
using Newtonsoft.Json;
using Octokit;
using System.Collections.Immutable;

namespace LabelWare;

public class Program
{
    public static Configuration C = new();
    public static string ConfigPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LabelWare", "DefaultConfig.json");
    public static GitHubClient Client = null!;
    public static void Main(string[] args)
    {
        try
        {
            if(File.Exists(ConfigPath))
            {
                C = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(ConfigPath), new JsonSerializerSettings()
                {
                    ObjectCreationHandling = ObjectCreationHandling.Replace,
                }) ?? throw new NullReferenceException();
            }
        }
        catch(Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        MainScreen.Run();
    }

    public static void SaveConfig()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath));
            File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(C));
        }
        catch(Exception e)
        {
            e.Log();
        }
    }
}
