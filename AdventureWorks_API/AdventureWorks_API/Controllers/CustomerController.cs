using AdventureWorks_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventureWorks_API.Controllers
{
  //  [Authorize]
    [Route("/[controller]")]
    [ApiController]
   
    public class CustomerController : ControllerBase
    {
        private readonly AdventureWorksContext _context;
        public CustomerController(AdventureWorksContext context)
        {
            _context = context;
        }

        // GET: /Customer
        [HttpGet]
        
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomer()
        {
            return await _context.Customer.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var claims = User.Claims;

            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
                return NotFound();
            return customer;
        }
    }
}
