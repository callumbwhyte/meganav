using System.Net;
using System.Net.Http;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace Cogworks.Meganav.Web.Controllers.API
{
    [PluginController(Constants.PackageName)]
    public class MeganavEntityApiController : UmbracoAuthorizedJsonController
    {
        public HttpResponseMessage GetById(int id)
        {
            var entity = Services.ContentService.GetById(id);

            if (entity != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    id = entity.Id,
                    name = entity.Name,
                    icon = entity.ContentType.Icon,
                    url = Umbraco.Url(entity.Id),
                    published = entity.Published,
                    naviHide = entity.HasProperty("umbracoNaviHide") && entity.GetValue<bool>("umbracoNaviHide")
                });
            }

            return null;
        }
    }
}