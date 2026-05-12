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

        EnsureSuccess(await _managementApi.ChangeLanguageVariantWorkflowInternalAsync(BuildVariantPath(identifier), changeModel));
    }

    /// <inheritdoc />
    public async Task PublishLanguageVariantAsync(LanguageVariantIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        EnsureSuccess(await _managementApi.PublishLanguageVariantInternalAsync(BuildVariantPath(identifier)));
    }

    /// <inheritdoc />
    public async Task SchedulePublishingOfLanguageVariantAsync(LanguageVariantIdentifier identifier, ScheduleModel scheduleModel)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(scheduleModel);

        EnsureSuccess(await _managementApi.SchedulePublishingOfLanguageVariantInternalAsync(BuildVariantPath(identifier), scheduleModel));
    }

    /// <inheritdoc />
    public async Task SchedulePublishingAndUnpublishingOfLanguageVariantAsync(LanguageVariantIdentifier identifier, SchedulePublishAndUnpublishModel schedule)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(schedule);

        EnsureSuccess(await _managementApi.SchedulePublishingAndUnpublishingOfLanguageVariantInternalAsync(BuildVariantPath(identifier), schedule));
    }

    /// <inheritdoc />
    public async Task CancelPublishingOfLanguageVariantAsync(LanguageVariantIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        EnsureSuccess(await _managementApi.CancelPublishingOfLanguageVariantInternalAsync(BuildVariantPath(identifier)));
    }

    /// <inheritdoc />
    public async Task UnpublishLanguageVariantAsync(LanguageVariantIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        EnsureSuccess(await _managementApi.UnpublishLanguageVariantInternalAsync(BuildVariantPath(identifier)));
    }

    /// <inheritdoc />
    public async Task CancelUnpublishingOfLanguageVariantAsync(LanguageVariantIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        EnsureSuccess(await _managementApi.CancelUnpublishingOfLanguageVariantInternalAsync(BuildVariantPath(identifier)));
    }

    /// <inheritdoc />
    public async Task ScheduleUnpublishingOfLanguageVariantAsync(LanguageVariantIdentifier identifier, ScheduleModel scheduleModel)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(scheduleModel);

        EnsureSuccess(await _managementApi.ScheduleUnpublishingOfLanguageVariantInternalAsync(BuildVariantPath(identifier), scheduleModel));
    }

    /// <inheritdoc />
    public async Task CreateNewVersionOfLanguageVariantAsync(LanguageVariantIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        EnsureSuccess(await _managementApi.CreateNewVersionOfLanguageVariantInternalAsync(BuildVariantPath(identifier)));
    }
}
