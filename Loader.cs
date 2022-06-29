using System;
using System.Reflection;
using UmbraClient;
using UnityEngine;

namespace UmbraMenu
{
    public class Loader
    {
        public static GameObject gameObject;
        public static UmbraClient<UmbraMod> client;

        public static void Load()
        {
            while (gameObject = GameObject.Find("Umbra Mod"))
                UnityEngine.Object.Destroy(gameObject);
            
            gameObject = new GameObject("Umbra Mod");
            UmbraMod.instance = gameObject.AddComponent<UmbraMod>();
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
            
            LoadAssembly();

            while (true)
            {
                try
                {
                    client = new UmbraClient<UmbraMod>(UmbraMod.instance, 13370);
                    break;
                }
                catch
                {
                    // ignore
                }
            }
        }

        public static void Unload()
        {
            UnityEngine.Object.Destroy(gameObject);
        }

        private static void LoadAssembly()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                var resourceName = "UmbraMenu." +
                                   new AssemblyName(args.Name).Name + ".dll";

                using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            var assemblyData = new byte[stream.Length];
            stream.Read(assemblyData, 0, assemblyData.Length);
            return Assembly.Load(assemblyData);
            };
        }
    }
}
