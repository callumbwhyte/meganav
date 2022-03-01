using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;

namespace Our.Umbraco.Meganav.PropertyEditors
{
    [DataEditor(Constants.PropertyEditorAlias, Constants.PropertyEditorName, Group = "pickers")]
    internal class MeganavPropertyEditor : DataEditor
    {
        private readonly IIOHelper _ioHelper;

        public MeganavPropertyEditor(IDataValueEditorFactory dataValueEditorFactory, IIOHelper ioHelper)
            : base(dataValueEditorFactory, EditorType.PropertyValue)
        {
            _ioHelper = ioHelper;
        }

        protected override IConfigurationEditor CreateConfigurationEditor() => new MeganavConfigurationEditor(_ioHelper);

        protected override IDataValueEditor CreateValueEditor() => DataValueEditorFactory.Create<MeganavValueEditor>();
    }
}