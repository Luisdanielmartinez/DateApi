using System.ComponentModel.DataAnnotations.Schema;

namespace chatApi.Entities
{
    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }
        public string Url{ get; set; }
        public bool IsMain { get; set; }
        //el public id para borrar la foto 
        public string PublicId { get; set; }

        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }
    }
}