using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AuthDemos.Core.DTOs.TypeForms;

public class TypeformWebhookDto
{
    [JsonPropertyName("event_id")]
    public string EventId { get; set; }

    [JsonPropertyName("event_type")]
    public string EventType { get; set; }

    [JsonPropertyName("form_response")]
    public FormResponse FormResponse { get; set; }
}

public class FormResponse
{
    [JsonPropertyName("form_id")]
    public string FormId { get; set; }

    [JsonPropertyName("token")]
    public string Token { get; set; }

    [JsonPropertyName("landed_at")]
    public string LandedAt { get; set; }

    [JsonPropertyName("submitted_at")]
    public string SubmittedAt { get; set; }

    [JsonPropertyName("definition")]
    public Definition Definition { get; set; }

    [JsonPropertyName("answers")]
    public List<Answer> Answers { get; set; }

    [JsonPropertyName("ending")]
    public Ending Ending { get; set; }
}

public class Definition
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("fields")]
    public List<Field> Fields { get; set; }

    [JsonPropertyName("endings")]
    public List<Ending> Endings { get; set; }

    [JsonPropertyName("settings")]
    public Settings Settings { get; set; }
}

public class Field
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("ref")]
    public string Ref { get; set; }
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("properties")]
    public FieldProperties Properties { get; set; }

    [JsonPropertyName("choices")]
    public List<Choice> Choices { get; set; }
}

public class Choice
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("ref")]
    public string Ref {  get; set; }
    
    [JsonPropertyName("label")]
    public string Label { get; set; }
}

public class FieldProperties
{

    [JsonPropertyName("fields")]
    public List<Field> Fields { get; set; }
}

public class Answer
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("field")]
    public FieldRef Field { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("phone_number")]
    public string PhoneNumber { get; set; }

    [JsonPropertyName("choice")]
    public ChoiceAnswer Choice { get; set; }

    [JsonPropertyName("boolean")]
    public bool? Boolean { get; set; }

    [JsonPropertyName("date")]
    public string Date { get; set; }
}

public class FieldRef
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("ref")]
    public string Ref { get; set; }
    
}

public class ChoiceAnswer
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("label")]
    public string Label { get; set; }

    [JsonPropertyName("ref")]
    public string Ref { get; set; }
}

public class Ending
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("ref")]
    public string Ref { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("properties")]
    public EndingProperties Properties { get; set; }
}

public class EndingProperties
{
    [JsonPropertyName("button_text")]
    public string ButtonText { get; set; }

    [JsonPropertyName("show_button")]
    public bool ShowButton { get; set; }

    [JsonPropertyName("share_icons")]
    public bool ShareIcons { get; set; }

    [JsonPropertyName("button_mode")]
    public string ButtonMode { get; set; }
}

public class Settings
{
    [JsonPropertyName("partial_responses_to_all_integrations")]
    public bool PartialResponsesToAllIntegrations { get; set; }
}
