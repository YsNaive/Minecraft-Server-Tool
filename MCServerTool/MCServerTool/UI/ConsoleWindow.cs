using System;
using ImGuiNET;
using ImTK.UI;
using MCServerTool.Kernel;

namespace MCServerTool.UI
{
    public class ConsoleWindow : Window
    {
        private string _inputBuffer = "";

        public ConsoleWindow() : base("Console", "ConsoleWindow")
        {
            // Windows are not closable by default unless a close button is drawn.
        }

        protected override void OnRenderSelf()
        {
            var kernel = KernelContext.Current;

            if (kernel == null)
            {
                ImGui.Text("No server selected.");
                return;
            }

            ImGui.Text($"Console for {kernel.Instance.Name}");
            ImGui.Separator();

            // Dummy console output area for now
            var contentRegion = ImGui.GetContentRegionAvail();
            var consoleHeight = contentRegion.Y - ImGui.GetFrameHeightWithSpacing();

            if (ImGui.BeginChild("ConsoleOutput", new System.Numerics.Vector2(0, consoleHeight), ImGuiChildFlags.Borders, ImGuiWindowFlags.HorizontalScrollbar))
            {
                if (!kernel.IsRunning)
                {
                    ImGui.Text("Server is offline.");
                }
                else
                {
                    ImGui.Text("Server is running. (Log output will appear here)");
                }
            }
            ImGui.EndChild();

            ImGui.SetNextItemWidth(-1);
            bool enterPressed = ImGui.InputText("##ConsoleInput", ref _inputBuffer, 256, ImGuiInputTextFlags.EnterReturnsTrue);

            if (enterPressed && !string.IsNullOrWhiteSpace(_inputBuffer))
            {
                if (kernel.IsRunning)
                {
                    // TODO: Send command to kernel process
                    Console.WriteLine($"Command sent: {_inputBuffer}");
                }
                _inputBuffer = "";
                ImGui.SetKeyboardFocusHere(-1); // Keep focus on input box after pressing enter
            }
        }
    }
}
