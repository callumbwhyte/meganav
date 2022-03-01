using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

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