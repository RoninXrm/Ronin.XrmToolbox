using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using XrmToolBox.Extensibility;

namespace Ronin.XrmToolbox
{
    public partial class WebResourceMapperController : PluginControlBase
    {
        private Settings mySettings;
        private readonly BindingList<WebResourceRow> webResourceRows = new BindingList<WebResourceRow>();
        private readonly DataverseWebResourceService dataverseService = new DataverseWebResourceService();
        private readonly WebResourceFileService fileService = new WebResourceFileService();

        private string currentOrganizationUrl;
        private string currentSolutionUniqueName;

        public WebResourceMapperController()
        {
            InitializeComponent();
            dgvWebResources.AutoGenerateColumns = false;
            dgvWebResources.DataSource = webResourceRows;

            pnlTop.Resize += (s, e) => SyncTopPanelLayout();
            pnlRootFolder.Resize += (s, e) => SyncRootFolderPanelLayout();
            btnLoadWebResources.SizeChanged += (s, e) => SyncTopPanelLayout();
            btnBrowseRootFolder.SizeChanged += (s, e) => SyncRootFolderPanelLayout();
            flpActions.SizeChanged += (s, e) => SyncRootFolderPanelLayout();
            cmbSolutions.SelectedIndexChanged += cmbSolutions_SelectedIndexChanged;
        }

        // ── Lifecycle ────────────────────────────────────────────────────────────

        private void MyPluginControl_Load(object sender, EventArgs e)
        {
            if (!SettingsManager.Instance.TryLoad(GetType(), out mySettings))
            {
                mySettings = new Settings();
                LogWarning("Settings not found => a new settings file has been created!");
            }
            else
            {
                LogInfo("Settings found and loaded");
            }

            if (Service != null)
            {
                LoadSolutions();
            }
        }

        private void MyPluginControl_OnCloseTool(object sender, EventArgs e)
        {
            SaveCurrentMappings();
            SettingsManager.Instance.Save(GetType(), mySettings);
        }

        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);

            if (detail != null)
            {
                currentOrganizationUrl = detail.WebApplicationUrl;

                if (mySettings != null)
                {
                    mySettings.LastUsedOrganizationWebappUrl = currentOrganizationUrl;
                }

                txtRootFolder.Text = string.Empty;
                LogInfo("Connection has changed to: {0}", currentOrganizationUrl);
            }

