using System.Collections.Generic;
using System.Runtime.Serialization;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Our.Umbraco.Meganav.Models
{
    [DataContract]
    internal class MeganavItem : IMeganavItem
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "target")]
        public string Target { get; set; }

        [DataMember(Name = "level")]
        public int Level { get; set; }

        [DataMember(Name = "itemType")]
        public IMeganavItemType ItemType { get; set; }

        [DataMember(Name = "content")]
        public IPublishedContent Content { get; set; }

        [DataMember(Name = "settings")]
        public IPublishedElement Settings { get; set; }

        [DataMember(Name = "children")]
        public IEnumerable<MeganavItem> Children { get; set; } = new List<MeganavItem>();

        IEnumerable<IMeganavItem> IMeganavItem.Children => Children;
    }
}