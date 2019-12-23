using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace Restaurant.Data
{
    public class RestaurantAppContextFactory : IDesignTimeDbContextFactory<RestaurantAppContext>
    {
        public RestaurantAppContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .Build();

            var builder = new DbContextOptionsBuilder<RestaurantAppContext>();

            builder.UseSqlServer("Server=.;Database=RestaurantApp;Trusted_Connection=True;MultipleActiveResultSets=true");

            builder.ConfigureWarnings(w => w.Throw(RelationalEventId.QueryClientEvaluationWarning));

            return new RestaurantAppContext(builder.Options);
        }
    }
}
