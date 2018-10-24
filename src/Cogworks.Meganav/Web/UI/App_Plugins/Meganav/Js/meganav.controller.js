function Meganav($scope, meganavResource) {

    $scope.items = [];
    $scope.propertyFields = $scope.model.config.properties || [];

    if (!_.isEmpty($scope.model.value)) {
        // retreive the saved items
        $scope.items = $scope.model.value;

        // get updated entities for content
        getItemEntities($scope.items);
    }

    $scope.add = function () {
        openSettings({}, function (model) {
            // add item to scope
            $scope.items.push(buildNavItem(model.value));
        });
    };

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
        return $scope.model.config.removeNaviHideItems === true ? item.naviHide !== true : true;
    };

    $scope.$on("formSubmitting", function (ev, args) {
        $scope.model.value = $scope.items;
    });

    function getItemEntities(items) {
        _.each(items, function (item) {
            if (item.udi) {
                meganavResource.getByUdi(item.udi).then(function (response) {
                    angular.extend(item, response.data);
                });

                if (item.children.length > 0) {
                    getItemEntities(item.children);
                }
            }
        });
    }

    function openSettings(item, callback) {
        // Update Properties appropriately
        if (!angular.isObject(item.properties)) {
            item.properties = {};
        }

        // Get available property fields
        var props = angular.copy($scope.propertyFields)
            .filter(isPropertyFieldEnabled, item)
            .map(setPropertyFieldValue, item);

        // Clone original item so our changes aren't live
        clonedItem = angular.copy(item);

        // Assign value to new empty object to break refs
        // Prevent accidentally auto changing old values
        $scope.settingsOverlay = {
            title: "Settings",
            view: "/App_Plugins/Meganav/Views/settings.html",
            show: true,
            value: clonedItem,
            properties: props,
            submit: function(model) {
                // Update Properties
                model.value.properties = getItemProperties(model.value, model.properties);
                !callback || callback(model);

                // close settings
                closeSettings();
            }
        };
    }

    function closeSettings() {
        $scope.settingsOverlay.show = false;
        $scope.settingsOverlay = null;
    }

    // Create/Update Nav Item
    function buildNavItem(data, old) {
        var defaults = {
            id: 0,
            name: "",
            title: "",
            target: "",
            url: "#",
            achor: "",
            level: 0,
            children: data.children || [],
            icon: "icon-link",
            published: true,
            naviHide: data.naviHide
        };
        return angular.extend(old || defaults, data);
    }

    // Check if property field should be enabled for current item
    function isPropertyFieldEnabled(prop) {
        return (
            angular.isUndefined(prop.applyTo) ||
                !angular.isArray(prop.applyTo) ||
                !prop.applyTo.length ||
                prop.applyTo.indexOf(this.level || 0) >= 0
        );
    }

    // Set the items values on the property field
    function setPropertyFieldValue(prop) {
        var val = this.properties[prop.key];
        prop.value = val;
        return prop;
    }

    // Get the properties for an item from an array of property fields
    function getItemProperties(item, props) {
        props = _.isEmpty(props) ? $scope.propertyFields : props;
        return props
            .filter(isPropertyFieldEnabled, item)
            .reduce(function(props, p) { return (props[p.key] = p.value, props) }, {});
    }
}

angular.module("umbraco").controller("Cogworks.Meganav.MeganavController", Meganav);

app.requires.push("ui.tree");