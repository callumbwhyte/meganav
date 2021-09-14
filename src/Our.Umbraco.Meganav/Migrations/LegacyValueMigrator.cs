using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Our.Umbraco.Meganav.Models;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Our.Umbraco.Meganav.Migrations
{
    internal class LegacyValueMigrator
    {
        private readonly IContentTypeService _contentTypeService;
        private readonly IContentService _contentService;
        private readonly ILogger _logger;

        public LegacyValueMigrator(IContentTypeService contentTypeService, IContentService contentService, ILogger logger)
        {
            _contentTypeService = contentTypeService;
            _contentService = contentService;
            _logger = logger;
        }

        public void MigrateContentValues(IDataType dataType)
        {
            var contentTypes = _contentTypeService.GetAll()
                .Where(x => x.PropertyTypes.Any(p => p.DataTypeId == dataType.Id))
                .ToList();

            var contentTypeIds = contentTypes.Select(x => x.Id).ToArray();

            var contentItems = _contentService.GetPagedOfTypes(contentTypeIds, 0, int.MaxValue, out long totalRecords, null);

            var propertyAliases = contentTypes
                .SelectMany(x => x.PropertyTypes)
                .Where(x => x.DataTypeId == dataType.Id)
                .Select(x => x.Alias)
                .Distinct()
                .ToArray();

            MigrateContentValues(contentItems, propertyAliases);
        }

        public void MigrateContentValues(IEnumerable<IContent> contentItems, string[] propertyAliases)
        {
            foreach (var content in contentItems)
            {
                var properties = content.Properties.Where(x => propertyAliases.InvariantContains(x.Alias));

                foreach (var property in properties)
                {
                    foreach (var propertyValue in property.Values)
                    {
                        try
                        {
                            var entities = ConvertToEntity(propertyValue.EditedValue);

                            var value = JsonConvert.SerializeObject(entities);

                            property.SetValue(value, propertyValue.Culture, propertyValue.Segment);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error<LegacyValueMigrator>(ex, "Failed to migrate Meganav values for {alias} on content {id}", property.Alias, content.Id);
                        }
                    }
                }

                _contentService.Save(content);
            }
        }

        private IEnumerable<MeganavEntity> ConvertToEntity(object value)
        {
            if (!(value is JToken data))
            {
                if (value is string stringValue)
                {
                    data = JToken.Parse(stringValue);
                }
                else
                {
                    yield break;
                }
            }

            foreach (var item in data.Children())
            {
                var entity = new MeganavEntity
                {
                    Title = item.Value<string>("title"),
                    Target = item.Value<string>("target"),
                    Visible = !item.Value<bool>("naviHide")
                };

                var id = item.Value<int>("id");

                GuidUdi.TryParse(item.Value<string>("udi"), out GuidUdi udi);

                if (id > 0 || udi?.Guid != Guid.Empty)
                {
                    var contentItem = udi != null
                        ? _contentService.GetById(udi.Guid)
                        : _contentService.GetById(id);

                    if (contentItem != null)
                    {
                        entity.Udi = contentItem.GetUdi();
                    }
                }
                else
                {
                    entity.Url = item.Value<string>("url");
                }

                var children = item.Value<JArray>("children");

                if (children != null)
                {
                    entity.Children = ConvertToEntity(children);
                }

                var ignoreProperties = new[] { "id", "key", "udi", "name", "title", "description", "target", "url", "children", "icon", "published", "naviHide", "culture" };

                var settings = item.ToObject<IDictionary<string, object>>();

                settings.RemoveAll(x => ignoreProperties.InvariantContains(x.Key));

                entity.Settings = settings;

                yield return entity;
            }
        }
    }
}