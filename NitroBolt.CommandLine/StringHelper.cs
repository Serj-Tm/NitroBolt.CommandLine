using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NitroBolt.Functional
{
  static internal partial class StringHelper
  {
    public static string JoinToString(this IEnumerable<string> items, string separator = null)
    {
      return string.Join(separator, items);
    }

  }
}
