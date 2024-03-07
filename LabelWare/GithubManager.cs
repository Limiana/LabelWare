using AdysTech.CredentialManager;
using Octokit;
using Octokit.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LabelWare;
public class GithubManager
{
    public string? PAT;
    public GithubManager()
    {
        
    }

    public async Task Run(bool wet)
    {
        if(Program.Configuration.Count == 0)
        {
            Console.WriteLine("Configuration is not loaded");
            return;
        }
        if(PAT == null)
        {
            Console.WriteLine("Token is not set");
            return;
        }
        var client = new GitHubClient(new ProductHeaderValue("LavelWare"), new InMemoryCredentialStore(new(PAT, AuthenticationType.Bearer)));
        var repos = await client.Repository.GetAllForUser("Limiana");
        foreach(var repo in repos)
        {
            try
            {
                var addLabels = Program.Configuration.ToList() ;
                Console.WriteLine($"  Repository {repo.FullName}");
                if(repo.Fork)
                {
                    Console.WriteLine($"    Is fork, skipping");
                    continue;
                }
                var labels = await client.Issue.Labels.GetAllForRepository(repo.Id);
                foreach (var existingLabel in labels)
                {
                    Console.WriteLine($"    Label {existingLabel.Name}");
                    var newLabel = addLabels.FirstOrDefault(x => x.Name.Equals(existingLabel.Name, StringComparison.OrdinalIgnoreCase));
                    if (newLabel != null)
                    {
                        Console.WriteLine($"      Exists.");
                        addLabels.Remove(newLabel);
                        var needsUpdate = false;
                        if (newLabel.Name != existingLabel.Name)
                        {
                            Console.WriteLine($"      Name mismatch, {newLabel.Name} != {existingLabel.Name}.");
                            needsUpdate = true;
                        }
                        if (newLabel.Description != existingLabel.Description)
                        {
                            Console.WriteLine($"      Description mismatch, {newLabel.Description} != {existingLabel.Description}.");
                            needsUpdate = true;
                        }
                        if (!newLabel.Color.Equals(existingLabel.Color, StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine($"      Color mismatch, {newLabel.Color} != {existingLabel.Color}.");
                            needsUpdate = true;
                        }
                        if (needsUpdate)
                        {
                            if (wet)
                            {
                                await client.Issue.Labels.Update(repo.Id, existingLabel.Name, new LabelUpdate(newLabel.Name, newLabel.Color) { Description = newLabel.Description });
                                Console.WriteLine($"      Label updated.");
                            }
                            else
                            {
                                Console.WriteLine($"      Would update this label.");
                            }
                        }
                    }
                    else
                    {
                        if (wet)
                        {
                            await client.Issue.Labels.Delete(repo.Id, existingLabel.Name);
                            Console.WriteLine($"      Label {existingLabel.Name} deleted.");
                        }
                        else
                        {
                            Console.WriteLine($"      Not exists and would be deleted.");
                        }
                    }
                }
                foreach (var newLabel in addLabels)
                {
                    if (wet)
                    {
                        await client.Issue.Labels.Create(repo.Id, new(newLabel.Name, newLabel.Color) { Description = newLabel.Description });
                        Console.WriteLine($"      Label {newLabel} added.");
                    }
                    else
                    {
                        Console.WriteLine($"  Would add {newLabel}");
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
