using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Areas.Admin.Models;
using Project.DAL;

namespace Project.Areas.Admin.ViewComponents
{
   
    public class ContactMessageViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public ContactMessageViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var message =await _dbContext.ContactMessages.ToListAsync();
            var isALlReadMessages=message.All(x=> x.isRead);
            return View(new ContactMessageReadViewModel
            {
                contactMessages = message,
                isReadAllMessage = isALlReadMessages
            });
        }
    }
}
