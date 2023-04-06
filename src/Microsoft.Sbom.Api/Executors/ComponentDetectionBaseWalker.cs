﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Sbom.Api.Config.Extensions;
using Microsoft.Sbom.Extensions;
using Microsoft.ComponentDetection.Common;
using Microsoft.ComponentDetection.Contracts;
using Microsoft.ComponentDetection.Contracts.BcdeModels;
using Microsoft.Sbom.Api.Exceptions;
using Microsoft.Sbom.Api.Utils;
using Microsoft.Sbom.Common.Config;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Channels;
using System.Threading.Tasks;
using ILogger = Serilog.ILogger;

namespace Microsoft.Sbom.Api.Executors
{
    /// <summary>
    /// Abstract class that runs component detection tool in the given folder.
    /// </summary>
    public abstract class ComponentDetectionBaseWalker
    {
        private readonly ILogger log;
        private readonly ComponentDetectorCachedExecutor componentDetector;
        private readonly IConfiguration configuration;
        private readonly ISbomConfigProvider sbomConfigs;

        private ComponentDetectionCliArgumentBuilder cliArgumentBuilder;

        public ComponentDetectionBaseWalker(
            ILogger log,
            ComponentDetectorCachedExecutor componentDetector,
            IConfiguration configuration,
            ISbomConfigProvider sbomConfigs)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.componentDetector = componentDetector ?? throw new ArgumentNullException(nameof(componentDetector));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.sbomConfigs = sbomConfigs ?? throw new ArgumentNullException(nameof(sbomConfigs)); 
        }

        public (ChannelReader<ScannedComponent> output, ChannelReader<ComponentDetectorException> error) GetComponents(string buildComponentDirPath)
        {
            log.Debug($"Scanning for packages under the root path {buildComponentDirPath}.");

            // If the buildComponentDirPath is null or empty, make sure we have a ManifestDirPath create a new temp directory with a random name.
            if (!Directory.Exists(configuration.BuildComponentPath?.Value) && Directory.Exists(configuration.ManifestDirPath?.Value))
            {
                log.Debug($"The build component directory path {buildComponentDirPath} does not exist. Creating a new temp directory.");
                buildComponentDirPath = IConfiguration.RandomTempPath;
                Directory.CreateDirectory(buildComponentDirPath);
            }

            var verbosity = configuration.Verbosity.Value switch
            {
                LogEventLevel.Verbose => VerbosityMode.Verbose,
                _ => VerbosityMode.Normal,
            };

            cliArgumentBuilder = new ComponentDetectionCliArgumentBuilder().Scan().Verbosity(verbosity);

            // Enable SPDX22 detector which is disabled by default.
            cliArgumentBuilder.AddDetectorArg("SPDX22SBOM", "EnableIfDefaultOff");

            if (sbomConfigs.TryGet(Constants.SPDX22ManifestInfo, out ISbomConfig spdxSbomConfig))
            {
                var directory = Path.GetDirectoryName(spdxSbomConfig.ManifestJsonFilePath);
                if (!string.IsNullOrEmpty(directory))
                {
                    cliArgumentBuilder.AddArg("DirectoryExclusionList", directory);
                }
            }

            var output = Channel.CreateUnbounded<ScannedComponent>();
            var errors = Channel.CreateUnbounded<ComponentDetectorException>();

            if (string.IsNullOrEmpty(buildComponentDirPath))
            {
                output.Writer.Complete();
                errors.Writer.Complete();
                return (output, errors);
            }

            async Task Scan(string path)
            {
                cliArgumentBuilder.SourceDirectory(buildComponentDirPath);
                var cmdLineParams = configuration.ToComponentDetectorCommandLineParams(cliArgumentBuilder);

                var scanResult = await componentDetector.ScanAsync(cmdLineParams);

                if (scanResult.ResultCode != ProcessingResultCode.Success)
                {
                    await errors.Writer.WriteAsync(new ComponentDetectorException($"Component detector failed. Result: {scanResult.ResultCode}."));
                    return;
                }

                var uniqueComponents = FilterScannedComponents(scanResult);

                foreach (var component in uniqueComponents)
                {
                    await output.Writer.WriteAsync(component);
                }
            }

            Task.Run(async () =>
            {
                try
                {
                    await Scan(buildComponentDirPath);
                }
                catch (Exception e)
                {
                    log.Error($"Unknown error while running CD scan: {e}");
                    await errors.Writer.WriteAsync(new ComponentDetectorException("Unknown exception", e));
                    return;
                }
                finally
                {
                    output.Writer.Complete();
                    errors.Writer.Complete();
                }
            });

            return (output, errors);
        }

        protected abstract IEnumerable<ScannedComponent> FilterScannedComponents(ScanResult result);
    }
}
