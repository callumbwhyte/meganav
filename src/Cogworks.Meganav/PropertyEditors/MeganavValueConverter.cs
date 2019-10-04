using Cogworks.Meganav.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.PublishedCache;

namespace Cogworks.Meganav.PropertyEditors
{
    public class MeganavValueConverter : PropertyValueConverterBase
    {
        private readonly IPublishedSnapshotAccessor publishedSnapshotAccessor;
        private readonly IDataTypeService dataTypeService;

        public MeganavValueConverter(IPublishedSnapshotAccessor publishedSnapshotAccessor, ILogger logger, IDataTypeService dataTypeService)
        {
            this.dataTypeService = dataTypeService;
            this.publishedSnapshotAccessor = publishedSnapshotAccessor;
            this.logger = logger;
        }

        private bool RemoveNaviHideItems;
        private readonly ILogger logger;

        public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType)
        {
            return PropertyCacheLevel.Element;
        }

        public override Type GetPropertyValueType(IPublishedPropertyType propertyType)
        {
            return typeof(IEnumerable<MeganavItem>);
        }

        public override bool IsConverter(IPublishedPropertyType propertyType)
        {
            return propertyType.EditorAlias.Equals(Constants.PropertyEditorAlias);
        }

        public override bool? IsValue(object value, PropertyValueLevel level)
        {
            return value?.ToString() != "[]";
        }

        public override object ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object source, bool preview)
        {
            return source?.ToString();
        }

        public override object ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object inter, bool preview)
        {
            if (inter == null)
            {
                return Enumerable.Empty<MeganavItem>();
            }
            
            IDataType dataType = dataTypeService.GetDataType(propertyType.DataType.Id);
            IDictionary<string, object> preValues = dataType.Configuration as IDictionary<string, object>;
            if (preValues.ContainsKey("removeNaviHideItems"))
            {
                RemoveNaviHideItems = preValues["removeNaviHideItems"].ToString() == "1";
            }

            try
            {
                IEnumerable<MeganavItemDto> dtos = JsonConvert.DeserializeObject<IEnumerable<MeganavItemDto>>(inter.ToString());                
                return BuildMenu(dtos, preview);
            }
            catch (Exception ex)
            {
                logger.Error<MeganavValueConverter>(ex, "Failed to convert Meganav");
            }

            return null;
        }

        internal IEnumerable<MeganavItem> BuildMenu(IEnumerable<MeganavItemDto> dtos, bool preview, int level = 0)
        {
            List<MeganavItem> meganav = new List<MeganavItem>();

            foreach (MeganavItemDto dto in dtos)
            {
                LinkType type = LinkType.External;
                string url = dto.Url;
                string contentTypeAlias = string.Empty;

                if (dto.Udi != null)
                {
                    type = dto.Udi.EntityType == Umbraco.Core.Constants.UdiEntityType.Media
                        ? LinkType.Media
                        : LinkType.Content;

                    IPublishedContent content = type == LinkType.Media ?
                         publishedSnapshotAccessor.PublishedSnapshot.Media.GetById(preview, dto.Udi.Guid) :
                         publishedSnapshotAccessor.PublishedSnapshot.Content.GetById(preview, dto.Udi.Guid);

                    if (content == null || content.ContentType.ItemType == PublishedItemType.Element)
                    {
                        continue;
                    }

                    if (RemoveNaviHideItems && !content.IsVisible())
                    {
                        continue;
                    }

                    url = content.Url();
                    contentTypeAlias = content.ContentType.Alias;
                }

                meganav.Add(
                    new MeganavItem
                    {
                        Children = BuildMenu(dto.Children, preview, level + 1),
                        ContentTypeAlias = contentTypeAlias,
                        Level = level,
                        Name = dto.Name,
                        Target = dto.Target,
                        Type = type,
                        Udi = dto.Udi,
                        Url = url + dto.QueryString
                    }
                );
            }

            return meganav;
        }
    }
}