using System.Text.Json.Serialization;

namespace GymManagement.Contracts.Subscriptions;

// Tell to the JSON serializer to represent enum values as strings
// when serializing to JSON, rather than as intergers.
// This JsonConverter is useful when you wnat the JSON representation
// to be more human-readable
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SubscriptionType
{
    Free,
    Starter,
    Pro
}