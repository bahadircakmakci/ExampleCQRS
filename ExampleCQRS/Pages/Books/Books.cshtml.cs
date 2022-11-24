using ExampleCQRS.Application.Commands.Books;
using ExampleCQRS.Application.Queries.Books;
using ExampleCQRS.Domain.Entities.Concrete;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace ExampleCQRS.Pages.Books
{
    public class BooksModel : PageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public Book Book { get; set; }
        public BooksModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task OnGetAsync(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                Book = await _mediator.Send(new GetBookQueryById(Guid.Parse(id)));
            }

        }
        public async Task<IActionResult> OnPostAsync()
        {
            await _mediator.Send(new AddOrEditBookCommand(Book));
            return RedirectToPage("Index");
        }
    }
}
