using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace Ronin.XrmToolbox
{
    // Do not forget to update version number and author (company attribute) in AssemblyInfo.cs class
    // To generate Base64 string for Images below, you can use https://www.base64-image.de/
    [Export(typeof(IXrmToolBoxPlugin)),
        ExportMetadata("Name", "Web Resource Mapper"),
        ExportMetadata("Description", "Plugin to manage the mapping of WebResources from a local folder to a Dynamics 365 solution."),
        // Please specify the base64 content of a 32x32 pixels image
        ExportMetadata("SmallImageBase64", "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAAANZSURBVFhH7ZddaBRXGIafySZpzGxsmE0k/gU3PzsbmrTBUKxColAvqiBNVqy9UFQCerFo/wje2KVSCrahCSaQi2JooS4VoYEW0cs0URCEYNVUSSKCdxqJSDKjNmn260XODOskGyfWrTc+8MLMOd/3nXfOOfMHSycEHAduAE+Vrqu2kDf4ZfM+MA5IBj1QMVnhPeAJIFUNIQm/bQggod0dUvbJOVlmbnFMPFGxvsjxNmQgAPwIFGz8sJyDnRtYHVk+1zM7wzJzM2VHfqeosRWgQMUGPDUWxK+BbUC0ZE0hOw7XcPPSfW4M3AMgtzQ8F6FphD5qJ6+0AiCqcp6LBhQCXwC16txhCPhWHXcAn23dV8Wdaw+58+dDAAqqNrHy0/OQEwBJMWtNMDnwA4/OnwDoBD5X+UeBBqewWqph4HuAMwtsJtF1PWVZ1lnLss42NTXdBaRuc5kAEiheJUbsG1l3clzCPVMS+rhDAkUlc7lajgDS2Nh418nXdT3lra/0Sy7QAtDb20t+fr5r0TRNTdO0XQAVFRUMDg66V160cQ9vbj0CgHX5NBNn5i60cHkejydnAKisrCzXNK0coL+/n5GREbf29PQ0ra2tADEcN7ZtZ1RXV5frWst7Q1YfuyLhnikJ90xJXpkpgOxsq5X2i9ulqiEkgHR3d8+rky6nnq9N2NLSQjAYBKB421HyV9W4fTPjtwF4d/sahi/e5/bQBMFgkObmZjdmMXwZMAyDRCIBwKML3zE5eApJ/QNArrEWgN+6bpH86ioAiUQCwzDSKizOc5fAtm2xLEvi8bg7dYGiFVJY+4HkGmuf2VjxeHxe7kJKy/FnwFEymZRoNPrMoICYpinJZHJefCY5eZrjwrZt78wsyujoKGNjYwBUV1cTiUS8IYui6zqoB88LGfivOAZ8bcJs8trAawOv3IB7G8ZiMW9fVunr64N0A68K18CWnXu9fVnlj19/hnQD+79s98ZklZ++bgO1CQUgNTvrjckaaWMJ6q9m3tvtf9I1gHp1kOnDMRtKqTHr0z/D/ZAD3AIih9qOUfPOegCGh65wqvMEwF9AnRrEF0t9EB0EIqHSFZh19W5jTX0DxUYJwFvAgfSEl0mt+hP2TqdXj1WsL5Y6A397GxZgZilL8C8zUStqFucoygAAAABJRU5ErkJggg=="),
        // Please specify the base64 content of a 80x80 pixels image
        ExportMetadata("BigImageBase64", "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"),
        ExportMetadata("BackgroundColor", "Lavender"),
        ExportMetadata("PrimaryFontColor", "Black"),
        ExportMetadata("SecondaryFontColor", "Gray")]
    public class WebResourceMapper : PluginBase
    {
        public override IXrmToolBoxPluginControl GetControl()
        {
            return new WebResourceMapperController();
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        public WebResourceMapper()
        {
            // If you have external assemblies that you need to load, uncomment the following to 
            // hook into the event that will fire when an Assembly fails to resolve
            // AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolveEventHandler);
        }

        /// <summary>
        /// Event fired by CLR when an assembly reference fails to load
        /// Assumes that related assemblies will be loaded from a subfolder named the same as the Plugin
        /// For example, a folder named Sample.XrmToolBox.MyPlugin 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private Assembly AssemblyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            Assembly loadAssembly = null;
            Assembly currAssembly = Assembly.GetExecutingAssembly();

            // base name of the assembly that failed to resolve
            var argName = args.Name.Substring(0, args.Name.IndexOf(","));

            // check to see if the failing assembly is one that we reference.
            List<AssemblyName> refAssemblies = currAssembly.GetReferencedAssemblies().ToList();
            var refAssembly = refAssemblies.Where(a => a.Name == argName).FirstOrDefault();

            // if the current unresolved assembly is referenced by our plugin, attempt to load
            if (refAssembly != null)
            {
                // load from the path to this plugin assembly, not host executable
                string dir = Path.GetDirectoryName(currAssembly.Location).ToLower();
                string folder = Path.GetFileNameWithoutExtension(currAssembly.Location);
                dir = Path.Combine(dir, folder);

                var assmbPath = Path.Combine(dir, $"{argName}.dll");

                if (File.Exists(assmbPath))
                {
                    loadAssembly = Assembly.LoadFrom(assmbPath);
                }
                else
                {
                    throw new FileNotFoundException($"Unable to locate dependency: {assmbPath}");
                }
            }

            return loadAssembly;
        }
    }
}