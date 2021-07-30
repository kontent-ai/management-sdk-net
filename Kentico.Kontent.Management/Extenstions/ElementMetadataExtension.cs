using Kentico.Kontent.Management.Models.Types.Elements;
using System;

namespace Kentico.Kontent.Management.Extenstions
{
    public static class ElementMetadataExtension
    {
        public static TextElementMetadataModel ToTextElement(this ElementMetadataBase source)
        {
            if(source.Type != ElementMetadataType.Text) 
            {
                throw new InvalidOperationException($"type {source.Type} cannot be converted to {nameof(TextElementMetadataModel)}");
            }

            return source as TextElementMetadataModel;
        }
    }
}
