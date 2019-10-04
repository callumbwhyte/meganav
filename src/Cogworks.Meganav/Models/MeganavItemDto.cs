using System.Collections.Generic;
using System.Runtime.Serialization;
using Umbraco.Core;

namespace Cogworks.Meganav.Models
{
    [DataContract]
    internal class MeganavItemDto
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "target")]
        public string Target { get; set; }

        [DataMember(Name = "udi")]
        public GuidUdi Udi { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "queryString")]
        public string QueryString { get; set; }

        [DataMember(Name = "children")]
        public IEnumerable<MeganavItemDto> Children { get; set; }
    }
}
