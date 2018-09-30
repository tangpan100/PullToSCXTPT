using System.Collections;
using System.Reflection;
using System.Web.Services.Protocols;

namespace PullToScxtpt.Helper
{
    /// <summary>
    /// 通用反射泛型类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Reflect<T> where T : class
    {
        private static Hashtable m_objCache = null;

        /// <summary>
        ///
        /// </summary>
        public static Hashtable ObjCache
        {
            get
            {
                if (m_objCache == null)
                {
                    m_objCache = new Hashtable();
                }
                return m_objCache;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="assemblyFullName"></param>
        /// <returns></returns>
        public static T CreateObjInstance(string typeName, string assemblyFullName)
        {
            return CreateObjInstance(typeName, assemblyFullName, true);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="assemblyFullName"></param>
        /// <param name="bCache">是否缓存</param>
        /// <returns></returns>
        public static T CreateObjInstance(string typeName, string assemblyFullName, bool bCache)
        {
            string CacheKey = assemblyFullName + "_" + typeName;
            T objType = null;
            if (bCache)
            {
                objType = (T)ObjCache[CacheKey];
                if (!ObjCache.ContainsKey(CacheKey))
                {
                    Assembly assObj = LoadAssemblyByCach(assemblyFullName);
                    object obj = assObj.CreateInstance(typeName);
                    objType = (T)obj;
                    ObjCache.Add(CacheKey, objType);
                }
                return objType;
            }
            objType = (T)LoadAssemblyByCach(assemblyFullName).CreateInstance(CacheKey);
            return objType;
        }

        /// <summary>
        /// 装载程序集
        /// </summary>
        /// <param name="assemblyFullName"></param>
        /// <returns></returns>
        private static Assembly LoadAssemblyByCach(string assemblyFullName)
        {
            Assembly assObj = (Assembly)ObjCache[assemblyFullName];
            if (assObj == null)
                assObj = Assembly.Load(assemblyFullName);
            return assObj;
        }
    }
}