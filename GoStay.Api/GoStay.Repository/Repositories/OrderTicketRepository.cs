using Dapper;
using GoStay.Common.Constants;
using GoStay.Data.Ticket;
using GoStay.DataDto.OrderDto;
using GoStay.Repository.DapperHelper;

namespace GoStay.Repository.Repositories
{
    public class OrderTicketRepository
    {
        public static List<OrderTicketAdminDto> GetListOrderTicket(int pageIndex, int pageSize)
        {
            var p = new DynamicParameters();
            p.Add("@pageIndex", pageIndex == null ? null : string.Join(",", pageIndex), System.Data.DbType.String);
            p.Add("@pageSize", pageSize == null ? null : string.Join(",", pageSize), System.Data.DbType.String);

            return DapperExtensions.QueryDapperStoreProc<OrderTicketAdminDto>(Procedures.sq_GetAllOrderTicket, p).ToList();
        }
    }
}
