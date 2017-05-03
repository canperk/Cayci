using Cayci.Provider.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cayci.Provider.Repository
{
    public abstract class RepositoryBase
    {
        public RepositoryBase()
        {
            Context = new MongoContext();
        }
        protected MongoContext Context { get; set; }
    }
}
