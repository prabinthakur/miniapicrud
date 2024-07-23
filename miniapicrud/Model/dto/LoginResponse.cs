namespace miniapicrud.Model.dto
{
    public record LoginResponse(bool Flag,string Token=null!,string Message=null!);
    
}
