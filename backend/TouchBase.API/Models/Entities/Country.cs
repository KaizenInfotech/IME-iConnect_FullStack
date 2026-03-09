namespace TouchBase.API.Models.Entities;

public class Country
{
    public int Id { get; set; }
    public string? CountryCode { get; set; }
    public string? CountryName { get; set; }
}

public class Category
{
    public int Id { get; set; }
    public string? CatName { get; set; }
}
