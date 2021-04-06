using BowlingLeague.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BowlingLeague.Components
{
    public class TeamViewComponent : ViewComponent
    {
        private BowlingLeagueContext context;
        //constructor
        public TeamViewComponent(BowlingLeagueContext ctx)
        {
            context = ctx;
        }

        //method tells us what we should do when view compnet is called 
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedTeam = RouteData?.Values["teamName"];

            //this is what we will return when this view is called
            return View(context.Teams
                .Distinct()
                .OrderBy(x => x));
        }

    }
}