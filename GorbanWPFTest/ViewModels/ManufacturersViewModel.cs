using System.Collections.ObjectModel;
using System.Windows;
using GorbanWPFTest.Helpers;
using GorbanWPFTest.Models;
using GorbanWPFTest.Services;

namespace GorbanWPFTest.ViewModels
{
    public class ManufacturersViewModel : BaseViewModel
    {
        private readonly IApiRepository _apiRepository;
        private ObservableCollection<Manufacturer> _manufacturers;
        private Manufacturer _selectedManufacturer;
        private string _newName;
        private string _newDescription;
        private bool _isLoading;

        public ObservableCollection<Manufacturer> Manufacturers
        {
            get => _manufacturers;
            set => SetProperty(ref _manufacturers, value);
        }

        public Manufacturer SelectedManufacturer
        {
            get => _selectedManufacturer;
            set
            {
                SetProperty(ref _selectedManufacturer, value);
                if (value != null)
                {
                    NewName = value.Name;
                    NewDescription = value.Description;
                }
            }
        }

        public string NewName
        {
            get => _newName;
            set => SetProperty(ref _newName, value);
        }

        public string NewDescription
        {
            get => _newDescription;
            set => SetProperty(ref _newDescription, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public RelayCommand LoadCommand { get; }
        public RelayCommand AddCommand { get; }
        public RelayCommand UpdateCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand ClearCommand { get; }

        public ManufacturersViewModel(IApiRepository apiRepository)
        {
            _apiRepository = apiRepository;
            Manufacturers = new ObservableCollection<Manufacturer>();

            LoadCommand = new RelayCommand(async () => await LoadManufacturersAsync());
            AddCommand = new RelayCommand(async () => await AddManufacturerAsync(), () => !string.IsNullOrWhiteSpace(NewName));
            UpdateCommand = new RelayCommand(async () => await UpdateManufacturerAsync(), () => SelectedManufacturer != null && !string.IsNullOrWhiteSpace(NewName));
            DeleteCommand = new RelayCommand(async () => await DeleteManufacturerAsync(), () => SelectedManufacturer != null);
            ClearCommand = new RelayCommand(ClearFields);

            LoadCommand.Execute(null);
        }

        private async Task LoadManufacturersAsync()
        {
            IsLoading = true;
            try
            {
                var items = await _apiRepository.GetManufacturersAsync();
                Manufacturers.Clear();
                foreach (var item in items)
                {
                    Manufacturers.Add(item);
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

        private async Task AddManufacturerAsync()
        {
            try
            {
                var newManufacturer = new Manufacturer
                {
                    Name = NewName,
                    Description = NewDescription
                };

                var result = await _apiRepository.CreateManufacturerAsync(newManufacturer);
                Manufacturers.Add(result);
                ClearFields();
                MessageBox.Show("Производитель добавлен!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task UpdateManufacturerAsync()
        {
            try
            {
                SelectedManufacturer.Name = NewName;
                SelectedManufacturer.Description = NewDescription;

                var result = await _apiRepository.UpdateManufacturerAsync(SelectedManufacturer.Id, SelectedManufacturer);

                var index = Manufacturers.IndexOf(SelectedManufacturer);
                Manufacturers[index] = result;

                ClearFields();
                MessageBox.Show("Производитель обновлен!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task DeleteManufacturerAsync()
        {
            if (MessageBox.Show("Удалить производителя?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    await _apiRepository.DeleteManufacturerAsync(SelectedManufacturer.Id);
                    Manufacturers.Remove(SelectedManufacturer);
                    ClearFields();
                    MessageBox.Show("Производитель удален!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ClearFields()
        {
            NewName = string.Empty;
            NewDescription = string.Empty;
            SelectedManufacturer = null;
        }
    }
}