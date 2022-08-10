(function () {

    "use strict";

    angular.module("umbraco")
        .controller("Our.Umbraco.Meganav.Dialogs.SettingsDialogSettingsController",
            function ($scope, contentResource) {

                var vm = this;

                vm.loading = true;

                vm.settings = $scope.model.value.settings;

                vm.$onInit = function () {
                    $scope.$watch("model.value.itemType", () => {
                        vm.itemType = $scope.model.value.itemType;
                        loadSettingsType();
                    });

                    $scope.$on("formSubmitting", () => {
                        $scope.model.value.settings = getContentValues(vm.content);
                    });
                };

                function loadSettingsType() {
                    if (vm.itemType && vm.itemType.settingsType) {
                        contentResource.getScaffoldByKey("-1", vm.itemType.settingsType)
                            .then(content => {
                                var variant = content.variants[0];
                                vm.content = setContentValues(variant, vm.settings);
                                vm.loading = false;
                            });
                    }
                }

                function getContentValues(content) {
                    var values = {};
                    content.tabs.forEach(tab => {
                        tab.properties.forEach(property => {
                            values[property.alias] = property.value;
                        });
                    });
                    return values;
                }

                function setContentValues(content, values) {
                    values = values || {};
                    content.tabs.forEach(tab => {
                        tab.properties.forEach(property => {
                            property.value = values[property.alias];
                        });
                    });
                    return content;
                }

            });

})();