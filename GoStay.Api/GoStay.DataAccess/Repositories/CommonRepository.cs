using GoStay.DataAccess.DBContext;
using GoStay.DataAccess.Interface;

namespace GoStay.DataAccess.Repositories
{
	public class CommonRepository<T> : Repository<CommonDBContext, T>, ICommonRepository<T> where T : class
	{
		public CommonRepository(CommonDBContext context) : base(context)
		{

		}
	}
}
