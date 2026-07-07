using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using GorbanWPFTest.Models;
using RestSharp;

namespace GorbanWPFTest.Services
{
    public class ApiRepository : IApiRepository
    {
        private readonly RestClient _client;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiRepository()
        {
            _client = new RestClient("http://localhost:5006/api/");
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                ReferenceHandler = ReferenceHandler.Preserve,
                MaxDepth = 64
            };
        }

        private void Log(string message)
        {
            Console.WriteLine(message);
            Debug.WriteLine(message);
        }

        private async Task<T> ExecuteRequestAsync<T>(RestRequest request)
        {
            var method = request.Method.ToString();
            var resource = request.Resource;

            Log($"========================================");
            Log($"ЗАПРОС: {method} {resource}");
            Log($"Время: {DateTime.Now:HH:mm:ss.fff}");

            try
            {
                var response = await _client.ExecuteAsync(request);

                Log($"СТАТУС: {(int)response.StatusCode} {response.StatusCode}");
                Log($"ЗАГОЛОВКИ ОТВЕТА:");
                if (response.Headers != null)
                {
                    foreach (var header in response.Headers)
                    {
                        Log($"  {header.Name}: {header.Value}");
                    }
                }

                if (!response.IsSuccessful)
                {
                    Log($"ОШИБКА: {response.StatusCode} - {response.ErrorMessage}");
                    Log($"ТЕЛО ОШИБКИ: {response.Content ?? "null"}");
                    throw new Exception($"Ошибка API: {response.StatusCode} - {response.ErrorMessage}");
                }

                if (string.IsNullOrEmpty(response.Content))
                {
                    Log($"ТЕЛО ОТВЕТА: <пусто>");
                    return default!;
                }

                Log($"ТЕЛО ОТВЕТА (длина {response.Content.Length} символов):");
                Log(response.Content);
                Log($"========================================");

                try
                {
                    var result = JsonSerializer.Deserialize<T>(response.Content, _jsonOptions)!;
                    Log($"ДЕСЕРИАЛИЗАЦИЯ УСПЕШНА: {typeof(T).Name}");
                    return result;
                }
                catch (JsonException jsonEx)
                {
                    Log($"ОШИБКА ДЕСЕРИАЛИЗАЦИИ: {jsonEx.Message}");
                    Log($"ПОЗИЦИЯ: LineNumber={jsonEx.LineNumber}, BytePosition={jsonEx.BytePositionInLine}");

                    if (jsonEx.Path != null)
                    {
                        Log($"ПУТЬ: {jsonEx.Path}");
                    }

                    Log($"ТИП ОЖИДАЛСЯ: {typeof(T).Name}");
                    Log($"========================================");
                    throw;
                }
            }
            catch (Exception ex)
            {
                Log($"ИСКЛЮЧЕНИЕ: {ex.GetType().Name}");
                Log($"СООБЩЕНИЕ: {ex.Message}");
                Log($"СТЕК: {ex.StackTrace}");
                Log($"========================================");
                throw;
            }
        }

        private void LogJson(string label, object data)
        {
            try
            {
                var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
                Log($"{label}:");
                Log(json);
            }
            catch
            {
                Log($"{label}: {data}");
            }
        }

        public async Task<List<Manufacturer>> GetManufacturersAsync()
        {
            var request = new RestRequest("manufacturers", Method.Get);
            return await ExecuteRequestAsync<List<Manufacturer>>(request);
        }

        public async Task<Manufacturer> CreateManufacturerAsync(Manufacturer manufacturer)
        {
            Log("СОЗДАНИЕ ПРОИЗВОДИТЕЛЯ:");
            LogJson("ДАННЫЕ", manufacturer);

            var request = new RestRequest("manufacturers", Method.Post);
            var json = JsonSerializer.Serialize(manufacturer);
            Log($"JSON ЗАПРОСА: {json}");
            request.AddJsonBody(json);

            return await ExecuteRequestAsync<Manufacturer>(request);
        }

