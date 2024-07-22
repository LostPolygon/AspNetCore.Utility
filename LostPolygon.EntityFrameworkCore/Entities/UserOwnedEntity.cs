using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LostPolygon.EntityFrameworkCore.Entities;

namespace LostPolygon.EntityFrameworkCore;

public abstract class UserOwnedEntity<TUserId, TId> : IIdOwner<TId> where TId : struct {
    [Key]
    [Column(Order = 1)]
    public TId Id { get; protected set; } = default!;

    [Column(Order = 2)]
    public TUserId UserId { get; set; }

    protected UserOwnedEntity() {
        UserId = default!;
    }

    protected UserOwnedEntity(TUserId userId) {
        UserId = userId;
    }
}

public abstract class UserOwnedEntity<TUserId> : UserOwnedEntity<TUserId, int> {
    protected UserOwnedEntity() {
    }

    protected UserOwnedEntity(TUserId userId) : base(userId) {
    }
}
