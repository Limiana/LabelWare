using LabelWare.DataFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelWare.Screens;
public unsafe static class ConfigureSourceRepoScreen
{
    public static void Run()
    {
        if(Client == null)
        {
            Console.WriteLine("You must set your token first.");
            return;
        }
        Console.WriteLine($"""
            [0] - Cancel
            [1] - Organization
            [2] - User
            """);
        var r = GetChoice();
        if(r == 1)
        {
            Process(true);
        }
        else if(r == 2)
        {
            Process(false);
        }
        else
        {
            Console.WriteLine("Source repo selection cancelled");
        }
    }

    static void Process(bool isOrg)
    {
        Console.WriteLine($"Enter {(isOrg?"Organization":"User")} name (case-sensitive):");
        var org = Console.ReadLine() ?? "";
        var repos = (isOrg?Client.Repository.GetAllForOrg(org):Client.Repository.GetAllForUser(org)).Result.OrderBy(x => x.Name).ToList();
        Console.WriteLine($"The following repos are available for this {(isOrg?"Organization":"User")}:");
        for(int i = 0; i < repos.Count; i++)
        {
            Console.WriteLine($"[{i + 1}] - {repos[i].Name}");
        }
        Console.WriteLine("Enter a number to select a source repo or 0 to exit.");
        var repo = GetChoice();
        if(repo == 0)
        {
            return;
        }
        else if(repo - 1 < repos.Count)
        {
            var newRepo = new RepositoryDescriptor(org, repos[repo - 1].Name, isOrg);
            Console.WriteLine($"Repository {newRepo} is now selected as label source");
            C.SourceRepository = newRepo;
            SaveConfig();
            var labels = Client.Issue.Labels.GetAllForRepository(newRepo.UserOrOrg, newRepo.RepoName).Result;
            foreach(var l in labels)
            {
                Console.WriteLine($"> {l.Name} ({l.Description})");
            }
        }
    }
}
