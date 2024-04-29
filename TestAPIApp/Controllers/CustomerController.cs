using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestAPIApp.Model;
using TestAPIApp.Service;

namespace TestAPIApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {

        private readonly Service.CustomerService _customerService;

        public CustomerController(Service.CustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost(Name = "GetCustomerOrders")]
        public IActionResult GetCustomerOrders([FromBody] Model.CustomerInputModel customerInput)
        {
            var outputbook = _customerService.getCustomerOrders(customerInput);

            return Ok(outputbook);
        }
    }
}
