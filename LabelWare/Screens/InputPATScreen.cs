using Octokit;
using Octokit.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelWare.Screens;
public unsafe class InputPATScreen
{
    public static void Run()
    {
        Console.WriteLine($"""
                Enter your personal access token. 
                It must have permissions to manage labels for every repository you have configured.
                Your token will be stored on your computer as a plain text. Dispose of this token once you finish using LabelWare.
                """);
        var str = Console.ReadLine();
        if(str.IsNullOrEmpty())
        {
            if(C.Token == "")
            {
                Console.WriteLine("Token remains unchanged");
            }
        }
        else
        {
            if(str == "0")
            {
                C.Token = "";
                SaveConfig();
                Console.WriteLine("Token removed from settings file");
            }
            else
            {
                C.Token = str!;
                SaveConfig();
                try
                {
                    CreateClient();
                    Console.WriteLine($"Token set successfully.");
                }
                catch(Exception e)
                {
                    Console.WriteLine($"""
                    Error validating token: 
                    {e}
                    """);
                }
            }
        }
    }
}
