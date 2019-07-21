(function () {

    "use strict";

    angular.module("umbraco")
        .controller("Our.Umbraco.Meganav.Dialogs.SettingsDialogLinkController",
            function ($scope, $controller) {

                var vm = this;

                // set currentTarget to value
                $scope.model.currentTarget = $scope.model.value;

                // extend Umbraco Link Picker controller
                angular.extend(vm, $controller("Umbraco.Editors.LinkPickerController", { $scope: $scope }));

                // set value to selected target
                $scope.$on("formSubmitting", () => {
                    angular.extend($scope.model.value, $scope.model.target);
                });

                // set icon when selecting tree node
                $scope.$watch("dialogTreeApi", () => {
                    $scope.dialogTreeApi.callbacks.treeNodeSelect(function (args) {
                        $scope.model.target.icon = args.node.icon;
                    });
                });

            });

})();