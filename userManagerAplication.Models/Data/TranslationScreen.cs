using System;
using System.Collections.Generic;

namespace userManagerAplication.Models.Data;

public partial class TranslationScreen
{
    public int IdLanguage { get; set; }

    public string? Translation { get; set; }

    public string? Value { get; set; }

    public bool? Status { get; set; }

    public int? IdScreen { get; set; }

    public virtual Screen? IdScreenNavigation { get; set; }
}
