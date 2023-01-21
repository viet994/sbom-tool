// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using AutoMapper;
using PowerArgs;
using System.Threading.Tasks;
using Microsoft.Sbom.Api.Config.Args;
using Microsoft.Sbom.Common.Config;

namespace Microsoft.Sbom.Api.Config
{
    /// <summary>
    /// Converts the command line arguments and config file parameters to <see cref="ConfigurationSetting{T}"/> objects.
    /// Finally combines the two into one <see cref="IConfiguration"/> object.
    /// 
    /// Throws an error if the same parameters are defined in both the config file and command line.
    /// </summary>
    /// <typeparam name="T">The action args parameter.</typeparam>
    public class ContextBuilder<T> : IContextBuilder<T>
        where T : CommonArgs
    {
        private readonly IMapper mapper;
        private readonly ConfigFileParser configFileParser;

        public ContextBuilder(IMapper mapper, ConfigFileParser configFileParser)
        {
            this.mapper = mapper;
            this.configFileParser = configFileParser;
        }

        public IContext GetContext(T args)
        {
            //IContext commandLineArgs;

            // Set current action for the config validators and convert command line arguments to configuration
            return args switch
            {
                ValidationArgs validationArgs => mapper.Map<ContextAdapter>(validationArgs),
                GenerationArgs generationArgs => mapper.Map<ContextAdapter>(generationArgs),
                _ => throw new ValidationArgException($"Unsupported configuration type found {typeof(T)}")
            };


            // Read config file if present, or use default.
            //var configFromFile = commandLineArgs.ConfigFilePath != null ?
            //                            await configFileParser.ParseFromJsonFile(commandLineArgs.ConfigFilePath.Value) :
            //                            new ConfigFile();

            // Convert config file arguments to configuration.
            //var configFileArgs = mapper.Map<ConfigFile, Configuration>(configFromFile);

            // Combine both configs, include defaults.
            //return mapper.Map(commandLineArgs, configFileArgs);
        }
    }

    public interface IContextBuilder<T> 
        where T : CommonArgs
    {
        IContext GetContext(T args);
    }
}