        public async Task<Manufacturer> UpdateManufacturerAsync(Guid id, Manufacturer manufacturer)
        {
            Log($"ОБНОВЛЕНИЕ ПРОИЗВОДИТЕЛЯ ID: {id}");
            LogJson("ДАННЫЕ", manufacturer);

            var request = new RestRequest($"manufacturers/{id}", Method.Put);
            var json = JsonSerializer.Serialize(manufacturer);
            Log($"JSON ЗАПРОСА: {json}");
            request.AddJsonBody(json);

            return await ExecuteRequestAsync<Manufacturer>(request);
        }

        public async Task<bool> DeleteManufacturerAsync(Guid id)
        {
            Log($"УДАЛЕНИЕ ПРОИЗВОДИТЕЛЯ ID: {id}");
            var request = new RestRequest($"manufacturers/{id}", Method.Delete);
            var response = await _client.ExecuteAsync(request);

            Log($"СТАТУС УДАЛЕНИЯ: {(int)response.StatusCode} {response.StatusCode}");
            return response.IsSuccessful;
        }

        public async Task<List<EquipmentType>> GetEquipmentTypesAsync()
        {
            var request = new RestRequest("equipmenttypes", Method.Get);
            return await ExecuteRequestAsync<List<EquipmentType>>(request);
        }

        public async Task<EquipmentType> CreateEquipmentTypeAsync(EquipmentType equipmentType)
        {
            Log("СОЗДАНИЕ ТИПА ТЕХНИКИ:");
            LogJson("ДАННЫЕ", equipmentType);

            var request = new RestRequest("equipmenttypes", Method.Post);
            var json = JsonSerializer.Serialize(equipmentType);
            Log($"JSON ЗАПРОСА: {json}");
            request.AddJsonBody(json);

            return await ExecuteRequestAsync<EquipmentType>(request);
        }

        public async Task<EquipmentType> UpdateEquipmentTypeAsync(Guid id, EquipmentType equipmentType)
        {
            Log($"ОБНОВЛЕНИЕ ТИПА ТЕХНИКИ ID: {id}");
            LogJson("ДАННЫЕ", equipmentType);

            var request = new RestRequest($"equipmenttypes/{id}", Method.Put);
            var json = JsonSerializer.Serialize(equipmentType);
            Log($"JSON ЗАПРОСА: {json}");
            request.AddJsonBody(json);

            return await ExecuteRequestAsync<EquipmentType>(request);
        }

        public async Task<bool> DeleteEquipmentTypeAsync(Guid id)
        {
            Log($"УДАЛЕНИЕ ТИПА ТЕХНИКИ ID: {id}");
            var request = new RestRequest($"equipmenttypes/{id}", Method.Delete);
            var response = await _client.ExecuteAsync(request);

            Log($"СТАТУС УДАЛЕНИЯ: {(int)response.StatusCode} {response.StatusCode}");
            return response.IsSuccessful;
        }

        public async Task<List<Country>> GetCountriesAsync()
        {
            var request = new RestRequest("countries", Method.Get);
            return await ExecuteRequestAsync<List<Country>>(request);
        }

        public async Task<Country> CreateCountryAsync(Country country)
        {
            Log("СОЗДАНИЕ СТРАНЫ:");
            LogJson("ДАННЫЕ", country);

            var request = new RestRequest("countries", Method.Post);
            var json = JsonSerializer.Serialize(country);
            Log($"JSON ЗАПРОСА: {json}");
            request.AddJsonBody(json);

            return await ExecuteRequestAsync<Country>(request);
        }

        public async Task<Country> UpdateCountryAsync(Guid id, Country country)
        {
            Log($"ОБНОВЛЕНИЕ СТРАНЫ ID: {id}");
            LogJson("ДАННЫЕ", country);

            var request = new RestRequest($"countries/{id}", Method.Put);
            var json = JsonSerializer.Serialize(country);
            Log($"JSON ЗАПРОСА: {json}");
            request.AddJsonBody(json);

            return await ExecuteRequestAsync<Country>(request);
        }

