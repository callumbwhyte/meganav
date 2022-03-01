using Microsoft.Extensions.DependencyInjection;
using Our.Umbraco.Meganav.PublishedContent;
using Our.Umbraco.Meganav.ValueReferences;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace Our.Umbraco.Meganav.Composing
{
    public class MeganavComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Services.AddTransient<PublishedElementFactory>();

            builder.DataValueReferenceFactories().Append<MeganavValueReferenceFactory>();
        }
    }
}