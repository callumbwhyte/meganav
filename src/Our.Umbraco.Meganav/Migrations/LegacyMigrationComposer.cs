using Umbraco.Core;
using Umbraco.Core.Composing;

namespace Our.Umbraco.Meganav.Migrations
{
    public class LegacyMigrationComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Register<LegacyValueMigrator>();

            composition.Components().Append<LegacyMigrationComponent>();
        }
    }
}