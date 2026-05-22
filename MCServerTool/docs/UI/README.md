# UI 子系統與視覺架構 (UI Architecture)

本文件說明了 MCServerTool 在構建使用者介面時的架構選擇與工程實務。

## 1. 視窗架構 (Window Lifecycle)

專案全面棄用直接在主迴圈撰寫 `ImGui.Begin()` / `ImGui.End()` 的作法，而是繼承 ImTK 的 `ImTK.UI.Window` 基礎類別：
* `ServerListWindow`、`ServerInfoWindow` 與 `ConsoleWindow` 在程式啟動時利用 `Window.Open<T>()` 開啟。
* 這些視窗並未限制於特定的 Docking 排版，而是藉由 `NoClose` (透過不繪製關閉按鈕) 等 Flag 在畫面中作為浮動/可依附面板存在，保留使用者最大彈性。
* 針對即用即拋的工具視窗（例如 `CreateServerInstanceWindow`），則在觸發後動態開啟，並於完成任務後呼叫 `Close()` 進行銷毀。

## 2. 菜單系統 (Main Menu Integration)

透過 ImTK 的 `[MainMenu("Path/To/Menu")]` Attribute，將建立伺服器等靜態方法自動掛載至應用的主選單上。這種高度解耦的設計確保了 UI 與核心業務邏輯的乾淨分離。

## 3. 表單與欄位綁定 (Field Drawers)

在 `ServerInfoWindow` 等需要大量輸入欄位的編輯器中，我們不直接使用 `ImGui.InputText`。
* **統一性**：一律使用 `FieldDrawerFactory` 建立對應型別的 `IFieldDrawer`。
* **資料綁定**：利用 `RegisterValueChangedCallback` 將 Drawer 與底層的 `ServerInstance` 進行雙向綁定。
* **記憶體防護**：Drawer 會被加入到 `VisualElement` 的 `hierarchy` 中由系統自動走訪渲染。若需手動排版（例如與額外的按鈕放置於同一行），則選擇將其脫離 `hierarchy`，於 `OnRenderSelf` 中手動呼叫其 `Render()`。

---

# 工程設計妥協 (Design Notes)

## 關於原生的對話框 (DialogUtils)

**背景與痛點：**
在建立伺服器時，我們需要一個「選擇資料夾」的功能。然而，引入 C# 原生的 Windows Forms (`FolderBrowserDialog`) 或 WPF 套件，不僅會破壞 `.NET 10 Exe` 的跨平台與精簡原則，還會帶來不必要的依賴與體積膨脹。

**工程妥協：**
我們選擇建立 `MCServerTool.Utils.DialogUtils`，以「外部 Process」的方式呼叫系統內建腳本來達成目的：
* **Windows**：透過 `Process` 執行隱藏的 `powershell.exe` 腳本，呼叫系統內建的 `.NET` 組件來彈出資料夾選擇視窗。
* **Linux**：呼叫 `zenity --file-selection --directory`。
* **macOS**：呼叫 `osascript` 的 AppleScript 彈出對話框。

**結論：**
這個決策犧牲了一點點彈出時的效能（啟動 Process 需要數十毫秒），但換來了對 C# 專案依賴的「絕對零污染」，這對於一個基於 ImGui 的跨平台輕量級管理工具來說，是一個非常划算的架構交易。