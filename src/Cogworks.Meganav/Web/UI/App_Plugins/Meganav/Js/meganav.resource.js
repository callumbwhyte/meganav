angular.module("umbraco.resources").factory("meganavResource", function ($http) {
    return {
        getById: function (id) {
            return $http.get("backoffice/Meganav/MeganavEntityApi/GetById?id=" + id);
        }
    }
});