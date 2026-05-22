# Changelog

All notable changes to the MCServerTool project will be documented in this file.

## [Unreleased]

### Added
- Initialize base project structure (`MCServerTool`) using `.NET 10`.
- Create `.slnx` solution mapping the main application and `ImTK` submodules.
- Establish `AGENTS.md` specifying AI guidelines and strict rules against modifying the `ImTK` submodule.
- Adopt and adapt project documentation guidelines (`DocStandards.md`, `NamingConventions.md`) in `docs/Project/`.
- Implement `Program.cs` as the application entry point, integrating `ImTKSilk` to launch a default window and configure `LocalDataPath` via `ImTKEnvironment`.
