using System;
using System.Runtime.Serialization;

namespace Our.Umbraco.Meganav.Models
{
    [DataContract]
    internal class MeganavItemType : IMeganavItemType
    {
        [DataMember(Name = "id")]
        public Guid Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "alias")]
        public string Alias { get; set; }

        [DataMember(Name = "icon")]
        public string Icon { get; set; }

        [DataMember(Name = "settingsType")]
        public Guid? SettingsType { get; set; }
    }
}