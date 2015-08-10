namespace OctoStyle.Installer
{
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
#else
            "Release";
#endif

            var octoStyleOutputPath = Path.Combine(@"..\..\src\OctoStyle.Console\bin\", mode);

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

            Compiler.WixLocation = @"..\..\packages\WiX.3.9.2\tools";
            Compiler.WixSdkLocation = @"..\..\packages\WiX.3.9.2\tools\sdk";
            Compiler.BuildMsi(project);
        }
    }
}
