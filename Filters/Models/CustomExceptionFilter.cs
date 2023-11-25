using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Data.SqlClient;

namespace Filters.Models
{
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        private readonly string connectionString = "Data Source=DKOTHA-L-5509\\SQLEXPRESS;Initial Catalog=Task1;Persist Security Info=True;User ID=sa;Password=Welcome2evoke@1234"; // Replace with your database connection string

        public override void OnException(ExceptionContext context)
        {
            var controllerName = context.RouteData.Values["controller"].ToString();
            var actionName = context.RouteData.Values["action"].ToString();
            var exceptionMessage = context.Exception.Message;

            LogException(controllerName, actionName, exceptionMessage);

            // Additional handling if needed, e.g., returning a custom error response
            context.Result = new JsonResult(new { error = "An error occurred." })
            {
                StatusCode = 500,
            };

            // Ensure the exception is marked as handled
            context.ExceptionHandled = true;
        }

        private void LogException(string controllerName, string actionName, string exceptionMessage)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("INSERT INTO LogTable (Message, LogDate) VALUES (@Message, @LogDate)", connection))
                {
                    command.Parameters.AddWithValue("@Message", $"Controller: {controllerName}, Action: {actionName}, Exception: {exceptionMessage}");
                    command.Parameters.AddWithValue("@LogDate", DateTime.Now);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}

