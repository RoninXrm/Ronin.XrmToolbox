using System.Collections.Generic;

namespace Ronin.XrmToolbox
{
    /// <summary>
    /// Plugin settings. XML serializable.
    /// Mappings are grouped by connection URL then solution unique name.
    /// File paths are stored relative to each connection's RootFolder.
    /// </summary>
    public class Settings
    {
        public string LastUsedOrganizationWebappUrl { get; set; }
        public List<ConnectionMappingSettings> Connections { get; set; } = new List<ConnectionMappingSettings>();
    }

    /// <summary>
    /// Per-connection settings: root folder and per-solution mappings.
    /// </summary>
    public class ConnectionMappingSettings
    {
        public string OrganizationUrl { get; set; }
        public string RootFolder { get; set; }
        public List<SolutionMappingSettings> Solutions { get; set; } = new List<SolutionMappingSettings>();
    }

    /// <summary>
    /// Per-solution web resource mappings for one connection.
    /// </summary>
    public class SolutionMappingSettings
    {
        public string SolutionUniqueName { get; set; }
        public string RootFolder { get; set; }
        public List<string> DeclinedPublisherPrefixFolders { get; set; } = new List<string>();
        public List<WebResourceMappingEntry> Mappings { get; set; } = new List<WebResourceMappingEntry>();
    }

    /// <summary>
    /// A single web resource to local file mapping.
    /// RelativeFilePath is relative to the parent ConnectionMappingSettings.RootFolder.
    /// </summary>
    public class WebResourceMappingEntry
    {
        public string WebResourceId { get; set; }
        public string Name { get; set; }
        public string RelativeFilePath { get; set; }
    }
}