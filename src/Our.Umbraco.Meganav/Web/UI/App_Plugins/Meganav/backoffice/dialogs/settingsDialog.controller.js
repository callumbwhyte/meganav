(function () {

    "use strict";

    angular.module("umbraco")
        .controller("Our.Umbraco.Meganav.Dialogs.SettingsDialogController",
            function ($scope) {

                var vm = this;

                vm.loading = true;

                // model passed to subviews
                vm.model = $scope.model;

                vm.itemTypes = $scope.model.context.itemTypes;

                vm.close = function () {
                    if ($scope.model && $scope.model.close) {
                        $scope.model.close();
                    }
                };

                vm.save = function () {
                    if ($scope.model && $scope.model.submit) {
                        $scope.$broadcast("formSubmitting", { scope: $scope });
                        $scope.model.submit($scope.model);
                    }
                };

                vm.$onInit = function () {
                    $scope.$watch("model.value.itemType", () => {
                        vm.itemType = $scope.model.value.itemType;
                        loadTabs();
                    });

                    vm.loading = false;
                };

                function loadTabs() {
                    vm.tabs = [
                        {
                            name: "Link",
                            alias: "link",
                            icon: "icon-link",
                            view: "/App_Plugins/Meganav/backoffice/dialogs/settingsDialog.link.html",
                            active: true
                        }
                    ];

                    if (vm.itemType && vm.itemType.settingsType) {
                        vm.tabs.push({
                            name: "Settings",
                            alias: "settings",
                            icon: "icon-settings",
                            view: "/App_Plugins/Meganav/backoffice/dialogs/settingsDialog.settings.html",
                            active: false
                        });
                    }
                }

            });

})();