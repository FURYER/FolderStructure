using System;
using System.IO;
using TextCopy;
using DotNetEnv;

class Program
{
    static void Main(string[] args)
    {
        Env.Load();

        string[] ignoreFolders = Env.GetString("IgnoreFolders", "").Split(',');

        if (args.Length == 0)
        {
            Console.WriteLine("Please provide a directory path.");
            return;
        }

        string path = args[0];
        if (path == null)
        {
            Console.WriteLine("Path is null.");
            return;
        }
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        if (directoryInfo.Exists)
        {
            var output = string.Empty;
            PrintDirectoryStructure(directoryInfo.FullName, 0, ref output, ignoreFolders);
            ClipboardService.SetText(output);
        }
        else
        {
            Console.WriteLine("The specified path does not exist or is inaccessible.");
        }
    }

    static void PrintDirectoryStructure(string dir, int level, ref string output, string[] ignoreFolders)
    {
        string indent = new string(' ', level * 4);

        DirectoryInfo directory = new DirectoryInfo(dir);

        if (ignoreFolders.Contains(directory.Name, StringComparer.OrdinalIgnoreCase))
        {
            return;
        }

        output += $"{indent}/{directory.Name}\n";

        foreach (var subDir in directory.GetDirectories())
        {
            PrintDirectoryStructure(subDir.FullName, level + 1, ref output, ignoreFolders);
        }

        foreach (var file in directory.GetFiles())
        {
            output += $"{indent}    {file.Name}\n";
        }
    }
}
