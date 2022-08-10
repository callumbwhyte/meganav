(function () {

    "use strict";

    angular.module("umbraco")
        .factory("localizationHelper",
            function (localizationService) {
                return {
                    translate: function (obj) {
                        return localizationService.localizeMany(Object.values(obj))
                            .then(function (values) {
                                Object.keys(obj).forEach((x, i) => {
                                    obj[x] = values[i];
                                });
                                return obj;
                            });
                    }
                };
            });

})();