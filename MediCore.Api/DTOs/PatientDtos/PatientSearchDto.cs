namespace MediCore.Api.DTOs.PatientDtos;

public class PatientSearchDto
{
    //for searching patient it required search bar for enter data to search
    //and page numbers in more the 20 records present in database 
    //pagesize is how many records should see the user like first 5 or certain records only
    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}