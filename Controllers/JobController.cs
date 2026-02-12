using AiJobAnalyzer.ViewModels;
using Microsoft.AspNetCore.Mvc;

public class JobController : Controller
{
    private readonly OpenAiService _ai;

    public JobController(OpenAiService ai)
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

        vm.Result = await _ai.AnalyzeJobAsync(vm.JobText);
        return View(vm);
    }
}
