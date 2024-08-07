using System.ComponentModel.DataAnnotations;

namespace Triarch.BusinessLogic.Models.Definitions;

public class Genre
{    
    public string GenreName { get; set; } = string.Empty;

    public int GenreOrder { get; set; }
}