using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;

namespace StackExchange.Profiling.Helpers
{
    /// <summary>
    /// Provides a converter for IReadOnlyDictionary{string, T}.
    /// </summary>
    internal class ReadOnlyDictionaryConverter<T> : JavaScriptConverter
    {
        public override IEnumerable<Type> SupportedTypes
        {
            get
            {
                return new[] { typeof(IReadOnlyDictionary<string, T>) };
            }
        }

        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            // return serializer.ConvertToType<Dictionary<string, T>>(dictionary);

            return dictionary.ToDictionary(p => p.Key, p => serializer.ConvertToType<T>(p.Value));
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            return (IDictionary<string, object>)obj;
        }
    }
}
