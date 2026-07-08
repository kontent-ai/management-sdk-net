using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.Publishing;

namespace Kontent.Ai.Management.Api;

/// <inheritdoc cref="IManagementApi"/>
internal partial interface IManagementApi
{
    /// <summary>Changes the workflow/step of a language variant.</summary>
    /// <param name="variantPath">The item/variant path segment, e.g. <c>{item}/variants/{language}</c>.</param>
    /// <param name="changeModel">The workflow and step to move the variant to.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Put("/items/{**variantPath}/change-workflow")]
    internal Task<IApiResponse> ChangeLanguageVariantWorkflowInternalAsync(
        string variantPath,
        [Body] ChangeLanguageVariantWorkflowModel changeModel,
        CancellationToken cancellationToken = default);

    /// <summary>Publishes a language variant immediately.</summary>
    /// <param name="variantPath">The item/variant path segment, e.g. <c>{item}/variants/{language}</c>.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Put("/items/{**variantPath}/publish")]
    internal Task<IApiResponse> PublishLanguageVariantInternalAsync(
        string variantPath,
        CancellationToken cancellationToken = default);

    /// <summary>Schedules publishing of a language variant.</summary>
    /// <param name="variantPath">The item/variant path segment, e.g. <c>{item}/variants/{language}</c>.</param>
    /// <param name="schedule">When to publish the variant.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Put("/items/{**variantPath}/publish")]
    internal Task<IApiResponse> SchedulePublishingOfLanguageVariantInternalAsync(
        string variantPath,
        [Body] ScheduleModel schedule,
        CancellationToken cancellationToken = default);

    /// <summary>Schedules publishing and later unpublishing of a language variant.</summary>
    /// <param name="variantPath">The item/variant path segment, e.g. <c>{item}/variants/{language}</c>.</param>
    /// <param name="schedule">When to publish and when to unpublish the variant.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Put("/items/{**variantPath}/schedule-publish-and-unpublish")]
    internal Task<IApiResponse> SchedulePublishingAndUnpublishingOfLanguageVariantInternalAsync(
        string variantPath,
        [Body] SchedulePublishAndUnpublishModel schedule,
        CancellationToken cancellationToken = default);

    /// <summary>Cancels scheduled publishing of a language variant.</summary>
    /// <param name="variantPath">The item/variant path segment, e.g. <c>{item}/variants/{language}</c>.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Put("/items/{**variantPath}/cancel-scheduled-publish")]
    internal Task<IApiResponse> CancelPublishingOfLanguageVariantInternalAsync(
        string variantPath,
        CancellationToken cancellationToken = default);

    /// <summary>Unpublishes and archives a language variant immediately.</summary>
    /// <param name="variantPath">The item/variant path segment, e.g. <c>{item}/variants/{language}</c>.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Put("/items/{**variantPath}/unpublish-and-archive")]
    internal Task<IApiResponse> UnpublishLanguageVariantInternalAsync(
        string variantPath,
        CancellationToken cancellationToken = default);

    /// <summary>Schedules unpublishing and archiving of a language variant.</summary>
    /// <param name="variantPath">The item/variant path segment, e.g. <c>{item}/variants/{language}</c>.</param>
    /// <param name="schedule">When to unpublish the variant.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Put("/items/{**variantPath}/unpublish-and-archive")]
    internal Task<IApiResponse> ScheduleUnpublishingOfLanguageVariantInternalAsync(
        string variantPath,
        [Body] ScheduleModel schedule,
        CancellationToken cancellationToken = default);

    /// <summary>Cancels scheduled unpublishing of a language variant.</summary>
    /// <param name="variantPath">The item/variant path segment, e.g. <c>{item}/variants/{language}</c>.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Put("/items/{**variantPath}/cancel-scheduled-unpublish")]
    internal Task<IApiResponse> CancelUnpublishingOfLanguageVariantInternalAsync(
        string variantPath,
        CancellationToken cancellationToken = default);

    /// <summary>Creates a new version of a published language variant.</summary>
    /// <param name="variantPath">The item/variant path segment, e.g. <c>{item}/variants/{language}</c>.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Put("/items/{**variantPath}/new-version")]
    internal Task<IApiResponse> CreateNewVersionOfLanguageVariantInternalAsync(
        string variantPath,
        CancellationToken cancellationToken = default);
}
