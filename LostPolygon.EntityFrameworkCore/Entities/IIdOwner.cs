namespace LostPolygon.EntityFrameworkCore.Entities;

public interface IIdOwner<out TId> where TId : struct {
    TId Id { get; }
}
