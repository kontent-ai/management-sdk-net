using System;
using Kentico.Kontent.Management.Models.Items;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kentico.Kontent.Management.Models.Items.Elements
{
    public abstract class BaseElement
    {
        public BaseElement(JToken data = null) { }

        public abstract dynamic ToDynamic(string elementId);
    }
}