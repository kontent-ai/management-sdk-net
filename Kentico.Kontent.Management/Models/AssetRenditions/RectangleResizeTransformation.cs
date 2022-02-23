using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.AssetRenditions
{
    public class RectangleResizeTransformation : ImageTransformation
    {
        [JsonProperty("mode", Required = Required.Always)]
        public override ImageTransformationMode Mode => ImageTransformationMode.Rect;

        [JsonProperty("fit", Required = Required.Always)]
        public ImageTransformationFit Fit { get; set; }

        [JsonProperty("custom_width", Required = Required.Always)]
        public int CustomWidth { get; set; }

        [JsonProperty("custom_height", Required = Required.Always)]
        public int CustomHeight { get; set; }

        [JsonProperty("x", Required = Required.Always)]
        public int X { get; set; }

        [JsonProperty("y", Required = Required.Always)]
        public int Y { get; set; }

        [JsonProperty("width", Required = Required.Always)]
        public int Width { get; set; }

        [JsonProperty("height", Required = Required.Always)]
        public int Height { get; set; }
    }
}