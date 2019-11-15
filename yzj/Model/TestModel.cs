using System.ComponentModel.DataAnnotations;

namespace yzj.Model
{
    public class TestModel
    {
        [Key]
        public int uid { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}
