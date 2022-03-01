using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;

namespace Our.Umbraco.Meganav.Migrations
{
    public class LegacyMigrationComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Services.AddTransient<LegacyValueMigrator>();

            builder.AddNotificationHandler<DataTypeSavingNotification, LegacyDataTypeChangingHandler>();
        }
    }
}