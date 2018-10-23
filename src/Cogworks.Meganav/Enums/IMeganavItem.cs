using System;
using System.Collections.Generic;
using Cogworks.Meganav.Converters;
using Cogworks.Meganav.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Umbraco.Core;
using Umbraco.Core.Models;
namespace Cogworks.Meganav.Models
{
    public interface IMeganavItem
    {
        [Obsolete("Use Udi instead")]
        int Id { get; set; }
        GuidUdi Udi { get; set; }
        string Title { get; set; }
        string Target { get; set; }
        string Url { get; set; }
        [JsonIgnore]
        IPublishedContent Content { get; set; }
        [JsonConverter(typeof(DictionaryConverter))]
        IDictionary<string, object> Properties { get; set; }
        #region Internal
        [JsonConverter(typeof(ChildConverter<MeganavItem>))]
        IEnumerable<IMeganavItem> Children { get; set; }
        [JsonIgnore]
        ItemType ItemType { get; set; }
        [JsonIgnore]
        int Level { get; set; }
        #endregion
    }
}