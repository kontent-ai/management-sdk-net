using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kentico.Kontent.Management.Models.LanguageVariants;
using Kentico.Kontent.Management.Models.LanguageVariants.Elements;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Modules.Extensions;

namespace Kentico.Kontent.Management.Modules.ModelBuilders
{
    internal class ElementModelProvider : IElementModelProvider
    {
        public T GetStronglyTypedElements<T>(IEnumerable<dynamic> elements) where T : new()
        {
            var type = typeof(T);
            var instance = new T();

            var properties = type.GetProperties().Where(x => x.SetMethod?.IsPublic ?? false).ToList();

            foreach (var element in elements)
            {
                var property = properties.FirstOrDefault(x => x.PropertyType?.BaseType == typeof(BaseElement) && x.GetCustomAttribute<KontentElementIdAttribute>().ElementId == element.element.id);
                if (property == null)
                {
                    continue;
                }

                property.SetValue(instance, ToElement(element, property.PropertyType));
            }

            return instance;
        }

        public IEnumerable<dynamic> GetDynamicElements<T>(T stronglyTypedElements)
        {
            var type = typeof(T);

            var elements = type.GetProperties()
                .Where(x => (x.GetMethod?.IsPublic ?? false) && x.PropertyType?.BaseType == typeof(BaseElement) && x.GetValue(stronglyTypedElements) != null)
                .Select(x =>
                {
                    var element = (BaseElement)x.GetValue(stronglyTypedElements);
                    element.Element = Reference.ById(x.GetKontentElementId());
                    return element?.ToDynamic();
                });

            return elements;
        }

        private BaseElement ToElement(dynamic source, Type type)
        {
            if (type == typeof(TextElement))
            {
                return new TextElement
                {
                    // TODO extend by codename + external ID
                    // check with ondrej if really needed as it does not make sense
                    Element = Reference.ById(Guid.Parse(source?.element?.id)),
                    Value = source?.value,
                };
            }
            else if (type == typeof(NumberElement))
            {
                return new NumberElement
                {
                    Element = Reference.ById(Guid.Parse(source?.element?.id)),
                    Value = Convert.ToDecimal(source?.value),
                };
            }
            else if (type == typeof(RichTextElement))
            {
                var comp = (source?.components as IEnumerable<dynamic>)?.Select(component => new ComponentModel
                {
                    Id = Guid.Parse(component.id),
                    Type = Reference.ById(Guid.Parse(component.type.id)),
                    Elements = (component.elements as IEnumerable<dynamic>)
                });
                return new RichTextElement
                {
                    Element = Reference.ById(Guid.Parse(source?.element?.id)),
                    Value = source?.value,
                    Components = (source?.components as IEnumerable<dynamic>)?.Select(component => new ComponentModel
                    {
                        Id = Guid.Parse(component.id),
                        Type = Reference.ById(Guid.Parse(component.type.id)),
                        Elements = (component.elements as IEnumerable<dynamic>)
                    })
                };
            }
            else if (type == typeof(AssetElement))
            {
                return new AssetElement
                {
                    Element = Reference.ById(Guid.Parse(source?.element?.id)),
                    Value = (source?.value as IEnumerable<dynamic>)?
                        .Select(assetWithRenditionsReferences =>
                            new AssetWithRenditionsReference(
                                Reference.ById(Guid.Parse(assetWithRenditionsReferences.id)),
                                (assetWithRenditionsReferences.renditions as IEnumerable<dynamic>)?.Select<dynamic, Reference>(renditionIdentifier => Reference.ById(Guid.Parse(renditionIdentifier.id))))),
                };
            }
            else if (type == typeof(DateTimeElement))
            {
                return new DateTimeElement
                {
                    Element = Reference.ById(Guid.Parse(source?.element?.id)),
                    Value = Convert.ToDateTime(source?.value)
                };
            }
            else if (type == typeof(LinkedItemsElement))
            {
                return new LinkedItemsElement
                {
                    Element = Reference.ById(Guid.Parse(source?.element?.id)),
                    Value = (source?.value as IEnumerable<dynamic>)?.Select<dynamic, Reference>(identifier => Reference.ById(Guid.Parse(identifier.id)))
                };
            }
            else if (type == typeof(MultipleChoiceElement))
            {
                return new MultipleChoiceElement
                {
                    Element = Reference.ById(Guid.Parse(source?.element?.id)),
                    Value = (source?.value as IEnumerable<dynamic>).Select<dynamic, Reference>(identifier => Reference.ById(Guid.Parse(identifier.id)))
                };
            }
            else if (type == typeof(TaxonomyElement))
            {
                return new TaxonomyElement
                {
                    Element = Reference.ById(Guid.Parse(source?.element?.id)),
                    Value = (source?.value as IEnumerable<dynamic>)?.Select<dynamic, Reference>(identifier => Reference.ById(Guid.Parse(identifier.id)))
                };
            }
            else if (type == typeof(UrlSlugElement))
            {
                return new UrlSlugElement
                {
                    Element = Reference.ById(Guid.Parse(source?.element?.id)),
                    Mode = source?.mode?.ToString(),
                    Value = source?.value?.ToString()
                };
            }
            else if (type == typeof(CustomElement))
            {
                return new CustomElement
                {
                    Element = Reference.ById(Guid.Parse(source?.element?.id)),
                    Value = source?.value?.ToString(),
                    SearchableValue = source?.searchable_value?.ToString()
                };
            }
            else if(type == typeof(SubpagesElement))
            {
                return new SubpagesElement
                {
                    Element = Reference.ById(Guid.Parse(source?.element?.id)),
                    Value = (source?.value as IEnumerable<dynamic>)?.Select<dynamic, Reference>(identifier => Reference.ById(Guid.Parse(identifier.id)))
                };
            }

            throw new ArgumentOutOfRangeException($"{type} is not a valid element");
        }
    }
}
