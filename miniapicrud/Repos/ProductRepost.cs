using AutoMapper;
using Microsoft.EntityFrameworkCore;
using miniapicrud.Data;
using miniapicrud.Model;
using miniapicrud.Model.dto;

namespace miniapicrud.IProductRepos
{
    public class ProductRepost(IMapper mapper, ApplicationDbContext db) : IProductRepo
    {
        public async Task<ResponseDto> Add(AddRequestDto request)
        {
            db.Products.Add(mapper.Map<Product>(request));
            await db.SaveChangesAsync();
            return new ResponseDto
            {
                flag = true,
                message = "saved"
            };
        }

        public async Task<ResponseDto> Delete(int id)
        {
            db.Products.Remove(await db.Products.FindAsync(id));
            await db.SaveChangesAsync();
            return new ResponseDto(true, "deleted");
        }

        public async Task<List<TotalPriceDtoResponse>> getall()
        {


            return mapper.Map<List<TotalPriceDtoResponse>>(await db.Products.ToListAsync());

        }

        public async Task<TotalPriceDtoResponse> getbyid(int id) => mapper.Map<TotalPriceDtoResponse>(await db.Products.FindAsync(id));


        public async Task<ResponseDto> Update(UpdateRequestDto request)
        {
            db.Products.Update(mapper.Map<Product>(request));
            await db.SaveChangesAsync();
            return new ResponseDto(true, "updated");
        }


       




    }
}
