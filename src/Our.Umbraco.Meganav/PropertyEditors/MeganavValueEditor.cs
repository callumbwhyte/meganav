using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Our.Umbraco.Meganav.Models;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Editors;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Strings;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;

namespace Our.Umbraco.Meganav.PropertyEditors
{
    internal class MeganavValueEditor : DataValueEditor
    {
        private readonly IContentService _contentService;
        private readonly IPublishedUrlProvider _publishedUrlProvider;
        private readonly IUmbracoContextFactory _umbracoContextFactory;

        public MeganavValueEditor(IContentService contentService, IPublishedUrlProvider publishedUrlProvider, IUmbracoContextFactory umbracoContextFactory, ILocalizedTextService localizedTextService, IShortStringHelper shortStringHelper, IJsonSerializer jsonSerializer)
            : base(localizedTextService, shortStringHelper, jsonSerializer)
        {
            View = "/App_Plugins/Meganav/backoffice/propertyeditors/editor.html";
            ValueType = "JSON";

            _contentService = contentService;
            _publishedUrlProvider = publishedUrlProvider;
            _umbracoContextFactory = umbracoContextFactory;
        }

        public override object ToEditor(IProperty property, string culture = null, string segment = null)
        {
            var data = property.GetValue(culture, segment);

            if (!(data is string value))
            {
                return null;
            }

            var entities = JsonConvert.DeserializeObject<IEnumerable<MeganavEditorEntity>>(value);

            using (var context = _umbracoContextFactory.EnsureUmbracoContext())
            {
                EnrichEntities(context.UmbracoContext, entities, culture);
            }

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

        private void EnrichEntities(IUmbracoContext umbracoContext, IEnumerable<MeganavEditorEntity> entities, string culture)
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

                    entity.Url = umbracoContext.Content.GetById(content.Id)?.Url(_publishedUrlProvider, culture);
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