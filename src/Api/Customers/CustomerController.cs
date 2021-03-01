using MediatR;
using Microsoft.AspNetCore.Mvc;
using SampleStore.Application.Customers;
using SampleStore.Application.Customers.RegisterCustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SampleStore.Api.Customers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType(typeof(CustomerDto), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> RegisterCustomer()
        {
            var customer = await _mediator.Send(new RegisterCustomerCommand("bob@domain.com", "Bob"));

            return Created(string.Empty, customer);
        }
    }
}
