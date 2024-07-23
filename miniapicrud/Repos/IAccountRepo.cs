using miniapicrud.Model.dto;

namespace miniapicrud.Repos
{
    public interface IAccountRepo
    {

        Task<ResponseDto> Register(RegisterDto reg);
        Task<LoginResponse>Login(LoginDto login);
    }
}
