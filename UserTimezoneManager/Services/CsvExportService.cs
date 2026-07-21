using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Ronin.XrmToolbox.UserTimezoneManager.Logging;
using Ronin.XrmToolbox.UserTimezoneManager.Models;

namespace Ronin.XrmToolbox.UserTimezoneManager.Services
{
    /// <summary>
    /// Exports user timezone data to a CSV file selected via a save-file dialog.
    /// </summary>
    public class CsvExportService
    {
        private readonly ILogger _logger;

        public CsvExportService(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Prompts the user for a file path and writes the supplied rows as CSV.
        /// </summary>
        public void Export(IEnumerable<UserTimezoneModel> rows, IWin32Window owner = null)
        {
            using (var dlg = new SaveFileDialog
            {
                Title = "Export User Timezones",
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                DefaultExt = "csv",
                FileName = $"UserTimezones_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
            })
            {
                if (dlg.ShowDialog(owner) != DialogResult.OK)
                {
                    _logger.Log("CSV export cancelled.");
                    return;
                }

                var path = dlg.FileName;
                var count = 0;

                try
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("Full Name,Email Address,Current Timezone");

                    foreach (var row in rows)
                    {
                        sb.AppendLine($"{Escape(row.FullName)},{Escape(row.EmailAddress)},{Escape(row.CurrentTimezoneName)}");
                        count++;
                    }

                    File.WriteAllText(path, sb.ToString(), Encoding.UTF8);

                    _logger.Log($"CSV export complete. {count} row(s) written to: {path}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"CSV export failed: {ex.Message}");
                }
            }
        }

        private static string Escape(string value)
        {
            if (value == null) return string.Empty;
            if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
                return $"\"{value.Replace("\"", "\"\"")}\"";
            return value;
        }
    }
}
