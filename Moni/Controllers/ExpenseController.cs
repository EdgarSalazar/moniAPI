using Microsoft.AspNetCore.Mvc;
using Moni.Repository;
using Moni.Models;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;

namespace Moni.Controllers
{
    [Route("api/[controller]")]
    public class ExpenseController : BalanceBaseController<Expense>
    {
        public ExpenseController(MoniContext context)
        {
            Repository = new ExpenseRepository(context);
        }

        public ExpenseRepository Repository { get; set; }

        // GET api/values
        [HttpGet]
        [Authorize("Bearer")]
        public ActionResult Get(int itensPerPage = 10, int page = 0, string fields = null)
        {
            TreatFields(ref fields);

            var paginatedData = Repository.GetAllWithPagination(itensPerPage, page);

            return Ok(new
            {
                Total = paginatedData.Total,
                Data = ExtractQueryFields(fields, paginatedData.Query).ToDynamicList()
            });
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult Get(int id, string fields = null)
        {
            TreatFields(ref fields);

            // Uses GetAll() instead of Find() because we want to get only specific fields.
            return Ok(ExtractQueryFields(fields, Repository.GetAll()).FirstOrDefault("Id == @0", id));
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Balance balance)
        {

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }
    }
}
