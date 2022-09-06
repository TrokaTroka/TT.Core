using TT.Core.Domain.Entities;

namespace TT.Core.Domain.Interfaces.Repositories;

public interface IPhotoRepository : IBaseRepository<PhotosBook>
{
    Task<List<PhotosBook>> GetByIdBook(Guid idBook);
    Task CreateMany(List<PhotosBook> photo);
}
