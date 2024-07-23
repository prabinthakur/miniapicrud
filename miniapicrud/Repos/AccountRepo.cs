using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using miniapicrud.Data;
using miniapicrud.Model;
using miniapicrud.Model.dto;
using System.CodeDom.Compiler;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace miniapicrud.Repos
{
    public class AccountRepo(ApplicationDbContext db,IMapper mapper,IConfiguration config) : IAccountRepo
    {
        public async Task<LoginResponse> Login(LoginDto login)
        {

            var user=await FindUserByEmail(login.Email);
            if (user!= null)
            {
                bool verifypassword = BCrypt.Net.BCrypt.Verify(login.Password, user.Password);
                if (!verifypassword) {

                    return new LoginResponse(false, "InvalidCrediential");
                
                }
                string Token = GenerateToken(user);
                return new LoginResponse(true, Token, null);
            }
            return new LoginResponse(false, null, "User doesnot exist");

           
        }

      
        private async Task<User>FindUserByEmail(string email)
        {

            email=email.ToLower();
            return await db.users.FirstOrDefaultAsync(u => u.Email.ToLower() == email);
        }



        private string GenerateToken(User user) {

            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
            var credential = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim("Fullname",user.Name),
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role,user.Role)

            };
            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credential


                );
            return new JwtSecurityTokenHandler().WriteToken(token); 
        }


        public async Task<ResponseDto> Register(RegisterDto reg)
        {
           

            var user=await FindUserByEmail(reg.Email);
            if (user!=null)
            {
                return new ResponseDto(false, "user already registerd");
            }

            var adduser = mapper.Map<User>(reg);
            adduser.Password=BCrypt.Net.BCrypt.HashPassword(reg.Password);
            db.users.Add(adduser);
            await db.SaveChangesAsync();
            
            return new ResponseDto(true,"Created");

        }

    }
}
