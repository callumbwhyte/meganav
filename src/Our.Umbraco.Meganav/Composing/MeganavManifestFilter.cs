using System.Collections.Generic;
using System.Diagnostics;
using Umbraco.Cms.Core.Manifest;
using Umbraco.Cms.Core.Semver;
using Umbraco.Extensions;

namespace Our.Umbraco.Meganav.Composing
{
    internal class MeganavManifestFilter : IManifestFilter
    {
        public void Filter(List<PackageManifest> manifests)
        {
            manifests.Add(new PackageManifest
            {
                PackageName = "Meganav",
                Scripts = new[]
                {
                    "/App_Plugins/Meganav/lib/angular-ui-tree.min.js",
                    "/App_Plugins/Meganav/backoffice/components/meganavActions.component.js",
                    "/App_Plugins/Meganav/backoffice/components/meganavAdd.component.js",
                    "/App_Plugins/Meganav/backoffice/components/meganavItemType.component.js",
                    "/App_Plugins/Meganav/backoffice/components/meganavPreview.component.js",
                    "/App_Plugins/Meganav/backoffice/dialogs/itemTypeDialog.controller.js",
                    "/App_Plugins/Meganav/backoffice/dialogs/settingsDialog.controller.js",
                    "/App_Plugins/Meganav/backoffice/dialogs/settingsDialog.link.controller.js",
                    "/App_Plugins/Meganav/backoffice/dialogs/settingsDialog.settings.controller.js",
                    "/App_Plugins/Meganav/backoffice/helpers/dialogHelper.service.js",
                    "/App_Plugins/Meganav/backoffice/helpers/localizationHelper.service.js",
                    "/App_Plugins/Meganav/backoffice/propertyeditors/configuration.controller.js",
                    "/App_Plugins/Meganav/backoffice/propertyeditors/editor.controller.js"
                },
                Stylesheets = new[]
                {
                    "/App_Plugins/Meganav/lib/angular-ui-tree.min.css",
                    "/App_Plugins/Meganav/backoffice/meganav.css"
                },
                AllowPackageTelemetry = true,
                Version = GetVersion()
            });
        }

        private static string GetVersion()
        {
            var assembly = typeof(MeganavManifestFilter).Assembly;
            try
            {
                var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.GetAssemblyFile().FullName);
                if (fileVersionInfo.ProductVersion != null && SemVersion.TryParse(fileVersionInfo.ProductVersion, out var productVersion))
                {
                    return productVersion.ToSemanticStringWithoutBuild();
                }
            }
            catch
            {
                //default to assembly version
            }

            return assembly.GetName().Version.ToString(3);
        }
    }
}