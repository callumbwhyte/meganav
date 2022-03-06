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
    }
}