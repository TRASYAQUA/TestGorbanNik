using GorbanWPFTest.Models;

namespace GorbanWPFTest.Services
{
    public interface IApiRepository
    {
        Task<List<Manufacturer>> GetManufacturersAsync();
        Task<Manufacturer> CreateManufacturerAsync(Manufacturer manufacturer);
        Task<Manufacturer> UpdateManufacturerAsync(Guid id, Manufacturer manufacturer);
        Task<bool> DeleteManufacturerAsync(Guid id);

        Task<List<EquipmentType>> GetEquipmentTypesAsync();
        Task<EquipmentType> CreateEquipmentTypeAsync(EquipmentType equipmentType);
        Task<EquipmentType> UpdateEquipmentTypeAsync(Guid id, EquipmentType equipmentType);
        Task<bool> DeleteEquipmentTypeAsync(Guid id);

        Task<List<Country>> GetCountriesAsync();
        Task<Country> CreateCountryAsync(Country country);
        Task<Country> UpdateCountryAsync(Guid id, Country country);
        Task<bool> DeleteCountryAsync(Guid id);

        Task<List<EquipmentItem>> GetEquipmentItemsAsync();
        Task<EquipmentItem> GetEquipmentItemAsync(Guid id);
        Task<List<EquipmentItem>> GetFilteredEquipmentItemsAsync(
            Guid? manufacturerId = null,
            Guid? equipmentTypeId = null,
            Guid? countryId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            string? search = null);
        Task<EquipmentItem> CreateEquipmentItemAsync(EquipmentItem equipmentItem);
        Task<EquipmentItem> UpdateEquipmentItemAsync(Guid id, EquipmentItem equipmentItem);
        Task<bool> DeleteEquipmentItemAsync(Guid id);
    }
}