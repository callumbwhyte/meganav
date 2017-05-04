function Prevalues($scope, meganavPreValueConfigService) {


    $scope.meganavSettingsOverlay = {};
    $scope.meganavSettingsOverlay.view = "views/propertyeditors/grid/dialogs/editconfig.html";
    $scope.meganavSettingsOverlay.show = false;
    $scope.meganavSettingsOverlay.title = "Settings";
    $scope.meganavSettingsOverlay.config = $scope.model.value;

    $scope.meganavSettingsOverlay.submit = function (model) {
        $scope.model.value = model.config;

        $scope.meganavSettingsOverlay.show = false;
    };

    $scope.meganavSettingsOverlay.close = function (oldModel) {
        $scope.meganavSettingsOverlay.show = false;
    };

    $scope.editConfig = function (config) {
        $scope.meganavSettingsOverlay.show = true;
        meganavPreValueConfigService.config.settings = config;
    };

    $scope.removeConfigValue = function (collection, index) {
        collection.splice(index, 1);
    };
}

angular.module("umbraco").controller("Cogworks.Meganav.MeganavPrevalueController", Prevalues);


app.service('meganavPreValueConfigService',
    function () {

        var _config = {};

        return {
            config: _config
        };
    });