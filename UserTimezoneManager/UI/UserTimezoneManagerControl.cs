using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using Ronin.XrmToolbox.UserTimezoneManager.Logging;
using Ronin.XrmToolbox.UserTimezoneManager.Models;
using Ronin.XrmToolbox.UserTimezoneManager.Services;
using XrmToolBox.Extensibility;

namespace Ronin.XrmToolbox.UserTimezoneManager.UI
{
    /// <summary>
    /// Main XrmToolBox plugin control for bulk user timezone management.
    /// Follows the XrmToolBox PluginControlBase pattern, using WorkAsync for
    /// all Dataverse operations to keep the UI responsive.
    /// </summary>
    public partial class UserTimezoneManagerControl : PluginControlBase
    {
        // ── Services ─────────────────────────────────────────────────────────────
        private readonly TimezoneService _timezoneService = new TimezoneService();
        private UserService _userService;
        private readonly ViewService _viewService = new ViewService();
        private CsvExportService _csvExportService;
        private ILogger _logger;

        // ── Data ─────────────────────────────────────────────────────────────────
        private BindingList<UserTimezoneModel> _allUsers = new BindingList<UserTimezoneModel>();
        private BindingList<UserTimezoneModel> _filteredUsers = new BindingList<UserTimezoneModel>();
        private List<TimezoneOption> _timezones = new List<TimezoneOption>();
        private bool _timezonesLoaded = false;
        private bool _viewsLoaded = false;

        public UserTimezoneManagerControl()
        {
            InitializeComponent();
        }

        // ── Lifecycle ─────────────────────────────────────────────────────────────

        private void UserTimezoneManagerControl_Load(object sender, EventArgs e)
        {
            _logger = new TextBoxLogger(txtLog);
            _userService = new UserService(_timezoneService);
            _csvExportService = new CsvExportService(_logger);

            // Wire up grid data source using the filtered list
            dgvUsers.AutoGenerateColumns = false;
            dgvUsers.DataSource = _filteredUsers;

            // Explicitly ensure multi-select configuration is active
            dgvUsers.MultiSelect = true;
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Wire up checkbox column header click to toggle all selections
            dgvUsers.ColumnHeaderMouseClick += dgvUsers_ColumnHeaderMouseClick;

            _logger?.Log("User Timezone Manager loaded. Connect to a Dataverse environment to begin.");
        }

        private void UserTimezoneManagerControl_OnCloseTool(object sender, EventArgs e)
        {
            // Nothing to persist for this plugin
        }

        /// <summary>Called by XrmToolBox when a new connection is established.</summary>
        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail,
            string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);
            _logger?.Log($"Connected to: {detail?.OrganizationFriendlyName ?? detail?.OrganizationServiceUrl}");

            // Invalidate cached data so the new environment's data is loaded fresh
            _timezoneService.InvalidateCache();
            _timezonesLoaded = false;
            _viewsLoaded = false;
            _allUsers.Clear();
            _filteredUsers.Clear();
            cmbViews.Items.Clear();
            cmbBulkTimezone.Items.Clear();

