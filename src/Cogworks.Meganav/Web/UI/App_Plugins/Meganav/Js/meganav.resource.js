angular.module("umbraco.resources").factory("meganavResource", function ($http, iconHelper) {
    return {
        getByUdi: function (udi) {
            return $http.get("backoffice/Meganav/MeganavEntityApi/GetByUdi?udi=" + encodeURIComponent(udi))
                .then(function (response) {
                    var item = response.data;
                    item.icon = iconHelper.convertFromLegacyIcon(item.icon);
                    return response;
                }
            );
        }
    }
});