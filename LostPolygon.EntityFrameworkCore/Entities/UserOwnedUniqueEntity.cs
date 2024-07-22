using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LostPolygon.EntityFrameworkCore;

public abstract class UserOwnedUniqueEntity<TUserId> {
    [Key]
    [Column(Order = 1)]
    public TUserId UserId { get; set; }

    protected UserOwnedUniqueEntity() {
        UserId = default!;
    }

    protected UserOwnedUniqueEntity(TUserId userId) {
        UserId = userId;
    }
}
