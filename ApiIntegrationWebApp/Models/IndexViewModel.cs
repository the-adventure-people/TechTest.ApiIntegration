using ApiIntegration.Models;

namespace ApiIntegrationWebApp.Models
{
    public class IndexViewModel
    {
        public List<Tour> Tours { get; set; }

        // Postback
        public string SubmitBtn { get; set; }
    }
}
