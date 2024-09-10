using Puya.CommandLine;
using System.Collections.Generic;

namespace Puya.Git
{
    public class GitWorkingChanges
    {
        public List<string> Modified { get; set; }
        public List<string> Deleted { get; set; }
        public List<string> Untracked { get; set; }
        public GitWorkingChanges()
        {
            Modified = new List<string>();
            Deleted = new List<string>();
            Untracked = new List<string>();
        }
    }
    public class GitStagedChanges
    {
        public List<string> Modified { get; set; }
        public List<string> Deleted { get; set; }
        public List<string> NewFile { get; set; }
        public GitStagedChanges()
        {
            Modified = new List<string>();
            Deleted = new List<string>();
            NewFile = new List<string>();
        }
    }
    public class GitStatusResponse
    {
        public ShellExecuteResponse ShellResponse { get; set; }
        public bool Success => ShellResponse != null && ShellResponse.Succeeded;
        public string Branch { get; set; }
        public string UpToDateWith { get;set; }
        public GitStagedChanges Staged { get; set; }
        public GitWorkingChanges Working { get; set; }
        public GitStatusResponse()
        {
            Staged = new GitStagedChanges();
            Working = new GitWorkingChanges();
        }
    }
}
