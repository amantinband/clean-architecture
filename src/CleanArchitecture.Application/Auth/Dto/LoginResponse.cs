using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Auth.Dto;

public class LoginResponse(string token, string refreshToken)
{
    public string Token { get; set; } = token;
    public string RefreshToken { get; set; } = refreshToken;
}
