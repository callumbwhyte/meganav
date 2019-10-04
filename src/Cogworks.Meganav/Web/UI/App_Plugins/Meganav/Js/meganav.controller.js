function Meganav($scope, angularHelper, entityResource, iconHelper, localizationService, editorService) {

    var vm = {
        labels: {
            general_recycleBin: ""
        }
    };

    $scope.renderModel = [];

    if (!Array.isArray($scope.model.value)) {
        $scope.model.value = [];
    }

    var currentForm = angularHelper.getCurrentForm($scope);

    $scope.renderModel = $scope.model.value

    getItemEntities($scope.renderModel);

    function getItemEntities(items) {
        _.each(items, function (item) {
            if (item.udi) {
                var entityType = item.udi.indexOf("media") > -1 ? "Media" : "Document";
                entityResource.getById(item.udi, entityType).then(function (data) {
                    item.icon = iconHelper.convertFromLegacyIcon(data.icon);
                    item.published = (data.metaData && data.metaData.IsPublished === false && entityType === "Document") ? false : true;
                    item.trashed = data.trashed;
                    if (link.trashed) {
                        item.url = vm.labels.general_recycleBin;
                    }
                });

                if (item.children.length > 0) {
                    getItemEntities(item.children);
                }
            }
        });
    }

    $scope.onRemove = function (item) {
        remove($scope.renderModel, item);

        currentForm.$setDirty();
    };

    function remove(renderModel, item) {
        var model = renderModel;
        if (model) {
            for (var i = 0; i < model.length; i++) {

                if (model[i] === item) {
                    model.splice(i, 1);
                    return true;
                } else {
                    remove(model[i].children, item);
                }
            }
        }
    }

    $scope.isVisible = function (item) {
        return $scope.model.config.removeNaviHideItems == true ? item.naviHide !== true : true;
    };

    $scope.$on("formSubmitting", function (ev, args) {
        $scope.model.value = $scope.renderModel;
    });

    $scope.openLinkPicker = function (link, $index) {
        var target = link ? {
            name: link.name,
            anchor: link.queryString,
            udi: link.udi,
            url: link.url,
            target: link.target
        } : null;

        var linkPicker = {
            currentTarget: target,
            dataTypeKey: "",
            ignoreUserStartNodes: true,
            submit: function (model) {
                if (model.target.url || model.target.anchor) {
                    // if an anchor exists, check that it is appropriately prefixed
                    if (model.target.anchor && model.target.anchor[0] !== '?' && model.target.anchor[0] !== '#') {
                        model.target.anchor = (model.target.anchor.indexOf('=') === -1 ? '#' : '?') + model.target.anchor;
                    }
                    if (link) {
                        link.udi = model.target.udi;
                        link.name = model.target.name || model.target.url || model.target.anchor;
                        link.queryString = model.target.anchor;
                        link.target = model.target.target;
                        link.url = model.target.url;
                    } else {
                        link = {
                            name: model.target.name || model.target.url || model.target.anchor,
                            queryString: model.target.anchor,
                            target: model.target.target,
                            udi: model.target.udi,
                            url: model.target.url
                        };
                        $scope.renderModel.push(link);
                    }

                    link.description = link.url + (link.queryString ? link.queryString : '');
                    link.children = [];

                    if (link.udi) {
                        var entityType = model.target.isMedia ? "Media" : "Document";

                        entityResource.getById(link.udi, entityType).then(function (data) {
                            link.icon = iconHelper.convertFromLegacyIcon(data.icon);
                            link.published = (data.metaData && data.metaData.IsPublished === false && entityType === "Document") ? false : true;
                            link.trashed = data.trashed;
                            if (link.trashed) {
                                link.url = vm.labels.general_recycleBin;
                            }
                        });
                    } else {
                        link.icon = "icon-link";
                        link.published = true;
                    }

                    currentForm.$setDirty();
                }
                editorService.close();
            },
            close: function () {
                editorService.close();
            }
        };
        editorService.linkPicker(linkPicker);
    };

    function init() {
        localizationService.localizeMany(["general_recycleBin"])
            .then(function (data) {
                vm.labels.general_recycleBin = data[0];
            });
    }

    init();
}

app.requires.push("ui.tree");
angular.module("umbraco").controller("Cogworks.Meganav.MeganavController", Meganav);