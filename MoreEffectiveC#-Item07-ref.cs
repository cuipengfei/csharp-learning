using System;
using FluentAssertions;
using Xunit;

namespace csharp_learning
{
    public class MyUtil
    {
        public static object GeneratePoint()
        {
            return new {X = 1.0, Y = 2.0};
        }
    }
}