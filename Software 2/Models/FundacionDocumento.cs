namespace Software_2.Models;

public partial class FundacionDocumento
{
    public int IdFundacionDocumento { get; set; }

    public int IdFundacion { get; set; }

    public int IdDocumento { get; set; }

    public virtual Documento IdDocumentoNavigation { get; set; } = null!;

    public virtual Fundación IdFundacionNavigation { get; set; } = null!;
}
