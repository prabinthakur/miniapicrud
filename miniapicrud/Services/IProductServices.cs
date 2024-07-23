using miniapicrud.Model.dto;

namespace miniapicrud.Services
{
    public interface IProductServices
    {


        Task<ResponseDto> Add(AddRequestDto request);
        Task<ResponseDto> Update(UpdateRequestDto request);

        Task<List<TotalPriceDtoResponse>> getall();

        Task<TotalPriceDtoResponse> getbyid(int id);
        Task<ResponseDto> Delete(int id);


    }
}
