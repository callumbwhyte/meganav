(function () {

    "use strict";

    angular.module("umbraco")
        .component("meganavItemType", {
            bindings: {
                type: "=",
                onEdit: "&?",
                onRemove: "&?"
            },
            templateUrl: "/App_Plugins/Meganav/backoffice/components/meganavItemType.html",
            controllerAs: "vm"
        });

})();