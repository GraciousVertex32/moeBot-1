﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using moeInterface;

namespace Plugins
{
    class Plugin
    {
        // Let info.json Load in
        // Parse the info.json


        /********************************************************************
         * {
         *   "name": "Example Plugin",
         *   "id": "expl",
         *   "alternatedid": "explP",
         *   "version": "1.0.0",
         *   "compatible-version": "(1.0.0",
         *   "description": "This is A Example Plugin Created by Your Cat.",
         *   "contributors": ["Cat", "Neko"],
         *   "credits": ,
         *   "root": "./example",
         *   "config": "./example/config",
         *   "repository":
         *   {
         *     "type": "git",
         *     "url": "https://github.com/example/moeBot-plugin-example"
         *   },
         * }
         ********************************************************************/


        // name
        // id
        // 
        public static void LoadIn()
        {
            Loader l = new Loader();
            l.Plugin();

        }
    }
    class Loader
    {
        public void Plugin()
        {
            string PluginName = null;

            Dictionary<string, IPlugin> _Plugins = new Dictionary<string, IPlugin>();
            ICollection<IPlugin> plugins = Load();
            Console.WriteLine("All plugins loaded.");

            foreach (var plugin in plugins)
            {
                _Plugins.Add(plugin.Name, plugin);
                PluginName = plugin.Name;
                Console.WriteLine(PluginName);
            }

            Console.WriteLine(_Plugins[PluginName].Do());
        }

        private ICollection<IPlugin> Load()
        {
            Console.WriteLine("Initializing Plugins...");
            List<string> dllFileNames = PathFinder();
            ICollection<Assembly> assemblies = new List<Assembly>(PathFinder().Count);

            foreach (string dllFile in dllFileNames)
            {
                AssemblyName an = AssemblyName.GetAssemblyName(dllFile);
                Console.WriteLine(an);
                Assembly assembly = Assembly.Load(an);
                assemblies.Add(assembly);
            }

            Type pluginType = typeof(IPlugin);
            ICollection<Type> pluginTypes = new List<Type>();
            foreach (Assembly assembly in assemblies)
            {
                if (assembly != null)
                {
                    Type[] types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        if (type.IsInterface || type.IsAbstract)
                        {
                            continue;
                        }
                        else
                        {
                            if (type.GetInterface(pluginType.FullName) != null)
                            {
                                pluginTypes.Add(type);
                                Console.WriteLine("Added " + type + " type.");
                            }
                        }
                    }
                }
            }

            ICollection<IPlugin> plugins = new List<IPlugin>(pluginTypes.Count);
            foreach (Type type in pluginTypes)
            {
                IPlugin plugin = (IPlugin)Activator.CreateInstance(type);
                plugins.Add(plugin);
            }
            return plugins;
        }
        
        //Find Path
        
        private List<string> PathFinder()
        {
            List<string> pluginpath = new List<string>();
            try
            {
                //获取程序的基目录
                string path = AppDomain.CurrentDomain.BaseDirectory;
                //合并路径，指向插件所在目录。
                path = Path.Combine(path, "Plugins");
                foreach (string filename in Directory.GetFiles(path, "*.dll"))
                {
                    pluginpath.Add(filename);
                    Console.WriteLine("Added " + filename);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return pluginpath;
        }

        //private 
    }

    class Initializer
    {
        public static void PreInitialization()
        {
            //Todo
        }
        public static void Initialization()
        {
            //Todo
        }
    }
}
 