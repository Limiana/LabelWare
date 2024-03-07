using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelWare;
#nullable disable
public class LabelConfiguration
{
    public string Name;
    public string Color;
    public string Description;
    public string[] Replace;

    public LabelConfiguration(string name, string color, string description, string[] replace)
    {
        this.Name = name;
        this.Color = color.Replace($"#", "");
        if (Color.Length != 6) throw new InvalidDataException($"Color must be 6 digit hexadecimal number, was {Color}");
        this.Description = description;
        if (Description.Length > 100) throw new InvalidDataException("Description must be under 100 symbols");
        this.Replace = replace.Where(x => x.Length > 0).ToArray();
    }

    public override string ToString()
    {
        return $"[{Name}|{Color}|{Description}|{string.Join(", ", Replace)}]";
    }
}
