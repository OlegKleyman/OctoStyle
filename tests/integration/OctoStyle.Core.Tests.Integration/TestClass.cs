//This is a test class used for analyzer tests. It's designed to be fail standards and is not for use
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctoStyle.Core.Tests.Integration
{
    public class TestClass
    {
        public void TestMethod()
        {
            if (1.ToString() == "1")
            {
                var t = 1 + 1;
            }
            2.ToString();
        }
    }
}
