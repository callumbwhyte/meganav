using Umbraco.Core.PropertyEditors;

namespace Our.Umbraco.Meganav.PropertyEditors
{
    internal class MeganavConfiguration
    {
        [ConfigurationField(
            "maxItems",
            "Max Items",
            "number",
            Description = "The maximum number of top-level nav items allowed"
        )]
        public int? MaxItems { get; set; }
    }
}