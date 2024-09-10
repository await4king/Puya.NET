using Puya.Git;
using System;

namespace Puya.CommandLine
{
    public static partial class Shell
    {
        public static GitStatusResponse GitStatus(string path = "")
        {
            var response = new GitStatusResponse();

            var sr = response.ShellResponse = Execute("git.exe", "status", path);

            if (sr.Succeeded)
            {
                if (!string.IsNullOrEmpty(sr.Output))
                {
                    var lines = sr.Output.Split('\n');
                    var staged = false;
                    var unstaged = false;
                    var untracked = false;
                    var file = "";

                    for (var i = 0; i < lines.Length; i++)
                    {
                        var line = lines[i]?.Trim();

                        if (string.IsNullOrEmpty(line))
                        {
                            continue;
                        }

                        if (line.StartsWith("on branch ", StringComparison.OrdinalIgnoreCase))
                        {
                            response.Branch = line.Substring(10).Trim();
                            continue;
                        }
                        if (line.StartsWith("Your branch is up to date with ", StringComparison.OrdinalIgnoreCase))
                        {
                            response.UpToDateWith = line.Substring(31).Trim();

                            if (response.UpToDateWith.Length > 0 && response.UpToDateWith[0] == '\'' && response.UpToDateWith[response.UpToDateWith.Length - 1] == '\'')
                            {
                                response.UpToDateWith = response.UpToDateWith.Substring(1, response.UpToDateWith.Length - 2);
                            }

                            continue;
                        }
                        if (line.StartsWith("Changes to be committed", StringComparison.OrdinalIgnoreCase))
                        {
                            staged = true;
                        }
                        if (line.StartsWith("Changes not staged", StringComparison.OrdinalIgnoreCase))
                        {
                            staged = false;
                            unstaged = true;
                        }
                        if (line.StartsWith("Untracked files", StringComparison.OrdinalIgnoreCase))
                        {
                            untracked = true;
                        }
                        if (line.StartsWith("modified:", StringComparison.OrdinalIgnoreCase))
                        {
                            file = line.Substring(9).Trim();

                            if (staged)
                            {
                                response.Staged.Modified.Add(file);
                            }
                            if (unstaged)
                            {
                                response.Working.Modified.Add(file);
                            }

                            continue;
                        }
                        if (line.StartsWith("deleted:", StringComparison.OrdinalIgnoreCase))
                        {
                            file = line.Substring(8).Trim();

                            if (staged)
                            {
                                response.Staged.Deleted.Add(file);
                            }
                            if (unstaged)
                            {
                                response.Working.Deleted.Add(file);
                            }

                            continue;
                        }
                        if (line.StartsWith("new file:", StringComparison.OrdinalIgnoreCase))
                        {
                            file = line.Substring(9).Trim();

                            if (staged)
                            {
                                response.Staged.Deleted.Add(file);
                            }

                            continue;
                        }
                        if (untracked)
                        {
                            if (line.IndexOf("git add", StringComparison.OrdinalIgnoreCase) < 0)
                            {
                                response.Working.Untracked.Add(line);
                            }
                        }
                    }
                }
            }

            return response;
        }
    }
}
