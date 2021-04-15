﻿// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.CommandLine.Parsing;

#nullable enable

namespace Microsoft.DotNet.Interactive.Parsing
{
    internal class DirectiveLanguageService : LanguageSpecificParseResult
    {
        private ParseResult? _parseResult;

        public DirectiveLanguageService(
            Parser DirectiveParser,
            DirectiveNode node)
        {
            this.DirectiveParser = DirectiveParser;
            Node = node;
        }

        public Parser DirectiveParser { get; }

        public DirectiveNode Node { get; }


        private void EnsureParsed()
        {
            if (_parseResult is null)
            {
                _parseResult = DirectiveParser.Parse(Node.Text);
            }
        }
    }
}