            LoadTimezonesThenViews();
        }

        // ── Loading ───────────────────────────────────────────────────────────────

        /// <summary>
        /// Step 1: Load timezone definitions (cached after first call).
        /// Step 2: Load system views (on same connection event).
        /// </summary>
        private void LoadTimezonesThenViews()
        {
            if (Service == null) return;

            _logger?.Log("Retrieving timezone definitions...");

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading timezones...",
                Work = (worker, args) =>
                {
                    args.Result = _timezoneService.GetTimezones(Service);
                },
                PostWorkCallBack = args =>
                {
                    if (args.Error != null)
                    {
                        _logger?.LogError($"Failed to load timezones: {args.Error.Message}");
                        return;
                    }

                    _timezones = (List<TimezoneOption>)args.Result;
                    _timezonesLoaded = true;

                    // Populate bulk timezone combo
                    cmbBulkTimezone.BeginUpdate();
                    cmbBulkTimezone.Items.Clear();
                    foreach (var tz in _timezones)
                        cmbBulkTimezone.Items.Add(tz);
                    cmbBulkTimezone.EndUpdate();

                    // Populate combo column on the grid
                    colNewTimezone.DataSource = new List<TimezoneOption>(_timezones);
                    colNewTimezone.DisplayMember = nameof(TimezoneOption.DisplayName);
                    colNewTimezone.ValueMember = nameof(TimezoneOption.TimezoneCode);

                    _logger?.Log($"Loaded {_timezones.Count} timezone definitions.");

                    // Now load the views
                    LoadViews();
                }
            });
        }

        private void LoadViews()
        {
            if (Service == null) return;

            _logger?.Log("Retrieving system views for system users...");

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading views...",
                Work = (worker, args) =>
                {
                    args.Result = _viewService.GetSystemUserViews(Service);
                },
                PostWorkCallBack = args =>
                {
                    if (args.Error != null)
                    {
                        _logger?.LogError($"Failed to load views: {args.Error.Message}");
                        return;
                    }

                    var views = (List<SystemView>)args.Result;
                    _viewsLoaded = true;

                    cmbViews.BeginUpdate();
                    cmbViews.Items.Clear();
                    cmbViews.Items.Add(new SystemView { Name = "(Default – Enabled Users)", FetchXml = null });
                    foreach (var v in views)
                        cmbViews.Items.Add(v);
                    cmbViews.SelectedIndex = 0;
                    cmbViews.EndUpdate();

                    _logger?.Log($"Loaded {views.Count} system view(s).");
                }
            });
        }

        private void tsbLoadUsers_Click(object sender, EventArgs e)
        {
            if (Service == null)
            {
                MessageBox.Show("Please connect to a Dataverse environment first.",
                    "Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!_timezonesLoaded)
            {
                LoadTimezonesThenViews();
                return;
            }

            LoadUsers();
        }

        private void LoadUsers()
        {
            var selectedView = cmbViews.SelectedItem as SystemView;
            _logger?.Log(selectedView?.FetchXml != null
                ? $"Loading users using view: {selectedView.Name}..."
                : "Loading enabled users...");

            _allUsers.Clear();
            _filteredUsers.Clear();

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading users...",
                Work = (worker, args) =>
                {
                    List<UserTimezoneModel> users;
                    if (selectedView?.FetchXml != null)
                        users = _userService.GetUsersByFetchXml(Service, selectedView.FetchXml);
                    else
                        users = _userService.GetUsers(Service);

                    args.Result = users;
                },
                PostWorkCallBack = args =>
                {
                    if (args.Error != null)
                    {
                        _logger?.LogError($"Failed to load users: {args.Error.Message}");
                        return;
                    }

                    var users = (List<UserTimezoneModel>)args.Result;

                    foreach (var u in users)
                        _allUsers.Add(u);

                    ApplyFilter();
                    _logger?.Log($"Loaded {users.Count} user(s).");
                }
            });
        }

        // ── Filtering ─────────────────────────────────────────────────────────────

        private void txtFilter_TextChanged(object sender, EventArgs e) => ApplyFilter();

        private void ApplyFilter()
        {
            var filter = txtFilter.Text.Trim();

            _filteredUsers.Clear();

            IEnumerable<UserTimezoneModel> source = _allUsers;
            if (!string.IsNullOrEmpty(filter))
                source = source.Where(u => u.FullName != null &&
                    u.FullName.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0);

            foreach (var u in source)
                _filteredUsers.Add(u);
        }

        // ── Grid events ───────────────────────────────────────────────────────────

        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Only handle clicks on the "Apply" column (colApply)
            if (e.ColumnIndex != colApply.Index) return;

            // Don't interfere with multi-select when Ctrl or Shift are pressed
            if ((Control.ModifierKeys & (Keys.Control | Keys.Shift)) != 0)
                return;

            var model = _filteredUsers[e.RowIndex];
            ApplySingleUser(model);
        }

        private void dgvUsers_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Suppress errors thrown when the combo column value is not yet in its DataSource
            // (can happen briefly while timezones are still loading).
            e.Cancel = true;
        }

        private void dgvUsers_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Handle "Select All/Deselect All" when clicking the checkbox column header
            if (colSelect.Index == e.ColumnIndex)
            {
                // Determine the new state based on current selections
                bool allSelected = _filteredUsers.All(u => u.IsSelected);
                bool newState = !allSelected;

                // Toggle all selections
                foreach (var user in _filteredUsers)
                    user.IsSelected = newState;
            }
        }

        // ── Apply single ──────────────────────────────────────────────────────────

        private void ApplySingleUser(UserTimezoneModel model)
        {
            if (Service == null) return;

            var newCode = model.NewTimezoneCode;
            var newName = _timezones.FirstOrDefault(t => t.TimezoneCode == newCode)?.DisplayName ?? newCode.ToString();

            _logger?.Log($"Updating timezone for {model.FullName}...");

            WorkAsync(new WorkAsyncInfo
            {
                Work = (worker, args) =>
                {
                    var oldName = _userService.UpdateUserTimezone(Service, model.UserId, newCode);
                    args.Result = oldName;
                },
                PostWorkCallBack = args =>
                {
                    if (args.Error != null)
                    {
                        _logger?.LogError($"Failed to update {model.FullName}: {args.Error.Message}");
                        return;
                    }

                    var oldName = (string)args.Result;
                    model.CurrentTimezoneCode = newCode;
                    model.CurrentTimezoneName = newName;
                    _logger?.Log($"Success. Timezone changed from {oldName} to {newName}");

                    // Force grid refresh for the affected row
                    dgvUsers.InvalidateRow(_filteredUsers.IndexOf(model));
                }
            });
        }

        // ── Bulk apply ────────────────────────────────────────────────────────────

        private void btnApplyToSelected_Click(object sender, EventArgs e)
        {
            if (Service == null) return;

            var bulkTz = cmbBulkTimezone.SelectedItem as TimezoneOption;
            if (bulkTz == null)
            {
                MessageBox.Show("Please select a timezone to apply.", "No Timezone Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Collect selected users via checkbox column
            var targets = _filteredUsers.Where(u => u.IsSelected).ToList();

            if (targets.Count == 0)
            {
                MessageBox.Show("Please select at least one user in the grid.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _logger?.Log($"Bulk update: applying '{bulkTz.DisplayName}' to {targets.Count} user(s)...");
            tsbLoadUsers.Enabled = false;
            btnApplyToSelected.Enabled = false;

            WorkAsync(new WorkAsyncInfo
            {
                Message = $"Applying timezone to {targets.Count} user(s)...",
                Work = (worker, args) =>
                {
                    int succeeded = 0, failed = 0;
                    var errors = new List<string>();

                    for (int i = 0; i < targets.Count; i++)
                    {
                        var model = targets[i];
                        worker.ReportProgress((int)((double)i / targets.Count * 100),
                            $"Updating {model.FullName} ({i + 1}/{targets.Count})...");

                        try
                        {
                            var oldName = _userService.UpdateUserTimezone(Service, model.UserId, bulkTz.TimezoneCode);

                            // Model updates must happen on UI thread – queue them via ProgressChanged
                            worker.ReportProgress(-1, new BulkProgressItem
                            {
                                Model = model,
                                NewCode = bulkTz.TimezoneCode,
                                NewName = bulkTz.DisplayName,
                                OldName = oldName,
                                Success = true
                            });
                            succeeded++;
                        }
                        catch (Exception ex)
                        {
                            worker.ReportProgress(-1, new BulkProgressItem
                            {
                                Model = model,
                                Success = false,
                                ErrorMessage = ex.Message
                            });
                            failed++;
                        }
                    }

                    args.Result = new BulkResult { Processed = targets.Count, Succeeded = succeeded, Failed = failed };
                },
                ProgressChanged = progress =>
                {
                    // String messages update the XrmToolBox status bar; BulkProgressItem updates models
                    if (progress.UserState is string msg)
                    {
                        SetWorkingMessage(msg);
                    }
                    else if (progress.UserState is BulkProgressItem item)
                    {
                        if (item.Success)
                        {
                            item.Model.CurrentTimezoneCode = item.NewCode;
                            item.Model.CurrentTimezoneName = item.NewName;
                            _logger?.Log($"Updated {item.Model.FullName}: {item.OldName} → {item.NewName}");
                        }
                        else
                        {
                            _logger?.LogError($"Failed {item.Model.FullName}: {item.ErrorMessage}");
                        }
                    }
                },
                PostWorkCallBack = args =>
                {
                    tsbLoadUsers.Enabled = true;
                    btnApplyToSelected.Enabled = true;

                    if (args.Error != null)
                    {
                        _logger?.LogError($"Bulk update error: {args.Error.Message}");
                        return;
                    }

                    var result = (BulkResult)args.Result;
                    _logger.Log("─────────────────────────────────");
                    _logger?.Log($"Processed : {result.Processed} users");
                    _logger?.Log($"Succeeded : {result.Succeeded}");
                    _logger?.Log($"Failed    : {result.Failed}");
                    _logger.Log("─────────────────────────────────");

                    // Clear all checkbox selections after update
                    foreach (var user in _filteredUsers)
                        user.IsSelected = false;

                    dgvUsers.Refresh();
                }
            });
        }

        // ── View selection ────────────────────────────────────────────────────────

        private void cmbViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Only auto-load if we already have users visible (don't trigger on initial population)
            if (_allUsers.Count > 0 && _viewsLoaded)
                LoadUsers();
        }

        // ── Export CSV ────────────────────────────────────────────────────────────

        private void tsbExportCsv_Click(object sender, EventArgs e)
        {
            _csvExportService.Export(_filteredUsers, this);
        }

        // ── Close tool ────────────────────────────────────────────────────────────

        private void tsbClose_Click(object sender, EventArgs e) => CloseTool();

        // ── Private helper types ──────────────────────────────────────────────────

        private class BulkProgressItem
        {
            public UserTimezoneModel Model { get; set; }
            public int NewCode { get; set; }
            public string NewName { get; set; }
            public string OldName { get; set; }
            public bool Success { get; set; }
            public string ErrorMessage { get; set; }
        }

        private class BulkResult
        {
            public int Processed { get; set; }
            public int Succeeded { get; set; }
            public int Failed { get; set; }
        }
    }
}
