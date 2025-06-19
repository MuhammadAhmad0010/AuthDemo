using AuthDemos.Core.DTOs.TypeForms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Security.Cryptography;
using System.Text;

public class VerifyTypeformSignatureAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var request = context.HttpContext.Request;
        request.EnableBuffering();

        TypeFormCredentials config = context.HttpContext.RequestServices.GetService<IOptions<TypeFormCredentials>>().Value;
        var secret = config.Secret;

        using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        request.Body.Position = 0;

        if (!request.Headers.TryGetValue("Typeform-Signature", out StringValues signatureHeader))
        {
            context.Result = new UnauthorizedObjectResult("Missing Typeform signature");
            return;
        }

        var receivedSignature = signatureHeader.ToString();
        var computedSignature = ComputeSignature(body, secret);

        if (receivedSignature != computedSignature)
        {
            context.Result = new UnauthorizedObjectResult("Invalid signature");
            return;
        }

        await next(); 
    }

    private string ComputeSignature(string payload, string secret)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
        return "sha256=" + Convert.ToBase64String(hash);
    }
}
