using Microsoft.EntityFrameworkCore;
using TT.Core.Domain.Entities;
using TT.Core.Domain.Interfaces.Repositories;
using TT.Core.Infra.Data.Context;

namespace TT.Core.Infra.Data.Repositories;

public class PhotoRepository : BaseRepository<PhotosBook> ,IPhotoRepository
{
    public PhotoRepository(TrokaTrokaDbContext context) : base(context)
    { }
    public async Task<List<PhotosBook>> GetByIdBook(Guid idBook)
    {
        return await _context.PhotosBooks.Where(c => c.IdBook == idBook).ToListAsync();
    }
    public async Task CreateMany(List<PhotosBook> photos)
    {
        await _context.PhotosBooks.AddRangeAsync(photos);
        await _context.SaveChangesAsync();
    }
}
