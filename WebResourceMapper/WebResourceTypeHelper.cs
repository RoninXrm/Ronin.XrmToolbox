namespace Ronin.XrmToolbox
{
    public static class WebResourceTypeHelper
    {
        public static int ResolveWebResourceType(string filePath)
        {
            switch (System.IO.Path.GetExtension(filePath).ToLowerInvariant())
            {
                case ".htm":
                case ".html":
                    return 1;
                case ".css":
                    return 2;
                case ".js":
                    return 3;
                case ".xml":
                    return 4;
                case ".png":
                    return 5;
                case ".jpg":
                case ".jpeg":
                    return 6;
                case ".gif":
                    return 7;
                case ".xap":
                    return 8;
                case ".xsl":
                case ".xslt":
                    return 9;
                case ".ico":
                    return 10;
                case ".svg":
                    return 11;
                case ".resx":
                    return 12;
                default:
                    return 3;
            }
        }

        public static string ResolveWebResourceTypeLabel(int typeCode)
        {
            switch (typeCode)
            {
                case 1: return "HTML";
                case 2: return "CSS";
                case 3: return "JavaScript";
                case 4: return "XML";
                case 5: return "PNG";
                case 6: return "JPG";
                case 7: return "GIF";
                case 8: return "Silverlight";
                case 9: return "XSL";
                case 10: return "ICO";
                case 11: return "SVG";
                case 12: return "RESX";
                default: return string.Format("Type {0}", typeCode);
            }
        }
    }
}