            if (newService != null)
            {
                LoadSolutions();
            }
        }

        // ── Layout helpers ───────────────────────────────────────────────────────

        private void SyncTopPanelLayout()
        {
            const int margin = 8;
            int btnRight = pnlTop.ClientSize.Width - margin;
            btnLoadWebResources.Left = btnRight - btnLoadWebResources.Width;
            cmbSolutions.Width = btnLoadWebResources.Left - margin - cmbSolutions.Left;
        }

        private void SyncRootFolderPanelLayout()
        {
            const int margin = 8;
            int panelRight = pnlRootFolder.ClientSize.Width - margin;
            btnBrowseRootFolder.Left = panelRight - btnBrowseRootFolder.Width;
            txtRootFolder.Width = btnBrowseRootFolder.Left - margin - txtRootFolder.Left;
            flpActions.Left = panelRight - flpActions.Width;
        }

        // ── UI event handlers ────────────────────────────────────────────────────

        private void tsbClose_Click(object sender, EventArgs e)
        {
            CloseTool();
        }

        private void btnLoadWebResources_Click(object sender, EventArgs e)
        {
            ExecuteMethod(LoadWebResourcesForSelectedSolution);
        }

        private void btnBrowseRootFolder_Click(object sender, EventArgs e)
        {
            using (var folderBrowser = new FolderBrowserDialog())
            {
                if (Directory.Exists(txtRootFolder.Text))
                {
                    folderBrowser.SelectedPath = txtRootFolder.Text;
                }

                if (folderBrowser.ShowDialog(this) == DialogResult.OK)
                {
                    txtRootFolder.Text = folderBrowser.SelectedPath;
                    SaveRootFolder();
                    fileService.AutoMapRowsByRootFolder(webResourceRows, txtRootFolder.Text, GetCurrentDeclinedPrefixTokens());
                    dgvWebResources.Refresh();
                }
            }
        }

        private void txtRootFolder_TextChanged(object sender, EventArgs e)
        {
            SaveRootFolder();
        }

        private void btnDownloadSelected_Click(object sender, EventArgs e)
        {
            ExecuteMethod(DownloadSelectedWebResources);
        }

        private void btnUploadNew_Click(object sender, EventArgs e)
        {
            ExecuteMethod(UploadNewWebResource);
        }

        private void btnUpdateSelected_Click(object sender, EventArgs e)
        {
            ExecuteMethod(UpdateSelectedWebResources);
        }

        private void btnResetAutoMap_Click(object sender, EventArgs e)
        {
            ExecuteMethod(ResetAndAutoMapAllRows);
        }

        private void dgvWebResources_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            if (dgvWebResources.Columns[e.ColumnIndex] != colBrowseFile)
            {
                return;
            }

            var row = dgvWebResources.Rows[e.RowIndex].DataBoundItem as WebResourceRow;
            if (row == null)
            {
                return;
            }

            using (var openFile = new OpenFileDialog())
            {
                openFile.Title = "Select local file";
                var root = txtRootFolder.Text;
                var absolute = fileService.GetAbsolutePath(row.LocalFilePath, root);
                var initialDir = !string.IsNullOrWhiteSpace(absolute) && Directory.Exists(Path.GetDirectoryName(absolute))
                    ? Path.GetDirectoryName(absolute)
                    : root;

                if (Directory.Exists(initialDir))
                {
                    openFile.InitialDirectory = initialDir;
                }

                if (openFile.ShowDialog(this) == DialogResult.OK)
                {
                    row.LocalFilePath = fileService.MakeRelativePath(openFile.FileName, root) ?? openFile.FileName;
                    dgvWebResources.Refresh();
                }
            }
        }

        private void cmbSolutions_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedSolution = cmbSolutions.SelectedItem as SolutionListItem;
            if (selectedSolution == null)
            {
                return;
            }

            SaveCurrentMappings();
            currentSolutionUniqueName = selectedSolution.UniqueName;
            RestoreRootFolderForCurrentSolution();
        }

        private void RestoreRootFolderForCurrentSolution()
        {
            if (mySettings == null || string.IsNullOrWhiteSpace(currentOrganizationUrl)
                || string.IsNullOrWhiteSpace(currentSolutionUniqueName))
            {
                txtRootFolder.Text = string.Empty;
                return;
            }

            var solution = FindSolutionSettings(currentOrganizationUrl, currentSolutionUniqueName);
            txtRootFolder.Text = !string.IsNullOrWhiteSpace(solution?.RootFolder)
                ? solution.RootFolder
                : string.Empty;
        }

        // ── Settings: root folder ────────────────────────────────────────────────

        private void RestoreRootFolder()
        {
            if (mySettings == null || string.IsNullOrWhiteSpace(currentOrganizationUrl))
            {
                return;
            }

            var connection = GetOrCreateConnectionSettings();
            txtRootFolder.Text = connection.RootFolder ?? string.Empty;
        }

        private void SaveRootFolder()
        {
            if (mySettings == null || string.IsNullOrWhiteSpace(currentSolutionUniqueName))
            {
                return;
            }

            var connection = GetOrCreateConnectionSettings();
            var solution = connection.Solutions.FirstOrDefault(
                s => string.Equals(s.SolutionUniqueName, currentSolutionUniqueName, StringComparison.OrdinalIgnoreCase));

            if (solution == null)
            {
                solution = new SolutionMappingSettings { SolutionUniqueName = currentSolutionUniqueName };
                connection.Solutions.Add(solution);
            }

            solution.RootFolder = txtRootFolder.Text;
        }

        private bool EnsureRootFolderSelected()
        {
            if (string.IsNullOrWhiteSpace(txtRootFolder.Text))
            {
                MessageBox.Show("Please select a root folder first.", "Root Folder Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (!Directory.Exists(txtRootFolder.Text))
            {
                MessageBox.Show("The selected root folder does not exist. Please choose a valid folder.", "Invalid Root Folder", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        // ── Settings: mappings ───────────────────────────────────────────────────

        private void RestoreMappings()
        {
            if (mySettings == null || string.IsNullOrWhiteSpace(currentOrganizationUrl)
                || string.IsNullOrWhiteSpace(currentSolutionUniqueName))
            {
                return;
            }

            var solution = FindSolutionSettings(currentOrganizationUrl, currentSolutionUniqueName);
            if (solution == null)
            {
                return;
            }

            foreach (var row in webResourceRows)
            {
                var entry = solution.Mappings.FirstOrDefault(
                    m => string.Equals(m.WebResourceId, row.WebResourceId.ToString(), StringComparison.OrdinalIgnoreCase));

                if (entry != null && !string.IsNullOrWhiteSpace(entry.RelativeFilePath))
                {
                    row.LocalFilePath = entry.RelativeFilePath;
                }
            }
        }

        private void SaveCurrentMappings()
        {
            if (mySettings == null || string.IsNullOrWhiteSpace(currentOrganizationUrl)
                || string.IsNullOrWhiteSpace(currentSolutionUniqueName)
                || webResourceRows.Count == 0)
            {
                return;
            }

            var connection = GetOrCreateConnectionSettings();
            var solution = connection.Solutions.FirstOrDefault(
                s => string.Equals(s.SolutionUniqueName, currentSolutionUniqueName, StringComparison.OrdinalIgnoreCase));

            if (solution == null)
            {
                solution = new SolutionMappingSettings { SolutionUniqueName = currentSolutionUniqueName };
                connection.Solutions.Add(solution);
            }

            solution.RootFolder = txtRootFolder.Text;
            solution.Mappings.Clear();

            foreach (var row in webResourceRows.Where(r => !string.IsNullOrWhiteSpace(r.LocalFilePath)))
            {
                solution.Mappings.Add(new WebResourceMappingEntry
                {
                    WebResourceId = row.WebResourceId.ToString(),
                    Name = row.Name,
                    RelativeFilePath = fileService.MakeRelativePath(
                        fileService.GetAbsolutePath(row.LocalFilePath, txtRootFolder.Text),
                        txtRootFolder.Text) ?? row.LocalFilePath
                });
            }
        }

        private ConnectionMappingSettings GetOrCreateConnectionSettings()
        {
            var connection = mySettings.Connections.FirstOrDefault(
                c => string.Equals(c.OrganizationUrl, currentOrganizationUrl, StringComparison.OrdinalIgnoreCase));

            if (connection == null)
            {
                connection = new ConnectionMappingSettings { OrganizationUrl = currentOrganizationUrl };
                mySettings.Connections.Add(connection);
            }

            return connection;
        }

        private SolutionMappingSettings FindSolutionSettings(string orgUrl, string solutionUniqueName)
        {
            return mySettings.Connections
                .FirstOrDefault(c => string.Equals(c.OrganizationUrl, orgUrl, StringComparison.OrdinalIgnoreCase))
                ?.Solutions
                .FirstOrDefault(s => string.Equals(s.SolutionUniqueName, solutionUniqueName, StringComparison.OrdinalIgnoreCase));
        }

        // ── Data operations ──────────────────────────────────────────────────────

        private void LoadSolutions()
        {
            if (Service == null)
            {
                LogWarning("Not connected. Please connect to a Dataverse environment first.");
                return;
            }

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading unmanaged solutions...",
                Work = (worker, args) =>
                {
                    args.Result = dataverseService.GetUnmanagedSolutions(Service);
                },
                PostWorkCallBack = args =>
                {
                    if (args.Error != null)
                    {
                        LogError(args.Error.ToString());
                        MessageBox.Show(args.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var solutions = args.Result as List<SolutionListItem>;
                    cmbSolutions.DataSource = solutions ?? new List<SolutionListItem>();
                    cmbSolutions.DisplayMember = nameof(SolutionListItem.DisplayName);
                    cmbSolutions.ValueMember = nameof(SolutionListItem.Id);
                    LogInfo("Loaded {0} unmanaged solution(s)", cmbSolutions.Items.Count);
                }
            });
        }

        private void LoadWebResourcesForSelectedSolution()
        {
            if (Service == null)
            {
                LogWarning("Not connected. Please connect to a Dataverse environment first.");
                return;
            }

            var selectedSolution = cmbSolutions.SelectedItem as SolutionListItem;
            if (selectedSolution == null)
            {
                MessageBox.Show("Please select a solution first.", "Solution Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveCurrentMappings();
            currentSolutionUniqueName = selectedSolution.UniqueName;

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading web resources...",
                Work = (worker, args) =>
                {
                    args.Result = dataverseService.GetWebResourcesForSolution(Service, selectedSolution.Id);
                },
                PostWorkCallBack = args =>
                {
                    if (args.Error != null)
                    {
                        LogError(args.Error.ToString());
                        MessageBox.Show(args.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    webResourceRows.Clear();
                    var rows = args.Result as List<WebResourceRow> ?? new List<WebResourceRow>();
                    foreach (var row in rows)
                    {
                        webResourceRows.Add(row);
                    }

                    RestoreMappings();
                    fileService.AutoMapRowsByRootFolder(webResourceRows, txtRootFolder.Text, GetCurrentDeclinedPrefixTokens());
                    dgvWebResources.Refresh();
                    LogInfo("Loaded {0} web resource(s) for solution {1}", webResourceRows.Count, selectedSolution.DisplayName);
                }
            });
        }

        private void DownloadSelectedWebResources()
        {
            if (!EnsureRootFolderSelected())
            {
                return;
            }

            var selectedRows = GetSelectedRows().ToList();
            if (selectedRows.Count == 0)
            {
                MessageBox.Show("Please select at least one web resource.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedSolution = cmbSolutions.SelectedItem as SolutionListItem;
            ApplyPublisherPrefixDownloadRule(selectedRows, selectedSolution);

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Downloading selected web resources...",
                Work = (worker, args) =>
                {
                    foreach (var row in selectedRows)
                    {
                        if (string.IsNullOrWhiteSpace(row.ContentBase64))
                        {
                            continue;
                        }

                        var targetPath = fileService.EnsureRowFilePath(row, txtRootFolder.Text);
                        if (string.IsNullOrWhiteSpace(targetPath))
                        {
                            continue;
                        }

                        var bytes = Convert.FromBase64String(row.ContentBase64);
                        fileService.EnsureDirectory(targetPath);
                        fileService.WriteAllBytesAsync(targetPath, bytes).GetAwaiter().GetResult();
                    }
                },
                PostWorkCallBack = args =>
                {
                    if (args.Error != null)
                    {
                        LogError(args.Error.ToString());
                        MessageBox.Show(args.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    dgvWebResources.Refresh();
                    LogInfo("Download completed for {0} selected web resource(s)", selectedRows.Count);
                }
            });
        }

        private void ApplyPublisherPrefixDownloadRule(List<WebResourceRow> rows, SolutionListItem selectedSolution)
        {
            if (rows == null || rows.Count == 0 || selectedSolution == null)
            {
                return;
            }

            var publisherPrefix = selectedSolution.PublisherPrefix;
            if (string.IsNullOrWhiteSpace(publisherPrefix))
            {
                return;
            }

            var solutionSettings = GetOrCreateCurrentSolutionSettings();
            var declinedPrefixes = solutionSettings.DeclinedPublisherPrefixFolders ?? new List<string>();

            var prefixToken = publisherPrefix + "_";
            var prefixFolderPath = Path.Combine(txtRootFolder.Text, prefixToken);
            var prefixFolderExists = Directory.Exists(prefixFolderPath);

            bool? createPublisherFolder = null;

            if (!prefixFolderExists && declinedPrefixes.Any(p => string.Equals(p, prefixToken, StringComparison.OrdinalIgnoreCase)))
            {
                createPublisherFolder = false;
            }

            foreach (var row in rows)
            {
                if (!string.IsNullOrWhiteSpace(row.LocalFilePath))
                {
                    continue;
                }

                if (!row.Name.StartsWith(prefixToken, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (!prefixFolderExists && !createPublisherFolder.HasValue)
                {
                    var result = MessageBox.Show(
                        string.Format("Do you want to create a folder for the Publisher Prefix {0}", prefixToken),
                        "Publisher Prefix Folder",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    createPublisherFolder = result == DialogResult.Yes;

                    if (createPublisherFolder.Value)
                    {
                        Directory.CreateDirectory(prefixFolderPath);
                        prefixFolderExists = true;

                        if (declinedPrefixes.RemoveAll(p => string.Equals(p, prefixToken, StringComparison.OrdinalIgnoreCase)) > 0)
                        {
                            solutionSettings.DeclinedPublisherPrefixFolders = declinedPrefixes;
                        }
                    }
                    else
                    {
                        if (!declinedPrefixes.Any(p => string.Equals(p, prefixToken, StringComparison.OrdinalIgnoreCase)))
                        {
                            declinedPrefixes.Add(prefixToken);
                            solutionSettings.DeclinedPublisherPrefixFolders = declinedPrefixes;
                        }
                    }
                }

                var relativePath = row.Name.Replace('/', Path.DirectorySeparatorChar);

                if (createPublisherFolder.HasValue && !createPublisherFolder.Value)
                {
                    relativePath = relativePath.Substring(prefixToken.Length)
                                               .TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                }

                row.LocalFilePath = relativePath;
            }
        }

        private SolutionMappingSettings GetOrCreateCurrentSolutionSettings()
        {
            var connection = GetOrCreateConnectionSettings();
            var solution = connection.Solutions.FirstOrDefault(
                s => string.Equals(s.SolutionUniqueName, currentSolutionUniqueName, StringComparison.OrdinalIgnoreCase));

            if (solution == null)
            {
                solution = new SolutionMappingSettings
                {
                    SolutionUniqueName = currentSolutionUniqueName,
                    DeclinedPublisherPrefixFolders = new List<string>()
                };
                connection.Solutions.Add(solution);
            }
            else if (solution.DeclinedPublisherPrefixFolders == null)
            {
                solution.DeclinedPublisherPrefixFolders = new List<string>();
            }

            return solution;
        }

        private void UploadNewWebResource()
        {
            if (Service == null)
            {
                LogWarning("Not connected. Please connect to a Dataverse environment first.");
                return;
            }

            if (!EnsureRootFolderSelected())
            {
                return;
            }

            var selectedSolution = cmbSolutions.SelectedItem as SolutionListItem;
            if (selectedSolution == null)
            {
                MessageBox.Show("Please select a solution first.", "Solution Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string selectedFile;
            using (var openFile = new OpenFileDialog())
            {
                openFile.Title = "Select file to upload as web resource";
                if (Directory.Exists(txtRootFolder.Text))
                {
                    openFile.InitialDirectory = txtRootFolder.Text;
                }

                if (openFile.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                selectedFile = openFile.FileName;
            }

            var relativePath = fileService.MakeRelativePath(selectedFile, txtRootFolder.Text) ?? string.Empty;

            string webResourceName;
            string webResourceDisplayName;
            using (var dialog = new UploadWebResourceDialog(selectedSolution.PublisherPrefix, relativePath, selectedFile))
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                webResourceName = dialog.WebResourceName;
                webResourceDisplayName = dialog.WebResourceDisplayName;
            }

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Uploading new web resource...",
                Work = (worker, args) =>
                {
                    var bytes = fileService.ReadAllBytesAsync(selectedFile).GetAwaiter().GetResult();
                    var contentBase64 = Convert.ToBase64String(bytes);
                    var webResourceType = fileService.ResolveWebResourceType(selectedFile);

                    var webResourceId = dataverseService.CreateWebResource(
                        Service,
                        webResourceName,
                        webResourceDisplayName,
                        webResourceType,
                        contentBase64);

                    dataverseService.AddWebResourceToSolution(Service, webResourceId, selectedSolution.UniqueName);

                    args.Result = new WebResourceRow
                    {
                        WebResourceId = webResourceId,
                        Name = webResourceName,
                        DisplayName = webResourceDisplayName,
                        TypeCode = webResourceType,
                        TypeLabel = WebResourceTypeHelper.ResolveWebResourceTypeLabel(webResourceType),
                        LocalFilePath = string.IsNullOrWhiteSpace(relativePath) ? selectedFile : relativePath,
                        ContentBase64 = contentBase64
                    };
                },
                PostWorkCallBack = args =>
                {
                    if (args.Error != null)
                    {
                        LogError(args.Error.ToString());
                        MessageBox.Show(args.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var newRow = args.Result as WebResourceRow;
                    if (newRow != null)
                    {
                        webResourceRows.Add(newRow);
                    }

                    dgvWebResources.Refresh();
                    LogInfo("Uploaded new web resource '{0}' from file {1}", webResourceName, selectedFile);
                }
            });
        }

        private void UpdateSelectedWebResources()
        {
            if (Service == null)
            {
                LogWarning("Not connected. Please connect to a Dataverse environment first.");
                return;
            }

            if (!EnsureRootFolderSelected())
            {
                return;
            }

            var selectedRows = GetSelectedRows().Where(r => !string.IsNullOrWhiteSpace(r.LocalFilePath)).ToList();
            if (selectedRows.Count == 0)
            {
                MessageBox.Show("Please select at least one row with a local file path.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Updating and publishing selected web resources...",
                Work = (worker, args) =>
                {
                    var updatedIds = new List<Guid>();

                    foreach (var row in selectedRows)
                    {
                        var absolutePath = fileService.GetAbsolutePath(row.LocalFilePath, txtRootFolder.Text);
                        if (!File.Exists(absolutePath))
                        {
                            LogWarning("File not found for web resource {0}: {1}", row.Name, absolutePath);
                            continue;
                        }

                        var bytes = fileService.ReadAllBytesAsync(absolutePath).GetAwaiter().GetResult();
                        var contentBase64 = Convert.ToBase64String(bytes);
                        dataverseService.UpdateWebResourceContent(Service, row.WebResourceId, contentBase64);
                        row.ContentBase64 = contentBase64;
                        updatedIds.Add(row.WebResourceId);
                    }

                    dataverseService.PublishWebResources(Service, updatedIds);
                    args.Result = updatedIds.Count;
                },
                PostWorkCallBack = args =>
                {
                    if (args.Error != null)
                    {
                        LogError(args.Error.ToString());
                        MessageBox.Show(args.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var updatedCount = args.Result is int count ? count : 0;
                    LogInfo("Updated and published {0} web resource(s)", updatedCount);
                }
            });
        }

        private void ResetAndAutoMapAllRows()
        {
            if (!EnsureRootFolderSelected())
            {
                return;
            }

            if (webResourceRows.Count == 0)
            {
                MessageBox.Show("No web resources are loaded.", "Nothing to map", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (var row in webResourceRows)
            {
                row.LocalFilePath = null;
            }

            fileService.AutoMapRowsByRootFolder(webResourceRows, txtRootFolder.Text, GetCurrentDeclinedPrefixTokens());
            dgvWebResources.Refresh();
            LogInfo("Reset mappings and auto-mapped {0} web resource row(s)", webResourceRows.Count);
        }

        private IEnumerable<string> GetCurrentDeclinedPrefixTokens()
        {
            if (mySettings == null
                || string.IsNullOrWhiteSpace(currentOrganizationUrl)
                || string.IsNullOrWhiteSpace(currentSolutionUniqueName))
            {
                return Enumerable.Empty<string>();
            }

            return FindSolutionSettings(currentOrganizationUrl, currentSolutionUniqueName)
                       ?.DeclinedPublisherPrefixFolders
                   ?? Enumerable.Empty<string>();
        }

        private IEnumerable<WebResourceRow> GetSelectedRows()
        {
            dgvWebResources.EndEdit();
            return webResourceRows.Where(r => r.Selected);
        }
    }
}