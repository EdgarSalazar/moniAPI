using Moni.Models;

namespace Moni.Repository
{
    public class RevenueRepository : GenericRepository<Revenue>
    {
        public RevenueRepository(MoniContext context) : base(context)
        {

        }
    }
}
