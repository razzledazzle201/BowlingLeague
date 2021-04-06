using BowlingLeague.Models;
using BowlingLeague.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BowlingLeague.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private BowlingLeagueContext _context { get; set; }

        public HomeController(ILogger<HomeController> logger, BowlingLeagueContext con)
        {
            _logger = logger;
            _context = con;
        }

        public IActionResult Index(long? teamId, string teamName, int pageNum = 0)
        {
            int pageSize = 5;

            return View(new IndexViewModel
            {
                //get list of Bowlers fo the page
                Bowlers = _context.Bowlers
                    .Where(x => x.TeamId == teamId || teamId == null)
                    .OrderBy(x => x.BowlerFirstName)
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize)
                    .ToList(),

                PageNumInfo = new PageNumInfo
                {
                    NumItemsPerPage = pageSize,
                    CurrentPage = pageNum,

                    //if no team selected then get the full count, Else only count the 
                    //selected bowlers 
                    TotalNumItems = (teamId == null ? _context.Bowlers.Count()
                        : _context.Bowlers.Where(x => x.TeamId == teamId).Count())
                },

                Team = teamName

            });


        }

        public IActionResult Party()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}