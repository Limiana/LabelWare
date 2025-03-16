using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelWare.DataFiles;
public unsafe class Configuration
{
    public string Token = "";
    public RepositoryDescriptor SourceRepository = new();
    public List<RepositoryDescriptor> WatchedRepos = [];
}
