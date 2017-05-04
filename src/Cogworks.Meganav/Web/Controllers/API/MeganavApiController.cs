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
                    icon = entity.ContentType.Icon,
                    url = Umbraco.Url(entity.Id),
                    published = entity.Published,
                    navihide = entity.GetValue<bool>("umbracoNaviHide")
                });
            }

            return null;
        }
    }
}