using EA.WebApiServerSide.Model;
using EA.WebApiServerSide.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EA.WebApiServerSide
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        #region Default

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //public void ConfigureServices(IServiceCollection services)
        //{
        //    string con = "Server=(localdb)\\mssqllocaldb;Database=usersdbstore;Trusted_Connection=True;MultipleActiveResultSets=true";
        //    services.AddDbContext<UsersContext>(options => options.UseSqlServer(con));
        //    services.AddMvc();
        //}

        //public IConfiguration Configuration { get; }

        //// This method gets called by the runtime. Use this method to add services to the container.
        //public void ConfigureServices(IServiceCollection services)
        //{
        //    services.AddMvc();
        //}

        //// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        //{
        //    if (env.IsDevelopment())
        //    {
        //        app.UseDeveloperExceptionPage();
        //    }

        //    app.UseMvc();
        //}

        #endregion Default

        public void ConfigureServices(IServiceCollection services)
        {
            //DB name DbSetEmployee Deprecated
            // string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=person;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            // services.AddDbContext<EmployeeContext>(options => options.UseSqlServer(connectionString));
            // Connection string from appsettings

            services.AddDbContext<EmployeeContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("EmployeeDbConnection")));
            // Регистрация через DI
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }
    }
}