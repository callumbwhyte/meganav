using System;
using System.Collections.Generic;
using System.Linq;
using Cogworks.Meganav.Enums;
using Cogworks.Meganav.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Umbraco.Core.Logging;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web;

namespace Cogworks.Meganav.ValueConverters
{
    [PropertyValueCache(PropertyCacheValue.All, PropertyCacheLevel.Content)]
    [PropertyValueType(typeof(IEnumerable<MeganavItem>))]
    public class MeganavValueConverter : PropertyValueConverterBase
    {
        public override bool IsConverter(PublishedPropertyType propertyType)
        {
            return propertyType.PropertyEditorAlias.Equals(Constants.PropertyEditorAlias);
        }

        public override object ConvertDataToSource(PublishedPropertyType propertyType, object source, bool preview)
        {
            try
            {
                var items = JsonConvert.DeserializeObject<IEnumerable<MeganavItem>>(source.ToString());

                return BuildMenu(items);
            }
            catch (Exception ex)
            {
                LogHelper.Error<MeganavValueConverter>("Failed to convert Meganav", ex);
            }

            return null;
        }

        internal IEnumerable<MeganavItem> BuildMenu(IEnumerable<MeganavItem> items, int level = 0)
        {
            items = items.ToList();

            foreach (var item in items)
            {
                if (item.Config == null)
                {
                    item.Config = new Dictionary<string, object>();
                }
               
                item.Level = level;
                // it's likely a content item
                if (item.Id > 0)
                {
                    var umbracoContent = UmbracoContext.Current.ContentCache.GetById(item.Id);

                    if (umbracoContent != null)
                    {
                        // test if umbracoNaviHide is available
                        var umbracoNaviHide = false;
                        if (umbracoContent.HasProperty("umbracoNaviHide"))
                        {
                            umbracoNaviHide = umbracoContent.GetPropertyValue<bool>("umbracoNaviHide");
                        }

                        if (!umbracoNaviHide)
                        {
                            item.ItemType = ItemType.Content;
                            item.Content = umbracoContent;
                        }
                        else
                        {
                            // umbraconavihide, remove item from nav
                            items = items.Where(x => x.Id != item.Id);
                            continue;
                        }
                    }
                    else
                    {
                        // item is not in umbraco cache, so probably  not published, remove item from nav
                        items = items.Where(x => x.Id != item.Id);
                        continue;
                    }
                }

                // process child items
                if (item.Children.Any())
                {
                    var childLevel = item.Level + 1;

                    BuildMenu(item.Children, childLevel);
                }
            }

            return items;
        }
    }
}