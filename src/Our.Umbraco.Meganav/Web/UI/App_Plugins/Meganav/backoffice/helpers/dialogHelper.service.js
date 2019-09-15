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
                    },
                    filePicker: function (options) {
                        var defaults = {
                            section: "settings",
                            treeAlias: "files",
                            entityType: "file",
                            isDialog: true,
                            filter: function (file) {
                                return !(file.name.indexOf(options.extension) !== -1);
                            },
                            filterCssClass: "not-allowed"
                        };
                        return editorService.treePicker(Object.assign(defaults, options));
                    }
                };
            });

})();