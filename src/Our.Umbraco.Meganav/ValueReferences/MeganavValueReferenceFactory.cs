using System.Collections.Generic;
using Newtonsoft.Json;
using Our.Umbraco.Meganav.Models;
using Umbraco.Cms.Core.Models.Editors;
using Umbraco.Cms.Core.PropertyEditors;

namespace Our.Umbraco.Meganav.ValueReferences
{
    internal class MeganavValueReferenceFactory : IDataValueReferenceFactory, IDataValueReference
    {
        public bool IsForEditor(IDataEditor dataEditor) => dataEditor.Alias.Equals(Constants.PropertyEditorAlias);

        public IDataValueReference GetDataValueReference() => this;

        public IEnumerable<UmbracoEntityReference> GetReferences(object value)
        {
            var entities = JsonConvert.DeserializeObject<IEnumerable<MeganavEntity>>(value?.ToString());

            return GetReferences(entities);
        }

        private IEnumerable<UmbracoEntityReference> GetReferences(IEnumerable<MeganavEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (entity.Children != null)
                {
                    var references = GetReferences(entity.Children);

                    foreach (var reference in references)
                    {
                        yield return reference;
                    }
                }

                if (entity.Udi != null)
                {
                    yield return new UmbracoEntityReference(entity.Udi);
                }
            }
        }
    }
}