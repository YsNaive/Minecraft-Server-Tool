using System;
using ImGuiNET;
using ImTK.UI;
using MCServerTool.Kernel;
using MCServerTool.Data;

namespace MCServerTool.UI
{
    public class ServerListWindow : Window
    {
        public ServerListWindow() : base("Servers", "ServerListWindow")
        {
            // Windows are not closable by default unless a close button is drawn.
        }

        protected override void OnRenderSelf()
        {
            if (ImGui.Button("Add Server", new System.Numerics.Vector2(-1, 0)))
            {
                var kernel = ServerManager.Instance.CreateNewInstance();
                KernelContext.Current = kernel;
            }

            ImGui.Separator();

            var kernels = ServerManager.Instance.Kernels;
            foreach (var kernel in kernels)
            {
                bool isSelected = KernelContext.Current == kernel;

                // Construct a display string showing status
                string status = kernel.IsRunning ? "[Running]" : "[Stopped]";
                string label = $"{status} {kernel.Instance.Name}###{kernel.Instance.Id}";

                if (ImGui.Selectable(label, isSelected))
                {
                    KernelContext.Current = kernel;
                }
            }
        }
    }
}
