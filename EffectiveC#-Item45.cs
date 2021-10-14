using System;
using System.IO;
using Xunit;

namespace csharp_learning
{
    public class EffectiveC__Item45
    {
        // 45: 考虑在方法约定遭到违背时抛出异常
        // 如果某方法与其调用者之间的约定无法得到遵守，那么该方法就应抛出异常，但这并不是说只要遇到调用者不满意的情况就一定得抛出异常。
        [Fact]
        public void ShouldNotThrowExceptionWhenCallFileExist()
        {
            Assert.False(File.Exists("unknow.txt"));
        }

        [Fact]
        public void ShouldThrowExceptionWhenCallFileOpenGivenInvalidFile()
        {
            Assert.Throws<FileNotFoundException>(() => File.Open("invalid_file", FileMode.Open));
        }
        
        public class Worker
        {
            // 如果执行某个操作的方法有可能抛出异常，就应该同时提供与之影响的测试（tryxxx）方法,作为一种防护措施，提高健壮性；
            public bool TryDoWork()
            {
                if (!CheckConditions())
                {
                    return false;
                }

                Work();
                return true;
            }

            public void DoWork()
            {
                Work();
            }

            private bool CheckConditions()
            {
                return true;
            }

            private void Work()
            {
                // Do something
            }
        }
    }
}