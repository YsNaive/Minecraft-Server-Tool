using System;
using ImGuiNET;
using ImTK.UI;
using MCServerTool.Kernel;
using MCServerTool.Utils;

namespace MCServerTool.UI
{
    public class CreateServerInstanceWindow : Window
    {
        private IFieldDrawer<string> _nameDrawer;
        private IFieldDrawer<string> _dirDrawer;

        public CreateServerInstanceWindow() : base("Create Server Instance", "CreateServerInstanceWindow")
        {
            // Center the window when it appears
            ImGui.SetNextWindowPos(new System.Numerics.Vector2(ImGui.GetIO().DisplaySize.X * 0.5f, ImGui.GetIO().DisplaySize.Y * 0.5f), ImGuiCond.Appearing, new System.Numerics.Vector2(0.5f, 0.5f));

            _nameDrawer = (IFieldDrawer<string>)FieldDrawerFactory.Create().FromType(typeof(string)).Label("Name").Build();
            _nameDrawer.value = "New Server";
            hierarchy.Add((VisualElement)_nameDrawer);

            _dirDrawer = (IFieldDrawer<string>)FieldDrawerFactory.Create().FromType(typeof(string)).Label("Directory").Build();
            _dirDrawer.value = "%Auto%";
            // We add the directory drawer manually in OnRenderSelf so we can place the button next to it
        }

        protected override void OnRenderSelf()
        {
            // The _nameDrawer is rendered automatically since it was added to the hierarchy.

            ImGui.BeginGroup();

            ((VisualElement)_dirDrawer).Render();

            if (ImGui.Button("Choose Folder"))
            {
                string? path = DialogUtils.OpenFolderDialog();
                if (!string.IsNullOrEmpty(path))
                {
                    _dirDrawer.value = path;
                }
            }

            ImGui.SameLine();
            if (ImGui.Button("OK"))
            {
                var kernel = ServerManager.Instance.CreateNewInstance(_nameDrawer.value, _dirDrawer.value);
                KernelContext.Current = kernel;
                Close();
            }

            ImGui.SameLine();
            if (ImGui.Button("Cancel"))
            {
                Close();
            }

            ImGui.EndGroup();
        }
    }
}
