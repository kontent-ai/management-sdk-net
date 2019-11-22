using System.Reflection;
using System.Net.Http;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Kentico.Kontent.Management.Tests.Mocks
{
    class IgnoreHttpContentContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty prop = base.CreateProperty(member, memberSerialization);

            // HttpContent is an abtract class that cannot be deserialized directly, we handle that separately 
            if (prop.PropertyType == typeof(HttpContent))
            {
                prop.ShouldDeserialize = obj => false;
            }

            return prop;
        }
    }
}
