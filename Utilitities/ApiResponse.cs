using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Todos.Domain.Model;

namespace Utilitities
{
    public class ApiResponse
    {
        public StandardResponse createResponse(string status,string message, string uniqueId)
        {
            return new StandardResponse
            {
                status = status,
                uniqueId = uniqueId,
                message = message
            };
        }
    }

    public class ResponseHelper
    {
        private readonly ApiResponse _apiResponse;

        public ResponseHelper(ApiResponse apiResponse)
        {
            _apiResponse = apiResponse;
        }

        // Generic function to accept a list and return it with a standard response
        public (List<T>, StandardResponse) CreateListResponse<T>(List<T> listOfLists, string status, string message, string uniqueId)
        {
            // Generate the response using the ApiResponse 
            var response = _apiResponse.createResponse(status, message, uniqueId);

            // Returning the list along with the response
            return (listOfLists, response);
        }

    }
}
