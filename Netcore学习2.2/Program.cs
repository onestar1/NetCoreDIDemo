using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Super.Core;
using Super.Core.Helper;
using Super.Core.MyException;
using Super.Core.Plugins;
 

namespace Netcore学习2._2
{
    public class Program
    {
        //public static void Main(string[] args)
        //{
        //    CreateWebHostBuilder(args).Build().Run();
          
        //}
        public static void Main(string[] args)
        {
            var host = WebHost.CreateDefaultBuilder(args)
                .UseKestrel(options => options.AddServerHeader = false)
                .UseStartup<Startup>()
                
                .Build();
           // RegistAtStart();
            host.Run();
            
        }


        /// <summary>
        /// 加载指定目录下所有dll
        /// </summary>
        /// <param name="pluginsDirectory"></param>
        public static void RegistAtStart()
        {

            string pluginsDirectory = PathHelper.GetApppath("/plugins");
            List<string> dllFiles = GetPluginFiles(pluginsDirectory).ToList();
            foreach (string dllFileName in dllFiles)//加载这些文件
            {
                Assembly assembly = InstallDll(dllFileName);
                //将程序集添加到当前应用程序域
                //  BuildManager.AddReferencedAssembly(assembly);
            }

        }
        /// <summary>
        /// 获取插件程序集文件
        /// </summary>
        /// <param name="pluginDirectory">插件所在目录</param>
        /// <returns></returns>
        static IEnumerable<string> GetPluginFiles(string pluginDirectory)
        {
            if (!System.IO.Directory.Exists(pluginDirectory))
                throw new SuperException("未能找到指定的插件目录:" + pluginDirectory);

            //搜索当前目录(包括子目录)下所有dll文件
            string[] dllFiles = System.IO.Directory.GetFiles(pluginDirectory, "*.dll", System.IO.SearchOption.AllDirectories);
            return dllFiles;
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      
        static Assembly InstallDll(string dllFileName)
        {
            string newFileName = dllFileName;
            FileInfo fileInfo = new FileInfo(dllFileName);
            DirectoryInfo copyFolder;

            if (!string.IsNullOrWhiteSpace(PathHelper.ApplicationPath))
            {
                //获取asp.net dll运行目录
                // AppDomain.CurrentDomain.BaseDirectory;
                copyFolder = new DirectoryInfo(PathHelper.ApplicationPath);
            }
            else
            {
                var directorypath = PathHelper.GetApppath("/pluginscopy");
                if (!Directory.Exists(directorypath))
                {
                    Directory.CreateDirectory(directorypath);
                } 

                copyFolder = new DirectoryInfo(directorypath);

            }
            newFileName = copyFolder.FullName + "\\" + fileInfo.Name;

            Assembly assembly = null;
            PluginInfo pluginfo = null;
            try
            {
                try
                {
                    System.IO.File.Copy(dllFileName, newFileName, true);
                }
                catch
                {
                    //在某些情况下会出现"正由另一进程使用，因此该进程无法访问该文件"错误，所以先重命名再复制
                    File.Move(newFileName, newFileName + Guid.NewGuid().ToString("N") + ".locked");
                    System.IO.File.Copy(dllFileName, newFileName, true);
                }
                //  var assemblyname = AssemblyName.GetAssemblyName();
                //  assembly = Assembly.LoadFile(newFileName);//改成用文件进行加载
                //排除以 Super.Plugin 开头的，非插件的dll，主要是插件基类
                //if (assembly.FullName.StartsWith("Super.Plugin") && !assembly.FullName.Contains("Super.Plugin.Payment.Alipay.Base"))
                //{
                // assembly= AssemblyLoadContext.Default.LoadFromAssemblyPath( newFileName));

                assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(newFileName);
              
                var classfullname = assembly.GetTypes()[0].FullName + "," + assembly.GetName().Name;
                pluginfo = AddPluginInfoTest(fileInfo);//添加插件信息

                var result = Type.GetType(pluginfo.ClassFullName);
                var type = assembly.GetType();
                foreach (var item in assembly.GetTypes())
                {
                    if (typeof(IPlugin).IsAssignableFrom(item))
                    {
                        type = item;
                        break;
                    }
                }
                //var name = type.FullName;
                //var testtype = typeof(name);
                //向插件注入信息
                IPlugin plugin = Instance.Get<IPlugin>(type);
                //实例化 ：  classfullname ：类的完整名称xxx.xx,程序集名称
                plugin.WorkDirectory = fileInfo.Directory.FullName;

                // }
            }
            catch (IOException ex)
            {
                Log.Error("插件复制失败(" + dllFileName + ")！", ex);
                //   if (pluginfo != null)//插件复制失败时，移除插件安装信息
                //    RemovePlugin(pluginfo);
            }
            catch (Exception ex)
            {
                Log.Error("插件加载失败(" + dllFileName + ")！", ex);
                //  if (pluginfo != null)//插件加载失败时，移除插件安装信息
                //RemovePlugin(pluginfo);
            }
            return assembly;
        }
        /// <summary>
        /// 测试添加插件 不需要配置文件
        /// </summary>
        /// <param name="dllFile"></param>
        /// <returns></returns>
        static PluginInfo AddPluginInfoTest(FileInfo dllFile)
        {
            PluginInfo pluginInfo;
            string pluginId = dllFile.Name.Replace(".dll", "");
            string installedConfigPath = PathHelper.GetApppath("/plugins/configs/") + pluginId + ".config";
            string webPath = "/plugins/configs/" + pluginId + ".config";
            if (!FileHelper.Exists(PathHelper.GetApppath(webPath)))//检查是否已经安装过
            {//未安装过

                //查找插件自带的配置文件
                FileInfo[] configFiles = dllFile.Directory.GetFiles("plugin.config", SearchOption.TopDirectoryOnly);
                if (configFiles.Length > 0)
                {
                    //读取插件自带的配置信息
                    pluginInfo = (PluginInfo)XmlHelper.DeserializeFromXML(typeof(PluginInfo), configFiles[0].FullName);

                    //使用程序集名称为插件唯一标识
                    pluginInfo.PluginId = pluginId;

                    //记录插件所在目录
                    pluginInfo.PluginDirectory = dllFile.Directory.FullName;

                    //更新插件时间
                    pluginInfo.AddedTime = DateTime.Now;

                    //序列化,将插件信息保存到系统插件配置文件中
                    XmlHelper.SerializeToXml(pluginInfo, installedConfigPath);
                }
                else
                    throw new FileNotFoundException("未找到插件" + pluginId + "的配置文件");
            }
            else
            {//读取系统插件配置文件中的配置信息
                pluginInfo = (PluginInfo)XmlHelper.DeserializeFromXML(typeof(PluginInfo), installedConfigPath);
            }

            //将插件信息保存至内存插件列表中
            //   UpdatePluginList(pluginInfo);

            return pluginInfo;
        }
        /// <summary>
        /// 已安装插件
        /// </summary>
        static Dictionary<PluginType, List<PluginInfo>> IntalledPlugins = new Dictionary<PluginType, List<PluginInfo>>();//此处可
        /// <summary>
        /// 更新插件列表
        /// </summary>
        /// <param name="plugin"></param>
        static void UpdatePluginList(PluginInfo plugin)
        {
            foreach (var pluginType in plugin.PluginTypes)
                IntalledPlugins[pluginType].Add(plugin);
        }
        //public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .UseStartup<Startup>();


    }
}
