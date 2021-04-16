// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using Microsoft.DotNet.Interactive.Commands;
using Microsoft.DotNet.Interactive.FSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.DotNet.Interactive.Jupyter.Tests
{
    public class CancelTests
    {
        [Fact] // #776
        public async Task Cancel_infinite_loop_happens_within_3sec()
        {
             using var kernel = new FSharpKernel();

             var code = 
@"
let mutable x = 0
while true do
    x <- x + 1
";
            var submitCode = new SubmitCode(code);

            var task = kernel.SendAsync(submitCode);  // we don't want to await it. It's infinite, what we can do is only to interrupt

            var interruption = kernel.SendAsync(new Cancel()); // cancelling

            await Task.Delay(3000); // let's wait until something happens

            task.IsCompleted.Should().BeTrue();
        }
    }
}
