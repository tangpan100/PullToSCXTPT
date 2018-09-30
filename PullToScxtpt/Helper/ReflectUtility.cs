using System;
using System.ComponentModel;
using System.Reflection;

namespace PullToScxtpt.Helper
{
    public static class ReflectUtility
    {
        /// <summary>
        /// 获取实体相关属性的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetInstanceValue(object obj, string propertyName)
        {
            object objRet = null;

            if (string.IsNullOrEmpty(propertyName) == false)
            {
                PropertyDescriptor descriptor = TypeDescriptor.GetProperties(obj).Find(propertyName, true);

                if (descriptor != null)
                {
                    objRet = descriptor.GetValue(obj);
                }
            }

            return objRet;
        }

        #region 属性字段设置

        public static BindingFlags bf = BindingFlags.Public |
                                         BindingFlags.NonPublic |
                                         BindingFlags.Instance |
                                         BindingFlags.Static;
        //BindingFlags.DeclaredOnly;

        public static object InvokeMethod(object obj, string methodName, object[] args)
        {
            object objReturn = null;
            Type type = obj.GetType();
            objReturn = type.InvokeMember(methodName, bf | BindingFlags.InvokeMethod, null, obj, args);
            return objReturn;
        }

        public static void SetField(object obj, string name, object value)
        {
            FieldInfo fi = obj.GetType().GetField(name, bf);
            fi.SetValue(obj, value);
        }

        public static object GetField(object obj, string name)
        {
            FieldInfo fi = obj.GetType().GetField(name, bf);
            return fi.GetValue(obj);
        }

        public static FieldInfo[] GetFields(object obj)
        {
            FieldInfo[] fieldInfos = obj.GetType().GetFields(bf);
            return fieldInfos;
        }

        public static void SetProperty(object obj, string name, object value)
        {
            PropertyInfo fieldInfo = obj.GetType().GetProperty(name, bf);
            value = Convert.ChangeType(value, fieldInfo.PropertyType);
            fieldInfo.SetValue(obj, value, null);
        }

        public static object GetProperty(object obj, string name)
        {
            PropertyInfo fieldInfo = obj.GetType().GetProperty(name, bf);
            return fieldInfo.GetValue(obj, null);
        }

        public static PropertyInfo[] GetProperties(object obj)
        {
            PropertyInfo[] propertyInfos = obj.GetType().GetProperties(bf);
            return propertyInfos;
        }

        #endregion 属性字段设置

        #region 获取Description

        public static string GetDescription(Enum value)
        {
            return GetDescription(value, null);
        }

        public static string GetDescription(Enum value, params object[] args)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            string text1;

            FieldInfo fi = value.GetType().GetField(value.ToString());

            var attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            text1 = (attributes.Length > 0) ? attributes[0].Description : value.ToString();

            if ((args != null) && (args.Length > 0))
            {
                return string.Format(null, text1, args);
            }
            return text1;
        }

        public static string GetDescription(MemberInfo member)
        {
            return GetDescription(member, null);
        }

        public static string GetDescription(MemberInfo member, params object[] args)
        {
            string text1;

            if (member == null)
            {
                throw new ArgumentNullException("member");
            }

            if (member.IsDefined(typeof(DescriptionAttribute), false))
            {
                var attributes =
                    (DescriptionAttribute[])member.GetCustomAttributes(typeof(DescriptionAttribute), false);
                text1 = attributes[0].Description;
            }
            else
            {
                return String.Empty;
            }

            if ((args != null) && (args.Length > 0))
            {
                return String.Format(null, text1, args);
            }
            return text1;
        }

        #endregion 获取Description

        #region 获取Attribute信息

        public static object GetAttribute(Type attributeType, Assembly assembly)
        {
            if (attributeType == null)
            {
                throw new ArgumentNullException("attributeType");
            }

            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            if (assembly.IsDefined(attributeType, false))
            {
                object[] attributes = assembly.GetCustomAttributes(attributeType, false);

                return attributes[0];
            }

            return null;
        }

        public static object GetAttribute(Type attributeType, MemberInfo type)
        {
            return GetAttribute(attributeType, type, false);
        }

        public static object GetAttribute(Type attributeType, MemberInfo type, bool searchParent)
        {
            if (attributeType == null)
            {
                return null;
            }

            if (type == null)
            {
                return null;
            }

            if (!(attributeType.IsSubclassOf(typeof(Attribute))))
            {
                return null;
            }

            if (type.IsDefined(attributeType, searchParent))
            {
                object[] attributes = type.GetCustomAttributes(attributeType, searchParent);

                if (attributes.Length > 0)
                {
                    return attributes[0];
                }
            }

            return null;
        }

        public static object[] GetAttributes(Type attributeType, MemberInfo type)
        {
            return GetAttributes(attributeType, type, false);
        }

        public static object[] GetAttributes(Type attributeType, MemberInfo type, bool searchParent)
        {
            if (type == null)
            {
                return null;
            }

            if (attributeType == null)
            {
                return null;
            }

            if (!(attributeType.IsSubclassOf(typeof(Attribute))))
            {
                return null;
            }

            if (type.IsDefined(attributeType, false))
            {
                return type.GetCustomAttributes(attributeType, searchParent);
            }

            return null;
        }

        #endregion 获取Attribute信息

        #region 创建对应实例

        /// <summary>
        /// 创建对应实例
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>对应实例</returns>
        public static object CreateInstance(string type)
        {
            Type tmp = null;
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                tmp = assemblies[i].GetType(type);
                if (tmp != null)
                {
                    return assemblies[i].CreateInstance(type);
                }
            }
            return null;
            //return Assembly.GetExecutingAssembly().CreateInstance(type);
        }

        /// <summary>
        /// 创建对应实例
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>对应实例</returns>
        public static object CreateInstance(Type type)
        {
            return CreateInstance(type.FullName);
        }

        #endregion 创建对应实例
    }
}