using LabelWare.Screens;
using Octokit;
using Octokit.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LabelWare;
public unsafe static class Utils
{
    public static bool IsNullOrEmpty(this string? s)
    {
        return string.IsNullOrEmpty(s);
    }

    public static void Log(this Exception e)
    {
        Console.WriteLine($"{e}");
    }

    public static int GetChoice()
    {
        var ret = Console.ReadLine();
        if(!ret.IsNullOrEmpty() && int.TryParse(ret, out var i))
        {
            return i;
        }
        return -1;
    }

    public static void CreateClient()
    {
        if(C.Token != "")
        {
            var cred = new InMemoryCredentialStore(new(C.Token, AuthenticationType.Bearer));
            Client = new GitHubClient(new("LabelWare"), cred);
            try
            {
                var limit = Client.RateLimit.GetRateLimits().Result;
                Console.WriteLine($"Token API Limit: {limit.Resources.Core.Remaining}/{limit.Resources.Core.Limit}, reset at {limit.Resources.Core.Reset.ToLocalTime()}");
            }
            catch(Exception e)
            {
                e.Log();
            }
        }
    }
}
