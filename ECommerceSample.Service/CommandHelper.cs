using ECommerceSample.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECommerceSample.Service
{
    public static class CommandHelper
    {
        //Her command'ı parametreleri ile birlikte Dictionary'e ekliyorum.
        public static List<List<string>> SplitCommandEntries(this string[] value)
        {
            return value?
                .Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList())
                .ToList();
        }

    }
}
