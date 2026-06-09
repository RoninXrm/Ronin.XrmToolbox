# Web Resource Mapper (XrmToolBox Plugin)

This is an **XrmToolBox plugin** for managing Dataverse web resources with a local folder workflow.

If you’ve ever bounced between web resources in Dataverse and files on disk and thought _"there has to be a cleaner way"_, this is for you.

This project was inspired by [**D365 Developer Extensions / D365 Developer Tools**](https://github.com/tsharp/D365DeveloperExtensions) and its web resource workflow in Visual Studio. I wanted something that gave me that same kind of experience while working in **VS Code** instead of being tied to full Visual Studio.

---

## What it does

- Loads **unmanaged solutions**
- Loads solution web resources via `solutioncomponent` (component type `61`)
- Lets you map web resources to local files
- Supports a root-folder-based workflow with **relative paths**
- Download selected web resources to disk
- Upload new web resources (with a naming dialog)
- Update selected web resources from local files
- Publish updated web resources automatically
- Bulk operations using DataGridView checkboxes
- Save mappings in plugin settings (grouped by connection + solution)

---

## UI at a glance

- Solution picker
- Root folder picker
- Web resources grid with mapping column
- Actions:
  - Load Web Resources
  - Download Selected
  - Upload New
  - Update Selected
  - Reset + Auto Map

---

## Mapping behavior highlights

- Mappings are persisted per **connection + solution**
- Paths are stored as **relative to root folder**
- Prefix-folder download logic supports publisher prefixes like `new_`
- "No" answers for prefix-folder creation can be remembered per solution

---

## Tech stack

- C# / WinForms
- .NET Framework 4.8
- XrmToolBox Plugin SDK (`PluginControlBase`)
- Dataverse SDK (`IOrganizationService`, `QueryExpression`)

---

## Local development

1. Open solution in Visual Studio
2. Build `Debug`
3. Run/debug with XrmToolBox plugin workflow

> The project post-build step copies the plugin assembly to a local `Plugins` folder in `bin\Debug`.

---

## Why this exists

I really liked the web resource workflow in D365 Developer Extensions, especially the practical day-to-day loop of mapping, downloading, updating, and publishing. This plugin exists to bring that same style of workflow into my XrmToolBox + VS Code setup, with solution-aware mappings and local-folder control.

---

## Fun fact

This entire plugin was developed through **vibe coding with GitHub Copilot** 🤖✨

---

## Credits

Icons provided by [**Flaticon (Pixel Perfect)**](https://www.flaticon.com/authors/pixel-perfect) and licensed under [**Creative Commons BY 3.0**](https://creativecommons.org/licenses/by/3.0/).

---

## Feedback

If you find bugs or want improvements, open an issue or PR. Happy to iterate with the community.
