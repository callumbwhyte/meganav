angular.module("umbraco.resources").factory("meganavResource", function ($http, iconHelper) {
  return {
    getById: function (id) {
      return $http.get("backoffice/Meganav/MeganavEntityApi/GetById?id=" + id)
        .then(function (response) {
          var item = response.data;
          item.icon = iconHelper.convertFromLegacyIcon(item.icon);
          return response;
        }
    );
  }
}
});