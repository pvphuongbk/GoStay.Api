

using GoStay.Data.Base;

namespace BaoTangBn.Service.AuthorityServices
{
    public interface IPermisionService
    {
        public ResponseBase CheckUserPermision(int[] permisions, int IdUser);
        public ResponseBase GetUserPermission(int IdUser);


    }
}
