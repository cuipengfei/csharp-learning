using System;
using Xunit;

namespace csharp_learning
{
    // The reason for different exception classes—in fact,
    // the only reason—is to make it easier to take different actions when
    // developers using your API write catch handlers.

    public class UnitTest47
    {
        private void HandleExceptionsV1()
        {
            try
            {
                Foo();
                Bar();
            }
            catch (MyFirstApplicationException e1)
            {
                FixProblem(e1);
            }
            catch (AnotherApplicationException e2)
            {
                ReportErrorAndContinue(e2);
            }
            catch (YetAnotherApplicationException e3)
            {
                ReportErrorAndShutdown(e3);
            }
            catch (Exception e)
            {
                ReportGenericError(e);
                throw;
            }
            finally
            {
                CleanupResource();
            }
        }

        private void HandleExceptionsV2()
        {
            try
            {
                Foo();
                Bar();
            }
            catch (Exception e)
            {
                switch (e.TargetSite.Name)
                {
                    case "Foo":
                        FixProblem(e);
                        break;
                    case "Bar":
                        ReportErrorAndContinue(e);
                        break;
                    default:
                        ReportErrorAndShutdown(e);
                        throw;
                }
            }
            finally
            {
                CleanupResource();
            }
        }

        private void CleanupResource()
        {
        }

        private void ReportGenericError(Exception exception)
        {
        }

        private void ReportErrorAndShutdown(Exception e3)
        {
        }

        private void ReportErrorAndContinue(Exception e2)
        {
        }

        private void FixProblem(Exception e1)
        {
        }

        private void Bar()
        {
        }

        private void Foo()
        {
        }
    }

    internal class YetAnotherApplicationException : Exception
    {
    }

    internal class AnotherApplicationException : Exception
    {
    }

    internal class MyFirstApplicationException : Exception
    {
    }
}