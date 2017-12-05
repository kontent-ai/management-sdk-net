using System;

using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Identifiers
{
    public abstract class Identifier<T>
        where T : Identifier<T>, new()
    {
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid? Id { get; private set; }

    

        public static T ById(Guid id)
        {
            return new T() { Id = id };
        }



        protected Identifier() { }
    }
}
