// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Runtime.InteropServices;
using Microsoft.DotNet.InternalAbstractions;
using Microsoft.NET.TestFramework;
using Microsoft.NET.TestFramework.Assertions;
using Microsoft.NET.TestFramework.Commands;
using Xunit;
using static Microsoft.NET.TestFramework.Commands.MSBuildTest;

namespace Microsoft.NET.Build.Tests
{
    public class GivenThatWeWantToBuildACrossTargettingSolution : SdkTest
    {
        [Fact]
        public void It_builds_successfully_for_netcoreapp10()
        {
            var testAsset = _testAssetsManager
                .CopyTestAsset("CrossTargettingSolution")
                .WithSource();

            var restoreCommand = new RestoreCommand(Stage0MSBuild, testAsset.TestRoot, "CrossTargettingSolution.sln");
            restoreCommand
                .Execute()
                .Should()
                .Pass();

            var buildCommand = new BuildCommand(Stage0MSBuild, testAsset.TestRoot, "CrossTargettingSolution.sln");
            buildCommand
                .Execute("/p:TargetFramework=netcoreapp1.0")
                .Should()
                .Pass();

            new DirectoryInfo(Path.Combine(buildCommand.ProjectRootPath, "NetCoreApp", "bin", "Debug", "netcoreapp1.0"))
                .Should()
                .OnlyHaveFiles(new[] {
                    "DesktopAndNetStandard.dll",
                    "DesktopAndNetStandard.pdb",
                    "NetCoreApp.deps.json",
                    "NetCoreApp.dll",
                    "NetCoreApp.pdb",
                    "NetCoreApp.runtimeconfig.dev.json",
                    "NetCoreApp.runtimeconfig.json"
                });
        }

        [Fact]
        public void It_builds_output_for_netstandard15()
        {
            var testAsset = _testAssetsManager
                .CopyTestAsset("CrossTargettingSolution")
                .WithSource();

            var restoreCommand = new RestoreCommand(Stage0MSBuild, testAsset.TestRoot, "CrossTargettingSolution.sln");
            restoreCommand
                .Execute()
                .Should()
                .Pass();

            var buildCommand = new BuildCommand(Stage0MSBuild, testAsset.TestRoot, "CrossTargettingSolution.sln");
            buildCommand
                .Execute("/p:TargetFramework=netstandard1.5")
                .Should()
                .Pass();

            new DirectoryInfo(Path.Combine(buildCommand.ProjectRootPath, "DesktopAndNetStandard", "bin", "Debug", "netstandard1.5"))
                .Should()
                .OnlyHaveFiles(new[] {
                    "DesktopAndNetStandard.deps.json",
                    "DesktopAndNetStandard.dll",
                    "DesktopAndNetStandard.pdb"
                });
        }
        [Fact]
        public void It_builds_output_for_the_current_RID()
        {
            var rid = RuntimeEnvironment.GetRuntimeIdentifier();
            
            var testAsset = _testAssetsManager
                .CopyTestAsset("CrossTargettingSolution")
                .WithSource();

            var restoreCommand = new RestoreCommand(Stage0MSBuild, testAsset.TestRoot, "CrossTargettingSolution.sln");
            restoreCommand
                .Execute($"/p:RuntimeIdentifier={rid}")
                .Should()
                .Pass();

            var buildCommand = new BuildCommand(Stage0MSBuild, testAsset.TestRoot, "CrossTargettingSolution.sln");
            buildCommand
                .Execute("/p:TargetFramework=netcoreapp1.0", $"/p:RuntimeIdentifier={rid}")
                .Should()
                .Pass();

            new DirectoryInfo(Path.Combine(buildCommand.ProjectRootPath, "DesktopAndNetStandard", "bin", "Debug", "netstandard1.5"))
                .Should()
                .OnlyHaveFiles(new[] {
                    "DesktopAndNetStandard.deps.json",
                    "DesktopAndNetStandard.dll",
                    "DesktopAndNetStandard.pdb"
                });
        }
    }
}
