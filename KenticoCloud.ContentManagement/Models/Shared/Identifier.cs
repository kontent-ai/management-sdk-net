using System;

namespace KenticoCloud.ContentManagement.Models.Identifiers
{
    public class Identifier<T>
        where T : Identifier<T>, new()
    {
        public Guid Id { get; private set; }

        public string Codename { get; private set; }

        public static T ById(Guid id)
        {
            return new T() { Id = id };
        }

        public static T ByCodename(string codename)
        {
            return new T() { Codename = codename };
        }
    }
}
