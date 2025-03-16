using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelWare.DataFiles;
public unsafe struct RepositoryDescriptor : IEquatable<RepositoryDescriptor>
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

    public override bool Equals(object? obj)
    {
        return obj is RepositoryDescriptor descriptor && Equals(descriptor);
    }

    public bool Equals(RepositoryDescriptor other)
    {
        return UserOrOrg == other.UserOrOrg &&
               RepoName == other.RepoName &&
               IsOrganization == other.IsOrganization;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(UserOrOrg, RepoName, IsOrganization);
    }

    public override string ToString()
    {
        return $"{UserOrOrg}/{RepoName} ({(IsOrganization ? "Organization" : "Personal")})";
    }

    public static bool operator ==(RepositoryDescriptor left, RepositoryDescriptor right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(RepositoryDescriptor left, RepositoryDescriptor right)
    {
        return !(left == right);
    }
}
