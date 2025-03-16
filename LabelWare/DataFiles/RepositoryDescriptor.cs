using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelWare.DataFiles;
public unsafe class RepositoryDescriptor
{
    public string UserOrOrg = "";
    public string RepoName = "";
    public bool IsOrganization = false;

    public RepositoryDescriptor()
    {
    }

    public RepositoryDescriptor(string userOrOrg, string repoName, bool isOrganization)
    {
        UserOrOrg = userOrOrg ?? throw new ArgumentNullException(nameof(userOrOrg));
        RepoName = repoName ?? throw new ArgumentNullException(nameof(repoName));
        IsOrganization = isOrganization;
    }

    public override string ToString()
    {
        return $"{UserOrOrg}/{RepoName} ({(IsOrganization ? "Organization" : "Personal")})";
    }
}
