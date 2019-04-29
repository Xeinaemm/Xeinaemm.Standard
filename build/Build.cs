using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.AppVeyor;
using Nuke.Common.CI.AzurePipelines;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.CI.TeamCity;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Tools.DotCover;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.InspectCode;
using Nuke.Common.Tools.ReportGenerator;
using Nuke.Common.Tools.Slack;
using Nuke.Common.Utilities;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.ChangeLog.ChangelogTasks;
using static Nuke.Common.ControlFlow;
using static Nuke.Common.Gitter.GitterTasks;
using static Nuke.Common.IO.CompressionTasks;
using static Nuke.Common.Tools.NuGet.NuGetTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.Git.GitTasks;
using static Nuke.Common.Tools.InspectCode.InspectCodeTasks;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.ReportGenerator.ReportGeneratorTasks;
using static Nuke.Common.Tools.Slack.SlackTasks;
using Nuke.Common.Tools.NuGet;

[CheckBuildProjectConfigurations]
[DotNetVerbosityMapping]
[UnsetVisualStudioEnvironmentVariables]
[AzurePipelines(
    AzurePipelinesImage.WindowsLatest,
    InvokedTargets = new[] { nameof(Pack) },
    NonEntryTargets = new[] { nameof(Restore) },
    ExcludedTargets = new[] { nameof(Clean) })]
partial class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.Pack);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter] bool IgnoreFailedSources;
    [Parameter("NuGet Api Key")] readonly string ApiKey;
    [Parameter("NuGet Source for Packages")] readonly string Source = "https://pkgs.dev.azure.com/Xeinaemm/Jarvis/_packaging/Jarvis-public/nuget/v3/index.json";

    [Solution] readonly Solution Solution;
    [GitVersion] readonly GitVersion GitVersion;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
    AbsolutePath PackagesDirectory => ArtifactsDirectory / "packages";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("*/bin", "*/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetInformationalVersion(GitVersion.InformationalVersion)
                .EnableNoRestore());
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(_ => _
                .SetProjectFile(Solution)
                .SetIgnoreFailedSources(IgnoreFailedSources));
        });

    Target Pack => _ => _
    .DependsOn(Compile)
    .Produces(PackagesDirectory / "*.nupkg")
    .Executes(() =>
    {
        DotNetPack(_ => _
            .SetProject(Solution)
            .SetNoBuild(InvokedTargets.Contains(Compile))
            .SetConfiguration(Configuration)
            .SetOutputDirectory(PackagesDirectory)
            .SetVersion(GitVersion.NuGetVersionV2));
    });

    Target Publish => _ => _
        .DependsOn(Clean, Pack)
        .Consumes(Pack)
        .Requires(() => ApiKey)
        .Requires(() => Configuration.Equals(Configuration.Release))
        .Executes(() =>
        {
            var packages = PackagesDirectory.GlobFiles("*.nupkg");

            DotNetNuGetPush(_ => _
                    .SetSource(Source)
                    .SetApiKey(ApiKey)
                    .SetArgumentConfigurator(x => x.Add("--interactive"))
                    .SetTargetPath(PackagesDirectory / "*.nupkg"));
        });
}
