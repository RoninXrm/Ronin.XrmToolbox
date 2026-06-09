using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Ronin.XrmToolbox
{
    /// <summary>
    /// Modal dialog for confirming the name and display name of a new web resource before upload.
    /// </summary>
    internal class UploadWebResourceDialog : Form
    {
        private readonly Label lblNameInfo;
        private readonly TextBox txtName;
        private readonly Label lblDisplayName;
        private readonly TextBox txtDisplayName;
        private readonly Button btnOk;
        private readonly Button btnCancel;

        /// <summary>
        /// The final web resource name chosen by the user.
        /// </summary>
        public string WebResourceName => txtName.Text.Trim();

        /// <summary>
        /// The final display name chosen by the user.
        /// </summary>
        public string WebResourceDisplayName => txtDisplayName.Text.Trim();

        public UploadWebResourceDialog(string publisherPrefix, string relativeFilePath, string absoluteFilePath)
        {
            var suggestedName = BuildSuggestedName(publisherPrefix, relativeFilePath, absoluteFilePath);
            var suggestedDisplayName = Path.GetFileName(absoluteFilePath);

            // ── Form ──────────────────────────────────────────────────────────────
            Text = "Upload New Web Resource";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MinimizeBox = false;
            MaximizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(560, 170);
            Padding = new Padding(16);

            // ── Name row ──────────────────────────────────────────────────────────
            lblNameInfo = new Label
            {
                Text = "Web Resource Name",
                AutoSize = true,
                Location = new Point(16, 16)
            };

            txtName = new TextBox
            {
                Text = suggestedName,
                Location = new Point(16, 36),
                Width = 528,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            // ── Display name row ──────────────────────────────────────────────────
            lblDisplayName = new Label
            {
                Text = "Display Name",
                AutoSize = true,
                Location = new Point(16, 72)
            };

            txtDisplayName = new TextBox
            {
                Text = suggestedDisplayName,
                Location = new Point(16, 92),
                Width = 528,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            // ── Buttons ───────────────────────────────────────────────────────────
            btnOk = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Size = new Size(90, 30),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };
            btnOk.Location = new Point(ClientSize.Width - 212, ClientSize.Height - 46);
            btnOk.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(WebResourceName))
                {
                    MessageBox.Show("Web Resource Name is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DialogResult = DialogResult.None;
                }
            };

            btnCancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Size = new Size(90, 30),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };
            btnCancel.Location = new Point(ClientSize.Width - 112, ClientSize.Height - 46);

            AcceptButton = btnOk;
            CancelButton = btnCancel;

            Controls.AddRange(new Control[]
            {
                lblNameInfo, txtName,
                lblDisplayName, txtDisplayName,
                btnOk, btnCancel
            });
        }

        private static string BuildSuggestedName(string publisherPrefix, string relativeFilePath, string absoluteFilePath)
        {
            // Use relative path when available, fall back to filename only
            var path = !string.IsNullOrWhiteSpace(relativeFilePath)
                ? relativeFilePath
                : Path.GetFileName(absoluteFilePath);

            // Normalise separators to forward slashes
            path = path.Replace('\\', '/');

            if (!string.IsNullOrWhiteSpace(publisherPrefix))
            {
                return string.Format("{0}_/{1}", publisherPrefix, path);
            }

            return path;
        }
    }
}
