using Our.Umbraco.Meganav.PublishedContent;
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
        }
    }
}