using System.Collections.ObjectModel;
using System.Windows;
using GorbanWPFTest.Helpers;
using GorbanWPFTest.Models;
using GorbanWPFTest.Services;

namespace GorbanWPFTest.ViewModels
{
    public class CountriesViewModel : BaseViewModel
    {
        private readonly IApiRepository _apiRepository;
        private ObservableCollection<Country> _countries;
        private Country _selectedCountry;
        private string _newName;
        private string _newCode;
        private bool _isLoading;

        public ObservableCollection<Country> Countries
        {
            get => _countries;
            set => SetProperty(ref _countries, value);
        }

        public Country SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                SetProperty(ref _selectedCountry, value);
                if (value != null)
                {
                    NewName = value.Name;
                    NewCode = value.Code;
                }
            }
        }

        public string NewName
        {
            get => _newName;
            set => SetProperty(ref _newName, value);
        }

        public string NewCode
        {
            get => _newCode;
            set => SetProperty(ref _newCode, value);
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

        public CountriesViewModel(IApiRepository apiRepository)
        {
            _apiRepository = apiRepository;
            Countries = new ObservableCollection<Country>();

            LoadCommand = new RelayCommand(async () => await LoadCountriesAsync());
            AddCommand = new RelayCommand(async () => await AddCountryAsync(), () => !string.IsNullOrWhiteSpace(NewName));
            UpdateCommand = new RelayCommand(async () => await UpdateCountryAsync(), () => SelectedCountry != null && !string.IsNullOrWhiteSpace(NewName));
            DeleteCommand = new RelayCommand(async () => await DeleteCountryAsync(), () => SelectedCountry != null);
            ClearCommand = new RelayCommand(ClearFields);

            LoadCommand.Execute(null);
        }

        private async Task LoadCountriesAsync()
        {
            IsLoading = true;
            try
            {
                var items = await _apiRepository.GetCountriesAsync();
                Countries.Clear();
                foreach (var item in items)
                {
                    Countries.Add(item);
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

        private async Task AddCountryAsync()
        {
            try
            {
                var newCountry = new Country
                {
                    Name = NewName,
                    Code = NewCode
                };

                var result = await _apiRepository.CreateCountryAsync(newCountry);
                Countries.Add(result);
                ClearFields();
                MessageBox.Show("Страна добавлена!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task UpdateCountryAsync()
        {
            try
            {
                SelectedCountry.Name = NewName;
                SelectedCountry.Code = NewCode;

                var result = await _apiRepository.UpdateCountryAsync(SelectedCountry.Id, SelectedCountry);

                var index = Countries.IndexOf(SelectedCountry);
                Countries[index] = result;

                ClearFields();
                MessageBox.Show("Страна обновлена!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task DeleteCountryAsync()
        {
            if (MessageBox.Show("Удалить страну?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    await _apiRepository.DeleteCountryAsync(SelectedCountry.Id);
                    Countries.Remove(SelectedCountry);
                    ClearFields();
                    MessageBox.Show("Страна удалена!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
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
            NewCode = string.Empty;
            SelectedCountry = null;
        }
    }
}