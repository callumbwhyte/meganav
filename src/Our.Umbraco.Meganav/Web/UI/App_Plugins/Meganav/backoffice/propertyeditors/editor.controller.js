(function () {

    "use strict";

    angular.module("umbraco")
        .controller("Our.Umbraco.Meganav.PropertyEditors.EditorController",
            function ($scope, $routeParams, overlayService, editorService, localizationHelper, contentResource) {

                var vm = this;

                vm.items = $scope.model.value || [];

                vm.itemTypes = $scope.model.config.itemTypes || [];

                vm.maxItems = $scope.model.config.maxItems;

                vm.labels = {
                    chooseType: "actions_chooseType"
                };

                vm.treeOptions = {
                    accept: function (source, dest) {
                        return isAllowedType(source.item, dest.item);
                    }
                };

                vm.addItem = function (position, parentItem) {
                    var item = createItem();
                    var callback = function (model) {
                        var items = parentItem ? parentItem.children : vm.items;
                        if (position > -1) {
                            items.splice(position, 0, model.value);
                        } else {
                            items.push(model.value);
                        }
                    };
                    var allowedTypes = getAllowedTypes(parentItem);
                    if (allowedTypes.length > 1) {
                        openChooseType(allowedTypes, function (model) {
                            item.itemType = model.selectedItem;
                            item.itemTypeId = model.selectedItem.id;
                            openSettings(item, allowedTypes, callback);
                        });
                    } else {
                        item.itemType = allowedTypes[0];
                        item.itemTypeId = item.itemType?.id;
                        openSettings(item, allowedTypes, callback);
                    }
                };

                vm.editItem = function (item, parentItem) {
                    var allowedTypes = getAllowedTypes(parentItem);
                    openSettings(item, allowedTypes, function (model) {
                        angular.extend(item, model.value);
                    });
                };

                vm.$onInit = function () {
                    localizationHelper.translate(vm.labels);

                    var setItemTypes = function (items) {
                        items.forEach(item => {
                            item.itemType = getItemType(item);
                            setItemTypes(item.children);
                        })
                    };

                    setItemTypes(vm.items);

                    if ($scope.umbProperty) {
                        var propertyActions = [
                            {
                                labelKey: "actions_expandAll",
                                icon: "arrow-down",
                                method: expandAll
                            },
                            {
                                labelKey: "actions_collapseAll",
                                icon: "arrow-up",
                                method: collapseAll
                            },
                            {
                                labelKey: "actions_populateContent",
                                icon: "filter-arrows",
                                method: populateContent
                            }
                        ];

                        $scope.umbProperty.setPropertyActions(propertyActions);
                    }

                    $scope.$on("formSubmitting", () => {
                        $scope.model.value = vm.items;
                    });
                };

                function openChooseType(allowedTypes, callback) {
                    overlayService.open({
                        title: vm.labels.chooseType,
                        availableItems: allowedTypes,
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

                function openSettings(item, itemTypes, callback) {
                    editorService.open({
                        value: angular.copy(item),
                        context: {
                            itemTypes: itemTypes
                        },
                        view: "/App_Plugins/Meganav/backoffice/dialogs/settingsDialog.html",
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

                function createItem(item = {}) {
                    return {
                        name: item.name,
                        title: item.title,
                        url: item.url,
                        target: item.target,
                        icon: item.icon,
                        udi: item.udi,
                        visible: item.visible || true,
                        published: item.published || true,
                        settings: item.settings || {},
                        children: item.children || []
                    };
                }

                function getItemType(item) {
                    if (vm.itemTypes.length == 1) {
                        return vm.itemTypes[0];
                    } else {
                        return vm.itemTypes.find(x => x.id == item.itemTypeId);
                    }
                }

                function getAllowedTypes(item) {
                    if (item && item.itemType && item.itemType.allowedTypes.length) {
                        return vm.itemTypes.filter(x => item.itemType.allowedTypes.includes(x.id));
                    } else {
                        return vm.itemTypes.filter(x => x.allowAtRoot);
                    }
                }

                function isAllowedType(item, parentItem) {
                    var allowedTypes = getAllowedTypes(parentItem);
                    return allowedTypes.length == 0 || item.itemType == null || allowedTypes.some(x => x.id == item.itemType.id);
                }

                function expandAll() {
                    $scope.$broadcast("angular-ui-tree:expand-all");
                }

                function collapseAll() {
                    $scope.$broadcast("angular-ui-tree:collapse-all");
                }

                function populateContent() {
                    contentResource.getChildren($routeParams.id, { cultureName: $scope.model.culture })
                        .then(result => {
                            result.items.forEach(content => {
                                contentResource.getNiceUrl(content.id)
                                    .then(url => {
                                        var item = createItem(content);
                                        item.url = url;
                                        vm.items.push(item);
                                    });
                            });
                        });
                }

            });

    app.requires.push("ui.tree");

})();