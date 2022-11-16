using GoStay.DataAccess.Interface;
using Microsoft.EntityFrameworkCore;

namespace GoStay.DataAccess.Base
{
	public abstract class PDataContext : DbContext, IDBContext
	{
		protected PDataContext(DbContextOptions options) : base(options)
		{

		}
	}
}
