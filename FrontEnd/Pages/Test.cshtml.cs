using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FrontEnd.DatabaseConnection;
using FrontEnd.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages
{
    public class TestModel : PageModel
    {
        public List<User> Users;
        public void OnGet()
        {
           Users =  DB.GetUsersList();
            Trace.WriteLine("koniec geta");
        }
    }
}
