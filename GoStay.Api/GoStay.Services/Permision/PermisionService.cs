using AutoMapper;
using GoStay.Data.Base;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;

namespace BaoTangBn.Service.AuthorityServices
{
    public class PermisionService : IPermisionService
    {
        private readonly ICommonRepository<Permision> _permisionRepository;
        private readonly ICommonRepository<UserPermision> _userPermisionRepository;
        private IMapper _mapper;

        public PermisionService(ICommonRepository<Permision> permisionRepository, ICommonRepository<UserPermision> userPermisionRepository, IMapper mapper)
        {
            //  mình gọi thằng authority trong pipeline ra, gắn nó vào thằng _authorityRepository để dùng
            _permisionRepository = permisionRepository;
            _userPermisionRepository = userPermisionRepository;
            _mapper = mapper;
        }
        public ResponseBase GetUserPermission(int IdUser)
        {
            ResponseBase response = new ResponseBase();
            try
            {
                var result = _userPermisionRepository.FindAll(x => x.IdUser == IdUser).Include(x => x.IdPermisionNavigation).Select(x => x.IdPermisionNavigation);
                var userPermisions = result.Select(x => x.Code).ToList();

                response.Code = ErrorCodeMessage.Success.Key;
                response.Message = ErrorCodeMessage.Success.Value;
                response.Data = userPermisions;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Data = false;
                return response;
            }
        }
        public ResponseBase CheckUserPermision(int[] permisions, int IdUser)
        {
            ResponseBase response = new ResponseBase();
            try
            {
                var  result = _userPermisionRepository.FindAll(x => x.IdUser == IdUser).Include(x=>x.IdPermisionNavigation).Select(x=>x.IdPermisionNavigation);
                var userPermisions = result.Select(x => x.Code).ToList();
                foreach (var item in permisions)
                {
                    if (userPermisions.Contains(item) == false)
                    {
                        response.Code = ErrorCodeMessage.NoPermission.Key;
                        response.Message = ErrorCodeMessage.NoPermission.Value;
                        response.Data = false;
                        return response;
                    }
                }
                response.Code = ErrorCodeMessage.Success.Key;
                response.Message = ErrorCodeMessage.Success.Value;
                response.Data = true;
                return response;
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
                response.Data = false;
                return response;
            }
        }
    }
}
