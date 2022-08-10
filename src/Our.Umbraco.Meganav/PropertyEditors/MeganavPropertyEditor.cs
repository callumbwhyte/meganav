using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

namespace Our.Umbraco.Meganav.PropertyEditors
{
    [DataEditor(Constants.PropertyEditorAlias, Constants.PropertyEditorName, Group = "pickers")]
    internal class MeganavPropertyEditor : DataEditor
    {
        private readonly IIOHelper _ioHelper;
        private readonly IEditorConfigurationParser _editorConfigurationParser;

        public MeganavPropertyEditor(IDataValueEditorFactory dataValueEditorFactory, IIOHelper ioHelper, IEditorConfigurationParser editorConfigurationParser)
            : base(dataValueEditorFactory, EditorType.PropertyValue)
        {
            _ioHelper = ioHelper;
            _editorConfigurationParser = editorConfigurationParser;
        }

        protected override IConfigurationEditor CreateConfigurationEditor() => new MeganavConfigurationEditor(_ioHelper, _editorConfigurationParser);

        protected override IDataValueEditor CreateValueEditor() => DataValueEditorFactory.Create<MeganavValueEditor>();
    }
}