using System.Collections.ObjectModel;
using System.Windows;
using GorbanWPFTest.Helpers;
using GorbanWPFTest.Models;
using GorbanWPFTest.Services;

namespace GorbanWPFTest.ViewModels
{
    public class EquipmentTypesViewModel : BaseViewModel
    {
        private readonly IApiRepository _apiRepository;
        private ObservableCollection<EquipmentType> _equipmentTypes;
        private EquipmentType _selectedEquipmentType;
        private string _newName;
        private string _newDescription;
        private bool _isLoading;

        public ObservableCollection<EquipmentType> EquipmentTypes
        {
            get => _equipmentTypes;
            set => SetProperty(ref _equipmentTypes, value);
        }

        public EquipmentType SelectedEquipmentType
        {
            get => _selectedEquipmentType;
            set
            {
                SetProperty(ref _selectedEquipmentType, value);
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

        public EquipmentTypesViewModel(IApiRepository apiRepository)
        {
            _apiRepository = apiRepository;
            EquipmentTypes = new ObservableCollection<EquipmentType>();

            LoadCommand = new RelayCommand(async () => await LoadEquipmentTypesAsync());
            AddCommand = new RelayCommand(async () => await AddEquipmentTypeAsync(), () => !string.IsNullOrWhiteSpace(NewName));
            UpdateCommand = new RelayCommand(async () => await UpdateEquipmentTypeAsync(), () => SelectedEquipmentType != null && !string.IsNullOrWhiteSpace(NewName));
            DeleteCommand = new RelayCommand(async () => await DeleteEquipmentTypeAsync(), () => SelectedEquipmentType != null);
            ClearCommand = new RelayCommand(ClearFields);

            LoadCommand.Execute(null);
        }

        private async Task LoadEquipmentTypesAsync()
        {
            IsLoading = true;
            try
            {
                var items = await _apiRepository.GetEquipmentTypesAsync();
                EquipmentTypes.Clear();
                foreach (var item in items)
                {
                    EquipmentTypes.Add(item);
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

        private async Task AddEquipmentTypeAsync()
        {
            try
            {
                var newEquipmentType = new EquipmentType
                {
                    Name = NewName,
                    Description = NewDescription
                };

                var result = await _apiRepository.CreateEquipmentTypeAsync(newEquipmentType);
                EquipmentTypes.Add(result);
                ClearFields();
                MessageBox.Show("Тип техники добавлен!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task UpdateEquipmentTypeAsync()
        {
            try
            {
                SelectedEquipmentType.Name = NewName;
                SelectedEquipmentType.Description = NewDescription;

                var result = await _apiRepository.UpdateEquipmentTypeAsync(SelectedEquipmentType.Id, SelectedEquipmentType);

                var index = EquipmentTypes.IndexOf(SelectedEquipmentType);
                EquipmentTypes[index] = result;

                ClearFields();
                MessageBox.Show("Тип техники обновлен!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task DeleteEquipmentTypeAsync()
        {
            if (MessageBox.Show("Удалить тип техники?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    await _apiRepository.DeleteEquipmentTypeAsync(SelectedEquipmentType.Id);
                    EquipmentTypes.Remove(SelectedEquipmentType);
                    ClearFields();
                    MessageBox.Show("Тип техники удален!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
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
            SelectedEquipmentType = null;
        }
    }
}