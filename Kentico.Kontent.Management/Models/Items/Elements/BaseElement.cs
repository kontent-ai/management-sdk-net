using System;
using System.Dynamic;
using Kentico.Kontent.Management.Models.Items;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kentico.Kontent.Management.Models.Items.Elements
{
    public abstract class BaseElement
    {
        // TODO Is there a way to force the inherited classes to implement constructor wit dynamic parameter?
        public abstract dynamic ToDynamic(string elementId);
    }
}