using System.Text.Json.Serialization;

namespace AuthDemos.Core.DTOs.TypeForms;

public class FormDefinitionDto
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("fields")]
    public List<Field>? Fields { get; set; }
}
