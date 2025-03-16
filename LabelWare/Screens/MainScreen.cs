﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LabelWare.Screens;
public unsafe class MainScreen
{
    public static string? Prepend = null;
    public static void Run()
    {
        while(true)
        {
            Console.WriteLine($"""
                == Welcome to LabelWare by NightmareXIV ==
                Please select an option by typing a number and pressing "Enter":
                [0] - Exit
                [1] - Input access token
                [2] - Configure source repository
                [3] - Add new target repository
                [4] - View current repository list
                """);
            if(Client == null) CreateClient();
            var result = Console.ReadLine();
            if(result != null && int.TryParse(result, out var value))
            {
                try
                {
                    if(value == 0)
                    {
                        return;
                    }
                    if(value == 1)
                    {
                        InputPATScreen.Run();
                    }
                    if(value == 2)
                    {
                        ConfigureSourceRepoScreen.Run();
                    }
                }
                catch(Exception e)
                {
                    e.Log();
                }
            }
        }
    }
}
