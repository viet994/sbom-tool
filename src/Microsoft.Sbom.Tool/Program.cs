// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Sbom.Api;
using Microsoft.Sbom.Api.Config;
using Microsoft.Sbom.Api.Config.Args;
using Microsoft.Sbom.Common;
using Microsoft.Sbom.Common.Config;
using Microsoft.Sbom.Contracts;
using Microsoft.Sbom.Extensions.DependencyInjection;
using PowerArgs;

namespace Microsoft.Sbom.Tool
{
    internal class Program
    {
        internal static string Name => NameValue.Value;

        internal static string Version => VersionValue.Value;

        private static readonly Lazy<string> NameValue = new Lazy<string>(() =>
        {
            return typeof(Program).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product ?? "sbomtool";
        });

        private static readonly Lazy<string> VersionValue = new Lazy<string>(() =>
        {
            return typeof(Program).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? string.Empty;
        });

        public static async Task Main(string[] args)
        {
            // Parse the CLI args
            var actionArgs = (await Args.InvokeActionAsync<SbomToolCmdRunner>(args)).ActionArgs;

            // Set the IoC container
            using var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((host, services) =>
                {
                    services = actionArgs switch
                    {
                        ValidationArgs v => services.AddHostedService<ValidationService>(),
                        GenerationArgs g => services.AddHostedService<GenerationService>(),
                        _ => services
                    };

                    services
                        .AddTransient(_ => FileSystemUtilsProvider.CreateInstance())
                        .AddTransient<ConfigFileParser>()
                        .AddSingleton(typeof(IConfigurationBuilder<>), typeof(ConfigurationBuilder<>))
                        .AddSingleton<IConfiguration>(x =>
                        {
                            var validationConfigurationBuilder = x.GetService<IConfigurationBuilder<ValidationArgs>>();
                            var generationConfigurationBuilder = x.GetService<IConfigurationBuilder<GenerationArgs>>();
                            var configuration = actionArgs switch
                            {
                                ValidationArgs v => validationConfigurationBuilder.GetConfiguration(v).GetAwaiter().GetResult(),
                                GenerationArgs g => generationConfigurationBuilder.GetConfiguration(g).GetAwaiter().GetResult(),
                                _ => default
                            };
                            return configuration;
                        })
                        .AddSbomTool();

                    //var config = ApiConfigurationBuilder.GetGenerationConfiguration(
                    //    buildDropPath: @"C:\Users\ksigmund\dev\CronFu",
                    //    outputPath: @"C:\Users\ksigmund\dev\temp",
                    //    specifications: new List<SBOMSpecification>() { new SBOMSpecification("SPDX", "2.2") },
                    //    manifestDirPath: @"C:\Users\ksigmund\dev\temp\_manifest",
                    //    validateSignature: false,
                    //    ignoreMissing: false,
                    //    rootPathFilter: null,
                    //    runtimeConfiguration: null,
                    //    packageName: "somepackagename",
                    //    packageVersion: "somepacakgeversion",
                    //    packageSupplier: "somepackagesupplier",
                    //    namespaceBaseUri: "mynamespace");

                    //services.AddSbomConfiguration(config);
                })
                .UseConsoleLifetime()
                .Build();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ContextProfile());
            });

            var mapper = new Mapper(mapperConfig);

            // Populate the context outside of the container
            switch (actionArgs)
            {
                case ValidationArgs v:
                    {
                        // validationContextBuilder
                        ContextAdapter.SetBuildDropPath(new (v.BuildDropPath));
                        ContextAdapter.SetOutputPath(new (v.OutputPath));
                        break;
                    }

                case GenerationArgs g:
                    {
                        // generationContextBuilder
                        var fileSystemUtils = FileSystemUtilsProvider.CreateInstance();
                        var configFileParser = new ConfigFileParser(fileSystemUtils);
                        var generationContextBuilder = new ContextBuilder<GenerationArgs>(mapper, configFileParser);

                        var foo = generationContextBuilder.GetContext(g);
                        //ContextAdapter.SetBuildDropPath(new (g.BuildDropPath));
                        //ContextAdapter.SetBuildComponentPath(new (g.BuildComponentPath));
                        //ContextAdapter.SetManifestDirPath(new (g.ManifestDirPath));
                        break;
                    }
            }

            // Run the service
            await host.RunAsync();

         }
    }
}
