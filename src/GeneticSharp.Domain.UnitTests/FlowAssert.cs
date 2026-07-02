using System;
using System.Diagnostics;
using NUnit.Framework;
using NUnit.Framework.Internal;
using NUnit.Framework.Legacy;

namespace GeneticSharp.Domain.UnitTests
{
    /// <summary>
    /// Asserts for execution flows.
    /// </summary>
    public static class FlowAssert
    {
        /// <summary>
        /// Asserts if at leas one execution flow run without exception..
        /// </summary>
        /// <param name="flows">The Execution flows.</param>
        public static void IsAtLeastOneOk(params Action[] flows)
        {
            if (flows == null)
            {
                throw new ArgumentNullException("flows");
            }

            bool ok = false;

            foreach (var a in flows)
            {
                try
                {
                    a();
                    ok = true;
                    break;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    Debug.WriteLine(ex.StackTrace);
                    ok = false;
                    DiscardRecordedAssertionFailure();
                }
            }

            ClassicAssert.IsTrue(ok);
        }

        /// <summary>
        /// Tries to run the execution flow until there is no exception or until hit the max attempts.
        /// </summary>
        /// <param name="maxAttempts">The max attempts.</param>
        /// <param name="flow">The execution flow.</param>
        public static void IsAtLeastOneAttemptOk(int maxAttempts, Action flow)
        {
            bool ok = false;
            string failedMessage = null;

            for(int i = 0; i < maxAttempts; i++)
            {
                try
                {
                    flow();
                    ok = true;
                    break;
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    Debug.WriteLine(ex.StackTrace);
                    ok = false;
                    failedMessage = ex.Message;
                    DiscardRecordedAssertionFailure();
                }
            }

            ClassicAssert.IsTrue(ok, $"All {maxAttempts} attempts failed\n\n{failedMessage}");
        }

        /// <summary>
        /// NUnit 4 permanently records every failed assertion into the current test's result
        /// (used to build "Multiple failures" reports), even if the thrown AssertionException
        /// is caught and the attempt is retried. Without discarding it here, a single failed
        /// attempt would fail the overall test even though a later attempt succeeds.
        /// </summary>
        private static void DiscardRecordedAssertionFailure()
        {
            TestExecutionContext.CurrentContext.CurrentResult.AssertionResults.Clear();
        }
    }
}
