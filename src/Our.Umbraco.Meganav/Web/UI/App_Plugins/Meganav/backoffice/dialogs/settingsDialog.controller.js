(function () {

    "use strict";

    angular.module("umbraco")
        .controller("Our.Umbraco.Meganav.Dialogs.SettingsDialogController",
            function ($scope, overlayService) {

                var vm = this;

                vm.loading = true;

                // model passed to subviews
                vm.model = $scope.model;

                vm.itemTypes = $scope.model.context.itemTypes;

                vm.actionButtons = [];

                vm.saveButton = {
                    type: "button",
                    labelKey: "buttons_save",
                    handler: function () {
                        vm.save();
                    }
                };

                vm.changeTypeButton = {
                    type: "button",
                    labelKey: "actions_changeType",
                    handler: function () {
                        openChooseType(function (model) {
                            $scope.model.value.itemType = model.selectedItem;
                        });
                    }
                };

                vm.close = function () {
                    if ($scope.model && $scope.model.close) {
                        $scope.model.close();
                    }
                };

                vm.save = function () {
                    if ($scope.model && $scope.model.submit) {
                        $scope.$broadcast("formSubmitting", { scope: $scope });
                        // ensure item type
                        if (vm.itemType) {
                            $scope.model.value.itemType = vm.itemType;
                            $scope.model.value.itemTypeId = vm.itemType.id;
                        }
                        // submit dialog
                        $scope.model.submit($scope.model);
                    }
                };

                vm.$onInit = function () {
                    if (vm.itemTypes.length > 1) {
                        vm.actionButtons.push(vm.changeTypeButton);
                    }

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

                function openChooseType(callback) {
                    overlayService.open({
                        title: "Choose type",
                        availableItems: getAllowedTypes(),
                        filter: false,
                        view: "itempicker",
                        submit: function (model) {
                            if (callback) {
                                callback(model);
                            }
                            overlayService.close();
                        },
                        close: function () {
                            overlayService.close();
                        }
                    });
                }

                function getAllowedTypes() {
                    return vm.itemTypes.filter(x => x.id !== vm.itemType.id)
                }

            });

})();