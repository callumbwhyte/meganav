using System.Collections.Generic;
using Cogworks.Meganav.Enums;
using Newtonsoft.Json;
using Umbraco.Core.Models;

namespace Cogworks.Meganav.Models
{
    public class MeganavItem
    {
        public int Id { get; set; }
        
        public string Title { get; set; }

        public string Target { get; set; }

        public string Url { get; set; }

        [JsonIgnore]
        public IPublishedContent Content { get; set; }

        #region Internal

        public IEnumerable<MeganavItem> Children { get; set; }

        [JsonIgnore]
        public ItemType ItemType { get; set; }

        [JsonIgnore]
        public int Level { get; set; }

        #endregion
    }
}