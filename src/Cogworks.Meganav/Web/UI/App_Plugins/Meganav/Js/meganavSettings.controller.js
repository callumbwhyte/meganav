function MeganavSettings($scope, $controller) {

    $scope.dialogOptions = {
        currentTarget: null
    };

    if ($scope.model.value) {
        $scope.dialogOptions.currentTarget = $scope.model.value;
    }

    $scope.$on("formSubmitting", function (ev, args) {
        $scope.model.value = $scope.target;
    });

    // load Umbraco Link Picker controller
    $controller('Umbraco.Dialogs.LinkPickerController', { $scope: $scope });
}

angular.module("umbraco").controller("Cogworks.Meganav.MeganavSettingsController", MeganavSettings);