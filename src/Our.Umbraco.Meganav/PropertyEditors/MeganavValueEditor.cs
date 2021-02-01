using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Our.Umbraco.Meganav.Models;
using Umbraco.Core.Models;
using Umbraco.Core.Models.Editors;
using Umbraco.Core.PropertyEditors;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Composing;

namespace Our.Umbraco.Meganav.PropertyEditors
{
    internal class MeganavValueEditor : DataValueEditor
    {
        private readonly IContentService _contentService;

        public MeganavValueEditor(IContentService contentService)
        {
            View = "/App_Plugins/Meganav/backoffice/propertyeditors/editor.html";
            ValueType = "JSON";

            _contentService = contentService;
        }

        public override object ToEditor(Property property, IDataTypeService dataTypeService, string culture = null, string segment = null)
        {
            var data = property.GetValue(culture, segment);

            if (!(data is string value))
            {
                return null;
            }

            var entities = JsonConvert.DeserializeObject<IEnumerable<MeganavEditorEntity>>(value);

            EnrichEntities(Current.UmbracoContext, entities, culture);

            return entities;
        }

        public override object FromEditor(ContentPropertyData editorValue, object currentValue)
        {
            if (!(editorValue.Value is JToken value))
            {
                return null;
            }

            var entities = value.ToObject<IEnumerable<MeganavEntity>>();

            SanitizeEntities(entities);

            return JsonConvert.SerializeObject(entities);
        }

        private void EnrichEntities(UmbracoContext umbracoContext, IEnumerable<MeganavEditorEntity> entities, string culture)
        {
            foreach (var entity in entities)
            {
                if (entity.Udi != null)
                {
                    var content = _contentService.GetById(entity.Udi.Guid);

                    if (content == null)
                    {
                        continue;
                    }

                    entity.Name = content.GetCultureName(culture) ?? content.Name;

                    entity.Icon = content.ContentType.Icon;

                    entity.Published = content.IsCulturePublished(culture ?? "") || content.Published;

                    entity.Url = umbracoContext?.Content.GetById(content.Id)?.Url(culture);
                }

                if (entity.Children != null)
                {
                    EnrichEntities(umbracoContext, entity.Children, culture);
                }
            }
        }

        private void SanitizeEntities(IEnumerable<MeganavEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (entity.Udi != null)
                {
                    entity.Url = null;
                }

                if (entity.Children != null)
                {
                    SanitizeEntities(entity.Children);
                }
            }
        }
    }
}