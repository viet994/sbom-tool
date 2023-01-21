using Microsoft.Sbom.Common.Config.Attributes;
using Microsoft.Sbom.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace Microsoft.Sbom.Common.Config
{
    public interface IContext
    {
        [DirectoryExists]
        [DirectoryPathIsWritable(ForAction = ManifestToolActions.Generate)]
        [ValueRequired]
        ConfigurationSetting<string> BuildDropPath { get; set; }

        [DirectoryExists]
        ConfigurationSetting<string> BuildComponentPath { get; set; }

        [FileExists]
        ConfigurationSetting<string> BuildListFile { get; set; }

        [Obsolete("This field is not provided by the user or configFile, set by system")]
        ConfigurationSetting<string> ManifestPath { get; set; }

        [DirectoryExists]
        [DirectoryPathIsWritable(ForAction = ManifestToolActions.Generate)]
        ConfigurationSetting<string> ManifestDirPath { get; set; }

        [FilePathIsWritable]
        [ValueRequired(ForAction = ManifestToolActions.Validate)]
        ConfigurationSetting<string> OutputPath { get; set; }


        ConfigurationSetting<string> RootPathFilter { get; set; }

        ConfigurationSetting<string> CatalogFilePath { get; set; }

        [DefaultValue(false)]
        ConfigurationSetting<bool> ValidateSignature { get; set; }

        [DefaultValue(false)]
        ConfigurationSetting<bool> IgnoreMissing { get; set; }

        ConfigurationSetting<string> PackageName { get; set; }

        ConfigurationSetting<string> PackageVersion { get; set; }

        [ValueRequired(ForAction = ManifestToolActions.Generate)]
        ConfigurationSetting<string> PackageSupplier { get; set; }

        ConfigurationSetting<IEnumerable<SBOMFile>> FilesList { get; set; }

        ConfigurationSetting<IEnumerable<SBOMPackage>> PackagesList { get; set; }

        ConfigurationSetting<string> TelemetryFilePath { get; set; }

        [ComponentDetectorArgument(nameof(DockerImagesToScan))]
        public ConfigurationSetting<string> DockerImagesToScan { get; set; }

        [FileExists]
        ConfigurationSetting<string> ExternalDocumentReferenceListFile { get; set; }

        ConfigurationSetting<string> NamespaceUriUniquePart { get; set; }

        [ValidUri(ForAction = ManifestToolActions.Generate, UriKind = UriKind.Absolute)]
        [ValueRequired(ForAction = ManifestToolActions.Generate)]
        ConfigurationSetting<string> NamespaceUriBase { get; set; }

        ConfigurationSetting<string> GenerationTimestamp { get; set; }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1311:Static readonly fields should begin with upper-case letter", Justification = "Private fields with the same name as public properties.")]
    public class ContextAdapter : IContext
    {
        private static readonly AsyncLocal<ConfigurationSetting<string>> buildDropPath = new ();
        private static readonly AsyncLocal<ConfigurationSetting<string>> buildComponentPath = new ();
        private static readonly AsyncLocal<ConfigurationSetting<string>> buildListFile = new ();
        private static readonly AsyncLocal<ConfigurationSetting<string>> manifestPath = new ();
        private static readonly AsyncLocal<ConfigurationSetting<string>> manifestDirPath = new ();
        private static readonly AsyncLocal<ConfigurationSetting<string>> outputPath = new ();
        private static readonly AsyncLocal<ConfigurationSetting<string>> rootPathFilter = new ();
        private static readonly AsyncLocal<ConfigurationSetting<string>> catalogFilePath = new ();
        private static readonly AsyncLocal<ConfigurationSetting<bool>> validateSignature = new ();
        private static readonly AsyncLocal<ConfigurationSetting<bool>> ignoreMissing = new ();
        private static readonly AsyncLocal<ConfigurationSetting<string>> packageName = new ();
        private static readonly AsyncLocal<ConfigurationSetting<string>> packageVersion = new ();
        private static readonly AsyncLocal<ConfigurationSetting<string>> packageSupplier = new ();
        private static readonly AsyncLocal<ConfigurationSetting<IEnumerable<SBOMFile>>> filesList = new ();
        private static readonly AsyncLocal<ConfigurationSetting<IEnumerable<SBOMPackage>>> packagesList = new ();
        private static readonly AsyncLocal<ConfigurationSetting<string>> telemetryFilePath = new ();
        private static readonly AsyncLocal<ConfigurationSetting<string>> dockerImagesToScan = new ();
        private static readonly AsyncLocal<ConfigurationSetting<string>> externalDocumentReferenceListFile = new ();
        private static readonly AsyncLocal<ConfigurationSetting<string>> namespaceUriUniquePart = new ();
        private static readonly AsyncLocal<ConfigurationSetting<string>> namespaceUriBase = new ();
        private static readonly AsyncLocal<ConfigurationSetting<string>> generationTimestamp = new ();

        public static void SetBuildDropPath(ConfigurationSetting<string> value) => buildDropPath.Value = value;

        public static void SetBuildComponentPath(ConfigurationSetting<string> value) => buildComponentPath.Value = value;

        public static void SetBuildListFile(ConfigurationSetting<string> value) => buildListFile.Value = value;

        public static void SetManifestPath(ConfigurationSetting<string> value) => manifestPath.Value = value;

        public static void SetManifestDirPath(ConfigurationSetting<string> value) => manifestDirPath.Value = value;

        public static void SetOutputPath(ConfigurationSetting<string> value) => outputPath.Value = value;

        public static void SetRootFilterPath(ConfigurationSetting<string> value) => rootPathFilter.Value = value;

        public static void SetCatalogFilePath(ConfigurationSetting<string> value) => catalogFilePath.Value = value;

        public static void SetValidateSignature(ConfigurationSetting<bool> value) => validateSignature.Value = value;

        public static void SetIgnoreMissing(ConfigurationSetting<bool> value) => ignoreMissing.Value = value;

        public static void SetPackageName(ConfigurationSetting<string> value) => packageName.Value = value;

        public static void SetPackageVersion(ConfigurationSetting<string> value) => packageVersion.Value = value;

        public static void SetPackageSupplier(ConfigurationSetting<string> value) => packageSupplier.Value = value;

        public static void SetFilesList(ConfigurationSetting<IEnumerable<SBOMFile>> value) => filesList.Value = value;

        public static void SetPackagesList(ConfigurationSetting<IEnumerable<SBOMPackage>> value) => packagesList.Value = value;

        public static void SetTelemetryFilePath(ConfigurationSetting<string> value) => telemetryFilePath.Value = value;

        public static void SetDockerImagesToScan(ConfigurationSetting<string> value) => dockerImagesToScan.Value = value;

        public static void SetExternalDocumentReferenceListFile(ConfigurationSetting<string> value) => externalDocumentReferenceListFile.Value = value;

        public static void SetNamespaceUriUniquePart(ConfigurationSetting<string> value) => namespaceUriUniquePart.Value = value;

        public static void SetNamespaceBaseUri(ConfigurationSetting<string> value) => namespaceUriBase.Value = value;

        public static void SetGenerationTimestamp(ConfigurationSetting<string> value) => generationTimestamp.Value = value;

        public ConfigurationSetting<string> BuildDropPath
        {
            get => buildDropPath.Value;
            set => SetBuildDropPath(value);
        }

        public ConfigurationSetting<string> BuildComponentPath
        {
            get => buildComponentPath.Value;
            set => SetBuildComponentPath(value);
        }

        public ConfigurationSetting<string> BuildListFile
        {
            get => buildListFile.Value;
            set => SetBuildListFile(value);
        }

        public ConfigurationSetting<string> ManifestPath
        {
            get => manifestPath.Value;
            set => SetManifestPath(value);
        }

        public ConfigurationSetting<string> ManifestDirPath
        {
            get => manifestDirPath.Value;
            set => SetManifestDirPath(value);
        }

        public ConfigurationSetting<string> OutputPath
        {
            get => outputPath.Value;
            set => SetOutputPath(value);
        }

        public ConfigurationSetting<string> RootPathFilter
        {
            get => rootPathFilter.Value;
            set => SetRootFilterPath(value);
        }

        public ConfigurationSetting<string> CatalogFilePath
        {
            get => catalogFilePath.Value;
            set => SetCatalogFilePath(value);
        }

        public ConfigurationSetting<bool> ValidateSignature
        {
            get => validateSignature.Value;
            set => SetValidateSignature(value);
        }

        public ConfigurationSetting<bool> IgnoreMissing
        {
            get => ignoreMissing.Value;
            set => SetIgnoreMissing(value);
        }

        public ConfigurationSetting<string> PackageName
        {
            get => packageName.Value;
            set => SetPackageName(value);
        }

        public ConfigurationSetting<string> PackageVersion
        {
            get => packageVersion.Value;
            set => SetPackageVersion(value);
        }

        public ConfigurationSetting<string> PackageSupplier
        {
            get => packageSupplier.Value;
            set => SetPackageSupplier(value);
        }

        public ConfigurationSetting<IEnumerable<SBOMFile>> FilesList
        {
            get => filesList.Value;
            set => SetFilesList(value);
        }

        public ConfigurationSetting<IEnumerable<SBOMPackage>> PackagesList
        {
            get => packagesList.Value;
            set => SetPackagesList(value);
        }

        public ConfigurationSetting<string> TelemetryFilePath
        {
            get => telemetryFilePath.Value;
            set => SetTelemetryFilePath(value);
        }

        public ConfigurationSetting<string> DockerImagesToScan
        {
            get => dockerImagesToScan.Value;
            set => SetDockerImagesToScan(value);
        }

        public ConfigurationSetting<string> ExternalDocumentReferenceListFile
        {
            get => externalDocumentReferenceListFile.Value;
            set => SetExternalDocumentReferenceListFile(value);
        }

        public ConfigurationSetting<string> NamespaceUriUniquePart
        {
            get => namespaceUriUniquePart.Value;
            set => SetNamespaceUriUniquePart(value);
        }

        public ConfigurationSetting<string> NamespaceUriBase
        {
            get => namespaceUriBase.Value;
            set => SetNamespaceBaseUri(value);
        }

        public ConfigurationSetting<string> GenerationTimestamp
        {
            get => generationTimestamp.Value;
            set => SetGenerationTimestamp(value);
        }
    }
}
