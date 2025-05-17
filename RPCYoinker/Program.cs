using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using dnlib.DotNet;

namespace RPCYoinker
{
    internal class Program
    {
        private static (string assemblyPath, string outputPath) GetPaths(string[] args)
        {
            if (args.Length >= 2)
                return (args[0], args[1]);

            string assemblyPath = Logger.ReadLine("Assembly path").Trim('"');
            string outputPath = Logger.ReadLine("Output path (default: output)", "output").Trim('"');

            return (assemblyPath, outputPath);
        }

        public static void Main(string[] args)
        {
            var (assemblyPath, outputPath) = GetPaths(args);

            if (!File.Exists(assemblyPath))
                Logger.Exit(Logger.LogLevel.Error);

            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);

            try
            {
                ModuleDefMD moduleDef = ModuleDefMD.Load(assemblyPath);
                Logger.Log("Loaded assembly", Logger.LogLevel.Success);

                List<RpcInfo> rpcInfos = moduleDef.Types.SelectMany(t => t.Methods
                    .Where(m => m.HasAttribute("PunRPC", "Rpc", "RpcAttribute")).Select(m => new RpcInfo
                    {
                        TypeName = t.FullName,
                        MethodName = m.Name,
                        Parameters = m.Parameters.Skip(1).Select(x => $"{x.Type.FullName} {x.Name}").ToList()
                    })).ToList();

                foreach (RpcInfo rpcInfo in rpcInfos)
                    Logger.Log(rpcInfo.ToString(), Logger.LogLevel.Success);

                Logger.Log($"Found {rpcInfos.Count} Rpcs");
                string outputFile = Path.Combine(outputPath, "output.txt");

                if (File.Exists(outputFile))
                    File.Delete(outputFile);

                var watermark = new[]
                {
                    "// RPCYoinker",
                    "// discord.gg/vMaZCngYpB",
                    $"// Assembly: {moduleDef.Assembly.FullName}",
                    $"// {new string('-', 50)}"
                };

                File.WriteAllLines(outputFile, watermark.Concat(rpcInfos.Select(x => x.ToString())));
                Logger.Log($"Rpcs wrote to file {outputFile}", Logger.LogLevel.Success);
            }
            catch (Exception ex)
            {
                Logger.Log($"Error: {ex}", Logger.LogLevel.Error);
            }

            Logger.Exit();
        }
    }
}