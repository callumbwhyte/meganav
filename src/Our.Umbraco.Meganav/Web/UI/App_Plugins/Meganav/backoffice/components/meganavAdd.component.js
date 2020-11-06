(function () {

    "use strict";

    angular.module("umbraco")
        .component("meganavAdd", {
            bindings: {
                onAdd: "&?"
            },
            templateUrl: "/App_Plugins/Meganav/backoffice/components/meganavAdd.html",
            controllerAs: "vm",
            controller: function () {
                var vm = this;

                vm.position = 0;

                vm.onMouseMove = function ($event) {
                    vm.position = $event.offsetX;
                };
            }
        });

})();