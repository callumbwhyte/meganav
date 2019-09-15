(function () {

    "use strict";

    angular.module("umbraco")
        .controller("Our.Umbraco.Meganav.Dialogs.ItemTypeDialogController",
            function ($scope, editorService, dialogHelper, contentTypeResource) {

                var vm = this;

                vm.loading = true;

                vm.itemType = $scope.model.value;

                vm.addIcon = function () {
                    editorService.iconPicker({
                        title: "Choose icon",
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
                        title: "Choose settings type",
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
                        title: "Choose view",
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