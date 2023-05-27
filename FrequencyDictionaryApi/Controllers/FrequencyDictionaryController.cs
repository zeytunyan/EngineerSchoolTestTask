using FrequencyDictionaryApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FrequencyDictionaryApi.Controllers
{
    [AllowAnonymous]
    [Route("frequencies")]
    [ApiController]
    public class FrequencyDictionaryController : ControllerBase
    {
        private readonly FrequencyDictionaryServcie _frequencyDictionaryServcie;

        public FrequencyDictionaryController(FrequencyDictionaryServcie frequencyDictionaryServcie) 
        { 
            _frequencyDictionaryServcie = frequencyDictionaryServcie;
        }

        [HttpPost("create")]
        public Dictionary<string, int> CreateWordFrequencies([FromBody] string text)
        {
            return _frequencyDictionaryServcie.CreateFrequencyDictionary(text);
        }
    }
}
