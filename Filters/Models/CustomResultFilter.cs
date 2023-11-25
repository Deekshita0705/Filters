using Microsoft.AspNetCore.Mvc.Filters;
using System.Data.SqlClient;

namespace Filters.Models
{
    public class CustomResultFilter : ResultFilterAttribute
    {
        private readonly string connectionString = "Data Source=DKOTHA-L-5509\\SQLEXPRESS;Initial Catalog=Task1;Persist Security Info=True;User ID=sa;Password=Welcome2evoke@1234"; // Replace with your database connection string

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            // This method is called before the result is executed.
            LogMessage("OnResultExecuting", context.RouteData);
        }
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            // This method is called after the result has been executed.
            LogMessage("OnResultExecuted", context.RouteData);
        }
        private void LogMessage(string methodName, RouteData routeData)
        {
            var controllerName = routeData.Values["controller"];
            var actionName = routeData.Values["action"];
            string message = methodName + " Controller:" + controllerName + " Action:" + actionName + " Date: "
                            + DateTime.Now.ToString() + Environment.NewLine;

            // Save log data to the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("INSERT INTO LogTable (Message, LogDate) VALUES (@Message, @LogDate)", connection))
                {
                    command.Parameters.AddWithValue("@Message", message);
                    command.Parameters.AddWithValue("@LogDate", DateTime.Now);
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
