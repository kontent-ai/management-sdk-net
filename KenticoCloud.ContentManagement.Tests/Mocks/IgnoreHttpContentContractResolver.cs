using System.Reflection;
using System.Net.Http;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace KenticoCloud.ContentManagement.Tests.Mocks
{
    class IgnoreHttpContentContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty prop = base.CreateProperty(member, memberSerialization);

            if (prop.PropertyType == typeof(HttpContent))
            {
                prop.ShouldDeserialize = obj => false;
            }

            return prop;
        }
    }
}
