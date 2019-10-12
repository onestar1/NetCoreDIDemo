using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.IO;
using Super.Core.Extention;
using Super.Core.Helper;
using Microsoft.Extensions.Hosting.Internal;

namespace Super.Core
{
    public static class PathHelper
    {
        /// <summary>
        /// 获取Url
        /// </summary>
        /// <param name="virtualUrl">虚拟Url</param>
        /// <returns></returns>
        public static string GetUrl(string virtualUrl)
        {
            if (!virtualUrl.IsNullOrEmpty())
            {
                UrlHelper urlHelper = new UrlHelper(AutofacHelper.GetScopeService<IActionContextAccessor>().ActionContext);

                return urlHelper.Content(virtualUrl);
            }
            else
                return null;
        }

        /// <summary>
        /// 获取绝对路径
        /// </summary>
        /// <param name="virtualPath">虚拟路径</param>
        /// <returns></returns>
        public static string GetAbsolutePath(string virtualPath)
        {
            string path = virtualPath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            if (path[0] == '~')
                path = path.Remove(0, 2);
            string rootPath = ApplicationPath;//AutofacHelper.GetScopeService<HostingEnvironment>().WebRootPath;
            
            var result =  Path.Combine(rootPath, path);
            if (path.StartsWith("/"))
            {
                result = rootPath + path;
            }
            return result;
        }

        /// <summary>
        /// 获取当前目录
        /// （网站为网站根目录，测试时为dll所在目录）
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetMapPath(string path)
        {
            string contentRootPath = ApplicationPath.Substring(0, ApplicationPath.LastIndexOf("bin"));// AutofacHelper.GetScopeService<IHostingEnvironment>().ContentRootPath;
            if (!string.IsNullOrWhiteSpace(path))
            {
                path = path.Replace("/", "\\");
                if (!path.StartsWith("\\"))
                    path = "\\" + path;
                path = path.Substring(path.IndexOf('\\') + (contentRootPath.EndsWith("\\") ? 1 : 0));
            }
            return contentRootPath + path;

        }
        ///// <summary>
        ///// 获取动态程序集路径
        ///// </summary>
        ///// <returns></returns>
        //public static string GetApppath()
        //{
        //    return System.Reflection.Assembly.GetExecutingAssembly().Location;
        //}
        /// <summary>
        /// 获取程序集路径
        /// </summary>
        public static string ApplicationPath
        {
            get
            {
                if (String.IsNullOrEmpty(AppDomain.CurrentDomain.RelativeSearchPath))
                {
                    return AppDomain.CurrentDomain.BaseDirectory; //exe folder for WinForms, Consoles, Windows Services
                }
                else
                {
                    return AppDomain.CurrentDomain.RelativeSearchPath; //bin folder for Web Apps 
                }
            }
        }

        public  static string GetApppath(string path="")
        {
          
                string contentRootPath = ApplicationPath.Substring(0, ApplicationPath.LastIndexOf("bin"));
                if (!string.IsNullOrWhiteSpace(path))
                {
                    path = path.Replace("/", "\\");
                    if (!path.StartsWith("\\"))
                        path = "\\" + path;
                    path = path.Substring(path.IndexOf('\\') + (contentRootPath.EndsWith("\\") ? 1 : 0));
                }
                return contentRootPath + path;
           
        }
    }
}
