using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Project.Data;
using Constants = Project.Data.Constants;

namespace Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =Constants.AdminRole)]
    public class BaseController : Controller
    {
        
    }
}
