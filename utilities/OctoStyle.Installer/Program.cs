namespace OctoStyle.Installer
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;

    using OctoStyle.Core;

    using WixSharp;

    using File = WixSharp.File;

    /// <summary>
    /// Contains application level methods.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The entry method into the application.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1303:ConstFieldNamesMustBeginWithUpperCaseLetter",
            Justification = StyleCopConstants.LocalConstantJustification)]
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

            var project = new Project(
                "OctoStyle",
                new Dir(
                    @"%ProgramFiles%\Omego2K\OctoStyle",
                    new File(Path.Combine(octoStyleOutputPath, "OctoStyle.Console.exe")),
                    new File(Path.Combine(octoStyleOutputPath, "OctoStyle.Core.dll")),
                    new File(Path.Combine(octoStyleOutputPath, "NDesk.Options.dll")),
                    new File(Path.Combine(octoStyleOutputPath, "Octokit.dll")),
                    new File(Path.Combine(octoStyleOutputPath, "StyleCop.CSharp.Rules.dll")),
                    new File(Path.Combine(octoStyleOutputPath, "StyleCop.dll")),
                    new File(Path.Combine(octoStyleOutputPath, "StyleCop.CSharp.dll"))));

            project.GUID = new Guid("B6FF0CA5-1560-441D-97BA-55D568B3D332");

            const string wixToolsLocation = @"packages\WiX.3.9.2\tools";

            Compiler.WixLocation = Path.Combine(rootSolutionDirectory, wixToolsLocation);
            Compiler.WixSdkLocation = Path.Combine(rootSolutionDirectory, wixToolsLocation, "sdk");
            Compiler.BuildMsi(project);
        }
    }
}
