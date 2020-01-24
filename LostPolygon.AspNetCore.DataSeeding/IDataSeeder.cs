using System.Threading;
using System.Threading.Tasks;

namespace LostPolygon.AspNetCore.DataSeeding {
    public interface IDataSeeder {
        Task Seed(CancellationToken cancellationToken);
    }
}
