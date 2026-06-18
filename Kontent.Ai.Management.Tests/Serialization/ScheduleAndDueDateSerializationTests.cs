using AwesomeAssertions;
using Kontent.Ai.Management.Configuration;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.Publishing;
using System.Text.Json;

namespace Kontent.Ai.Management.Tests.Serialization;

// Schedule/due-date times are DateTimeOffset in the SDK while the backend declares DateTime. The backend serializes
// UTC instants with a trailing "Z" (as seen on last_modified everywhere), so these guard that a populated value
// round-trips through System.Text.Json without a custom converter. The variant fixtures only ever carry nulls here.
public class ScheduleAndDueDateSerializationTests
{
    private static readonly JsonSerializerOptions Options = RefitSettingsProvider.CreateDefaultJsonSerializerOptions();

    [Fact]
    public void ScheduleResponse_ReadsUtcZValues()
    {
        const string json = """
            {
              "publish_time": "2024-03-15T14:30:00Z",
              "publish_display_timezone": "Europe/Prague",
              "unpublish_time": "2024-04-01T09:00:00.123Z",
              "unpublish_display_timezone": "America/New_York"
            }
            """;

        var schedule = JsonSerializer.Deserialize<ScheduleResponseModel>(json, Options);

        schedule!.PublishTime.Should().Be(new DateTimeOffset(2024, 3, 15, 14, 30, 0, TimeSpan.Zero));
        schedule.PublishDisplayTimeZone.Should().Be("Europe/Prague");
        schedule.UnpublishTime.Should().Be(new DateTimeOffset(2024, 4, 1, 9, 0, 0, 123, TimeSpan.Zero));
        schedule.UnpublishDisplayTimeZone.Should().Be("America/New_York");
    }

    [Fact]
    public void DueDate_ReadsUtcZValue()
    {
        var dueDate = JsonSerializer.Deserialize<DueDateModel>("""{ "value": "2024-03-15T14:30:00Z" }""", Options);

        dueDate!.Value.Should().Be(new DateTimeOffset(2024, 3, 15, 14, 30, 0, TimeSpan.Zero));
    }

    [Fact]
    public void DueDate_WriteRoundTripsInstant()
    {
        var original = new DueDateModel { Value = new DateTimeOffset(2024, 3, 15, 14, 30, 0, TimeSpan.Zero) };

        var json = JsonSerializer.Serialize(original, Options);
        var roundTripped = JsonSerializer.Deserialize<DueDateModel>(json, Options);

        roundTripped!.Value.Should().Be(original.Value);
    }
}
