using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IAutoCompleteSearchService
{
    void AddKeyword(string keyword);
    List<string> GetSuggestions(string prefix, int limit);
}
