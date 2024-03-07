using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelWare;
public static class Utils
{
    public static ImmutableList<LabelConfiguration> LoadLabelConfiguration()
    {
        var ret = new List<LabelConfiguration>();
        try
        {
            var lst = File.ReadAllText("labels.csv").ReplaceLineEndings().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if(lst.Length < 2)
            {
                Console.WriteLine($"Please populate labels.csv");
            }
            var separator = ",";
            for (int i = 1; i < lst.Length; i++)
            {
                var array = lst[i].Split(separator);
                if (array.Length < 4) continue; 
                var entry = new LabelConfiguration(array[0], array[1], array[2], array[3..]);
                ret.Add(entry);
                Console.WriteLine($"Loaded {entry}");
            }
        }
        catch(Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        return ret.ToImmutableList();
    }

    /// <summary>
    /// Safely selects an entry of the list at a specified index, returning default value if index is out of range.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static T SafeSelect<T>(this IList<T> list, int index)
    {
        if (index < 0 || index >= list.Count) return default;
        return list[index];
    }

    /// <summary>
    /// Safely selects an entry of the array at a specified index, returning default value if index is out of range.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static T SafeSelect<T>(this T[] list, int index)
    {
        if (index < 0 || index >= list.Length) return default;
        return list[index];
    }
}
