using System.Drawing;
using System.IO;
using System.Reflection;

namespace Ronin.XrmToolbox
{
    internal static class ImageResources
    {
        private static readonly Assembly Assembly = Assembly.GetExecutingAssembly();

        public static Image Load(string imageName)
        {
            var resourceName = string.Format("WebResourceMapper.Images.{0}", imageName);
            using (var stream = Assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    return null;
                }

                return Image.FromStream(new MemoryStream(ReadAllBytes(stream)));
            }
        }

        private static byte[] ReadAllBytes(Stream stream)
        {
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
