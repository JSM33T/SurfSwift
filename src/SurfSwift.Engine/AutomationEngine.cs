using Microsoft.Playwright;
using Newtonsoft.Json;
using SurfSwift.Engine.Helpers;
using SurfSwift.Entities;
using System.Data;
using System.Diagnostics;
using System.Management;

namespace SurfSwift.Engine
{
    public interface IAutomationEngine
    {
        Task Initialize(AutomationConfig automationConfig);
    }

    public class AutomationEngine : IAutomationEngine
    {
        private static string GetCommandLine(int pid)
        {
            var query = "SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + pid;
            var searcher = new ManagementObjectSearcher(query);
            foreach (var o in searcher.Get())
            {
                var process = (ManagementObject)o;
                return process["CommandLine"]?.ToString() ?? string.Empty;
            }
            return string.Empty;
        }
        public async Task Initialize(AutomationConfig automationConfig)
        {
            var automationId = Guid.NewGuid();

            //var chromePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
            var chromePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
            var args = $"--remote-debugging-port=1212 --uid={automationId} --user-data-dir=\"C:\\chrome-profile\"";

            ProcessStartInfo psi = new()
            {
                FileName = chromePath,
                Arguments = args,
                UseShellExecute = false
            };

            Process.Start(psi);

            using var playwright2 = await Playwright.CreateAsync();

            var browser2 = await playwright2.Chromium.ConnectOverCDPAsync("http://localhost:1212");

            var context2 = browser2.Contexts[0];
            var page = context2.Pages.Count > 0 ? context2.Pages[0] : await context2.NewPageAsync();


            var json = await File.ReadAllTextAsync(automationConfig.ActionScript);

            var actionList = JsonConvert.DeserializeObject<List<DynamicAction>>(json);

            //var page2 = await context2.NewPageAsync();

            var actionQueue = new Queue<DynamicAction>(actionList);

            while (actionQueue.Count > 0)
            {
                var actionData = actionQueue.Dequeue();

                if (actionData.IsBypassed)
                    continue;

                switch (actionData.Action?.ToLower())
                {
                    case "click":
                        // Wait for the element and click
                        await page.WaitForSelectorAsync(actionData.Selector);
                        await page.ClickAsync(actionData.Selector);
                        break;

                    //case "fill":
                    //    // Wait for the element and fill it
                    //    await page.WaitForSelectorAsync(actionData.Selector);
                    //    await page.FillAsync(actionData.Selector, actionData.Element);
                    //    break;

                    case "fill":
                        await ActionHelper.ExecuteWithWait(page, () => page.FillAsync(actionData.Selector, actionData.Element), actionData.Selector);
                        break;

                    // case "type":
                    //     await ActionHelper.ExecuteWithWait(page, () => page.TypeAsync(actionData.Selector, actionData.Element), actionData.Selector);
                    //     break;

                    case "check":
                        await ActionHelper.ExecuteWithWait(page, () => page.CheckAsync(actionData.Selector), actionData.Selector);
                        break;

                    case "uncheck":
                        await ActionHelper.ExecuteWithWait(page, () => page.UncheckAsync(actionData.Selector), actionData.Selector);
                        break;

                    case "hover":
                        await ActionHelper.ExecuteWithWait(page, () => page.HoverAsync(actionData.Selector), actionData.Selector);
                        break;

                    case "navigate":

                        await page.EvaluateAsync(@"() => { Object.defineProperty(navigator, 'webdriver', { get: () => undefined }); }");

                        if (!string.IsNullOrEmpty(actionData.Selector))
                            await page.GotoAsync(actionData.Selector);
                        else
                            Console.WriteLine("URL not provided for navigation.");
                        break;

                    case "wait":
                        await ActionHelper.WaitForElement(page, actionData.Selector);
                        break;

                    case "exec":
                        if (!string.IsNullOrEmpty(actionData.Selector))
                        {
                            var result = await page.EvaluateAsync<string>(actionData.Selector);
                            Console.WriteLine($"JS Execution Result: {result}");
                        }
                        else
                            Console.WriteLine("JavaScript code not provided for exec action.");
                        break;

                    case "select":
                        await ActionHelper.ExecuteWithWait(page,
                            () => page.SelectOptionAsync(actionData.Selector, actionData.Element), actionData.Selector);
                        break;

                    case "press":
                        await ActionHelper.ExecuteWithWait(page,
                            () => page.PressAsync(actionData.Selector, actionData.Element), actionData.Selector);
                        break;

                    case "scroll":
                        await page.EvaluateAsync("(selector) => document.querySelector(selector).scrollIntoView()", actionData.Selector);
                        break;

                    case "screenshot":
                        await page.ScreenshotAsync(new PageScreenshotOptions { Path = "screenshot.png" });
                        break;

                    case "gettext":
                        Console.WriteLine($"Text Content: {await page.InnerTextAsync(actionData.Selector!)}");
                        break;

                    case "getattribute":
                        Console.WriteLine($"Attribute Value: {await page.GetAttributeAsync(actionData.Selector, actionData.Element)}");
                        break;

                    case "exists":
                        Console.WriteLine($"Element Exists: {await page.QuerySelectorAsync(actionData.Selector) != null}");
                        break;

                    case "waitfortimeout":
                        await Task.Delay(int.Parse(actionData.Element));
                        break;

                    case "upload":
                        await ActionHelper.ExecuteWithWait(page, () => page.SetInputFilesAsync(actionData.Selector, actionData.Element), actionData.Selector);
                        break;

                    case "focus":
                        await ActionHelper.ExecuteWithWait(page, () => page.FocusAsync(actionData.Selector), actionData.Selector);
                        break;

                    case "decision":
                        var element = await page.QuerySelectorAsync(actionData.Selector);
                        var queue = element != null
                            ? actionData.OnSuccess
                            : actionData.OnFailure;

                        if (queue != null)
                        {
                            Console.WriteLine($"Condition {(element != null
                                ? "met"
                                : "not met")}," +
                                              $" executing actions...");

                            foreach (var action in queue)
                                actionQueue.Enqueue(action);
                        }
                        break;

                    case "gettable":
                        var table = await DataTableHelper.ExtractTableAsync(page, actionData.Selector);
                        Console.WriteLine($"Extracted Table '{actionData.Element}':");
                        foreach (DataRow row in table.Rows)
                        {
                            Console.WriteLine(string.Join(" | ", row.ItemArray));
                        }
                        break;


                    case "repeat":
                        if (actionData.Repeat > 0 && actionData.Steps != null)
                        {
                            // Enqueue the steps multiple times
                            for (int i = 0; i < actionData.Repeat; i++)
                            {
                                foreach (var step in actionData.Steps)
                                {
                                    actionQueue.Enqueue(step);
                                }
                            }
                        }
                        break;

                    case "download":
                        // Get the selector for the download button
                        var downloadSelector = actionData.Selector;

                        // Get the download path from parameters
                        var downloadPath = actionData.DownloadPath;

                        // Listen for the download event
                        var downloadTask = page.WaitForDownloadAsync();

                        // Perform the click action to trigger the download
                        await page.ClickAsync(downloadSelector!);

                        // Wait for the download to finish and save the file to the specified path
                        var download = await downloadTask;
                        await download.SaveAsAsync(downloadPath!);
                        Console.WriteLine($"File downloaded to {downloadPath}");
                        break;

                    case "kill":
                        var processes = Process.GetProcessesByName("chrome");
                        foreach (var process in processes)
                        {
                            var commandLine = GetCommandLine(process.Id);

                            if (!commandLine.Contains($"--uid={automationId}"))
                                continue;

                            Console.WriteLine($"Found process with PID: {process.Id}");

                            // Kill the process
                            process.Kill();
                            Console.WriteLine("Process killed.");
                        }
                        Console.WriteLine("Chrome started with remote debugging on port 9222.");
                        break;

                    default:
                        Console.WriteLine($"Unknown action: {actionData.Action}");
                        break;
                }
            }
            await browser2.CloseAsync();
        }
    }
}
