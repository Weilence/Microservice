using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Microservice.Service
{
    public class JsonResultFilter : IAsyncResultFilter
    {

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Result is ObjectResult objectResult)
            {
                context.Result = new OkObjectResult(JsonSerializer.Serialize(objectResult.Value));
            }

            await next();
        }
    }
}