using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;

namespace Cogworks.Meganav.Helpers
{
    public static class PreValueHelper
    {
        public static IDictionary<string, string> GetPreValues(int dataTypeId)
        {
            var context = ApplicationContext.Current;
            var dataTypeService = context.Services.DataTypeService;

            var preValueCollection = dataTypeService.GetPreValuesCollectionByDataTypeId(dataTypeId);

            return preValueCollection.PreValuesAsDictionary.ToDictionary(x => x.Key, x => x.Value.Value);
        }
    }
}