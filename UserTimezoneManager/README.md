# User Timezone Manager

User Timezone Manager is an XrmToolBox plugin for viewing and updating Dataverse user time zones.

This project was vibe coded using GitHub Copilot. Credit for product ideas and workflow direction belongs to the project owner; implementation execution was handled through Copilot-assisted development.

It is built for day-to-day admin workflows where you need to quickly review users, apply timezone changes one-by-one or in bulk, and export results.

## What it does

- Loads timezone definitions from Dataverse (`timezonedefinition`)
- Loads system-user views from Dataverse (`savedquery`)
- Loads users from:
  - default enabled-user query, or
  - selected system view FetchXML
- Shows each user’s current timezone and lets you choose a new timezone
- Applies timezone updates:
  - per user (Apply button in grid)
  - in bulk (selected users)
- Supports checkbox-based selection:
  - checkbox per row
  - click checkbox column header to select/deselect all
- Filters users by full name
- Exports visible rows to CSV

## Requirements

- XrmToolBox
- Access to a Dataverse environment
- Permissions to read/update:
  - `systemuser`
  - `usersettings`
  - `timezonedefinition`
  - `savedquery`

## How to use

1. Open **User Timezone Manager** in XrmToolBox.
2. Connect to your Dataverse environment.
3. Wait for timezone definitions and views to load.
4. (Optional) Pick a user view from the views dropdown.
5. Click **Load Users**.
6. (Optional) Filter by name.
7. Choose new timezone values in the grid.
8. Apply updates:
   - Single user: click **Apply** on that row
   - Bulk: check rows (or use header checkbox), pick bulk timezone, click **Apply To Selected**
9. (Optional) Export current grid contents via **Export CSV**.

## Notes

- Timezone definitions are cached per connection and refreshed when the connection changes.
- Bulk update progress and results are written to the log panel.
- Checkbox selections are cleared after a bulk update finishes.

## CSV export

Export writes UTF-8 CSV with columns:

- Full Name
- Email Address
- Current Timezone

## Project structure

- `UserTimezoneManagerPlugin.cs` - XrmToolBox plugin entry point
- `UI/UserTimezoneManagerControl.cs` - UI workflow and event handling
- `Services/` - Dataverse query/update and CSV export services
- `Models/` - grid and option models
- `Logging/` - log abstraction and RichTextBox logger implementation

## Build

Target framework: **.NET Framework 4.8**
