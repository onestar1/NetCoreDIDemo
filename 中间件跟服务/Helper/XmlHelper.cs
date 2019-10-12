using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace 中间件跟服务.Helper
{
    /// <summary>
    /// XML文档操作帮助类
    /// </summary>
    public class XmlHelper
    {
        /// <summary>
        /// 序列化为XML字符串
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            Type type = obj.GetType();
            MemoryStream Stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(type);
            try
            {
                //序列化对象
                xml.Serialize(Stream, obj);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            Stream.Position = 0;
            StreamReader sr = new StreamReader(Stream);
            string str = sr.ReadToEnd();

            sr.Dispose();
            Stream.Dispose();

            return str;
        }
        #region  XML序列化

        /// <summary>
        /// XML序列化
        /// </summary>
        /// <param name="obj">序列对象</param>
        /// <param name="filePath">XML文件路径</param>
        /// <returns>是否成功</returns>
        public static bool SerializeToXml(object obj, string filePath)
        {
            bool result = false;

            FileStream fs = null;
            try
            {
                fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(fs, obj);

                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return result;

        }

        /// <summary>
        /// XML反序列化
        /// </summary>
        /// <param name="type">目标类型(Type类型)</param>
        /// <param name="filePath">XML文件路径</param>
        /// <returns>序列对象</returns>
        public static object DeserializeFromXML(Type type, string filePath)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(type);
                return serializer.Deserialize(fs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }
        #endregion


        #region  XML序列化 OSS策略

        ///// <summary>
        ///// XML序列化
        ///// </summary>
        ///// <param name="obj">序列对象</param>
        ///// <param name="filePath">XML文件路径</param>
        ///// <returns>是否成功</returns>
        //public static bool SerializeToXmlByOSS(object obj, string filePath)
        //{
        //    bool result = false;

        //    try
        //    {
        //        string sDirectory = IOHelper.urlToVirtual(filePath);
        //        XmlSerializer xml = new XmlSerializer(obj.GetType());
        //        MemoryStream Stream = new MemoryStream();
        //        xml.Serialize(Stream, obj);

        //        byte[] b = Stream.ToArray();
        //        MemoryStream stream2 = new MemoryStream(b);
        //        HimallIO.CreateFile(sDirectory, stream2, Core.FileCreateType.Create);

        //        result = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return result;

        //}

        ///// <summary>
        ///// XML反序列化
        ///// </summary>
        ///// <param name="type">目标类型(Type类型)</param>
        ///// <param name="filePath">XML文件路径</param>
        ///// <returns>序列对象</returns>
        //public static object DeserializeFromXMLByOSS(Type type, string filePath)
        //{
        //    try
        //    {
        //        string sDirectory = IOHelper.urlToVirtual(filePath);
        //        if (HimallIO.ExistFile(sDirectory))
        //        {
        //            XmlSerializer xs = new XmlSerializer(type);
        //            byte[] b = HimallIO.GetFileContent(sDirectory);
        //            string str = System.Text.Encoding.Default.GetString(b);
        //            MemoryStream fs = new MemoryStream(b);
        //            return xs.Deserialize(fs);
        //        }
        //        else return DeserializeFromXML(type, filePath);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion
    }
}
