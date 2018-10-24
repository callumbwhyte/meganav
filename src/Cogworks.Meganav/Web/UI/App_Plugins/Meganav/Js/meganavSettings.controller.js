function MeganavSettings($scope, $controller, meganavResource) {
    
    $scope.dialogOptions = {
        currentTarget: null
    };
    
    if (!_.isEmpty($scope.model.value.url)) {
        $scope.dialogOptions.currentTarget = $scope.model.value;

        // v7.12 hack due to controller checking wrong variable
        $scope.model.url = $scope.model.value.url;
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
        meganavResource.getByUdi(args.node.udi).then(function (response) {
            angular.extend($scope.dialogOptions.currentTarget, response.data);
            });
       }
    }
}

angular.module("umbraco").controller("Cogworks.Meganav.MeganavSettingsController", MeganavSettings);