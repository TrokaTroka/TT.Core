using MongoDB.Driver;
using Microsoft.EntityFrameworkCore;
using TT.Core.Domain.Entities;
using TT.Core.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;

namespace TT.Core.Infra.Data.Repositories;

public class ResetPasswordRepository : IResetPasswordRepository
{
    private const string collectionName = "resetPasswords";
    private readonly MongoClient _client;

    public ResetPasswordRepository(IConfiguration configuration)
    {
        _client = new MongoClient(configuration.GetConnectionString("MongoDb"));
    }

    public async Task Add(ResetPassword resetPassword)
    {
        var server = _client.GetDatabase("trokatroka");

        await server.GetCollection<ResetPassword>(collectionName).InsertOneAsync(resetPassword);
    }

    public async Task<ResetPassword> GetByDocument(string document)
    {
        var server = _client.GetDatabase("trokatroka");

        var resetFind = await server.GetCollection<ResetPassword>(collectionName).FindAsync(rp => rp.Document == document);

        return await resetFind.FirstOrDefaultAsync();
    }

    public async Task<ResetPassword> GetByHashAndCpf(string hash, string document)
    {
        var server = _client.GetDatabase("trokatroka");

        var resetFind = await server.GetCollection<ResetPassword>(collectionName).FindAsync(rp => rp.Id == hash && rp.Document == document);

        return await resetFind.FirstOrDefaultAsync();
    }

    public async Task Update(ResetPassword resetPassword)
    {
        var server = _client.GetDatabase("trokatroka");

        await server.GetCollection<ResetPassword>(collectionName).ReplaceOneAsync(rp => rp.Id == resetPassword.Id, resetPassword);
    }

    public async Task Delete(ResetPassword resetPassword)
    {
        var server = _client.GetDatabase("trokatroka");

        await server.GetCollection<ResetPassword>(collectionName).DeleteOneAsync(rp => rp.Id == resetPassword.Id);
    }
}
