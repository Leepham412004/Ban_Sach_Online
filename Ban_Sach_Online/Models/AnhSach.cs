using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ban_Sach_Online.Models
{
    public class AnhSach
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnhSachId { get; set; }   // ✅ Identity duy nhất

        public string Url { get; set; }

        // Foreign key
        public int SachId { get; set; }      // ❌ KHÔNG có Identity
        public virtual Sach Sach { get; set; }
    }
}
