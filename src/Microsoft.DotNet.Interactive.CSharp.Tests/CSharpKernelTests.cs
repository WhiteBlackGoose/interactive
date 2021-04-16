// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.DotNet.Interactive.Commands;
using Microsoft.DotNet.Interactive.Tests;
using Xunit;
using Xunit.Abstractions;
using System;
using Microsoft.DotNet.Interactive.Events;

namespace Microsoft.DotNet.Interactive.CSharp.Tests
{
    public class CSharpKernelTests : LanguageKernelTestBase
    {
        public CSharpKernelTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Script_state_is_available_within_middleware_pipeline()
        {
            var variableCountBeforeEvaluation = 0;
            var variableCountAfterEvaluation = 0;

            using var kernel = new CSharpKernel();

            kernel.AddMiddleware(async (command, context, next) =>
            {
                var k = context.HandlingKernel as CSharpKernel;

                await next(command, context);

                variableCountAfterEvaluation = k.ScriptState.Variables.Length;
            });

            await kernel.SendAsync(new SubmitCode("var x = 1;"));

            variableCountBeforeEvaluation.Should().Be(0);
            variableCountAfterEvaluation.Should().Be(1);
        }

        [Fact]
        public async Task Instruction_r_should_not_raise_exceptions()
        {
            using var kernel = new CSharpKernel().UseNugetDirective();
            var rInstruction = new SubmitCode("#r \"nuget:Newtonsoft.Json,13.0.1\"");

            var result = await kernel.SendAsync(rInstruction);
            result.KernelEvents.Subscribe(eventSub => eventSub.Should().NotBeOfType<CommandFailed>());

            result = await kernel.SendAsync(new SubmitCode("1 + 1"));
            result.KernelEvents.Subscribe(eventSub => eventSub.Should().NotBeOfType<CommandFailed>());
        }
    }
}