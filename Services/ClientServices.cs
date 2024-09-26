using Core.DTOs;
using Core.Models;
using Infrastructure.Repositories;

namespace Services
{
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateClient(Client Client)
        {
            if (Client != null)
            {
                await _unitOfWork.Client.Add(Client);
                var result = _unitOfWork.Save();
                return result > 0;
            }
            return false;
        }

        public async Task<bool> DeleteClient(int ClientId)
        {
            if (ClientId > 0)
            {
                var ClientDetail = await _unitOfWork.Client.GetById(ClientId);
                if (ClientDetail != null)
                {
                    _unitOfWork.Client.Delete(ClientDetail);
                    var result = _unitOfWork.Save();
                    return result > 0;
                }
            }
            return false;
        }

        public async Task<Client?> GetClientById(int ClientId)
        {
            if (ClientId > 0)
            {
                return await _unitOfWork.Client.GetById(ClientId);
            }
            return null;
        }

        public async Task<bool> UpdateClient(ClientRequest ClientParam, int ClientId)
        {
            if (ClientParam != null)
            {
                var Client = await _unitOfWork.Client.GetById(ClientId);
                if (Client != null)
                {

                    _unitOfWork.Client.Update(Client);
                    var result = _unitOfWork.Save();
                    return result > 0;
                }
            }
            return false;
        }
    }

    public interface IClientService
    {
        Task<bool> CreateClient(Client Client);
        Task<Client?> GetClientById(int ClientId);
        Task<bool> UpdateClient(ClientRequest ClientParam, int ClientId);
        Task<bool> DeleteClient(int ClientId);
    }
}
