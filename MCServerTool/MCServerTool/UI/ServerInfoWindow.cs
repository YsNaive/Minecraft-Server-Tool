using System;
using System.Collections.Generic;
using ImGuiNET;
using ImTK.UI;
using ImTK.Core;
using MCServerTool.Kernel;

namespace MCServerTool.UI
{
    public class ServerInfoWindow : Window
    {
        private Dictionary<string, IFieldDrawer> _drawers = new Dictionary<string, IFieldDrawer>();
        private string? _currentInstanceId = null;

        public ServerInfoWindow() : base("Server Info", "ServerInfoWindow")
        {
            // Windows are not closable by default unless a close button is drawn.
        }

        private void InitializeDrawers(Data.ServerInstance instance)
        {
            _drawers.Clear();
            hierarchy.Clear(); // Clear any previously added visual elements

            // Note: the drawers are typed based on property types.
            // string properties
            _drawers["Name"] = FieldDrawerFactory.Create().FromType(typeof(string)).Label("Name").FromValue(instance.Name).Build();
            _drawers["Version"] = FieldDrawerFactory.Create().FromType(typeof(string)).Label("Version").FromValue(instance.Version).Build();
            _drawers["Core"] = FieldDrawerFactory.Create().FromType(typeof(string)).Label("Core").FromValue(instance.Core).Build();
            _drawers["Executable"] = FieldDrawerFactory.Create().FromType(typeof(string)).Label("Executable").FromValue(instance.ExecutableName).Build();
            _drawers["WorkingDirectory"] = FieldDrawerFactory.Create().FromType(typeof(string)).Label("Working Directory").FromValue(instance.WorkingDirectory).Build();
            _drawers["JavaPath"] = FieldDrawerFactory.Create().FromType(typeof(string)).Label("Java Path").FromValue(instance.JavaPath).Build();
            _drawers["JavaArguments"] = FieldDrawerFactory.Create().FromType(typeof(string)).Label("Java Arguments").FromValue(instance.JavaArguments).Build();

            // bool properties
            _drawers["EulaAccepted"] = FieldDrawerFactory.Create().FromType(typeof(bool)).Label("EULA Accepted").FromValue(instance.EulaAccepted).Build();
            _drawers["NoGui"] = FieldDrawerFactory.Create().FromType(typeof(bool)).Label("No GUI Flag").FromValue(instance.NoGui).Build();

            // Register callbacks
            ((FieldDrawer<string>)_drawers["Name"]).RegisterValueChangedCallback(evt => { instance.Name = evt.newValue; SaveInstance(); });
            ((FieldDrawer<string>)_drawers["Version"]).RegisterValueChangedCallback(evt => { instance.Version = evt.newValue; SaveInstance(); });
            ((FieldDrawer<string>)_drawers["Core"]).RegisterValueChangedCallback(evt => { instance.Core = evt.newValue; SaveInstance(); });
            ((FieldDrawer<string>)_drawers["Executable"]).RegisterValueChangedCallback(evt => { instance.ExecutableName = evt.newValue; SaveInstance(); });
            ((FieldDrawer<string>)_drawers["WorkingDirectory"]).RegisterValueChangedCallback(evt => { instance.WorkingDirectory = evt.newValue; SaveInstance(); });
            ((FieldDrawer<string>)_drawers["JavaPath"]).RegisterValueChangedCallback(evt => { instance.JavaPath = evt.newValue; SaveInstance(); });
            ((FieldDrawer<string>)_drawers["JavaArguments"]).RegisterValueChangedCallback(evt => { instance.JavaArguments = evt.newValue; SaveInstance(); });

            ((FieldDrawer<bool>)_drawers["EulaAccepted"]).RegisterValueChangedCallback(evt => { instance.EulaAccepted = evt.newValue; SaveInstance(); });
            ((FieldDrawer<bool>)_drawers["NoGui"]).RegisterValueChangedCallback(evt => { instance.NoGui = evt.newValue; SaveInstance(); });

            // Add them to hierarchy so they are rendered automatically
            foreach(var drawer in _drawers.Values)
            {
                hierarchy.Add((VisualElement)drawer);
            }

            _currentInstanceId = instance.Id;
        }

        private void SaveInstance()
        {
            if (KernelContext.Current != null)
            {
                ServerManager.Instance.SaveInstance(KernelContext.Current.Instance);
            }
        }

        private void SyncDrawerValues(Data.ServerInstance instance)
        {
            if (_drawers.Count > 0)
            {
                ((FieldDrawer<string>)_drawers["Name"]).SetValueWithoutNotify(instance.Name);
                ((FieldDrawer<string>)_drawers["Version"]).SetValueWithoutNotify(instance.Version);
                ((FieldDrawer<string>)_drawers["Core"]).SetValueWithoutNotify(instance.Core);
                ((FieldDrawer<string>)_drawers["Executable"]).SetValueWithoutNotify(instance.ExecutableName);
                ((FieldDrawer<string>)_drawers["WorkingDirectory"]).SetValueWithoutNotify(instance.WorkingDirectory);
                ((FieldDrawer<string>)_drawers["JavaPath"]).SetValueWithoutNotify(instance.JavaPath);
                ((FieldDrawer<string>)_drawers["JavaArguments"]).SetValueWithoutNotify(instance.JavaArguments);
                ((FieldDrawer<bool>)_drawers["EulaAccepted"]).SetValueWithoutNotify(instance.EulaAccepted);
                ((FieldDrawer<bool>)_drawers["NoGui"]).SetValueWithoutNotify(instance.NoGui);
            }
        }

        protected override void OnRenderLayout()
        {
            var kernel = KernelContext.Current;

            if (kernel == null)
            {
                // Must call Begin/End for the window
                bool isOpenForImGui = m_isOpen;
                bool isExpanded = Begin(ref isOpenForImGui, flags.Value);
                if (isExpanded)
                {
                    ImGui.Text("No server selected.");
                }
                End();

                if (!isOpenForImGui && m_isOpen)
                {
                    Close();
                }
                return;
            }

            var instance = kernel.Instance;

            if (instance != null)
            {
                // Check if selected instance has changed or we need initialization
                if (_currentInstanceId != instance.Id || _drawers.Count == 0)
                {
                    InitializeDrawers(instance);
                }
                else
                {
                    SyncDrawerValues(instance);
                }
            }

            // Normal Window base rendering, which will call OnRenderSelf and then render hierarchy (the drawers)
            base.OnRenderLayout();
        }

        protected override void OnRenderSelf()
        {
            var kernel = KernelContext.Current;
            if (kernel == null) return;
            var instance = kernel.Instance;

            ImGui.Text($"Editing: {instance?.Name}");
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
            // Rest of fields are rendered by hierarchy automatically
        }
    }
}
