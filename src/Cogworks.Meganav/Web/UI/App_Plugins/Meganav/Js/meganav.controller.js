function Meganav($scope, dialogService, meganavResource, angularHelper) {

    $scope.items = [];
    var currentForm = angularHelper.getCurrentForm($scope);

    if ($scope.model.value) {
        // retreive the saved items
        $scope.items = $scope.model.value;

        // get updated entities for content
        getItemEntities($scope.items);
    }


    $scope.editMenuItemSettings = function (menuItem) {

        var config = angular.copy($scope.model.config.settings);

        $scope.meganavSettingsOverlay = {};
        $scope.meganavSettingsOverlay.view = "/App_Plugins/Meganav/Views/configdialog.html";
        $scope.meganavSettingsOverlay.title = "Settings";
        $scope.meganavSettingsOverlay.show = true;

        $scope.meganavSettingsOverlay.settings = config;

        if (angular.isObject(menuItem.config)) {
            _.each(config, function (cfg) {
                var val = menuItem.config[cfg.key];
                if (val) {
                    cfg.value = val;
                }
            });
        }

        $scope.meganavSettingsOverlay.submit = function (model) {

            var configObject = {};

            _.each(model.settings,
                function (cfg) {
                    if (cfg.value) {
                        configObject[cfg.key] = cfg.value;
                    }
                });

            menuItem.config = configObject;

            console.log(menuItem);
            currentForm.$setDirty();

            $scope.meganavSettingsOverlay.show = false;
            $scope.meganavSettingsOverlay = null;
        };

        $scope.meganavSettingsOverlay.close = function (oldModel) {
            $scope.meganavSettingsOverlay.show = false;
            $scope.meganavSettingsOverlay = null;
        };

    }

    $scope.config = function (item) {
        $scope.editMenuItemSettings(item);
    }

    $scope.edit = function (item) {
        dialogService.linkPicker({
            currentTarget: item,
            callback: function (item) {
                item = buildNavItem(item);
            }
        });
    };

    $scope.remove = function (item) {
        item.remove();
    };

    $scope.$on("formSubmitting", function (ev, args) {
        console.log($scope.items);
        $scope.model.value = $scope.items;
    });

    $scope.add = function (item) {
        $scope.items.push(buildNavItem(item));
    };

    $scope.openLinkPicker = function () {
        dialogService.linkPicker({
            callback: function (data) {
                $scope.add(data);
            }
        });
    };



    function getItemEntities(items) {
        _.each(items, function (item) {
            if (item.id) {
                meganavResource.getById(item.id).then(function (response) {
                    angular.extend(item, response.data);
                });

                if (item.children.length > 0) {
                    getItemEntities(item.children);
                }
            }
        });
    }

    function buildNavItem(data) {
        return {
            id: data.id,
            title: data.name || data.title,
            target: data.target,
            url: data.url || "#",
            children: [],
            icon: data.icon || "icon-link",
            published: data.published,
            config: []
        }
    }
}

angular.module("umbraco").controller("Cogworks.Meganav.MeganavController", Meganav);

app.requires.push("ui.tree");
