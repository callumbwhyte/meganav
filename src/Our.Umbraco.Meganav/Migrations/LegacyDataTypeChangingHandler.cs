using System.Linq;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace Our.Umbraco.Meganav.Migrations
{
    internal class LegacyDataTypeChangingHandler : INotificationHandler<DataTypeSavingNotification>
    {
        private readonly IDataTypeService _dataTypeService;
        private readonly LegacyValueMigrator _legacyValueMigrator;

        public LegacyDataTypeChangingHandler(IDataTypeService dataTypeService, LegacyValueMigrator legacyValueMigrator)
        {
            _dataTypeService = dataTypeService;
            _legacyValueMigrator = legacyValueMigrator;
        }

        public void Handle(DataTypeSavingNotification notification)
        {
            var dataTypes = notification.SavedEntities
                .Where(x => x.EditorAlias == Constants.PropertyEditorAlias)
                .Where(x =>
                {
                    var existingEntity = _dataTypeService.GetDataType(x.Id);

                    return existingEntity?.EditorAlias != Constants.PropertyEditorAlias && LegacyEditors.Aliases.Any(existingEntity.EditorAlias.InvariantContains);
                });

            foreach (var dataType in dataTypes)
            {
                _legacyValueMigrator.MigrateContentValues(dataType);
            }
        }
    }
}