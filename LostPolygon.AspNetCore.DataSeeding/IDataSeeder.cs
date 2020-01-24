using System.Threading;
using System.Threading.Tasks;

namespace LostPolygon.Mvc.DataSeeding {
    public interface IDataSeeder {
        Task Seed(CancellationToken cancellationToken);
    }
}