        public async Task<bool> DeleteCountryAsync(Guid id)
        {
            Log($"УДАЛЕНИЕ СТРАНЫ ID: {id}");
            var request = new RestRequest($"countries/{id}", Method.Delete);
            var response = await _client.ExecuteAsync(request);

            Log($"СТАТУС УДАЛЕНИЯ: {(int)response.StatusCode} {response.StatusCode}");
            return response.IsSuccessful;
        }

        public async Task<List<EquipmentItem>> GetEquipmentItemsAsync()
        {
            var request = new RestRequest("equipmentitems", Method.Get);
            return await ExecuteRequestAsync<List<EquipmentItem>>(request);
        }

        public async Task<EquipmentItem> GetEquipmentItemAsync(Guid id)
        {
            Log($"ПОЛУЧЕНИЕ ПОЗИЦИИ ID: {id}");
            var request = new RestRequest($"equipmentitems/{id}", Method.Get);
            return await ExecuteRequestAsync<EquipmentItem>(request);
        }

        public async Task<List<EquipmentItem>> GetFilteredEquipmentItemsAsync(
            Guid? manufacturerId = null,
            Guid? equipmentTypeId = null,
            Guid? countryId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            string? search = null)
        {
            Log("ФИЛЬТРАЦИЯ ПОЗИЦИЙ:");
            Log($"  manufacturerId: {manufacturerId}");
            Log($"  equipmentTypeId: {equipmentTypeId}");
            Log($"  countryId: {countryId}");
            Log($"  minPrice: {minPrice}");
            Log($"  maxPrice: {maxPrice}");
            Log($"  search: {search ?? "null"}");

            var request = new RestRequest("equipmentitems/filter", Method.Get);

            if (manufacturerId.HasValue)
                request.AddParameter("manufacturerId", manufacturerId.Value);
            if (equipmentTypeId.HasValue)
                request.AddParameter("equipmentTypeId", equipmentTypeId.Value);
            if (countryId.HasValue)
                request.AddParameter("countryId", countryId.Value);
            if (minPrice.HasValue)
                request.AddParameter("minPrice", minPrice.Value);
            if (maxPrice.HasValue)
                request.AddParameter("maxPrice", maxPrice.Value);
            if (!string.IsNullOrWhiteSpace(search))
                request.AddParameter("search", search);

            return await ExecuteRequestAsync<List<EquipmentItem>>(request);
        }

        public async Task<EquipmentItem> CreateEquipmentItemAsync(EquipmentItem equipmentItem)
        {
            Log("СОЗДАНИЕ ПОЗИЦИИ ТЕХНИКИ:");
            LogJson("ДАННЫЕ", equipmentItem);

            var request = new RestRequest("equipmentitems", Method.Post);
            var json = JsonSerializer.Serialize(equipmentItem);
            Log($"JSON ЗАПРОСА: {json}");
            request.AddJsonBody(json);

            return await ExecuteRequestAsync<EquipmentItem>(request);
        }

        public async Task<EquipmentItem> UpdateEquipmentItemAsync(Guid id, EquipmentItem equipmentItem)
        {
            Log($"ОБНОВЛЕНИЕ ПОЗИЦИИ ID: {id}");
            LogJson("ДАННЫЕ", equipmentItem);

            var request = new RestRequest($"equipmentitems/{id}", Method.Put);
            var json = JsonSerializer.Serialize(equipmentItem);
            Log($"JSON ЗАПРОСА: {json}");
            request.AddJsonBody(json);

            return await ExecuteRequestAsync<EquipmentItem>(request);
        }

        public async Task<bool> DeleteEquipmentItemAsync(Guid id)
        {
            Log($"УДАЛЕНИЕ ПОЗИЦИИ ID: {id}");
            var request = new RestRequest($"equipmentitems/{id}", Method.Delete);
            var response = await _client.ExecuteAsync(request);

            Log($"СТАТУС УДАЛЕНИЯ: {(int)response.StatusCode} {response.StatusCode}");
            return response.IsSuccessful;
        }
    }
}