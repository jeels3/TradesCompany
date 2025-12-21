namespace TradesCompany.Application.Common
{
    public class ServiceResult
    {
        public bool Succeeded { get; set; }
        public string[] Errors { get; set; }

        public static ServiceResult Success()
        {
            return new ServiceResult { Succeeded = true, Errors = new string[] { } };
        }

        public static ServiceResult Failure(IEnumerable<string> errors)
        {
            return new ServiceResult { Succeeded = false, Errors = errors.ToArray() };
        }
    }
}
