using Dapper;
using GoStay.Common.Constants;
using GoStay.Data.Ticket;
using GoStay.DataDto.OrderDto;
using GoStay.Repository.DapperHelper;
using System.Reflection;

namespace GoStay.Repository.Repositories
{
    public class OrderTicketRepository
    {
        public static List<OrderTicketAdminDto> GetListOrderTicket(int? IdUser, int pageIndex, int pageSize)
        {
            var p = new DynamicParameters();
            p.Add("@IdUser", IdUser == null ? null : string.Join(",", IdUser), System.Data.DbType.String);
            p.Add("@pageIndex", pageIndex, System.Data.DbType.Int32);
            p.Add("@pageSize", pageSize, System.Data.DbType.Int32);
            return DapperExtensions.QueryDapperStoreProc<OrderTicketAdminDto>(Procedures.sq_GetAllOrderTicket, p).ToList();
        }
    }
}
