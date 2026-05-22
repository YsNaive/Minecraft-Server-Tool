using System;
using ImTK.Core;
using ImTK.Silk;

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

            ImTKSilk.Run(config);

            log.Info("MCServerTool Closed gracefully.");
        }
    }
}
