# 變更紀錄 (Changelog)

所有關於 MCServerTool 專案的重大更動都將記錄於此文件中。

## [未發布 (Unreleased)]

### 新增 (Added)
- 以 `.NET 10` 初始化 `MCServerTool` 基礎專案結構與進入點。
- 建立 `.slnx` 方案檔，用以管理主程式並引用 `ImTK` 模組。
- 建立 `AGENTS.md`，制定了 AI 開發指南，並明訂嚴禁修改 `ImTK` 模組內核的嚴格規定。
- 將專案的文檔規範 (`DocStandards.md`, `NamingConventions.md`) 移植至 `docs/Project/` 並針對本專案調整。
- 實作 `Program.cs` 作為程式進入點，透過 `ImTKEnvironment` 設定自訂快取路徑 (`LocalDataPath`)，並藉由 `ImTKSilk` 啟動基礎視窗。
- 加入 `launchSettings.json` 以便在 IDE 中能順利切換啟動選項。
