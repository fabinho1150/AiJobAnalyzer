using Microsoft.AspNetCore.Mvc;
using AiJobAnalyzer.ViewModels;

namespace AiJobAnalyzer.Controllers;

public class HomeController : Controller
{
    private readonly OpenAiService _ai;

    public HomeController(OpenAiService ai)
    {
        _ai = ai;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new JobAnalyzeViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Index(JobAnalyzeViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var result = await _ai.AnalyzeJobAsync(vm.JobText);

        vm.Result = result.result;
        vm.IsDemoMode = result.isDemo;

        return View(vm);
    }
}
