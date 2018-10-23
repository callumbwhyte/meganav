function Meganav($scope, dialogService, meganavResource, angularHelper) {

    $scope.items = [];
    var currentForm = angularHelper.getCurrentForm($scope);
    
    if (!_.isEmpty($scope.model.value)) {
        // retreive the saved items
        $scope.items = $scope.model.value;

        // get updated entities for content
        getItemEntities($scope.items);
    }


    $scope.editMenuItemSettings = function (menuItem) {

        var config = angular.copy($scope.model.config.settings);

        $scope.meganavSettingsOverlay = {};
        $scope.meganavSettingsOverlay.view = "/App_Plugins/Meganav/Views/configdialog.html";
    $scope.add = function () {
        openSettings(null, function (model) {
            // add item to scope
            $scope.items.push(buildNavItem(model.value));
        });
    };

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
        openSettings(item, function (model) {
            // update item in scope
            // assign new values via extend to maintain refs
            angular.extend(item, buildNavItem(model.value));
        });
    };

    $scope.remove = function (item) {
        item.remove();
    };

    $scope.isVisible = function (item) {
        return $scope.model.config.removeNaviHideItems == true ? item.naviHide !== true : true;
    };

    $scope.$on("formSubmitting", function (ev, args) {
        $scope.model.value = $scope.items;
    });



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

    function openSettings (item, callback) {
        // assign value to new empty object to break refs
        // prevent accidentally changing old values
        $scope.settingsOverlay = {
            title: "Settings",
            view: "/App_Plugins/Meganav/Views/settings.html",
            show: true,
            value: angular.extend({}, item),
            submit: function (model) {
                !callback || callback(model);
                // close settings
                closeSettings();
            }
        }
    }

    function closeSettings () {
        $scope.settingsOverlay.show = false;
        $scope.settingsOverlay = null;
    }

    function buildNavItem (data) {

        return {
            id: data.id,
            title: data.name,
            name: data.name,
            title: data.title,
            target: data.target,
            url: data.url || "#",
            children: data.children || [],
            icon: data.icon || "icon-link",
            published: data.published,
            config: []
            naviHide: data.naviHide
        }
    }
}

angular.module("umbraco").controller("Cogworks.Meganav.MeganavController", Meganav);

app.requires.push("ui.tree");