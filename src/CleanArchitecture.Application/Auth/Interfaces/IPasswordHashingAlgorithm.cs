using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Auth.Interfaces;

internal interface IPasswordHashingAlgorithm
{
    string Name { get; }
    string Hash(string password);
    bool Verify(string password, string hashedPassword);
}
