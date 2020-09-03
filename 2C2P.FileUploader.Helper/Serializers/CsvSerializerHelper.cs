using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using _2C2P.FileUploader.Helper.Attributes;

namespace _2C2P.FileUploader.Helper.Serializers
{
    public class CsvSerializerHelper
    {
        /// <summary>
        /// Deserialize csv
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <param name="returnValueSet"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string row, Func<string, string> returnValueSet = null) where T : class, new()
        {
            try
            {
                var result = default(T);
                if (!string.IsNullOrWhiteSpace(row))
                {
                    result = new T();
                    var regxPatternAttribute = result.GetType().GetCustomAttribute<CsvPatternAttribute>();
                    if (regxPatternAttribute != null && regxPatternAttribute is CsvPatternAttribute && !string.IsNullOrWhiteSpace(regxPatternAttribute.CsvRegxPattern))
                    {
                        var regex = new Regex(regxPatternAttribute.CsvRegxPattern);
                        var rowData = regex.Matches(row).ToArray();
                        var allproperties = result.GetType().GetProperties().Where(x => x.GetCustomAttribute<CsvIndexAttribute>() is CsvIndexAttribute);
                        foreach (var propInfoItem in allproperties)
                        {
                            var csvIndexAttribute = propInfoItem.GetCustomAttribute<CsvIndexAttribute>();
                            if (rowData.Length > csvIndexAttribute.Index)
                            {
                                var value = rowData.ToArray()[csvIndexAttribute.Index].Value;
                                if (!string.IsNullOrWhiteSpace(regxPatternAttribute.Ignore))
                                {
                                    value = value.Replace(regxPatternAttribute.Ignore, string.Empty);
                                }

                                propInfoItem.SetValue(result, value);
                            }
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
