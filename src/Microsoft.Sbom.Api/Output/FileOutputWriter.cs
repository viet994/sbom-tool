// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Threading.Tasks;
using Microsoft.Sbom.Common.Config;

namespace Microsoft.Sbom.Api.Output
{
    /// <summary>
    /// Writes a string to a file.
    /// TODO Use serilog.
    /// </summary>
    public class FileOutputWriter : IOutputWriter
    {
        private readonly IContext context;

        public FileOutputWriter(IContext context)
        {
            this.context = context;
        }

        public async Task WriteAsync(string output)
        {
            using FileStream fs = new FileStream(context.OutputPath.Value, FileMode.Create);
            using StreamWriter outputFile = new StreamWriter(fs);
            await outputFile.WriteAsync(output);
        }
    }
}
