using System.Collections.Generic;
using Umbraco.Core.Models.PublishedContent;

namespace Our.Umbraco.Meganav.Models
{
    public interface IMeganavItem
    {
        string Title { get; }

        string Url { get; }

        string Target { get; }

        int Level { get; }

        IMeganavItemType ItemType { get; }

        IPublishedContent Content { get; }

        IPublishedElement Settings { get; }

        IEnumerable<IMeganavItem> Children { get; }
    }
}