using LabelWare.DataFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelWare.Screens;
public static unsafe class SelectTargetReposScreen
{
    public static void Run()
    {
        while(true)
        {
            Console.WriteLine($"""
            The following users and/or organizations are currently preset in configuration:
            """);
            foreach(var x in C.WatchedRepos.Select(s => (s.UserOrOrg, s.IsOrganization)).Distinct().OrderBy(s => s.IsOrganization).ThenBy(s => s.UserOrOrg))
            {
                Console.WriteLine($"- {x.UserOrOrg} ({(x.IsOrganization ? "Organization" : "User")})");
            }
            Console.WriteLine($"""
            [0] - Cancel
            [1] - Add/remove repos from organization
            [2] - Add/remove repos from user
            """);
            var choice = GetChoice();
            if(choice == 1)
            {
                Process(true);
            }
            else if(choice == 2)
            {
                Process(false);
            }
            else if(choice == 0)
            {
                return;
            }
        }
    }

    private static void Process(bool isOrg)
    {
        Console.WriteLine($"Enter {(isOrg ? "Organization" : "User")} name (case-sensitive) or 0 to go back:");
        var org = Console.ReadLine();
        org ??= "";
        if(org == "0") return;
        var reposRaw = (isOrg ? Client.Repository.GetAllForOrg(org) : Client.Repository.GetAllForUser(org)).Result.OrderBy(x => x.Name);
        var repos = reposRaw.Select(x => new RepositoryDescriptor(org, x.Name, isOrg)).ToList();
        while(true)
        {
            Console.WriteLine($"The following repos are available for this {(isOrg ? "Organization" : "User")}:");
            for(var i = 0; i < repos.Count; i++)
            {
                Console.WriteLine($"[{(C.WatchedRepos.Contains(repos[i]) ? "x" : " ")}] [{i + 1}] - {repos[i].RepoName}");
            }
            Console.WriteLine($"""
                Enter repository number to toggle it. You can enter multiple separated by space.
                - Enter "All" to select all repos
                - Enter "RF" to remove forks from selection
                - Enter "None" to remove selection from all repos
                - Enter "0" to return back
                """);
            var result = GetChoice(out var resultStr);
            if(result == 0)
            {
                return;
            }
            else if(result > 0 && result <= repos.Count)
            {
                C.WatchedRepos.Toggle(repos[result - 1]);
            }
            else if(resultStr.Equals("None", StringComparison.OrdinalIgnoreCase))
            {
                C.WatchedRepos.RemoveWhere(x => x.IsOrganization == isOrg && x.UserOrOrg == org);
            }
            else if(resultStr.Equals("rf", StringComparison.OrdinalIgnoreCase))
            {
                C.WatchedRepos.RemoveWhere(x => x.IsOrganization == isOrg && x.UserOrOrg == org && reposRaw.Any(r => r.Name == x.RepoName && r.Fork));
            }
            else if(resultStr.Equals("All", StringComparison.OrdinalIgnoreCase))
            {
                foreach(var x in repos)
                {
                    C.WatchedRepos.Add(x);
                }
            }
            else
            {
                var array = resultStr.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(x => int.TryParse(x, out var r) ? r : 0).Distinct();
                foreach(var x in array)
                {
                    if(x > 0 && x <= repos.Count)
                    {
                        C.WatchedRepos.Toggle(repos[x - 1]);
                    }
                }
            }
            SaveConfig();
        }
    }
}
