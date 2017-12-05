using System;

using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Identifiers
{
    public abstract class Identifier<T>
        where T : Identifier<T>, new()
    {
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid Id { get; private set; }

        [JsonProperty("codename", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Codename { get; private set; }

        public static T ById(Guid id)
        {
            return new T() { Id = id };
        }

        public static T ByCodename(string codename)
        {
            return new T() { Codename = codename };
        }

        protected Identifier() { }
    }
}
