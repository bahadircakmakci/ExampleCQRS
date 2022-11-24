using ExampleCQRS.Application.Commands.Books;
using ExampleCQRS.Application.Queries.Books;
using ExampleCQRS.Domain.Entities.Concrete;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExampleCQRS.Pages.Books
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;
        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }
        [BindProperty]
        public List<Book> Books { get; set; }
        public async Task OnGetAsync()
        {
            Thread.Sleep(500);
            Books = await _mediator.Send(new GetBookQueryList());
        }
        public async Task<IActionResult> OnGetDeleteAsync(string id)
        {
            await _mediator.Send(new DeleteBookCommand(Guid.Parse(id)));
            return RedirectToPage("Index");
        }
    }
}
