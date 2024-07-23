using Azure;
using miniapicrud.Model.dto;

namespace miniapicrud.IProductRepos
{
    public interface IProductRepo
    {

        Task<ResponseDto> Add(AddRequestDto request);
        Task<ResponseDto> Update(UpdateRequestDto request);

        Task<List<TotalPriceDtoResponse>> getall();

        Task<TotalPriceDtoResponse> getbyid(int id);
        Task<ResponseDto> Delete(int id);
    }
}
