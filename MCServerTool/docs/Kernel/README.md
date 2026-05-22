# Kernel 子系統架構規格 (Kernel Architecture)

本文件描述了 MCServerTool 應用程式中，與 Minecraft 伺服器核心、實例管理相關的架構設計。

## 架構分離原則：Data Model vs Runtime Context

在管理伺服器的設計上，我們嚴格分離了「設定資料」與「執行時狀態」。

### 1. `ServerInstance` (Data Model)
* **位置**：`MCServerTool.Data.ServerInstance`
* **職責**：作為一個純粹的資料容器 (Data Class)，負責儲存所有可以被序列化 (Serialized) 的設定檔數據。
* **內容**：包含實例名稱、版本、核心類型、執行檔路徑、工作目錄、Java 路徑、記憶體設定參數、EULA 狀態以及 NoGUI 標記等。
* **儲存**：由系統以 JSON 格式儲存於 `local/server_instances/*.json` 之中。

### 2. `KernelContext` (Runtime Context)
* **位置**：`MCServerTool.Kernel.KernelContext`
* **職責**：代表「單一個伺服器實例」的執行環境與運行時狀態。
* **關聯性**：每個 `ServerInstance` 在應用程式生命週期內都具有一個常駐、1對1 綁定的 `KernelContext`。
* **狀態管理**：它負責追蹤底層的 Process (例如是否正在運行 `IsRunning`)、接管伺服器的標準輸出 (Console Log) 以及向伺服器發送指令。
* **全局指標**：系統維護了一個靜態指標 `KernelContext.Current`，用於標記使用者目前在 UI 上所選中（關注）的伺服器，使得所有的 Console 與 Info 面板能隨之動態切換顯示。

## 伺服器管理與註冊：ServerManager

`ServerManager` 是一個全局單例 (Singleton)，負責橋接磁碟 IO 與上述的核心架構。

### 1. 職責
* 啟動時掃描並解析 `local/server_instances/` 底下的所有 `.json` 檔案。
* 將解析出的 `ServerInstance` 封裝進新的 `KernelContext`，並維護一個全局的 `Kernels` 列表。
* 提供 API 讓 UI 可以創建與儲存新的伺服器實例。

### 2. 工作目錄自動分配機制 (`%Auto%`)
當使用者在 UI 透過 Drawer 建立新的伺服器實例時，如果將工作目錄欄位留空或填入 `%Auto%` 標記，`ServerManager` 會啟動自動分配機制：
* 將工作目錄自動指向 `ImTKEnvironment.LocalDataPath/servers/{InstanceId}`。
* 自動偵測該目錄是否存在，並於寫入實例時確保物理資料夾被正確建立 (`Directory.CreateDirectory`)。
* 此機制可避免使用者必須手動配置複雜路徑的困擾。
