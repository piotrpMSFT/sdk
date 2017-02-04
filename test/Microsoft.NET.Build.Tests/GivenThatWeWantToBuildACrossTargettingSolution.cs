// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Runtime.InteropServices;
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
        public void It_builds_solusuccessfully()
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
                .Execute()
                .Should()
                .Pass();

            buildCommand.GetOutputDirectory("netcoreapp1.0", Path.Combine("x64", "Debug"))
                .Should()
                .OnlyHaveFiles(new[] {
                    "x64SolutionBuild.runtimeconfig.dev.json",
                    "x64SolutionBuild.runtimeconfig.json",
                    "x64SolutionBuild.deps.json",
                    "x64SolutionBuild.dll",
                    "x64SolutionBuild.pdb"
                });
        }
    }
}
