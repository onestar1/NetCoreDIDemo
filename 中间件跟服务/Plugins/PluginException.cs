using System;
using System.Collections.Generic;
using System.Text;
using 中间件跟服务.MyException;

namespace 中间件跟服务.Plugins
{
    /// <summary>
    /// 插件异常
    /// </summary>
    public class PluginException : SuperException
    {
        public PluginException()
        {
            Log.Info(this.Message, this);
        }

        public PluginException(string message)
            : base(message)
        {
            Log.Info(message, this);
        }

        public PluginException(string message, Exception inner)
            : base(message, inner)
        {
            Log.Info(message, inner);
        }
    }
}
