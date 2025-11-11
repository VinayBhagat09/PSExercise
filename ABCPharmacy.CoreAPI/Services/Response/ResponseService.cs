using ABCPharmacy.CoreAPI.Models.DTOs;

namespace ABCPharmacy.CoreAPI.Services.Response
{
    public class ResponseService : IResponseService
    {
        public ResponseDTO Fail(string message)
        {
            return new ResponseDTO
            {
                meessage = message,
                isSucess = false
            };
        }

        public ResponseDTO Success(object? result = null, string? message = null)
        {
            return new ResponseDTO
            {
                isSucess = true,
                meessage = message ?? "Success",
                result = result
            };
        }
    }
}
