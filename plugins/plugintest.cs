using Super.Core;
using System;
 
namespace plugins
{
    public class plugintest : ITestPlugin
    {
        public string name = "这是测试插件";

        public   string WorkDirectory { set; get; }

        public void CheckCanEnable()
        {
            
        }

        public string getname()
        {
            return name;
        }
    }
}
