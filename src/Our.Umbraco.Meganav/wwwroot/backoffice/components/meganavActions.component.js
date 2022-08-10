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
            controllerAs: "vm",
            controller: function () {
                var vm = this;

                vm.toggleVisibility = function () {
                    var toggleChildren = function (item) {
                        item.children.forEach(child => {
                            child.visible = item.visible;
                            toggleChildren(child);
                        });
                    };
                    // set visibility
                    vm.item.visible = !vm.item.visible;
                    // apply to children
                    toggleChildren(vm.item);
                };
            }
        });

})();