using Umbraco.Core.Logging;
using Umbraco.Core.PropertyEditors;
using Umbraco.Core.Services;

namespace Our.Umbraco.Meganav.PropertyEditors
{
    [DataEditor(Constants.PropertyEditorAlias, Constants.PropertyEditorName, Group = "pickers")]
    internal class MeganavPropertyEditor : DataEditor
    {
        private readonly IContentService _contentService;

        public MeganavPropertyEditor(IContentService contentService, ILogger logger)
            : base(logger, EditorType.PropertyValue)
        {
            _contentService = contentService;
        }

        protected override IConfigurationEditor CreateConfigurationEditor() => new MeganavConfigurationEditor();

        protected override IDataValueEditor CreateValueEditor() => new MeganavValueEditor(_contentService);
    }
}