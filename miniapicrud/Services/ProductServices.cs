using miniapicrud.IProductRepos;
using miniapicrud.Model.dto;

namespace miniapicrud.Services
{
    public class ProductServices(IProductRepo Product) : IProductServices
    {
        public async Task<ResponseDto> Add(AddRequestDto request)=>await Product.Add(request);
       

        public async Task<ResponseDto> Delete(int id)=>await Product.Delete(id);
       

        public async Task<List<TotalPriceDtoResponse>> getall()=> await Product.getall();
       

        public async Task<TotalPriceDtoResponse> getbyid(int id)=>await Product.getbyid(id);


        public async Task<ResponseDto> Update(UpdateRequestDto request) => await Product.Update(request);
       
    }
}
