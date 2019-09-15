using System.Collections.Generic;
using Our.Umbraco.Meganav.Models;
using Umbraco.Core.PropertyEditors;

namespace Our.Umbraco.Meganav.PropertyEditors
{
    internal class MeganavConfiguration
    {
        [ConfigurationField(
            "itemTypes",
            "Item Types",
            "/App_Plugins/Meganav/backoffice/propertyeditors/configuration.html",
            Description = "Configure custom nav item types with unique settings"
        )]
        public IEnumerable<MeganavItemType> ItemTypes { get; set; }

        [ConfigurationField(
            "maxItems",
            "Max Items",
            "number",
            Description = "The maximum number of top-level nav items allowed"
        )]
        public int? MaxItems { get; set; }
    }
}