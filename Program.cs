using System;
using System.IO;
using TextCopy;
using DotNetEnv;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var enc1251 = Encoding.GetEncoding(1251);
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = enc1251;


        Env.Load();

        string[] ignoreFolders = Env.GetString("IgnoreFolders", "").Split(',');

        Console.WriteLine("Please enter a directory path:");
        string? path = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(path))
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
            output += $"{indent}/{directory.Name}\n";
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
