using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Netcore学习2._2.Models;
using Super.Core;
using Super.Core.Plugins;
 
namespace Netcore学习2._2.Controllers
{
    public class HomeController : Controller
    {
        
        public HomeController()
        {

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public string getname()
        {

            
            var testplugins = PluginsManagement.GetPlugins<ITestPlugin>(true);

            var testplugin= testplugins.FirstOrDefault(d => d.PluginInfo.PluginId == "plugins.test");
            return testplugin.Biz.getname();
        }
    }
}
