using GoStay.DataAccess.DBContext;
using GoStay.DataAccess.Interface;

namespace GoStay.DataAccess.UnitOfWork
{
	public class CommonUoW : UnitOfWork<CommonDBContext>, ICommonUoW
	{
        public CommonDBContext Context { get; set; }

        public CommonUoW(CommonDBContext context) : base(context)
		{
            Context = context;
        }
	}
}
