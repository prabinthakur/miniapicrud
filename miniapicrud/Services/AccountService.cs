using miniapicrud.Model.dto;
using miniapicrud.Repos;

namespace miniapicrud.Services
{
    public class AccountService(IAccountRepo account) : IAcccountService
    {
        public async Task<LoginResponse> Login(LoginDto login)=> await account.Login(login);
            
      
        public async Task<ResponseDto> Register(RegisterDto reg)=> await account.Register(reg);
        
    }
}
