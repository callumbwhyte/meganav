using System;
using System.Collections.Generic;
using Umbraco.Core;

namespace Our.Umbraco.Meganav.Models
{
    internal interface IMeganavEntity
    {
        string Title { get; }

        string Url { get; }

        string Target { get; }

        bool Visible { get; }

        GuidUdi Udi { get; }

        Guid? ItemTypeId { get; }

        IDictionary<string, object> Settings { get; }

        IEnumerable<IMeganavEntity> Children { get; }
    }
}