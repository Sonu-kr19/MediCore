using System;

namespace MediCore.Api.DTOs.TokenDtos;

public class TokenResponseDto
{
    public string AccessToken {get;set;}
    public string RefreshToken {get;set;}
}
