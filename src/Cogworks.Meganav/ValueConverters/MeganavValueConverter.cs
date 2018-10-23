using System;
using System.Collections.Generic;
using System.Linq;
using Cogworks.Meganav.Enums;
using Cogworks.Meganav.Helpers;
using Cogworks.Meganav.Models;
using Newtonsoft.Json;
using Umbraco.Core.Logging;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web;

namespace Cogworks.Meganav.ValueConverters
{
    [PropertyValueCache(PropertyCacheValue.All, PropertyCacheLevel.Content)]
    [PropertyValueType(typeof(IEnumerable<IMeganavItem>))]
    public class MeganavValueConverter : PropertyValueConverterBase
    {
        private bool RemoveNaviHideItems;

        public override bool IsConverter(PublishedPropertyType propertyType)
        {
            return propertyType.PropertyEditorAlias.Equals(Constants.PropertyEditorAlias);
        }

        public override object ConvertDataToSource(PublishedPropertyType propertyType, object source, bool preview)
        {
            var sourceString = source?.ToString();

            if (string.IsNullOrWhiteSpace(sourceString))
            {
                return null;
            }

            var preValues = PreValueHelper.GetPreValues(propertyType.DataTypeId);

            if (preValues.ContainsKey("removeNaviHideItems"))
            {
                RemoveNaviHideItems = preValues["removeNaviHideItems"] == "1";
            }

            try
            {
                var items = JsonConvert.DeserializeObject<IEnumerable<MeganavItem>>(sourceString);

                return BuildMenu(items);
            }
            catch (Exception ex)
            {
                LogHelper.Error<MeganavValueConverter>("Failed to convert Meganav", ex);
            }

            return null;
        }

        internal IEnumerable<IMeganavItem> BuildMenu(IEnumerable<IMeganavItem> items, int level = 0)
        {
            items = items.ToList();

            foreach (var item in items)
            {
                item.Level = level;

                // it's likely a content item
                if (item.Udi != null)
                {
                    var umbracoContent = UmbracoContext.Current.ContentCache.GetById(item.Udi.Guid);

                    if (umbracoContent != null)
                    {
                        // set item type
                        item.ItemType = ItemType.Content;

                        // skip item if umbracoNaviHide enabled
                        if (RemoveNaviHideItems && !umbracoContent.IsVisible())
                        {
                            continue;
                        }

                        // set content to node
                        item.Content = umbracoContent;

                        // set title to node name if no override is set
                        if (string.IsNullOrWhiteSpace(item.Title))
                        {
                            item.Title = umbracoContent.Name;
                        }
                    }
                }

                // Extra properties default value
                if (item.Properties == null)
                {
                    item.Properties = new Dictionary<string, object>();
                }

                // process child items
                if (item.Children.Any())
                {
                    var childLevel = item.Level + 1;

                    BuildMenu(item.Children, childLevel);
                }
            }

            // remove unpublished content items
            items = items.Where(x => x.Content != null || x.ItemType == ItemType.Link);

            return items;
        }
    }
}