namespace miniapicrud.Model.dto
{

    public record TotalPriceDtoResponse(string Name, string Description, double Price, int Quanity)
    {

        public double TotalPrice => Quanity * Price;
    }

}
