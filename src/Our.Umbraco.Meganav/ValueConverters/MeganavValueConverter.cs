using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Our.Umbraco.Meganav.Models;
using Our.Umbraco.Meganav.PropertyEditors;
using Our.Umbraco.Meganav.PublishedContent;
using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Models.DeliveryApi;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.PropertyEditors.DeliveryApi;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Core;
using Umbraco.Extensions;
using static Umbraco.Cms.Core.Constants;

namespace Our.Umbraco.Meganav.ValueConverters
{
    internal class MeganavValueConverter : PropertyValueConverterBase, IDeliveryApiPropertyValueConverter
	{
		private readonly IPublishedSnapshotAccessor _publishedSnapshotAccessor;
		private readonly IApiContentNameProvider _apiContentNameProvider;
		private readonly IApiContentRouteBuilder _apiContentRouteBuilder;
		private readonly IPublishedUrlProvider _publishedUrlProvider;
		private readonly IVariationContextAccessor _variationContextAccessor;
		private readonly IUmbracoContextFactory _umbracoContextFactory;
		private readonly PublishedElementFactory _publishedElementFactory;


		private MeganavConfiguration _config;

        public MeganavValueConverter(IPublishedSnapshotAccessor publishedSnapshotAccessor, IApiContentNameProvider apiContentNameProvider, IApiContentRouteBuilder apiContentRouteBuilder, IPublishedUrlProvider publishedUrlProvider, IVariationContextAccessor variationContextAccessor, IUmbracoContextFactory umbracoContextFactory, PublishedElementFactory publishedElementFactory)
        {
            _publishedSnapshotAccessor  = publishedSnapshotAccessor;
            _apiContentNameProvider = apiContentNameProvider;
            _apiContentRouteBuilder = apiContentRouteBuilder;
            _publishedElementFactory = publishedElementFactory;
            _publishedUrlProvider = publishedUrlProvider;
            _variationContextAccessor = variationContextAccessor;
            _umbracoContextFactory = umbracoContextFactory;
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

            using (var context = _umbracoContextFactory.EnsureUmbracoContext())
            {
                var items = BuildItems(context.UmbracoContext, entities);

                if (_config.MaxItems.HasValue == true)
                {
                    return items.Take(_config.MaxItems.Value);
                }

                return items;
            }
        }

        private IEnumerable<MeganavItem> BuildItems(IUmbracoContext umbracoContext, IEnumerable<MeganavEntity> entities, int level = 0)
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
                    Url = $"{entity.Url}{entity.QueryString}",
                    QueryString = entity.QueryString,
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
                            var contentType = umbracoContext.Content.GetContentType(itemType.SettingsType.Value);

                            if (contentType?.IsElement == true)
                            {
                                item.Settings = _publishedElementFactory.CreateElement(contentType, entity.Settings);
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
                    var culture = umbracoContext.PublishedRequest?.Culture;

                    var content = umbracoContext.Content.GetById(entity.Udi);

                    if (content == null)
                    {
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(entity.Title) == true)
                    {
                        item.Title = content.Name(_variationContextAccessor, culture);
                    }

                    item.Url = content.Url(_publishedUrlProvider, culture);

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

		public PropertyCacheLevel GetDeliveryApiPropertyCacheLevel(IPublishedPropertyType propertyType) => PropertyCacheLevel.Elements;

		public Type GetDeliveryApiPropertyValueType(IPublishedPropertyType propertyType) => typeof(IEnumerable<MeganavApiItem>);

		public object ConvertIntermediateToDeliveryApiObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object inter, bool preview, bool expanding)
		{

			IEnumerable<MeganavApiItem> DefaultValue() => Array.Empty<MeganavApiItem>();

			if (inter is not string value || value.IsNullOrWhiteSpace())
			{
				return DefaultValue();
			}

			var entities = JsonConvert.DeserializeObject<IEnumerable<MeganavEntity>>(value);
			if (entities == null || entities.Any() == false)
			{
				return DefaultValue();
			}

			IPublishedSnapshot publishedSnapshot = _publishedSnapshotAccessor.GetRequiredPublishedSnapshot();

			MeganavApiItem? BuildApiItems(IMeganavEntity item, int level)
			{
				switch (item.Udi?.EntityType)
				{
					case UdiEntityType.Document:
						IPublishedContent? content = publishedSnapshot.Content?.GetById(item.Udi.Guid);
                        IApiContentRoute? route = content != null ? _apiContentRouteBuilder.Build(content) : null;
					    return content == null || route == null
                            ? null
                            : MeganavApiItem.Content(
                                item.Title.IfNullOrWhiteSpace(_apiContentNameProvider.GetName(content)),
                                item.QueryString,
                                item.Target,
                                content.Key,
                                content.ContentType.Alias,
                                route, 
                                level,
                                item.Children.Select(entity => BuildApiItems(entity, level + 1)).WhereNotNull().ToArray());
					default:
						return MeganavApiItem.External(item.Title, $"{item.Url}{item.QueryString}", item.QueryString, item.Target, level, item.Children.Select(entity => BuildApiItems(entity, level + 1)).WhereNotNull().ToArray());
				}

			}

			return entities.Select(entity => BuildApiItems(entity, 0)).WhereNotNull().ToArray();

		}
	}
}