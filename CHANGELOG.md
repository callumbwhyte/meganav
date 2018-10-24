# Change Log
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/) and this project adheres to [Semantic Versioning](http://semver.org/).

## [1.5.0] - 2018-20-08
### ADDED
* Udi support
* Configurable Properties per treeitem

### Changed
* Re-targeting package to Umbraco 7.6.2+


## [1.1.2] - 2018-20-08
### Fixed
* Nav item settings dialog not functioning in Umbraco v7.12

## [1.1.1] - 2018-17-01
### Fixed
* Exception thrown when trying to render an empty Meganav

## [1.1.0] - 2017-23-07
### Added
* Support for UmbracoNaviHide on content items
* Handling legacy icons returned by Umbraco APIs
* Required field validation to nav item settings
* This CHANGELOG file
* AppVeyor configuration to allow for automated builds and tests
* Project version number to package.json for automated builds

### Changed
* Re-targeting package to Umbraco 7.4+
* Replaced Umbraco Link Picker with custom Umbraco UI overlay to allow for further customisation
* Updated documentation to reflect UmbracoNaviHide support

### Fixed
* Node names not updating after save
* Legacy node icons rendering incorrectly
* Changes persisting after editing a nav item then cancelling the changes
* Duplicate files in NuGet package

## [1.0.1] - 2017-10-03
### Added
* Nuspec file to allow for publishing to NuGet.org
* Assembly descriptions on the project DLLs

### Fixed
* Fix issues with NuGet packaging

## [1.0.0] - 2017-01-03
### Added
* Initial release of Meganav for Umbraco 7.5
* Build scripts
* README file with information about the project and screenshots
* MIT license in the form of a LICENSE.md file

[Unreleased]: https://github.com/thecogworks/meganav/compare/v1.1.2...HEAD
[1.1.2]: https://github.com/thecogworks/meganav/compare/v1.1.1...v1.1.2
[1.1.1]: https://github.com/thecogworks/meganav/compare/v1.1.0...v1.1.1
[1.1.0]: https://github.com/thecogworks/meganav/compare/v1.0.1...v1.1.0
[1.0.1]: https://github.com/thecogworks/meganav/compare/v1.0.0...v1.0.1