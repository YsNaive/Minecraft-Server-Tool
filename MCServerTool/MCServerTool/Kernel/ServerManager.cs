using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ImTK.Core;
using MCServerTool.Data;

namespace MCServerTool.Kernel
{
    public class ServerManager
    {
        public static ServerManager Instance { get; } = new ServerManager();

        private readonly string _instancesDirectory;

        public List<KernelContext> Kernels { get; private set; } = new List<KernelContext>();

        private ServerManager()
        {
            _instancesDirectory = Path.Combine(ImTKEnvironment.LocalDataPath, "server_instances");
        }

        public void Initialize()
        {
            if (!Directory.Exists(_instancesDirectory))
            {
                Directory.CreateDirectory(_instancesDirectory);
            }

            LoadInstances();
        }

        public void LoadInstances()
        {
            Kernels.Clear();
            var files = Directory.GetFiles(_instancesDirectory, "*.json");

            foreach (var file in files)
            {
                try
                {
                    string json = File.ReadAllText(file);
                    var instance = JsonSerializer.Deserialize<ServerInstance>(json);
                    if (instance != null)
                    {
                        Kernels.Add(new KernelContext(instance));
                    }
                }
                catch (Exception ex)
                {
                    // Basic error handling for now
                    Console.WriteLine($"Failed to load instance from {file}: {ex.Message}");
                }
            }

            if (Kernels.Count > 0 && KernelContext.Current == null)
            {
                KernelContext.Current = Kernels[0];
            }
        }

        public void SaveInstance(ServerInstance instance)
        {
            string file = Path.Combine(_instancesDirectory, $"{instance.Id}.json");
            string json = JsonSerializer.Serialize(instance, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(file, json);
        }

        public KernelContext CreateNewInstance()
        {
            var instance = new ServerInstance();
            // Default working directory to a subfolder based on id
            instance.WorkingDirectory = Path.Combine(ImTKEnvironment.LocalDataPath, "servers", instance.Id);

            SaveInstance(instance);

            var kernel = new KernelContext(instance);
            Kernels.Add(kernel);
            return kernel;
        }
    }
}
