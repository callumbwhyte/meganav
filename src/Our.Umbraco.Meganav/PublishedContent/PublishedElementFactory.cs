using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models.PublishedContent;

namespace Our.Umbraco.Meganav.PublishedContent
{
    internal class PublishedElementFactory
    {
        private readonly IPublishedModelFactory _modelFactory;

        public PublishedElementFactory(IPublishedModelFactory modelFactory)
        {
            _modelFactory = modelFactory;
        }

        public IPublishedElement CreateElement(IPublishedContentType contentType, IDictionary<string, object> values, bool isPreview = false)
        {
            var key = EnsureKey(values);

            var properties = values?.Select(x =>
            {
                var propertyType = contentType.GetPropertyType(x.Key);

                return new PublishedElementProperty(propertyType, null, x.Key, x.Value, isPreview);
            });

            IPublishedElement element = new PublishedElement(key, contentType, properties);

            element = _modelFactory.CreateModel(element);

            return element;
        }

        private Guid EnsureKey(IDictionary<string, object> values)
        {
            if (values.TryGetValue("key", out object idValue) == true)
            {
                values.Remove("key");

                if (Guid.TryParse(idValue?.ToString(), out Guid id) == true)
                {
                    return id;
                }
            }

            return Guid.NewGuid();
        }
    }
}