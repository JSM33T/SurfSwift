using Microsoft.Playwright;

namespace SurfSwift.Engine.Helpers
{
    /// <summary>
    /// Contains helper methods for interacting with Playwright page elements, including waiting, executing actions with wait, and retrying actions.
    /// </summary>
    public static class ActionHelper
    {
        /// <summary>
        /// Waits for an element to appear on the page within a specified timeout.
        /// </summary>
        /// <param name="page">The Playwright page instance.</param>
        /// <param name="selector">The CSS selector for the element to wait for.</param>
        /// <param name="timeoutMs">The timeout duration in milliseconds (default is 5000ms).</param>
        public static async Task WaitForElement(IPage page, string selector, int timeoutMs = 5000)
        {
            await page.Locator(selector).WaitForAsync(new LocatorWaitForOptions
            {
                State = null,
                Timeout = timeoutMs
            });
        }

        /// <summary>
        /// Executes an action after waiting for a specific element to appear on the page.
        /// </summary>
        /// <param name="page">The Playwright page instance.</param>
        /// <param name="action">The action to execute after waiting for the element.</param>
        /// <param name="selector">The CSS selector for the element to wait for.</param>
        /// <param name="timeoutMs">The timeout duration in milliseconds (default is 5000ms).</param>
        public static async Task ExecuteWithWait(IPage page, Func<Task> action, string selector, int timeoutMs = 5000)
        {
            await WaitForElement(page, selector, timeoutMs);
            await action();
        }

        /// <summary>
        /// Retries an action a specified number of times with a delay between attempts in case of failure.
        /// </summary>
        /// <param name="action">The action to retry.</param>
        /// <param name="retries">The number of retries (default is 3).</param>
        /// <param name="delayMs">The delay between retries in milliseconds (default is 500ms).</param>
        public static async Task RetryAction(Func<Task> action, int retries = 3, int delayMs = 500)
        {
            for (var i = 0; i < retries; i++)
            {
                try
                {
                    await action();
                    return;
                }
                catch (Exception)
                {
                    if (i == retries - 1) throw;
                    await Task.Delay(delayMs * (i + 1));
                }
            }
        }
    }
}
