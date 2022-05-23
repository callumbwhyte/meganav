using System.Collections.Generic;
using System.Linq;
using Our.Umbraco.Meganav.Models;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

namespace Our.Umbraco.Meganav.PropertyEditors
{
    internal class MeganavConfigurationEditor : ConfigurationEditor<MeganavConfiguration>
    {
        public MeganavConfigurationEditor(IIOHelper ioHelper)
            : base(ioHelper)
        {

        }

        public override IDictionary<string, object> ToValueEditor(object configuration)
        {
            var value = base.ToValueEditor(configuration);

            if (value.TryGetValue("itemTypes", out var data) == true)
            {
                if (data is IEnumerable<IMeganavItemType> itemTypes)
                {
                    foreach (var itemType in itemTypes.OfType<MeganavItemType>())
                    {
                        itemType.Icon = itemType.Icon ?? "icon-link";
                    }
                }
            }

            return value;
        }
    }
}