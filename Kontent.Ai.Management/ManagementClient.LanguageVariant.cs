using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Conversion;
using Kontent.Ai.Management.Exceptions;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.StronglyTyped;
using Kontent.Ai.Management.Models.Workflow;
using Kontent.Ai.Management.Configuration;
using Kontent.Ai.Management.Validation;
using System.Text.Json;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IEnumerable<LanguageVariantModel>> ListLanguageVariantsByItemAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return EnsureSuccess(await _managementApi.ListLanguageVariantsByItemInternalAsync(identifier.ToUrlSegment()));
    }

    /// <inheritdoc />
    public async Task<List<LanguageVariantModel<T>>> ListLanguageVariantsByItemAsync<T>(Reference identifier) where T : new()
        => (await ListLanguageVariantsByItemAsync(identifier)).Select(_modelProvider.GetLanguageVariantModel<T>).ToList();

    /// <inheritdoc />
    public async Task<IListingResponseModel<LanguageVariantModel>> ListLanguageVariantsByTypeAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var typeSegment = identifier.ToUrlSegment();
        var response = EnsureSuccess(await _managementApi.ListLanguageVariantsByTypeInternalAsync(typeSegment));

        return new ListingResponseModel<LanguageVariantModel>(
            (continuationToken, _) => GetNextLanguageVariantsByTypePageAsync(typeSegment, continuationToken),
            response.Pagination?.Token,
            url: string.Empty,
            response.Variants);
    }

    /// <inheritdoc />
    public async Task<IListingResponseModel<LanguageVariantModel<T>>> ListLanguageVariantsByTypeAsync<T>(Reference identifier) where T : new()
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var typeSegment = identifier.ToUrlSegment();
        var response = EnsureSuccess(await _managementApi.ListLanguageVariantsByTypeInternalAsync(typeSegment));

        return new ListingResponseMappedModel<LanguageVariantModel, LanguageVariantModel<T>>(
            (continuationToken, _) => GetNextLanguageVariantsByTypePageAsync(typeSegment, continuationToken),
            response.Pagination?.Token,
            url: string.Empty,
            response.Variants,
            _modelProvider.GetLanguageVariantModel<T>);
    }

    private async Task<IListingResponse<LanguageVariantModel>> GetNextLanguageVariantsByTypePageAsync(string typeSegment, string continuationToken)
        => EnsureSuccess(await _managementApi.ListLanguageVariantsByTypeInternalAsync(typeSegment, continuationToken));

    /// <inheritdoc />
    public async Task<IListingResponseModel<LanguageVariantModel>> ListLanguageVariantsOfContentTypeWithComponentsAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var typeSegment = identifier.ToUrlSegment();
        var response = EnsureSuccess(await _managementApi.ListLanguageVariantsOfContentTypeWithComponentsInternalAsync(typeSegment));

        return new ListingResponseModel<LanguageVariantModel>(
            (continuationToken, _) => GetNextLanguageVariantsWithComponentsPageAsync(typeSegment, continuationToken),
            response.Pagination?.Token,
            url: string.Empty,
            response.Variants);
    }

    private async Task<IListingResponse<LanguageVariantModel>> GetNextLanguageVariantsWithComponentsPageAsync(string typeSegment, string continuationToken)
        => EnsureSuccess(await _managementApi.ListLanguageVariantsOfContentTypeWithComponentsInternalAsync(typeSegment, continuationToken));

    /// <inheritdoc />
    public async Task<IListingResponseModel<LanguageVariantModel>> ListLanguageVariantsByCollectionAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var collectionSegment = identifier.ToUrlSegment();
        var response = EnsureSuccess(await _managementApi.ListLanguageVariantsByCollectionInternalAsync(collectionSegment));

        return new ListingResponseModel<LanguageVariantModel>(
            (continuationToken, _) => GetNextLanguageVariantsByCollectionPageAsync(collectionSegment, continuationToken),
            response.Pagination?.Token,
            url: string.Empty,
            response.Variants);
    }

    private async Task<IListingResponse<LanguageVariantModel>> GetNextLanguageVariantsByCollectionPageAsync(string collectionSegment, string continuationToken)
        => EnsureSuccess(await _managementApi.ListLanguageVariantsByCollectionInternalAsync(collectionSegment, continuationToken));

    /// <inheritdoc />
    public async Task<IListingResponseModel<LanguageVariantModel>> ListLanguageVariantsBySpaceAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var spaceSegment = identifier.ToUrlSegment(ReferenceKinds.Id | ReferenceKinds.Codename);
        var response = EnsureSuccess(await _managementApi.ListLanguageVariantsBySpaceInternalAsync(spaceSegment));

        return new ListingResponseModel<LanguageVariantModel>(
            (continuationToken, _) => GetNextLanguageVariantsBySpacePageAsync(spaceSegment, continuationToken),
            response.Pagination?.Token,
            url: string.Empty,
            response.Variants);
    }

    private async Task<IListingResponse<LanguageVariantModel>> GetNextLanguageVariantsBySpacePageAsync(string spaceSegment, string continuationToken)
        => EnsureSuccess(await _managementApi.ListLanguageVariantsBySpaceInternalAsync(spaceSegment, continuationToken));

    /// <inheritdoc />
    public async Task<LanguageVariantModel> GetLanguageVariantAsync(LanguageVariantIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return EnsureSuccess(await _managementApi.GetLanguageVariantInternalAsync(identifier.ToUrlSegment()));
    }

    /// <inheritdoc />
    public async Task<IManagementResult<T>> GetLanguageVariantAsync<T>(LanguageVariantIdentifier identifier, CancellationToken cancellationToken = default)
        where T : IContentItem, new()
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.GetLanguageVariantInternalAsync(identifier.ToUrlSegment(), cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return ManagementResult<T>.Failure(ParseErrors(response), response.StatusCode);
        }

        return ManagementResult<T>.Success(ProjectElements<T>(response.Content!.Elements), response.StatusCode);
    }

    /// <inheritdoc />
    public async Task<LanguageVariantModel> GetPublishedLanguageVariantAsync(LanguageVariantIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return EnsureSuccess(await _managementApi.GetPublishedLanguageVariantInternalAsync(identifier.ToUrlSegment()));
    }

    /// <inheritdoc />
    public async Task<LanguageVariantModel<T>> GetPublishedLanguageVariantAsync<T>(LanguageVariantIdentifier identifier) where T : new()
        => _modelProvider.GetLanguageVariantModel<T>(await GetPublishedLanguageVariantAsync(identifier));

    /// <inheritdoc />
    public async Task<LanguageVariantModel> UpsertLanguageVariantAsync(LanguageVariantIdentifier identifier, LanguageVariantUpsertModel languageVariantUpsertModel)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(languageVariantUpsertModel);

        return EnsureSuccess(await _managementApi.UpsertLanguageVariantInternalAsync(identifier.ToUrlSegment(), languageVariantUpsertModel));
    }

    /// <inheritdoc />
    public async Task<LanguageVariantModel> UpsertLanguageVariantAsync(LanguageVariantIdentifier identifier, LanguageVariantModel languageVariant)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(languageVariant);

        return await UpsertLanguageVariantAsync(identifier, new LanguageVariantUpsertModel(languageVariant));
    }

    /// <inheritdoc />
    public async Task<IManagementResult<T>> UpsertLanguageVariantAsync<T>(
        LanguageVariantIdentifier identifier,
        T variant,
        WorkflowStepIdentifier? workflow = null,
        CancellationToken cancellationToken = default)
        where T : IContentItem, new()
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(variant);

        var validation = ContentItemValidator.Validate(variant);
        if (!validation.IsSuccess)
        {
            return validation;
        }

        var upsertModel = new LanguageVariantUpsertModel
        {
            Elements = JsonSerializer.Deserialize<List<object>>(_contentConverter.WriteEnvelopes(variant))!,
            Workflow = workflow,
        };

        var response = await _managementApi.UpsertLanguageVariantInternalAsync(identifier.ToUrlSegment(), upsertModel, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return ManagementResult<T>.Failure(ParseErrors(response), response.StatusCode);
        }

        return ManagementResult<T>.Success(ProjectElements<T>(response.Content!.Elements), response.StatusCode);
    }

    /// <inheritdoc />
    public async Task DeleteLanguageVariantAsync(LanguageVariantIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        EnsureSuccess(await _managementApi.DeleteLanguageVariantInternalAsync(identifier.ToUrlSegment()));
    }

    // --- strongly-typed (generated-record) variant helpers ---

    private T ProjectElements<T>(IEnumerable<dynamic> elements) where T : IContentItem, new()
    {
        if (_autoScanContentTypes)
        {
            _contentConverter.Registry.Scan(typeof(T).Assembly);
        }

        var json = JsonSerializer.Serialize(elements ?? []);
        return _contentConverter.ReadEnvelopes<T>(json);
    }

    // Best-effort projection of a MAPI error body into result errors. Mirrors the parse ManagementException does,
    // but surfaces failures through IManagementResult instead of throwing.
    private static IReadOnlyList<ManagementError> ParseErrors(IApiResponse response)
    {
        var error = response.Error;
        var body = error?.Content;

        if (!string.IsNullOrWhiteSpace(body))
        {
            try
            {
                var model = JsonSerializer.Deserialize<ErrorResponseModel>(body, RefitSettingsProvider.CreateDefaultJsonSerializerOptions());
                if (model is not null)
                {
                    var errors = new List<ManagementError>();
                    if (!string.IsNullOrEmpty(model.Message))
                    {
                        errors.Add(new ManagementError(model.Message));
                    }
                    if (model.ValidationErrors is not null)
                    {
                        errors.AddRange(model.ValidationErrors
                            .Where(e => !string.IsNullOrEmpty(e.Message))
                            .Select(e => new ManagementError(e.Message)));
                    }
                    if (errors.Count > 0)
                    {
                        return errors;
                    }
                }
            }
            catch (Exception)
            {
                // Body wasn't the expected error envelope — fall through to the generic message.
            }
        }

        var status = error?.StatusCode ?? response.StatusCode;
        var reason = error?.ReasonPhrase ?? response.ReasonPhrase;
        return [new ManagementError($"CM API returned {(int)status} {reason}.")];
    }
}
