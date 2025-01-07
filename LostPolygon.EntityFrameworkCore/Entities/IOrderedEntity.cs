namespace LostPolygon.EntityFrameworkCore;

public interface IOrderedEntity {
    int Order { get; set; }
}
