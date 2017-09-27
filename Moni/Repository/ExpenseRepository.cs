using Moni.Models;

namespace Moni.Repository
{
    public class ExpenseRepository : GenericRepository<Expense>
    {
        public ExpenseRepository(MoniContext context) : base(context)
        {

        }
    }
}
