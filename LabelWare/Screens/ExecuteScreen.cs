using LabelWare.DataFiles;

namespace LabelWare.Screens;
public unsafe class ExecuteScreen
{
    public static void Run()
    {
        try
        {
            Console.WriteLine($"""
            {C.WatchedRepos.Count} repositories will have their labels REPLACED with labels from {C.SourceRepository}:
            {string.Join("\n", C.WatchedRepos.Select(X => X.ToString()))}
            Continue?
            [0] - No
            [1] - Yes
            """);
            var choice = GetChoice();
            if(choice != 1) return;
            List<RepositoryDescriptor> success = [];
            List<RepositoryDescriptor> fail = [];
            var sourceLabels = Client.Issue.Labels.GetAllForRepository(C.SourceRepository.UserOrOrg, C.SourceRepository.RepoName).Result;
            Console.WriteLine("Source labels:");
            foreach(var x in sourceLabels)
            {
                Console.WriteLine($"> {x.Name} ({x.Description}) #{x.Color}");
            }
            foreach(var repo in C.WatchedRepos)
            {
                try
                {
                    Console.WriteLine($"=========================\n> Now processing {repo}");
                    if(repo == C.SourceRepository)
                    {
                        Console.WriteLine("Can not process source repo");
                        continue;
                    }
                    var targetLabels = Client.Issue.Labels.GetAllForRepository(repo.UserOrOrg, repo.RepoName).Result;
                    foreach(var label in targetLabels)
                    {
                        var matchingSource = sourceLabels.FirstOrDefault(x => x.Name.Equals(label.Name, StringComparison.OrdinalIgnoreCase));
                        if(matchingSource != null)
                        {
                            if(matchingSource.Color == label.Color && matchingSource.Name == label.Name && matchingSource.Description == label.Description)
                            {
                                Console.WriteLine($">> Label {label.Name} is perfect, no change needed");
                            }
                            else
                            {
                                Console.WriteLine($">> Correcting label {label.Name}");
                                Client.Issue.Labels.Update(repo.UserOrOrg, repo.RepoName, label.Name, new(matchingSource.Name, matchingSource.Color) { Description = matchingSource.Description });
                            }
                        }
                        else
                        {
                            Console.WriteLine($">> Deleting label {label.Name}");
                            Client.Issue.Labels.Delete(repo.UserOrOrg, repo.RepoName, label.Name);
                        }
                    }
                    foreach(var newLabel in sourceLabels)
                    {
                        if(!targetLabels.Any(x => x.Name.Equals(newLabel.Name, StringComparison.OrdinalIgnoreCase)))
                        {
                            Console.WriteLine($">> Adding label {newLabel.Name}");
                            Client.Issue.Labels.Create(repo.UserOrOrg, repo.RepoName, new(newLabel.Name, newLabel.Color) { Description = newLabel.Description });
                        }
                    }
                    success.Add(repo);
                }
                catch(Exception e)
                {
                    e.Log();
                    fail.Add(repo);
                }
            }
            Console.WriteLine($"""
                
                ====================
                Successfully processed {success.Count} repos:
                {string.Join("\n", success.Select(x => $"- {x}"))}
                ====================
                Failed to processed {fail.Count} repos:
                {string.Join("\n", fail.Select(x => $"- {x}"))}
                """);
        }
        catch(Exception e)
        {
            e.Log();
        }
    }
}
