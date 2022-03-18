using Kentico.Kontent.Management.Models.Types.Elements;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using Kentico.Kontent.Management.Models.AssetRenditions;

namespace Kentico.Kontent.Management.Modules.ActionInvoker
{
    internal class BaseSpecifiedConcreteClassConverter : DefaultContractResolver
    {
        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            if (!objectType.IsAbstract && IsKnownConcreteClassWithDefinedConverter(objectType))
            {
                return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
            }

            return base.ResolveContractConverter(objectType);
        }

        private static bool IsKnownConcreteClassWithDefinedConverter(Type objectType) =>
            typeof(ElementMetadataBase).IsAssignableFrom(objectType) ||
            typeof(ImageTransformation).IsAssignableFrom(objectType);
    }
}
