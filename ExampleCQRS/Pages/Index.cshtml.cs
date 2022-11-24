using ExampleCQRS.Application.Commands.Books;
using ExampleCQRS.Application.Queries.Books;
using ExampleCQRS.Domain.Entities.Concrete;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExampleCQRS.Pages
{
    public class IndexModel : PageModel
    {
       
       
        public async Task OnGet()
        {
            
        }
         
    }
}