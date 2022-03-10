using System.Collections.Generic;

namespace Kentico.Kontent.Management.Modules.ModelBuilders
{
    /// <summary>
    /// Defines the contract for mapping dynamic elements to strongly typed models.
    /// </summary>
    internal interface IElementModelProvider
    {
        /// <summary>
        /// Builds a strongly typed element model from non-generic model.
        /// </summary>
        /// <typeparam name="T">Strongly typed elements model.</typeparam>
        /// <param name="elements">Dynamically typed element values</param>
        /// <returns>Strongly typed model of the element values.</returns>
        T GetStronglyTypedElements<T>(IEnumerable<dynamic> elements) where T : new();

        /// <summary>
        /// Converts strongly typed element values model to dynamic model.
        /// </summary>
        /// <typeparam name="T">Strongly typed elements model.</typeparam>
        /// <param name="stronglyTypedElements">Strongly typed element values.</param>
        /// <returns>Dynamic element model values.</returns>
        IEnumerable<dynamic> GetDynamicElements<T>(T stronglyTypedElements);
    }
}