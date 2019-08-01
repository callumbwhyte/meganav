(function () {

    "use strict";

    angular.module("umbraco")
        .component("meganavActions", {
            bindings: {
                item: "=",
                onEdit: "&?",
                onRemove: "&?"
            },
            templateUrl: "/App_Plugins/Meganav/backoffice/components/meganavActions.html",
            controllerAs: "vm"
        });

})();