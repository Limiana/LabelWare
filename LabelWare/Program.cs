using Octokit;
using System.Collections.Immutable;

namespace LabelWare;

internal class Program
{
    public static GithubManager GithubManager = new();
    public static ImmutableList<LabelConfiguration> Configuration = [];
    static void Main(string[] args)
    {
        Console.WriteLine($"Welcome to LabelWare by Limiana. At any time, type \"quit\" to exit current operation. ");
        Configuration = Utils.LoadLabelConfiguration();
        while (true)
        {
            Console.WriteLine("> Available commands:");
            Console.WriteLine("  > token [your token] - sets your Personal access token");
            Console.WriteLine("  > reload - reloads configuration files");
            Console.WriteLine("  > dry - performs dry run and displays what changes would have been made");
            Console.WriteLine("  > wet - performs changes according to loaded configuration");
            var command = Console.ReadLine() ?? "";
            if (command == "quit") Environment.Exit(0);
            if(command.StartsWith("token "))
            {
                GithubManager.PAT = command.Split(" ")[1];
                Console.WriteLine("Token set.");
            }
            if(command == "reload")
            {
                Configuration = Utils.LoadLabelConfiguration();
            }
            if (command == "dry")
            {
                var task = GithubManager.Run(false);
                task.Wait();
            }
            if (command == "wet")
            {
                var task = GithubManager.Run(true);
                task.Wait();
            }
        }
    }
}
