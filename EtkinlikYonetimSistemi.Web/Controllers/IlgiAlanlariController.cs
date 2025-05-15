using EtkinlikYonetimSistemi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EtkinlikYonetimSistemi.Web.Controllers
{
    public class IlgiAlanlariController : Controller
    {
        private readonly IIlgiAlaniRepository _ilgiAlaniRepository;

        public IlgiAlanlariController(IIlgiAlaniRepository ilgiAlaniRepository)
        {
            _ilgiAlaniRepository = ilgiAlaniRepository;
        }

        public async Task<IActionResult> Index()
        {
            var ilgiAlanlari = await _ilgiAlaniRepository.GetAllAsync();
            return View(ilgiAlanlari);
        }
    }
}
