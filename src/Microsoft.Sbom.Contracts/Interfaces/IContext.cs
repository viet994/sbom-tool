//using Microsoft.Sbom.Common.Config;
//using Microsoft.Sbom.Common.Config.Attributes;
using Microsoft.Sbom.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Sbom.Contracts.Interfaces
{
    //public interface IContext
    //{
    //    /// <summary>
    //    /// Gets or sets the root folder of the drop directory to validate or generate.
    //    /// </summary>
    //    ConfigurationSetting<string> BuildDropPath { get; set; } // context

    //    /// <summary>
    //    /// Gets or sets the folder containing the build components and packages.
    //    /// </summary>
    //    ConfigurationSetting<string> BuildComponentPath { get; set; } // context

    //    /// <summary>
    //    /// Gets or sets full file name of a list file that contains all files to be 
    //    /// validated.
    //    /// </summary>
    //    ConfigurationSetting<string> BuildListFile { get; set; } // context

    //    /// <summary>
    //    /// Gets or sets the path of the manifest json to use for validation.
    //    /// </summary>
    //    [Obsolete("This field is not provided by the user or configFile, set by system")]
    //    ConfigurationSetting<string> ManifestPath { get; set; } // context

    //    /// <summary>
    //    /// Gets or sets the root folder where the generated manifest (and other files like bsi.json) files will be placed.
    //    /// By default we will generate this folder in the same level as the build drop with the name '_manifest'.
    //    /// </summary>
    //    ConfigurationSetting<string> ManifestDirPath { get; set; } // context

    //    /// <summary>
    //    /// Gets or sets the path where the output json should be written.
    //    /// </summary>
    //    ConfigurationSetting<string> OutputPath { get; set; } // context

    //    /// <summary>
    //    /// Gets or sets if you're downloading only a part of the drop using the '-r' or 'root' parameter
    //    /// in the drop client, specify the same string value here in order to skip
    //    /// validating paths that are not downloaded.
    //    /// </summary>
    //    ConfigurationSetting<string> RootPathFilter { get; set; } // context

    //    /// <summary>
    //    /// Gets or sets the path of the signed catalog file used to validate the manifest.json.
    //    /// </summary>
    //    ConfigurationSetting<string> CatalogFilePath { get; set; } // context

    //    /// <summary>
    //    /// Gets or sets if set, will validate the manifest using the signed catalog file.
    //    /// </summary>
    //    ConfigurationSetting<bool> ValidateSignature { get; set; } // context

    //    /// <summary>
    //    /// Gets or sets if set, will not fail validation on the files presented in Manifest but missing on the disk.
    //    /// </summary>
    //    ConfigurationSetting<bool> IgnoreMissing { get; set; } // context

    //    /// <summary>
    //    /// Gets or sets the name of the package this SBOM represents.
    //    /// </summary>
    //    ConfigurationSetting<string> PackageName { get; set; } // context

    //    /// <summary>
    //    /// Gets or sets the version of the package this SBOM represents.
    //    /// </summary>
    //    ConfigurationSetting<string> PackageVersion { get; set; } // context

    //    /// <summary>
    //    /// Gets or sets supplier information of the package this SBOM represents.
    //    /// </summary>
    //    ConfigurationSetting<string> PackageSupplier { get; set; } // context

    //    /// <summary>
    //    /// Gets or sets a list of <see cref="SBOMFile"/> files provided to us from the API.
    //    /// We won't traverse the build root path to get a list of files if this is set, and 
    //    /// use the list provided here instead.
    //    /// </summary>
    //    ConfigurationSetting<IEnumerable<SBOMFile>> FilesList { get; set; } // context

    //    /// <summary>
    //    /// Gets or sets a list of <see cref="SBOMPackage"/> packages provided to us from the API.
    //    /// This list will be used to generate the packages in the final SBOM.
    //    /// </summary>
    //    ConfigurationSetting<IEnumerable<SBOMPackage>> PackagesList { get; set; } // context

    //    /// <summary>
    //    /// Gets or sets if specified, we will store the generated telemetry for the execution
    //    /// of the SBOM tool at this path.
    //    /// </summary>
    //    ConfigurationSetting<string> TelemetryFilePath { get; set; } // context

    //    /// <summary>
    //    /// Gets or sets comma separated list of docker image names or hashes to be scanned for packages, ex: ubuntu:16.04, 56bab49eef2ef07505f6a1b0d5bd3a601dfc3c76ad4460f24c91d6fa298369ab.
    //    /// </summary>
    //    //[ComponentDetectorArgument(nameof(DockerImagesToScan))]
    //    ConfigurationSetting<string> DockerImagesToScan { get; set; } // context

    //    /// <summary>
    //    /// Gets or sets full file path to a file that contains list of external SBOMs to be 
    //    /// included as External document reference.
    //    /// </summary>
    //    ConfigurationSetting<string> ExternalDocumentReferenceListFile { get; set; } // context

    //    /// <summary>
    //    /// Gets or sets unique part of the namespace uri for SPDX 2.2 SBOMs. This value should be globally unique.
    //    /// If this value is not provided, we generate a unique guid that will make the namespace globally unique.
    //    /// </summary>
    //    ConfigurationSetting<string> NamespaceUriUniquePart { get; set; } // context

    //    /// <summary>
    //    /// Gets or sets the base of the URI that will be used to generate this SBOM. This should be a value that identifies that
    //    /// the SBOM belongs to a single publisher (or company).
    //    /// </summary>
    //    ConfigurationSetting<string> NamespaceUriBase { get; set; } // context

    //    /// <summary>
    //    /// Gets or sets a timestamp in the format <code>yyyy-MM-ddTHH:mm:ssZ</code> that will be used as the generated timestamp for the SBOM.
    //    /// </summary>
    //    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1629:Documentation text should end with a period",
    //        Justification = "Code element in comment.")]
    //    ConfigurationSetting<string> GenerationTimestamp { get; set; } // context
    //}
}
