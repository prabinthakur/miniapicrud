using miniapicrud.Model.dto;

namespace miniapicrud.Services
{
    public interface  IAcccountService
    {
        Task<ResponseDto> Register(RegisterDto reg);
        Task<LoginResponse> Login(LoginDto login);
    }
}
