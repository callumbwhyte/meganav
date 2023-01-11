# Change Log

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/) and this project adheres to [Semantic Versioning](https://semver.org/).

## [4.0.0] - 2023-01-11
### Added
* Initial release of SuperValueConverters for Umbraco v9, v10 (LTS), and v11

## [3.0.3] - 2022-05-27
### Fixed
* Unable to create new Meganav properties due to null data type

## [3.0.2] - 2022-05-23
### Fixed
* `meganav-card` styles breaking backoffice minification
* "Populate from tree" breaks when no child nodes exist
* Exception when saving empty Meganav properties
* Legacy migrator runs when saving data type

## [3.0.1] - 2022-03-06
### Fixed
* "Change type" dialog now shows on nav items with no item type assigned
* Item types in the "Change type" dialog now default to using the link icon

## [3.0.0] - 2022-03-01
### Added
* Initial release of Meganav for Umbraco 9+
* Ability to create item types with Element Type settings, custom views, and permissions
* Ability to toggle the visibility of nav items
* Actions to expand / collapse Meganav, and populate with all nodes from the tree
* UI overhaul with improved accessibility

## [2.0.3] - 2022-05-27
### Fixed
* Unable to create new Meganav properties due to null data type

## [2.0.2] - 2022-05-23
### Fixed
* `meganav-card` styles breaking backoffice minification
* "Populate from tree" breaks when no child nodes exist
* Exception when saving empty Meganav properties
* Legacy migrator runs when saving data type

## [2.0.1] - 2022-03-06
### Fixed
* "Change type" dialog now shows on nav items with no item type assigned
* Item types in the "Change type" dialog now default to using the link icon

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

[Unreleased]: https://github.com/callumbwhyte/meganav/compare/release-4.0.0...HEAD
[4.0.0]: https://github.com/callumbwhyte/meganav/compare/release-3.0.3...release-4.0.0
[3.0.3]: https://github.com/callumbwhyte/meganav/compare/release-3.0.2...release-3.0.3
[3.0.2]: https://github.com/callumbwhyte/meganav/compare/release-3.0.1...release-3.0.2
[3.0.1]: https://github.com/callumbwhyte/meganav/compare/release-3.0.0...release-3.0.1
[3.0.0]: https://github.com/callumbwhyte/meganav/compare/release-2.0.0...release-3.0.0
[2.0.3]: https://github.com/callumbwhyte/meganav/compare/release-2.0.2...release-2.0.3
[2.0.2]: https://github.com/callumbwhyte/meganav/compare/release-2.0.1...release-2.0.2
[2.0.1]: https://github.com/callumbwhyte/meganav/compare/release-2.0.0...release-2.0.1
[2.0.0]: https://github.com/callumbwhyte/meganav/tree/release-2.0.0
[1.1.2]: https://github.com/callumbwhyte/meganav/compare/release-1.1.1...release-1.1.2
[1.1.1]: https://github.com/callumbwhyte/meganav/compare/release-1.1.0...release-1.1.1
[1.1.0]: https://github.com/callumbwhyte/meganav/compare/release-1.0.0...release-1.1.0
[1.0.0]: https://github.com/callumbwhyte/meganav/tree/release-1.0.0