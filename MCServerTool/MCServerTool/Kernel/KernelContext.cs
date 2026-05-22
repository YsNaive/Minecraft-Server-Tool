using System;
using System.Diagnostics;
using MCServerTool.Data;

namespace MCServerTool.Kernel
{
    public class KernelContext
    {
        public static KernelContext? Current { get; set; }

        public ServerInstance Instance { get; private set; }

        public bool IsRunning { get; private set; }

        public KernelContext(ServerInstance instance)
        {
            Instance = instance;
            IsRunning = false;
        }

        // Methods to start, stop, send commands will be implemented later
        public void Start()
        {
            // TODO: Implement process start
            IsRunning = true;
        }

        public void Stop()
        {
            // TODO: Implement process stop
            IsRunning = false;
        }
    }
}
