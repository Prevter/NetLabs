using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace NetLabs.Labs.Lab07.Pages
{
    public class Task2Model : PageModel
    {
        public static readonly string CorrectLogin = System.IO.File.ReadAllText("login.txt");
        public static readonly string CorrectPassword = System.IO.File.ReadAllText("password.txt");
        
        public static readonly Dictionary<string, int> LoginAttempts = new();
        private int GetAttempts()
        {
            // get session id
            var id = HttpContext.Session.Id;

            // if there is no entry for this session id, create one
            if (!LoginAttempts.ContainsKey(id))
                LoginAttempts.Add(id, 0);

            // return the number of attempts
            return LoginAttempts[id];
        }

        private void SetAttempts(int value)
        {
            // get session id
            var id = HttpContext.Session.Id;

            // if there is no entry for this session id, create one
            if (!LoginAttempts.ContainsKey(id)) { 
                LoginAttempts.Add(id, value);
                return;
            }

            // set the number of attempts
            LoginAttempts[id] = value;
        }

        public int Attempts
        {
            get => GetAttempts();
            set => SetAttempts(value);
        }

        public void OnGet()
        {

        }
    }
}
