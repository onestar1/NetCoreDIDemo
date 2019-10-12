using System;
using System.Collections.Generic;
using System.Text;
using Super.Core.MyException;

namespace Super.Core
{
   public class Instance
    {
        public static T Get<T>(string classFullName)
        {
            try
            {
                 Type sourceType =   Type.GetType(classFullName);
                if (sourceType == null)
                {

                    return default(T);
                }
                else
                {
                    return (T)Activator.CreateInstance(sourceType);
                }
            }
            catch (Exception ex)
            {
                throw new SuperException("创建实例异常", ex);
            }
        }
        public static T Get<T>(Type type)
        {
            try
            {
                
                    return (T)Activator.CreateInstance(type);
               
            }
            catch (Exception ex)
            {
                throw new SuperException("创建实例异常", ex);
            }
        }

    }
}
