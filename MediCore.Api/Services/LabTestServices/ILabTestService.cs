using System;
using MediCore.Api.DTOs.LabTestDto;
using MediCore.Domain.Entities;

namespace MediCore.Api.Services.LabTestServices;

public interface ILabTestService
{
    Task<int> AddLabTestAsync(LabTestRequestDto dto);

}
