using AuthDemos.Core.Entities;
using System.Text.Json.Serialization;

namespace AuthDemos.Core.DTOs.TypeForms;

public class TypeFormResponseDTO
{
    [JsonPropertyName("items")]
    public List<TypeformResponseItemDto> Items { get; set; }

    [JsonPropertyName("total_items")]
    public int TotalItems { get; set; }

    [JsonPropertyName("page_count")]
    public int PageCount { get; set; }
}

public class TypeformResponseItemDto
{
    [JsonPropertyName("landing_id")]
    public string LandingId { get; set; }

    [JsonPropertyName("token")]
    public string Token { get; set; }

    [JsonPropertyName("response_id")]
    public string ResponseId { get; set; }

    [JsonPropertyName("response_type")]
    public string ResponseType { get; set; }

    [JsonPropertyName("landed_at")]
    public DateTime LandedAt { get; set; }

    [JsonPropertyName("submitted_at")]
    public DateTime SubmittedAt { get; set; }

    [JsonPropertyName("answers")]
    public List<Answer> Answers { get; set; }

    [JsonPropertyName("thankyou_screen_ref")]
    public string ThankYouScreenRef { get; set; }
}

public class TypeFormAnswersDto /*: TypeFormAnswers*/
{
    public string QuestionTitle { get; set; }
    public string AnswerText { get; set; }
}
public class TypeFormCustomResponseDto
{
    public string ResponseId { get; set; }
    public List<TypeFormAnswersDto> TypeFormAnswers { get; set; }
}

