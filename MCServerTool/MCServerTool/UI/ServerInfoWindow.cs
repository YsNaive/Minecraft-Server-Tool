using System;
using ImGuiNET;
using ImTK.UI;
using MCServerTool.Kernel;

namespace MCServerTool.UI
{
    public class ServerInfoWindow : Window
    {
        public ServerInfoWindow() : base("Server Info", "ServerInfoWindow")
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

            var instance = kernel.Instance;

            ImGui.Text($"Editing: {instance.Name}");
            ImGui.Separator();

            // Status and Controls
            ImGui.Text($"Status: {(kernel.IsRunning ? "Running" : "Stopped")}");

            if (kernel.IsRunning)
            {
                if (ImGui.Button("Stop Server"))
                {
                    kernel.Stop();
                }
            }
            else
            {
                if (ImGui.Button("Start Server"))
                {
                    kernel.Start();
                }
            }

            ImGui.Separator();

            // Edit Fields
            string name = instance.Name;
            if (ImGui.InputText("Name", ref name, 128))
            {
                instance.Name = name;
                ServerManager.Instance.SaveInstance(instance);
            }

            string version = instance.Version;
            if (ImGui.InputText("Version", ref version, 32))
            {
                instance.Version = version;
                ServerManager.Instance.SaveInstance(instance);
            }

            string core = instance.Core;
            if (ImGui.InputText("Core", ref core, 32))
            {
                instance.Core = core;
                ServerManager.Instance.SaveInstance(instance);
            }

            string executable = instance.ExecutableName;
            if (ImGui.InputText("Executable", ref executable, 128))
            {
                instance.ExecutableName = executable;
                ServerManager.Instance.SaveInstance(instance);
            }

            string workingDir = instance.WorkingDirectory;
            if (ImGui.InputText("Working Directory", ref workingDir, 256))
            {
                instance.WorkingDirectory = workingDir;
                ServerManager.Instance.SaveInstance(instance);
            }

            string javaPath = instance.JavaPath;
            if (ImGui.InputText("Java Path", ref javaPath, 256))
            {
                instance.JavaPath = javaPath;
                ServerManager.Instance.SaveInstance(instance);
            }

            string javaArgs = instance.JavaArguments;
            if (ImGui.InputText("Java Arguments", ref javaArgs, 256))
            {
                instance.JavaArguments = javaArgs;
                ServerManager.Instance.SaveInstance(instance);
            }

            bool eula = instance.EulaAccepted;
            if (ImGui.Checkbox("EULA Accepted", ref eula))
            {
                instance.EulaAccepted = eula;
                ServerManager.Instance.SaveInstance(instance);
            }

            bool noGui = instance.NoGui;
            if (ImGui.Checkbox("No GUI Flag", ref noGui))
            {
                instance.NoGui = noGui;
                ServerManager.Instance.SaveInstance(instance);
            }
        }
    }
}
