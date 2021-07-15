using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.IdentityEntities;
using ministryofjusticeDomain.Interfaces;
using ministryofjusticeDomain.Interfaces.Repository;

namespace ministryofjusticeDomain.Repositories
{
     public class CommentRepo: GenericRepo<Comment>, ICommentRepo
    {
        private new readonly ApplicationDbContext _context;

        public CommentRepo(ApplicationDbContext context)
        :base(context)
        {
            _context = context;
        }

        
    }
}
