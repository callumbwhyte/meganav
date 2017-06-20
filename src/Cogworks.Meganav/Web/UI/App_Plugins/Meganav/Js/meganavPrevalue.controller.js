function Prevalues($scope, meganavPreValueConfigService) {

  // Setup Dialog Overlay
  $scope.meganavSettingsOverlay = {
    view: "views/propertyeditors/grid/dialogs/editconfig.html",
    title: "Properties",
    show: false,
    config: $scope.model.value || [],

    submit: function (model) {
      $scope.model.value = model.config;
      $scope.meganavSettingsOverlay.show = false;
    },

    close: function (oldModel) {
      $scope.meganavSettingsOverlay.show = false;
    }
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


app.service('meganavPreValueConfigService', function () {
  var _config = {};

  return {
    config: _config
  };
});