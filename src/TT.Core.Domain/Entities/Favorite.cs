using System;

namespace TT.Core.Domain.Entities;

public class Favorite : EntityBase
{
    public Favorite(Guid idUser, Guid idBook)
    {
        IdUser = idUser;
        IdBook = idBook;
    }

    public Guid IdUser { get; private set; }
    public Guid IdBook { get; private set; }

    public Book Book { get; set; }
    public Favorite()
    { }
}
