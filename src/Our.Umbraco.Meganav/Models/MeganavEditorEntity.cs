using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Our.Umbraco.Meganav.Models
{
    [DataContract]
    internal class MeganavEditorEntity : MeganavEntity
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "icon")]
        public string Icon { get; set; } = "icon-link";

        [DataMember(Name = "published")]
        public bool Published { get; set; } = true;

        [DataMember(Name = "children")]
        public new IEnumerable<MeganavEditorEntity> Children { get; set; } = new List<MeganavEditorEntity>();
    }
}