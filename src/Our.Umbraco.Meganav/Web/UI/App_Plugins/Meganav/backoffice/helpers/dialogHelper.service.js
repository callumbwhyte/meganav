(function () {

    "use strict";

    angular.module("umbraco")
        .factory("dialogHelper",
            function (editorService) {
                return {
                    elementTypePicker: function (options) {
                        var defaults = {
                            section: "settings",
                            treeAlias: "documentTypes",
                            entityType: "documentType",
                            filter: function (node) {
                                return !node.metaData.isElement;
                            },
                            filterCssClass: "not-allowed"
                        };
                        return editorService.treePicker(Object.assign(defaults, options));
                    }
                };
            });

})();