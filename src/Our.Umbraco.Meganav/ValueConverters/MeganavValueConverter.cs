using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Our.Umbraco.Meganav.Models;
using Our.Umbraco.Meganav.PropertyEditors;
using Our.Umbraco.Meganav.PublishedContent;
using Umbraco.Core;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web;
using Umbraco.Web.Composing;
using Umbraco.Web.PublishedCache;

namespace Our.Umbraco.Meganav.ValueConverters
{
    internal class MeganavValueConverter : PropertyValueConverterBase
    {
        private readonly PublishedElementFactory _publishedElementFactory;

        private MeganavConfiguration _config;

        public MeganavValueConverter(PublishedElementFactory publishedElementFactory)
        {
            _publishedElementFactory = publishedElementFactory;
        }

        public override bool IsConverter(IPublishedPropertyType propertyType)
        {
            return propertyType.EditorAlias.Equals(Constants.PropertyEditorAlias);
        }

        public override Type GetPropertyValueType(IPublishedPropertyType propertyType)
        {
            return typeof(IEnumerable<IMeganavItem>);
        }

        public override object ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object inter, bool preview)
        {
            EnsureConfiguration(propertyType);

            if (!(inter is string value))
            {
                return Enumerable.Empty<IMeganavItem>();
            }

            var entities = JsonConvert.DeserializeObject<IEnumerable<MeganavEntity>>(value);

            var items = BuildItems(Current.UmbracoContext, entities);

            if (_config.MaxItems.HasValue == true)
            {
                return items.Take(_config.MaxItems.Value);
            }

            return items;
        }

        private IEnumerable<MeganavItem> BuildItems(UmbracoContext umbracoContext, IEnumerable<MeganavEntity> entities, int level = 0)
        {
            foreach (var entity in entities)
            {
                if (entity.Visible == false)
                {
                    continue;
                }

                var item = new MeganavItem
                {
                    Title = entity.Title,
                    Url = entity.Url,
                    Target = entity.Target,
                    Level = level
                };

                if (entity.ItemTypeId != null)
                {
                    var itemType = _config.ItemTypes.FirstOrDefault(x => x.Id == entity.ItemTypeId.Value);

                    if (itemType != null)
                    {
                        if (itemType.AllowAtRoot == false && level == 0)
                        {
                            continue;
                        }

                        item.ItemType = itemType;

                        if (itemType.SettingsType.HasValue && entity.Settings != null)
                        {
                            if (umbracoContext?.Content is IPublishedCache2 publishedCache)
                            {
                                var contentType = publishedCache.GetContentType(itemType.SettingsType.Value);

                                if (contentType?.IsElement == true)
                                {
                                    item.Settings = _publishedElementFactory.CreateElement(contentType, entity.Settings);
                                }
                            }
                        }

                        if (itemType.AllowedTypes.Any() == true)
                        {
                            item.Children = item.Children.Where(x => itemType.AllowedTypes.Any(t => t == x.ItemType.Id));
                        }
                    }
                }

                if (entity.Udi != null)
                {
                    var culture = umbracoContext?.PublishedRequest?.Culture.Name;

                    var content = umbracoContext?.Content.GetById(entity.Udi);

                    if (content == null)
                    {
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(entity.Title) == true)
                    {
                        item.Title = content.Name(culture);
                    }

                    item.Url = content.Url(culture);

                    item.Content = content;
                }

                if (entity.Children != null)
                {
                    item.Children = BuildItems(umbracoContext, entity.Children, level + 1);
                }

                yield return item;
            }
        }

        private void EnsureConfiguration(IPublishedPropertyType propertyType)
        {
            if (_config == null)
            {
                _config = propertyType.DataType.ConfigurationAs<MeganavConfiguration>();
            }
        }
    }
}