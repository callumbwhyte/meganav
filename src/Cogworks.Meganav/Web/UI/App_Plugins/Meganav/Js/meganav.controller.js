function Meganav($scope, meganavResource) {

    $scope.items = [];

    if ($scope.model.value) {
        // retreive the saved items
        $scope.items = $scope.model.value;

        // get updated entities for content
        getItemEntities($scope.items);
    }

    $scope.add = function () {
        openSettings(null, function (model) {
            // add item to scope
            $scope.items.push(buildNavItem(model.value));
        });
    };

    $scope.edit = function (item) {
        openSettings(item, function (model) {
            // update item in scope
            // Assign new values via extend to maintain refs
            angular.extend(item, buildNavItem(model.value));
        });
    };

    $scope.remove = function (item) {
        item.remove();
    };

    $scope.$on("formSubmitting", function (ev, args) {
        $scope.model.value = $scope.items;
    });

    function getItemEntities (items) {
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
        // Assign value to new empty object to break refs
        // Prevent accidentally auto changing old values
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
            name: data.name,
            title: data.title,
            target: data.target,
            url: data.url || "#",
            children: [],
            icon: data.icon || "icon-link",
            published: true
        }
    }
}

angular.module("umbraco").controller("Cogworks.Meganav.MeganavController", Meganav);

app.requires.push("ui.tree");