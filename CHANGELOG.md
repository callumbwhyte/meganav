# Change Log

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/) and this project adheres to [Semantic Versioning](https://semver.org/).

## [2.0.0] - 2022-03-01
### Added
* Initial release of Meganav for Umbraco 8.7+
* Ability to create item types with Element Type settings, custom views, and permissions
* Ability to toggle the visibility of nav items
* Actions to expand / collapse Meganav, and populate with all nodes from the tree
* UI overhaul with improved accessibility

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
* AppVeyor configuration to allow for automated builds and tests
* Project version number to package.json for automated builds

### Changed
* Re-targeting package to Umbraco 7.4+
* Replaced Umbraco Link Picker with custom Umbraco UI overlay to allow for further customisation

### Fixed
* Node names not updating after save
* Legacy node icons rendering incorrectly
* Changes persisting after editing a nav item then cancelling the changes
* Duplicate files in NuGet package

## [1.0.1] - 2017-10-03
### Fixed
* Fix issues with NuGet packaging

## [1.0.0] - 2017-01-03
### Added
* Initial release of Meganav for Umbraco 7.5

[Unreleased]: https://github.com/callumbwhyte/meganav/compare/release-2.0.0...HEAD
[2.0.0]: https://github.com/callumbwhyte/meganav/tree/release-2.0.0
[1.1.2]: https://github.com/callumbwhyte/meganav/compare/release-1.1.1...release-1.1.2
[1.1.1]: https://github.com/callumbwhyte/meganav/compare/release-1.1.0...release-1.1.1
[1.1.0]: https://github.com/callumbwhyte/meganav/compare/release-1.0.0...release-1.1.0
[1.0.0]: https://github.com/callumbwhyte/meganav/tree/release-1.0.0