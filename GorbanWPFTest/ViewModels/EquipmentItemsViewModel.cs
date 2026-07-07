using System.Collections.ObjectModel;
using System.Windows;
using GorbanWPFTest.Helpers;
using GorbanWPFTest.Models;
using GorbanWPFTest.Services;

namespace GorbanWPFTest.ViewModels
{
    public class EquipmentItemsViewModel : BaseViewModel
    {
        private readonly IApiRepository _apiRepository;
        private ObservableCollection<EquipmentItem> _equipmentItems;
        private EquipmentItem _selectedItem;
        private string _searchText;
        private Guid? _selectedManufacturerId;
        private Guid? _selectedEquipmentTypeId;
        private Guid? _selectedCountryId;
        private decimal? _minPrice;
        private decimal? _maxPrice;
        private bool _isLoading;

        public ObservableCollection<EquipmentItem> EquipmentItems
        {
            get => _equipmentItems;
            set => SetProperty(ref _equipmentItems, value);
        }

        public EquipmentItem SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        public Guid? SelectedManufacturerId
        {
            get => _selectedManufacturerId;
            set => SetProperty(ref _selectedManufacturerId, value);
        }

        public Guid? SelectedEquipmentTypeId
        {
            get => _selectedEquipmentTypeId;
            set => SetProperty(ref _selectedEquipmentTypeId, value);
        }

        public Guid? SelectedCountryId
        {
            get => _selectedCountryId;
            set => SetProperty(ref _selectedCountryId, value);
        }

        public decimal? MinPrice
        {
            get => _minPrice;
            set => SetProperty(ref _minPrice, value);
        }

        public decimal? MaxPrice
        {
            get => _maxPrice;
            set => SetProperty(ref _maxPrice, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public RelayCommand LoadCommand { get; }
        public RelayCommand SearchCommand { get; }
        public RelayCommand ClearFiltersCommand { get; }

        public EquipmentItemsViewModel(IApiRepository apiRepository)
        {
            _apiRepository = apiRepository;
            EquipmentItems = new ObservableCollection<EquipmentItem>();

            LoadCommand = new RelayCommand(async () => await LoadItemsAsync());
            SearchCommand = new RelayCommand(async () => await SearchItemsAsync());
            ClearFiltersCommand = new RelayCommand(ClearFilters);

            LoadCommand.Execute(null);
        }

        private async Task LoadItemsAsync()
        {
            IsLoading = true;
            try
            {
                var items = await _apiRepository.GetEquipmentItemsAsync();
                EquipmentItems.Clear();
                foreach (var item in items)
                {
                    EquipmentItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task SearchItemsAsync()
        {
            IsLoading = true;
            try
            {
                var items = await _apiRepository.GetFilteredEquipmentItemsAsync(
                    SelectedManufacturerId,
                    SelectedEquipmentTypeId,
                    SelectedCountryId,
                    MinPrice,
                    MaxPrice,
                    SearchText);

                EquipmentItems.Clear();
                foreach (var item in items)
                {
                    EquipmentItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка поиска: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ClearFilters()
        {
            SearchText = string.Empty;
            SelectedManufacturerId = null;
            SelectedEquipmentTypeId = null;
            SelectedCountryId = null;
            MinPrice = null;
            MaxPrice = null;
            SearchCommand.Execute(null);
        }
    }
}