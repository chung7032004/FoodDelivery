using FoodDelivery.Common;
using FoodDelivery.DTOs;

namespace FoodDelivery.Service.Interfaces;
public interface IAddressService
{
    Task <Result<AddressResponse>> GetAddressById(Guid addressId, Guid userId);
    Task <Result<List<AddressResponse>>> GetMyAddresses(Guid userId);
    Task <Result<AddAddressResponse>> AddAddress (Guid userId, AddAddressRequest addressRequest);
    Task <Result> UpdateAddress (Guid userId,Guid addressId, AddressRequest addressRequest);
    Task <Result> DeleteAddress (Guid userId, Guid addressId);
    Task <Result> SetDefault (Guid userId, Guid addressId);
}