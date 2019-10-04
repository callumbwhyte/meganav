using Newtonsoft.Json;
using System.Collections.Generic;
using Umbraco.Web.Models;

namespace Cogworks.Meganav.Models
{
    public class MeganavItem : Link
    {
        #region Internal

        public IEnumerable<MeganavItem> Children { get; set; }

        [JsonIgnore]
        public int Level { get; set; }

        [JsonIgnore]
        public string ContentTypeAlias { get; set; }

        #endregion
    }
}