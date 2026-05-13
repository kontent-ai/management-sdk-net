using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.Publishing;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task ChangeLanguageVariantWorkflowAsync(LanguageVariantIdentifier identifier, ChangeLanguageVariantWorkflowModel changeModel)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(changeModel);

        EnsureSuccess(await _managementApi.ChangeLanguageVariantWorkflowInternalAsync(identifier.ToUrlSegment(), changeModel));
    }

    /// <inheritdoc />
    public async Task PublishLanguageVariantAsync(LanguageVariantIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        EnsureSuccess(await _managementApi.PublishLanguageVariantInternalAsync(identifier.ToUrlSegment()));
    }

    /// <inheritdoc />
    public async Task SchedulePublishingOfLanguageVariantAsync(LanguageVariantIdentifier identifier, ScheduleModel scheduleModel)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(scheduleModel);

        EnsureSuccess(await _managementApi.SchedulePublishingOfLanguageVariantInternalAsync(identifier.ToUrlSegment(), scheduleModel));
    }

    /// <inheritdoc />
    public async Task SchedulePublishingAndUnpublishingOfLanguageVariantAsync(LanguageVariantIdentifier identifier, SchedulePublishAndUnpublishModel schedule)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(schedule);

        EnsureSuccess(await _managementApi.SchedulePublishingAndUnpublishingOfLanguageVariantInternalAsync(identifier.ToUrlSegment(), schedule));
    }

    /// <inheritdoc />
    public async Task CancelPublishingOfLanguageVariantAsync(LanguageVariantIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        EnsureSuccess(await _managementApi.CancelPublishingOfLanguageVariantInternalAsync(identifier.ToUrlSegment()));
    }

    /// <inheritdoc />
    public async Task UnpublishLanguageVariantAsync(LanguageVariantIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        EnsureSuccess(await _managementApi.UnpublishLanguageVariantInternalAsync(identifier.ToUrlSegment()));
    }

    /// <inheritdoc />
    public async Task CancelUnpublishingOfLanguageVariantAsync(LanguageVariantIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        EnsureSuccess(await _managementApi.CancelUnpublishingOfLanguageVariantInternalAsync(identifier.ToUrlSegment()));
    }

    /// <inheritdoc />
    public async Task ScheduleUnpublishingOfLanguageVariantAsync(LanguageVariantIdentifier identifier, ScheduleModel scheduleModel)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(scheduleModel);

        EnsureSuccess(await _managementApi.ScheduleUnpublishingOfLanguageVariantInternalAsync(identifier.ToUrlSegment(), scheduleModel));
    }

    /// <inheritdoc />
    public async Task CreateNewVersionOfLanguageVariantAsync(LanguageVariantIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        EnsureSuccess(await _managementApi.CreateNewVersionOfLanguageVariantInternalAsync(identifier.ToUrlSegment()));
    }
}
