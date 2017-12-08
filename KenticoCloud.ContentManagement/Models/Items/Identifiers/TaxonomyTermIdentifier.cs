using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Items
{
    public class TaxonomyTermIdentifier
    {
        /// <summary>
        /// Gets codename of the identifier.
        /// </summary>
        [JsonProperty("codename", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Codename { get; private set; }


        private TaxonomyTermIdentifier()
        {
        }
        
        /// <summary>
        /// Creates identifier by codename.
        /// </summary>
        /// <param name="codename">The codename of the identifier.</param>
        public static TaxonomyTermIdentifier ByCodename(string codename)
        {
            return new TaxonomyTermIdentifier() { Codename = codename };
        }
    }
}
