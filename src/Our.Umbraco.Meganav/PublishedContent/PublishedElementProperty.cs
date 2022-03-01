using System;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace Our.Umbraco.Meganav.PublishedContent
{
    internal class PublishedElementProperty : IPublishedProperty
    {
        private readonly Lazy<object> _sourceValue;
        private readonly Lazy<object> _objectValue;
        private readonly Lazy<object> _xpathValue;

        public PublishedElementProperty(IPublishedPropertyType propertyType, IPublishedElement owner, string alias, object value, bool isPreview)
        {
            Alias = alias;
            PropertyType = propertyType;

            _sourceValue = new Lazy<object>(() => PropertyType.ConvertSourceToInter(owner, value, isPreview));
            _objectValue = new Lazy<object>(() => PropertyType.ConvertInterToObject(owner, PropertyCacheLevel.None, _sourceValue.Value, isPreview));
            _xpathValue = new Lazy<object>(() => PropertyType.ConvertInterToXPath(owner, PropertyCacheLevel.None, _sourceValue.Value, isPreview));
        }

        public string Alias { get; }

        public IPublishedPropertyType PropertyType { get; }

        public object GetSourceValue(string culture = null, string segment = null) => _sourceValue.Value;

        public object GetValue(string culture = null, string segment = null) => _objectValue.Value;

        public object GetXPathValue(string culture = null, string segment = null) => _xpathValue.Value;

        public bool HasValue(string culture = null, string segment = null) => _sourceValue != null;
    }
}