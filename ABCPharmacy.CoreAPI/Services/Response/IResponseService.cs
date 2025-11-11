using ABCPharmacy.CoreAPI.Models.DTOs;

namespace ABCPharmacy.CoreAPI.Services.Response
{
    public interface IResponseService
    {
        /// <summary>
        /// Create a sucess response object
        /// </summary>
        /// <param name="result">result to include in response</param>
        /// <param name="message">success message</param>
        /// <returns>Object Indicating success</returns>
        ResponseDTO Success(object? result = null, string? message = null);
        /// <summary>
        /// Creates a failure response Object
        /// </summary>
        /// <param name="message">Error message</param>
        /// <returns>Object indicating failure</returns>
        ResponseDTO Fail(string message);
    }
}
