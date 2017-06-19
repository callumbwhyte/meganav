function MeganavSettings($scope, $controller, meganavResource) {

    $scope.dialogOptions = {
        currentTarget: null
    };

    if ($scope.model.value) {
        $scope.dialogOptions.currentTarget = $scope.model.value;
    }

    $scope.$on("formSubmitting", function (ev, args) {
        $scope.model.value = $scope.target;
    });

    // extend Umbraco Link Picker controller
    $controller("Umbraco.Dialogs.LinkPickerController", { $scope: $scope });

    // register custom select handler
    $scope.dialogTreeEventHandler.bind("treeNodeSelect", nodeSelectHandler);

    // destroy custom select handler
    $scope.$on('$destroy', function () {
        $scope.dialogTreeEventHandler.unbind("treeNodeSelect", nodeSelectHandler);
    });

    function nodeSelectHandler (ev, args) {
        if (!args.node.metaData.listViewNode) {
            meganavResource.getById(args.node.id).then(function (response) {
                angular.extend($scope.target, response.data);
            });
        }
    }
}

angular.module("umbraco").controller("Cogworks.Meganav.MeganavSettingsController", MeganavSettings);