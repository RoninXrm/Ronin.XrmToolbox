using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ronin.XrmToolbox
{
    public class WebResourceFileService
    {
        /// <summary>
        /// Returns a path relative to rootFolder, or null if the path is not under rootFolder.
        /// </summary>
        public string MakeRelativePath(string absolutePath, string rootFolder)
        {
            if (string.IsNullOrWhiteSpace(absolutePath) || string.IsNullOrWhiteSpace(rootFolder))
            {
                return null;
            }

            var root = rootFolder.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                       + Path.DirectorySeparatorChar;

            if (absolutePath.StartsWith(root, StringComparison.OrdinalIgnoreCase))
            {
                return absolutePath.Substring(root.Length);
            }

            return null;
        }

        /// <summary>
        /// Resolves a relative path against rootFolder to an absolute path.
        /// Returns the input unchanged if it is already absolute or rootFolder is empty.
        /// </summary>
        public string GetAbsolutePath(string relativePath, string rootFolder)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
            {
                return null;
            }

            if (Path.IsPathRooted(relativePath))
            {
                return relativePath;
            }

            if (string.IsNullOrWhiteSpace(rootFolder))
            {
                return relativePath;
            }

            return Path.Combine(rootFolder, relativePath);
        }

        /// <summary>
        /// Attempts to auto-map rows to local files based on rootFolder.
        /// Stores relative paths on each row's LocalFilePath.
        /// If declinedPrefixTokens contains a matching prefix (e.g. brc_),
        /// auto-map first tries the path without that prefix.
        /// </summary>
        public void AutoMapRowsByRootFolder(IEnumerable<WebResourceRow> rows, string rootFolder, IEnumerable<string> declinedPrefixTokens = null)
        {
            if (!Directory.Exists(rootFolder))
            {
                return;
            }

            var declined = new HashSet<string>(
                (declinedPrefixTokens ?? Enumerable.Empty<string>())
                    .Where(p => !string.IsNullOrWhiteSpace(p)),
                StringComparer.OrdinalIgnoreCase);

            foreach (var row in rows)
            {
                var absolute = GetAbsolutePath(row.LocalFilePath, rootFolder);
                if (!string.IsNullOrWhiteSpace(absolute) && File.Exists(absolute))
                {
                    continue;
                }

                var relativeFromName = row.Name.Replace('/', Path.DirectorySeparatorChar);
                var candidates = new List<string>();

                foreach (var token in declined)
                {
                    if (!relativeFromName.StartsWith(token, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    var stripped = relativeFromName.Substring(token.Length)
                                                   .TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                    if (!string.IsNullOrWhiteSpace(stripped))
                    {
                        candidates.Add(stripped);
                    }
                }

                candidates.Add(relativeFromName);

                var matched = candidates
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .FirstOrDefault(c => File.Exists(Path.Combine(rootFolder, c)));

                if (!string.IsNullOrWhiteSpace(matched))
                {
                    row.LocalFilePath = matched;
                }
            }
        }

        /// <summary>
        /// Ensures a row has a file path set, generating one from rootFolder + name if needed.
        /// Returns the absolute path to use for file I/O.
        /// </summary>
        public string EnsureRowFilePath(WebResourceRow row, string rootFolder)
        {
            if (!string.IsNullOrWhiteSpace(row.LocalFilePath))
            {
                return GetAbsolutePath(row.LocalFilePath, rootFolder);
            }

            if (string.IsNullOrWhiteSpace(rootFolder))
            {
                return null;
            }

            var relative = row.Name.Replace('/', Path.DirectorySeparatorChar);
            row.LocalFilePath = relative;
            return Path.Combine(rootFolder, relative);
        }

        public void EnsureDirectory(string filePath)
        {
            var folder = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrWhiteSpace(folder) && !Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

        public string BuildWebResourceName(string filePath, string rootFolder)
        {
            if (Directory.Exists(rootFolder) && filePath.StartsWith(
                    rootFolder.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar,
                    StringComparison.OrdinalIgnoreCase))
            {
                var relative = filePath.Substring(rootFolder.TrimEnd(
                    Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Length + 1);
                return relative.Replace(Path.DirectorySeparatorChar, '/').Replace(Path.AltDirectorySeparatorChar, '/');
            }

            return Path.GetFileName(filePath);
        }

        public int ResolveWebResourceType(string filePath)
        {
            return WebResourceTypeHelper.ResolveWebResourceType(filePath);
        }

        public async Task<byte[]> ReadAllBytesAsync(string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
            {
                var bytes = new byte[stream.Length];
                var offset = 0;
                int read;
                while ((read = await stream.ReadAsync(bytes, offset, bytes.Length - offset)) > 0)
                {
                    offset += read;
                }

                return bytes;
            }
        }

        public async Task WriteAllBytesAsync(string filePath, byte[] bytes)
        {
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
            {
                await stream.WriteAsync(bytes, 0, bytes.Length);
            }
        }
    }
}
