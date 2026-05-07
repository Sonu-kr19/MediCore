namespace MediCore.Api.DTOs.PatientDtos;

public class PagedResult<T>
{
    //this is total page count and 
    //after search what data to show to user that data only.
    public List<T> Data { get; set; } = [];
    public int TotalCount { get; set; }
    
}