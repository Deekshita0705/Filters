using Filters.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Data.SqlClient;

namespace Filters.Models
{

    [AttributeUsage(AttributeTargets.Method)]
    public class AuthenticationFilterAttribute : ActionFilterAttribute
    {
        private readonly string connectionString = "Data Source=DKOTHA-L-5509\\SQLEXPRESS;Initial Catalog=Task1;Persist Security Info=True;User ID=sa;Password=Welcome2evoke@1234"; // Replace with your database connection string

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var email = filterContext.HttpContext.Request.Form["email"];
            var password = filterContext.HttpContext.Request.Form["password"];

            // Check if the user exists in the database
            var user = GetUserByEmailAndPassword(email, password);

            if (user != null)
            {
                // Log successful authentication
                LogAuthenticationSuccess(user.Email);
                var controller = filterContext.Controller as UserController;
                if (controller != null)
                {
                   
                    controller.ViewBag.Message = $"Authentication Success: {user.Email}";
                }
                
            }
            //else if(user == null)
            //{
                
            //    AddNewUser(email, password);
            //    LogNewUserAdded(email);
            //    var controller = filterContext.Controller as UserController;
            //    if (controller != null)
            //    {
            //        controller.ViewBag.Message = $"New User Added: {email}";
            //    }
            //}
            else 
            {
                // Add the new user to the database
                //LogAuthenticationFailure(email);

                var controller = filterContext.Controller as UserController;
                if (controller != null)
                {
                    controller.ViewBag.Message = $"Authentication Failure: Invalid email or password";
                }

            }

            base.OnActionExecuting(filterContext);
        }
        private User GetUserByEmailAndPassword(string email, string password)
        {
            var query = "SELECT Email, Password FROM Users WHERE Email = @Email";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var storedPassword = (string)reader["Password"];

                            if (password == storedPassword)
                            {
                                return new User
                                {
                                    Email = (string)reader["Email"],
                                    Password = storedPassword
                                };
                            }
                            else
                            {
                                
                                LogAuthenticationFailure(email);
                                return null;
                            }
                        }
                    }
                }
            }
            return null;
        }


        private void AddNewUser(string email, string password)
        {
            var query = $"INSERT INTO Users (Email, Password) VALUES ('{email}', '{password}')";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
            }
        }

        private void LogAuthenticationSuccess(string email)
        {
            Log("Authentication Success", email);
        }

        private void LogNewUserAdded(string email)
        {
            Log("New User Added", email);
        }
        private void LogAuthenticationFailure(string email)
        {
            Log("Authentication Failure", email);
        }
        private void Log(string message, string email)
        {
            var logEntry = new LogEntry
            {
                Message = $"{message}: {email}",
                DateTime = DateTime.Now
            };

            // This is a simple example, you should use parameterized queries to prevent SQL injection
            var query = $"INSERT INTO LogTable (Message, LogDate) VALUES ('{logEntry.Message}', '{logEntry.DateTime}')";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
            }
        }
    }

}


