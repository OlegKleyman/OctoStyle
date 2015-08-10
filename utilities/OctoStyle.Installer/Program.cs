namespace OctoStyle.Installer
{
    using System;
    using System.IO;

    using WixSharp;

    using File = WixSharp.File;

    public class Program
    {
        private static void Main()
        {
            const string mode = 
#if DEBUG
            "Debug";
#elif FULL
            "Full";
#else
            "Release";
#endif
            const string rootSolutionDirectory = @"..\..";
            var octoStyleOutputPath = Path.Combine(rootSolutionDirectory, @"src\OctoStyle.Console\bin\", mode);

            var project =
            new Project("OctoStyle",
                new Dir(@"%ProgramFiles%\Omego2K\OctoStyle",
                    new File(Path.Combine(octoStyleOutputPath, "OctoStyle.Console.exe")),
                    new File(Path.Combine(octoStyleOutputPath, "OctoStyle.Core.dll")),
                    new File(Path.Combine(octoStyleOutputPath, "NDesk.Options.dll")),
                    new File(Path.Combine(octoStyleOutputPath, "Octokit.dll")),
                    new File(Path.Combine(octoStyleOutputPath, "StyleCop.CSharp.Rules.dll")),
                    new File(Path.Combine(octoStyleOutputPath, "StyleCop.dll")),
                    new File(Path.Combine(octoStyleOutputPath, "StyleCop.CSharp.dll"))));

            project.GUID = new Guid("B6FF0CA5-1560-441D-97BA-55D568B3D332");

            Compiler.WixLocation = Path.Combine(rootSolutionDirectory, @"packages\WiX.3.9.2\tools");
            Compiler.WixSdkLocation = Path.Combine(rootSolutionDirectory, @"packages\WiX.3.9.2\tools\sdk");
            Compiler.BuildMsi(project);
        }
    }
}
