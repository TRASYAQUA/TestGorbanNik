using GorbanWPFTest.Services;

namespace GorbanWPFTest.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public ManufacturersViewModel ManufacturersVM { get; }
        public EquipmentTypesViewModel EquipmentTypesVM { get; }
        public CountriesViewModel CountriesVM { get; }
        public EquipmentItemsViewModel EquipmentItemsVM { get; }

        public MainWindowViewModel()
        {
            var apiRepository = new ApiRepository();

            ManufacturersVM = new ManufacturersViewModel(apiRepository);
            EquipmentTypesVM = new EquipmentTypesViewModel(apiRepository);
            CountriesVM = new CountriesViewModel(apiRepository);
            EquipmentItemsVM = new EquipmentItemsViewModel(apiRepository);
        }
    }
}