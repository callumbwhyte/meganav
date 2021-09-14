using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Core.Services.Implement;

namespace Our.Umbraco.Meganav.Migrations
{
    internal class LegacyMigrationComponent : IComponent
    {
        private readonly LegacyValueMigrator _legacyValueMigrator;

        public LegacyMigrationComponent(LegacyValueMigrator legacyValueMigrator)
        {
            _legacyValueMigrator = legacyValueMigrator;
        }

        public void Initialize()
        {
            DataTypeService.Saving += DataTypeService_Saving;
        }

        public void Terminate()
        {
            DataTypeService.Saving -= DataTypeService_Saving;
        }

        private void DataTypeService_Saving(IDataTypeService sender, SaveEventArgs<IDataType> e)
        {
            var dataTypes = e.SavedEntities
                .Where(x => x.EditorAlias == Constants.PropertyEditorAlias)
                .Where(x =>
                {
                    var existingEntity = sender.GetDataType(x.Id);

                    return existingEntity != null && LegacyEditors.Aliases.Any(existingEntity.EditorAlias.InvariantContains);
                });

            foreach (var dataType in dataTypes)
            {
                _legacyValueMigrator.MigrateContentValues(dataType);
            }
        }
    }
}