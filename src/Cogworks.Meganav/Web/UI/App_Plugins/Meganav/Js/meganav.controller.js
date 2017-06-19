function Meganav($scope, dialogService, meganavResource) {

    $scope.items = [];

    if ($scope.model.value) {
        // retreive the saved items
        $scope.items = $scope.model.value;

        // get updated entities for content
        getItemEntities($scope.items);
    }

    $scope.edit = function (item) {
        dialogService.linkPicker({
            // Assign value to new empty object to break refs
            // Prevent accidentally auto changing old values
            currentTarget: angular.extend({}, item),
            callback: function (data) {
                // Assign new values via extend to maintain refs
                angular.extend(item, buildNavItem(data));
            }
        });
    };

    $scope.remove = function (item) {
        item.remove();
    };

    $scope.$on("formSubmitting", function (ev, args) {
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

    function buildNavItem(data) {
        return {
            id: data.id,
            title: data.name || data.title,
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