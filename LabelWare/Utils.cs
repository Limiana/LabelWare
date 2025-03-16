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
        return GetChoice(out _);
    }

    public static int GetChoice(out string ret)
    {
        ret = Console.ReadLine() ?? "";
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

    /// <summary>
    /// Adds <paramref name="value"/> into HashSet if it doesn't exists yet or removes if it exists.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="hashSet"></param>
    /// <param name="value"></param>
    /// <returns>Whether <paramref name="hashSet"/> contains <paramref name="value"/> after function has been executed.</returns>
    public static bool Toggle<T>(this HashSet<T> hashSet, T value)
    {
        if(hashSet.Contains(value))
        {
            hashSet.Remove(value);
            return false;
        }
        else
        {
            hashSet.Add(value);
            return true;
        }
    }

    public static bool Toggle<T>(this List<T> list, T value)
    {
        if(list.Contains(value))
        {
            list.RemoveAll(x => x.Equals(value));
            return false;
        }
        else
        {
            list.Add(value);
            return true;
        }
    }
}
