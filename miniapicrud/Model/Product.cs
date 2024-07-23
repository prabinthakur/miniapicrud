using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace miniapicrud.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

      
        public double Price { get; set; }
        public int Quanity { get; set; }

        public DateTime Date { get; set; }

    }
}
