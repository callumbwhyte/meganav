using System.Collections.Generic;
using System.Runtime.Serialization;
using Umbraco.Core;

namespace Our.Umbraco.Meganav.Models
{
    [DataContract]
    internal class MeganavEntity : IMeganavEntity
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "target")]
        public string Target { get; set; }

        [DataMember(Name = "visible")]
        public bool Visible { get; set; } = true;

        [DataMember(Name = "udi")]
        public GuidUdi Udi { get; set; }

        [DataMember(Name = "settings")]
        public IDictionary<string, object> Settings { get; set; }

        [DataMember(Name = "children")]
        public IEnumerable<MeganavEntity> Children { get; set; } = new List<MeganavEntity>();

        IEnumerable<IMeganavEntity> IMeganavEntity.Children => Children;
    }
}