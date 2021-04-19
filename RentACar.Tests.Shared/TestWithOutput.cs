using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace RentACar.Tests.Shared
{
    public class TestWithOutput
    {
        protected readonly ITestOutputHelper TestOutput;

        public TestWithOutput(ITestOutputHelper output)
        {
            this.TestOutput = output;
        }
    }
}
