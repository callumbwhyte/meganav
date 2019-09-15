(function () {

    "use strict";

    angular.module("umbraco")
        .controller("Our.Umbraco.Meganav.PropertyEditors.ConfigurationController",
            function ($scope, editorService) {

                var vm = this;

                vm.types = $scope.model.value || [];

                vm.sortableOptions = {
                    cursor: "grabbing",
                    placeholder: "meganav-placeholder"
                };

                vm.addType = function () {
                    openSettings({ id: String.CreateGuid() }, function (model) {
                        vm.types.push(model.value);
                    });
                };

                vm.editType = function (item) {
                    openSettings(item, function (model) {
                        angular.extend(item, model.value);
                    });
                };

                vm.removeType = function (item) {
                    var position = vm.types.indexOf(item);
                    vm.types.splice(position, 1);
                };

                vm.$onInit = function () {
                    $scope.$on("formSubmitting", () => {
                        $scope.model.value = vm.types;
                    });
                };

                function openSettings(item, callback) {
                    editorService.open({
                        value: angular.copy(item),
                        context: {
                            itemTypes: vm.types
                        },
                        view: "/App_Plugins/Meganav/backoffice/dialogs/itemTypeDialog.html",
                        size: "small",
                        submit: function (model) {
                            if (callback) {
                                callback(model);
                            }
                            editorService.close();
                        },
                        close: function () {
                            editorService.close();
                        }
                    });
                }

            });

})();