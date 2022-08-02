using MicroService101.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.CircuitBreaker;
using Polly.Fallback;
using Polly.Retry;
using Polly.Wrap;

namespace MicroService101.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentAPIController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IStudentService _studentService;

        private readonly HttpClient _httpClient;
        private readonly AsyncFallbackPolicy<IActionResult> _fallbackPolicy;
        private readonly AsyncRetryPolicy<IActionResult> _retryPolicy;
        private static AsyncCircuitBreakerPolicy _circuitBreakerPolicy;
        private readonly AsyncPolicyWrap<IActionResult> _policy;

        public StudentAPIController(ILogger<WeatherForecastController> logger, IStudentService stdSvc, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _studentService = stdSvc;

            // Resilience Policies
            _fallbackPolicy = Policy<IActionResult>
                                .Handle<Exception>()
                                .FallbackAsync(Content("Sorry, we are currently experiencing issues. Please try again later"));

            _retryPolicy = Policy<IActionResult>
                .Handle<Exception>()
                .RetryAsync();

            if (_circuitBreakerPolicy == null)
            {
                _circuitBreakerPolicy = Policy
                    .Handle<Exception>()
                    .CircuitBreakerAsync(2, TimeSpan.FromMinutes(1));
            }

            _policy = Policy<IActionResult>
                .Handle<Exception>()
                .FallbackAsync(Content("Sorry, we are currently experiencing issues. Please try again later"))
                .WrapAsync(_retryPolicy)
                .WrapAsync(_circuitBreakerPolicy);

            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet]
        public IActionResult GetAllStudents()
        {
            var students = _studentService.GetAllStudents();
            return Ok(students);
        }

        [HttpGet]
        public IActionResult GetStudentInfoById(int id)
        {
            var info = _studentService.GetStudentInfoById(id);
            return Ok(info);
        }

        [HttpGet]
        public async Task<IActionResult> Books()
            => await ProxyTo("https://localhost:6001/books");

        [HttpGet]
        public async Task<IActionResult> Authors()
            => await ProxyTo("https://localhost:5001/authors");

        [HttpGet]
        public async Task<IActionResult> WeatherForcast()
            => await ProxyTo("https://localhost:7158/WeatherForecast");

        private async Task<IActionResult> ProxyTo(string url)
           => await _policy.ExecuteAsync(async () => Content(await _httpClient.GetStringAsync(url)));

        

    }
}
