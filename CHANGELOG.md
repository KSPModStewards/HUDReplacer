# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## Unreleased
## 1.3.3
### Fixed
* Fixed a bug where HUDReplacer would not replace any textures if there were
  zero scene-specific replacements or zero general replacements.

## 1.3.2
### Fixed
* Fixed a bug where a replacement with an explicitly specified size would not
  be selected if it didn't match the size of the texture it was replacing.

### Added
* If the debug menu is enabled HUDReplacer will now log exactly what texture
  replacements are occurred.

## 1.3.1
### Changed
* Optimized scene switch overhead down from 1.5s to 140ms (as tested with ZTheme).
* Improved error messages when texture names are not in the appropriate format.

### Fixed
* Fixed a bunch of crashes if texture names were not in the expected format.

## 1.3.0
This is the first release from KSPModStewards/HUDReplacer. For previous changelog
entries check out the releases on <https://github.com/UltraJohn/HUDReplacer>.

### Added
* HUDReplacer now has a `KSPAssembly` attribute.

### Changed
* @UltraJohn has relicensed HUDReplacer from ARR to GPLv3.
