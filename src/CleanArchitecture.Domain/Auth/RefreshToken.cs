using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Auth;

public class RefreshToken : Entity
{
    public Guid UserId { get; set; }
}
