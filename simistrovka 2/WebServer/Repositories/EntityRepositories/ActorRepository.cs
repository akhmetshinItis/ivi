using System.Linq.Expressions;
using HttpServerLibrary.Models;
using Microsoft.Data.SqlClient;
using my_http.Models.Entities;
using MyORMLibrary;

namespace my_http.Repositories.EntityRepositories
{
    public class ActorRepository : IRepository<Actor>
    {
        private readonly ORMContext<Actor> _dbContext;

        public ActorRepository()
        { 
            var connection = new SqlConnection(AppConfig.GetInstance().ConnectionString);
            _dbContext = new ORMContext<Actor>(connection);
        }
        
        public List<Actor> GetAll()
        {
            return _dbContext.GetAll();
        }

        public void Delete(Actor entity)
        {
            _dbContext.Delete(entity);
        }

        public void Update(Actor entity)
        {
            _dbContext.Update(entity);
        }

        public Actor GetById(int id)
        {
            return _dbContext.GetById(id);
        }

        public void Create(Actor entity)
        {
            _dbContext.Create(entity);
        }

        public IEnumerable<Actor> Where(Expression<Func<Actor, bool>> predicate)
        {
            return _dbContext.Where(predicate);
        }

        public Actor FirstOrDefault(Expression<Func<Actor, bool>> predicate)
        {
            return _dbContext.FirstOrDefault(predicate);
        }
    }
}