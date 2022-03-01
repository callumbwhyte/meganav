using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Our.Umbraco.Meganav.PublishedContent
{
    internal class PublishedElement : IPublishedElement
    {
        public PublishedElement(Guid key, IPublishedContentType contentType, IEnumerable<IPublishedProperty> properties)
        {
            Key = key;
            ContentType = contentType;
            Properties = properties;
        }

        public Guid Key { get; }

        public IPublishedContentType ContentType { get; }

        public IEnumerable<IPublishedProperty> Properties { get; }

        public IPublishedProperty GetProperty(string alias) => Properties.FirstOrDefault(x => x.PropertyType.Alias == alias);
    }
}