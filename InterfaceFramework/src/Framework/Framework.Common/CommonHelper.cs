using System;
using System.Collections.Generic;
using System.Text;
using Framework.Log;

namespace Framework.Common
{
    public class CommonHelper
    {
        /// <summary>
        /// 获取枚举特性描述信息
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetEnumDescription(System.Enum enumValue)
        {
            try
            {
                string str = enumValue.ToString();
                System.Reflection.FieldInfo field = enumValue.GetType().GetField(str);
                object[] objs = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if (objs == null || objs.Length == 0) return str;
                System.ComponentModel.DescriptionAttribute da = (System.ComponentModel.DescriptionAttribute)objs[0];
                return da.Description;
            }
            catch (Exception ex)
            {
                NLogHelper.Error(Guid.NewGuid().ToString(), ex.Message, "CommonHelper.GetEnumDescription()");
                return string.Empty;
            }
           
        }
    }
}
