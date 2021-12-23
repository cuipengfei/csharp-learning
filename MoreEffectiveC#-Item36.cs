using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace csharp_learning
{
    public class AggregateExceptionLearningTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public AggregateExceptionLearningTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void RaiseAggregateException()
        {
            var task = Task.Run( () => throw new Exception1("This exception is expected!"));
            
            try
            {
                task.Wait();
            }
            // #1
            catch (AggregateException e)
            {
                _testOutputHelper.WriteLine(e.ToString());
                throw;
            }
        }
        
        [Fact]
        public void HandleAggregateException()
        {
            var task = Task.Run( () => throw new Exception1("This exception is expected!"));
            
            try
            {
                task.Wait();
            }
            // #2
            catch (AggregateException ae) {
                ae.Handle( x => { // Handle an UnauthorizedAccessException
                    if (x is Exception1) {
                        _testOutputHelper.WriteLine("This exception is expected!");
                    }
                    return x is Exception1;
                });
            }
        }

        [Fact]
        public void HandleNestedThreadException()
        {
            var task = Task.Factory.StartNew(() => {
                var child1 = Task.Factory.StartNew(() =>
                {
                    throw new Exception1("This Exception is excepted");
                }, TaskCreationOptions.AttachedToParent);
                var child2 = Task.Factory.StartNew(() =>
                {
                    throw new Exception1("This Exception is excepted");
                }, TaskCreationOptions.AttachedToParent);
            });

            try
            {
                task.Wait();
            }
            // #2
            catch (AggregateException ae) {
                ae.Handle( x => { // Handle an UnauthorizedAccessException
                    if (x is Exception1) {
                        _testOutputHelper.WriteLine("This exception is expected!!!!!!");
                    }
                    return x is Exception1;
                });
                
                // var handlers = new Dictionary<Type, Action<Exception>>(); handlers.Add(typeof(Exception1),
                //     ex => _testOutputHelper.WriteLine("This exception is expected!!!!!!"));
                //
                // if (!HandleAggregateError(ae, handlers))
                // {
                //     throw;
                // }
            }
        }
        
        private static bool HandleAggregateError( AggregateException aggregate,
            Dictionary<Type, Action<Exception>> exceptionHandlers)
        {
            foreach (var exception in aggregate.InnerExceptions)
            {
                if (exception is AggregateException agEx)
                {
                    if (!HandleAggregateError(agEx, exceptionHandlers)) {
                        return false; }

                    continue;
                }

                if (exceptionHandlers.ContainsKey( exception.GetType()))
                {
                    exceptionHandlers[exception.GetType()](exception);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
    }
    
    public class Exception1 : Exception
    {
        public Exception1(string msg):base(msg){}

        public Exception1()
        {
        }

        public Exception1(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
    public class Exception2 : Exception
    {
        public Exception2(string msg):base(msg){}

        public Exception2()
        {
        }

        public Exception2(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}