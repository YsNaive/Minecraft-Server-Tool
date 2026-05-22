using System;
using ImTK.UI;

namespace MCServerTool.UI
{
    public static class MenuItems
    {
        [MainMenu("一般/新增實例 (Server)")]
        public static void AddServerInstance()
        {
            Window.Open<CreateServerInstanceWindow>();
        }
    }
}
