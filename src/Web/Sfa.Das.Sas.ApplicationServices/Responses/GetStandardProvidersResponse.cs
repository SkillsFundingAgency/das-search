namespace Sfa.Das.Sas.ApplicationServices.Responses
{
    public class GetStandardProvidersResponse
    {
        public enum ResponseCodes
        {
            Success,
            NoStandardFound
        }

        public int StandardId { get; set; }
        public int Level { get; set; }
        public string Title { get; set; }
        public string Postcode { get; set; }
        public string Keywords { get; set; }
        public bool HasErrors { get; set; }
        public ResponseCodes StatusCode { get; set; }
    }
}
