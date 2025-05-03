namespace Software_2.Models;

public partial class Documento
{
    public int IdDocumento { get; set; }

    public string RutaDoc { get; set; } = null!;

    public string TipoDocumento { get; set; } = null!;

    public virtual ICollection<FundacionDocumento> FundacionDocumentos { get; set; } = new List<FundacionDocumento>();
}
