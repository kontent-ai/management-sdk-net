using Kentico.Kontent.Management.Models.Types.Elements;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace Kentico.Kontent.Management.Modules.ActionInvoker
{
    internal class BaseSpecifiedConcreteClassConverter : DefaultContractResolver
    {
        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            if (typeof(ElementMetadataBase).IsAssignableFrom(objectType) && !objectType.IsAbstract)
                return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
            return base.ResolveContractConverter(objectType);
        }
    }
}
