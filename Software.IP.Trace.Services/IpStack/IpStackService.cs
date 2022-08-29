using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

namespace Software.IP.Trace.Services.IpStack {
    public class IpStackService : IIpStackService {
        private readonly HttpClient _httpClient;

        public IpStackService(HttpClient httpClient) {
            // fields
            _httpClient = httpClient;
            if (_httpClient.BaseAddress == null) {
                _httpClient.BaseAddress = new Uri("http://api.ipstack.com/");
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        public Task<IpStackDO?> GetAsync(string ip, string apiKey) {
            return _httpClient
                .GetAsync($"{ip}?access_key={apiKey}")
                .ToObservable()
                .SelectMany(response => response.Content.ReadAsStringAsync().ToObservable())
                .Select(JsonConvert.DeserializeObject<IpStackDO>)
                .ToTask();
        }
    }

    public interface IIpStackService {
        Task<IpStackDO?> GetAsync(string ip, string apiKey);
    }

    public class IpStackDO {
        public string ip { get; set; }
        public string hostname { get; set; }
        public string type { get; set; }
        public string continent_code { get; set; }
        public string continent_name { get; set; }
        public string region_code { get; set; }
        public string region_name { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
}
