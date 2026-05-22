using System;
using ImTK.Core;
using ImTK.Silk;
using ImTK.UI;
using MCServerTool.Kernel;
using MCServerTool.UI;

namespace MCServerTool
{
    class Program
    {
        static void Main(string[] args)
        {
            var log = new ImTK.Log.LogContext("MCServerTool");
            log.Info("Starting MCServerTool Application...");

            ImTKEnvironment.OrganizationName = "MCServerTool";
            ImTKEnvironment.ApplicationName = "MCServerTool";

            var config = new ImTKSilkConstant
            {
                windowTitle = "MCServerTool",
                configFolderPath = ImTKEnvironment.LocalDataPath,
                windowWidth = 1280,
                windowHeight = 800
            };

            // Initialize managers
            ServerManager.Instance.Initialize();

            // Open base windows before running the app loop
            Window.Open<ServerListWindow>();
            Window.Open<ServerInfoWindow>();
            Window.Open<ConsoleWindow>();

            ImTKSilk.Run(config);

            log.Info("MCServerTool Closed gracefully.");
        }
    }
}
