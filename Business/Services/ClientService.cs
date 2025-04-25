using Business.Models;
using Data.Interfaces;
using Data.Repositories;
using Domain.Extensions;

namespace Business.Services;

public interface IClientService
{
    Task<ClientResult> GetClientByIdAsync(string clientId);
    Task<ClientResult> GetClientsAsync();
}

public class ClientService(IClientRepository clientRepository) : IClientService
{
    private readonly IClientRepository _clientRepository = clientRepository;


    public async Task<ClientResult> GetClientsAsync()
    {
        var result = await _clientRepository.GetAllAsync();
        return result.MapTo<ClientResult>();
    }

    public async Task<ClientResult> GetClientByIdAsync(string clientId)
    {
        var result = await _clientRepository.GetAsync();
        return result.MapTo<ClientResult>();
    }

}