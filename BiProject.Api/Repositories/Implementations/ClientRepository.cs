using BiProject.Api.Data.Oltp;
using BiProject.Api.Data.Oltp.Entities;
using BiProject.Api.Repositories.Interfaces;

namespace BiProject.Api.Repositories.Implementations
{
    public class ClientRepository : OltpRepository<Client>, IClientRepository
    {
        public ClientRepository(OltpDbContext context) : base(context)
        {
        }
    }
}
