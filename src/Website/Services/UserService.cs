using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Models.Database;
using Website.Repositories;

namespace Website.Services
{
    public class UserService : IUserService
    {
        private IGenericRepository<AspNetUsers> UserRepository;
        private IGenericRepository<SolrQueryResponseRecord> SolrQueryResponseRepository;

        public UserService(IGenericRepository<AspNetUsers> userRepository, IGenericRepository<SolrQueryResponseRecord> solrQueryResponseRepository)
        {
            UserRepository = userRepository;
            SolrQueryResponseRepository = solrQueryResponseRepository;
        }

        public AspNetUsers GetUser(string userName)
        {
            return UserRepository.FirstOrDefault(u => u.UserName == userName);
        }        
    }
}
