using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace Builder.Services.OrderAPI.Utility
{
    public class TokenPropagationHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenPropagationHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Get the token from the current HTTP context in Service A
            await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            // Attach the token to the outgoing request to Service B
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            // Call the next handler in the pipeline (or send the request)
            return await base.SendAsync(request, cancellationToken);
        }
    }

}
