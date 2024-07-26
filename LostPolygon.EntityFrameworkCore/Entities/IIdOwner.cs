namespace LostPolygon.EntityFrameworkCore.Entities;

public interface IIdOwner<out TId> {
    TId Id { get; }
}
