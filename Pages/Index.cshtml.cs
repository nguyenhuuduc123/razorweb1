using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using razorweb.models;

namespace apprazor.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly MyBlogContext _myblog;
    public IndexModel(ILogger<IndexModel> logger,MyBlogContext myblog)
    {
        _logger = logger;
        _myblog = myblog;
    }
    
    public void OnGet()
    {
            var posts = (from a in _myblog.articles 
                        orderby a.Created
                        select a).ToList();
                ViewData["posts"] = posts;
            
    }
}
