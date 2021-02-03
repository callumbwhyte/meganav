using Our.Umbraco.Meganav.PublishedContent;
using Our.Umbraco.Meganav.ValueReferences;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace Our.Umbraco.Meganav.Startup
{
    [RuntimeLevel(MaxLevel = RuntimeLevel.Run)]
    public class MeganavComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Register<PublishedElementFactory>();

            composition.DataValueReferenceFactories().Append<MeganavValueReferenceFactory>();
        }
    }
}