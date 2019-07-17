using Umbraco.Core.Logging;
using Umbraco.Core.PropertyEditors;

namespace Our.Umbraco.Meganav.PropertyEditors
{
    [DataEditor(Constants.PropertyEditorAlias, Constants.PropertyEditorName, Group = "pickers")]
    internal class MeganavPropertyEditor : DataEditor
    {
        public MeganavPropertyEditor(ILogger logger)
            : base(logger, EditorType.PropertyValue)
        {

        }

        protected override IConfigurationEditor CreateConfigurationEditor() => new MeganavConfigurationEditor();
    }
}