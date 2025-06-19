using AuthDemo.Infrastructure.EFCore;
using AuthDemos.Core.DTOs.TypeForms;
using AuthDemos.Core.Entities;
using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace AuthDemo.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TypeFormsController : ControllerBase
    {
        private const string Secret = "";
        private readonly AuthDemoDbContext _dbContext;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly TypeFormCredentials _typeFormCredentials;
        public TypeFormsController(AuthDemoDbContext dbContext,
            IHttpClientFactory httpClientFactory,
            IOptions<TypeFormCredentials> typeFormCredentials)
        {
            _dbContext = dbContext;
            _httpClientFactory = httpClientFactory;
            _typeFormCredentials = typeFormCredentials.Value;
        }


        private async Task<TypeFormResponseDTO> GetAllTypeFormsResponseAsync(string formId, string accessToken, string responseId)
        {
            HttpClient client = _httpClientFactory.CreateClient();
            string url = $"{_typeFormCredentials.BaseUrl}/forms/{formId}/responses";
            if (responseId != null)
                url = url + $"?included_response_ids={responseId}";

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<TypeFormResponseDTO>(await response.Content.ReadAsStringAsync());

            else
                throw new Exception(await response.Content.ReadAsStringAsync());

        }

        private async Task<FormDefinitionDto> GetFormDefinitionAsync(string formId, string accessToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync($"{_typeFormCredentials.BaseUrl}/forms/{formId}");

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            var formDefintionDto = JsonSerializer.Deserialize<FormDefinitionDto>(json);

            return JsonSerializer.Deserialize<FormDefinitionDto>(json);
        }

        private List<Field> FlattenFields(List<Field> fields)
        {
            return fields.SelectMany(f =>
                new[] { f }.Concat(
                    f.Properties?.Fields != null
                        ? FlattenFields(f.Properties.Fields)
                        : Enumerable.Empty<Field>()
                )
            ).ToList();
        }

        private string ExtractAnswerValue(Answer answer)
        {
            switch (answer.Type)
            {
                case "text": return answer.Text;
                case "long_text": return answer.Text;
                case "short_text": return answer.Text;
                case "email": return answer.Email;
                case "phone_number": return answer.PhoneNumber;
                case "date": return answer.Date;
                case "boolean": return answer.Boolean.ToString();
                case "yes_no": return answer.Boolean.ToString();
                case "choice": return answer.Choice?.Label;

                default: return null;
            }
        }

        private bool VerifySignature(string receivedSignature, string payload, string secret)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
            var computedSignature = "sha256=" + Convert.ToBase64String(hash);
            return receivedSignature == computedSignature;
        }

        [HttpPost]
        [ActionName("Webhook")]
        [AllowAnonymous]
        [VerifyTypeformSignature]
        public async Task<IActionResult> Webhook()
        {
            try
            {
                using StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8, leaveOpen: true);
                var body = await reader.ReadToEndAsync();
                string payload = body;

                var model = JsonSerializer.Deserialize<TypeformWebhookDto>(payload);
                if (model != null)
                {
                    var submission = new TypeFormSubmission
                    {
                        FormId = model.FormResponse.FormId,
                        Token = model.FormResponse.Token,
                        EventId = model.EventId,
                        SubmittedAt = DateTime.Parse(model.FormResponse.SubmittedAt),
                        LandedAt = DateTime.Parse(model.FormResponse.LandedAt),
                    };

                    var fields = model.FormResponse.Definition.Fields;
                    var answers = model.FormResponse.Answers;

                    foreach (var answer in answers)
                    {
                        var field = fields.FirstOrDefault(f => f.Ref == answer.Field.Ref);
                        string response;

                        switch (answer.Type)
                        {
                            case "text": response = answer.Text; break;
                            case "email": response = answer.Email; break;
                            case "phone_number": response = answer.PhoneNumber; break;
                            case "boolean": response = answer.Boolean?.ToString(); break;
                            case "date": response = answer.Date; break;
                            case "choice": response = answer.Choice?.Label; break;
                            default: response = null; break;
                        }

                        submission.Answers.Add(new TypeFormAnswers
                        {
                            FieldRef = field?.Ref,
                            QuestionTitle = field?.Title ?? "Unknown",
                            FieldType = answer.Type,
                            AnswerText = response
                        });
                    }

                    _dbContext.TypeFormSubmission.Add(submission);
                    await _dbContext.SaveChangesAsync();
                }


                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _dbContext.TypeFormSubmission
                .Include(x => x.Answers)
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet]
        [ActionName(nameof(GetTypeFormResponses))]
        public async Task<IActionResult> GetTypeFormResponses(string responseId)
        {
            try
            {
                const string formId = "CmXHhBd6";
                string accessToken = _typeFormCredentials.AccessToken;

                var formResponses = await GetAllTypeFormsResponseAsync(formId, accessToken, responseId);
                var formDefinition = await GetFormDefinitionAsync(formId, accessToken);

                var flatFields = FlattenFields(formDefinition.Fields);

                var customResponses = formResponses.Items.Select(response => new TypeFormCustomResponseDto
                {
                    ResponseId = response.ResponseId,
                    TypeFormAnswers = response.Answers.Select(answer =>
                    {
                        var matchingField = flatFields.FirstOrDefault(f => f.Ref == answer.Field.Ref);
                        return new TypeFormAnswersDto
                        {
                            QuestionTitle = matchingField?.Title ?? "Unknown",
                            AnswerText = ExtractAnswerValue(answer)
                        };
                    }).ToList()
                }).ToList();

                if (responseId != null)
                    return Ok(customResponses.FirstOrDefault());

                return Ok(customResponses);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error retrieving Typeform responses", Details = ex.Message });
            }
        }
    }
}
