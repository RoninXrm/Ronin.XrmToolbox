using System;

namespace Ronin.XrmToolbox
{
    public class SolutionListItem
    {
        public Guid Id { get; set; }
        public string UniqueName { get; set; }
        public string DisplayName { get; set; }
        public string PublisherPrefix { get; set; }
    }

    public class WebResourceRow
    {
        public bool Selected { get; set; }
        public Guid WebResourceId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int TypeCode { get; set; }
        public string TypeLabel { get; set; }
        public string LocalFilePath { get; set; }
        public string ContentBase64 { get; set; }
    }
}
