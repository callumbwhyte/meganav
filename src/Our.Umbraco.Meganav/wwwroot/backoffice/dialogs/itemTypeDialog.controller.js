(function () {

    "use strict";

    angular.module("umbraco")
        .controller("Our.Umbraco.Meganav.Dialogs.ItemTypeDialogController",
            function ($scope, editorService, dialogHelper, localizationHelper, contentTypeResource) {

                var vm = this;

                vm.loading = true;

                vm.itemType = $scope.model.value;

                vm.itemTypes = $scope.model.context.itemTypes;

                vm.allowAllTypes = !vm.itemType.allowedTypes || vm.itemType.allowedTypes.length <= 0;

                vm.labels = {
                    settings: "general_settings",
                    permissions: "general_permissions",
                    addIcon: "dialogs_addIcon",
                    addSettingsType: "dialogs_addSettingsType",
                    addView: "dialogs_addView",
                    allowAllTypes: "general_allowAllTypes",
                    allowChosenTypes: "general_allowChosenTypes",
                    allowedAtRoot: "general_allowAtRoot",
                    notAllowedAtRoot: "general_allowAtRoot"
                };

                vm.addIcon = function () {
                    editorService.iconPicker({
                        title: vm.labels.addIcon,
                        icon: vm.itemType.icon,
                        submit: function (model) {
                            vm.itemType.icon = model.icon;
                            editorService.close();
                        },
                        close: function () {
                            editorService.close();
                        }
                    });
                };

                vm.removeIcon = function () {
                    vm.itemType.icon = null;
                };

                vm.addSettingsType = function () {
                    dialogHelper.elementTypePicker({
                        title: vm.labels.addSettingsType,
                        currentNode: vm.settingsElementType,
                        select: function (node) {
                            loadSettingsType(node.id)
                                .then(function () {
                                    editorService.close();
                                });
                        },
                        close: function () {
                            editorService.close();
                        }
                    });
                };

                vm.removeSettingsType = function () {
                    vm.settingsElementType = null;
                    vm.itemType.settingsType = null;
                };

                vm.addView = function () {
                    dialogHelper.filePicker({
                        title: vm.labels.addView,
                        extension: ".html",
                        select: function (node) {
                            vm.itemType.view = "/" + decodeURIComponent(node.id);
                            editorService.close();
                        },
                        close: function () {
                            editorService.close();
                        }
                    });
                };

                vm.removeView = function () {
                    vm.itemType.view = null;
                };

                vm.toggleAllowedType = function (itemType) {
                    var position = vm.itemType.allowedTypes.findIndex(x => x === itemType.id);
                    if (position !== -1) {
                        vm.itemType.allowedTypes.splice(position, 1);
                    } else {
                        vm.itemType.allowedTypes.push(itemType.id);
                    }
                };

                vm.toggleAllowAllTypes = function () {
                    vm.allowAllTypes = !vm.allowAllTypes;
                    vm.itemType.allowedTypes = [];
                };

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
                    localizationHelper.translate(vm.labels);

                    if (vm.itemType.settingsType) {
                        loadSettingsType(vm.itemType.settingsType);
                    }

                    vm.loading = false;
                };

                function loadSettingsType(id) {
                    return contentTypeResource.getById(id)
                        .then(contentType => {
                            vm.settingsElementType = contentType;
                            vm.itemType.settingsType = contentType.key;
                        });
                }

            });

})();