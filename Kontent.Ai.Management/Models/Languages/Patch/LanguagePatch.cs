namespace Kontent.Ai.Management.Models.Languages.Patch;

/// <summary>
/// Intent-revealing factories for the modify-language patch operations, ready to pass to
/// <c>ModifyLanguageAsync</c>. The endpoint supports only <c>replace</c>, so each factory names the property it
/// replaces and takes the correctly-typed value.
/// </summary>
public static class LanguagePatch
{
    /// <summary>Replaces the language's display name.</summary>
    public static LanguagePatchModel Name(string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        return new() { PropertyName = LanguagePropertyName.Name, Value = name };
    }

    /// <summary>Replaces the language's codename.</summary>
    public static LanguagePatchModel Codename(string codename)
    {
        ArgumentNullException.ThrowIfNull(codename);
        return new() { PropertyName = LanguagePropertyName.Codename, Value = codename };
    }

    /// <summary>Replaces the fallback language — the language used when the current language contains no content.</summary>
    public static LanguagePatchModel FallbackLanguage(Reference language)
    {
        ArgumentNullException.ThrowIfNull(language);
        return new() { PropertyName = LanguagePropertyName.FallbackLanguage, Value = language };
    }

    /// <summary>Sets whether the language is active.</summary>
    public static LanguagePatchModel IsActive(bool isActive) =>
        new() { PropertyName = LanguagePropertyName.IsActive, Value = isActive };
}
