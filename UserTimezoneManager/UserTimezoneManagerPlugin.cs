using System.ComponentModel.Composition;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;
using Ronin.XrmToolbox.UserTimezoneManager.UI;

namespace Ronin.XrmToolbox.UserTimezoneManager
{
    [Export(typeof(IXrmToolBoxPlugin)),
        ExportMetadata("Name", "User Timezone Manager"),
        ExportMetadata("Description", "View and manage Dataverse user time zones in bulk. Load users by system view, change timezones individually or en-masse, filter, and export to CSV."),
        ExportMetadata("SmallImageBase64", "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAABmJLR0QA/wD/AP+gvaeTAAAADUlEQVRYw2NgYGBgAAAABQABXvMHGgAAAABJRU5ErkJggg=="),
        ExportMetadata("BigImageBase64", "iVBORw0KGgoAAAANSUhEUgAAAFAAAABQCAYAAACOEfKtAAAABmJLR0QA/wD/AP+gvaeTAAAADUlEQVRYw2NgYGBgAAAABQABXvMHGgAAAABJRU5ErkJggg=="),
        ExportMetadata("BackgroundColor", "SteelBlue"),
        ExportMetadata("PrimaryFontColor", "White"),
        ExportMetadata("SecondaryFontColor", "LightCyan")]
    public class UserTimezoneManagerPlugin : PluginBase
    {
        public override IXrmToolBoxPluginControl GetControl()
        {
            return new UserTimezoneManagerControl();
        }
    }
}
