// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Sbom.Api.Config;
using Microsoft.Sbom.Api.Manifest;
using Microsoft.Sbom.Api.Output.Telemetry;
using Microsoft.Sbom.Api.Utils;
using Microsoft.Sbom.Api.Workflows;
using Microsoft.Sbom.Common.Config.Validators;
using Microsoft.Sbom.Common.Config;
using Microsoft.Sbom.Contracts;
using Microsoft.Sbom.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using PowerArgs;

namespace Microsoft.Sbom.Api
{
    /// <summary>
    /// Responsible for an API to generate SBOMs.
    /// </summary>
    public class SBOMGenerator : ISBOMGenerator
    {
        private readonly IWorkflow<SBOMGenerationWorkflow> generationWorkflow;
        private readonly ManifestGeneratorProvider generatorProvider;
        private readonly IRecorder recorder;
        //private readonly IEnumerable<ConfigValidator> configValidators;
        //private readonly ConfigSanitizer configSanitizer;

        public SBOMGenerator(
            IWorkflow<SBOMGenerationWorkflow> generationWorkflow,
            ManifestGeneratorProvider generatorProvider,
            IRecorder recorder//,
            /*IEnumerable<ConfigValidator> configValidators,
            ConfigSanitizer configSanitizer*/)
        {
            this.generationWorkflow = generationWorkflow;
            this.generatorProvider = generatorProvider;
            this.recorder = recorder;
            //this.configValidators = configValidators;
            //this.configSanitizer = configSanitizer;
        }

        /// <inheritdoc />
        public async Task<SBOMGenerationResult> GenerateSBOMAsync(
            
            string rootPath
            //,string componentPath,
            //SBOMMetadata metadata,
            //IList<SBOMSpecification> specifications = null,
            //RuntimeConfiguration configuration = null,
            //string manifestDirPath = null,
            //string externalDocumentReferenceListFile = null
            )
        {
            // populate the context fields here from the primative inputs
            //ContextAdapter.SetBuildDropPath(new (rootPath));

            // pass in primative values
            // construct context
            // sanitize/validate the context

            bool isSuccess = await generationWorkflow.RunAsync(/*IContext*/);

            // TODO: Telemetry?
            await recorder.FinalizeAndLogTelemetryAsync();

            var entityErrors = recorder.Errors.Select(error => error.ToEntityError()).ToList();

            return new SBOMGenerationResult(isSuccess, entityErrors);
        }

        /// <inheritdoc />
        public IEnumerable<AlgorithmName> GetRequiredAlgorithms(SBOMSpecification specification)
        {
            ArgumentNullException.ThrowIfNull(specification);

            // The provider will throw if the generator is not found.
            var generator = generatorProvider.Get(specification.ToManifestInfo());

            return generator
                    .RequiredHashAlgorithms
                    .ToList();
        }

        public IEnumerable<SBOMSpecification> GetSupportedSBOMSpecifications()
        {
            return generatorProvider
                    .GetSupportedManifestInfos()
                    .Select(g => g.ToSBOMSpecification())
                    .ToList();
        }

        //private Configuration ValidateConfig(Configuration config)
        //{
        //    foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(config))
        //    {
        //        configValidators.ForEach(v =>
        //        {
        //            v.CurrentAction = config.ManifestToolAction;
        //            v.Validate(property.DisplayName, property.GetValue(config), property.Attributes);
        //        });
        //    }

        //    configSanitizer.SanitizeConfig(config);
        //    return config;
        //}
    }
}
