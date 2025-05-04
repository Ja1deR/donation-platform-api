
namespace Software_2.Models
{
    using System.ComponentModel.DataAnnotations;

    namespace Software_2.Models
    {
        public class ComentarioSimpleDTO
        {
            [Required(ErrorMessage = "El comentario es requerido")]
            [StringLength(500, ErrorMessage = "Máximo 500 caracteres")]
            public string Comentario { get; set; }
        }
    }


}