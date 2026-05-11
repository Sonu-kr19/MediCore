
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediCore.Api.DTOs.PatientDtos;
using MediCore.Api.Services.PatientDocumentServices;
using MediCore.Domain.Enum;

namespace MediCore.Api.Controllers;

//created Patient Document upload
//in the route patientdocument
//for uploading and downloading using file stream

[ApiController]
[Route("api/v1/[controller]")]
public class PatientDocumentController : ControllerBase
{
    private readonly IPatientDocumentService _documentService;

    public PatientDocumentController(IPatientDocumentService documentService)
    {
        _documentService = documentService;
    }

    // Patient or Admin uploads an ID proof document.
    // [Authorize(Roles = $"{nameof(RoleOption.Admin)},{nameof(RoleOption.Patient)}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost("{id}/documents")]
    public async Task<IActionResult> UploadDocument(int id, [FromForm] PatientDocumentRequestDto dto)
    {
        var result = await _documentService.UploadDocumentAsync(id, dto);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    // Admin enters PatientID — shows all documents uploaded by that patient.
    // [Authorize(Roles = $"{nameof(RoleOption.Admin)},{nameof(RoleOption.Patient)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}/documents")]
    public async Task<IActionResult> GetDocuments(int id)
    {
        var result = await _documentService.GetDocumentsByPatientAsync(id);
        return Ok(result);
    }

    // Admin enters PatientID — downloads all documents of that patient.
    // Single document → returns file directly. Multiple → returns as zip.
    // [Authorize(Roles = $"{nameof(RoleOption.Admin)},{nameof(RoleOption.Patient)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}/documents/download")]
    public async Task<IActionResult> DownloadDocument(int id)
    {
        var (fileBytes, fileName, contentType) = await _documentService.DownloadDocumentAsync(id);
        return File(fileBytes, contentType, fileName);
    }

    // Admin verifies (approves or rejects) a specific document for a patient.
    // [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPut("{id}/documents/{documentId}/verify")]
    public async Task<IActionResult> VerifyDocument(int id, int documentId)
    {
        var result = await _documentService.VerifyDocumentAsync(id, documentId);
        return Ok(result);
    }
}