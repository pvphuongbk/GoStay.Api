using GoStay.DataAccess.DBContext;
using GoStay.DataAccess.Interface;

namespace GoStay.DataAccess.UnitOfWork
{
	public class CommonUoW : UnitOfWork<CommonDBContext>, ICommonUoW
	{


		public CommonUoW(CommonDBContext context) : base(context)
		{
		}

	}
}
