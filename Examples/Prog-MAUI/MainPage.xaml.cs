using System.Collections.ObjectModel;

namespace Prog_MAUI
{
    public partial class MainPage : ContentPage
    {
        public string lastURL = "";
        public ObservableCollection<string> Inventory { get; } = new ObservableCollection<string>();

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            Version.Text = "Yoctopuce library version : "+YAPI.GetAPIVersion();
        }

        private void OnUpdateClicked(object? sender, EventArgs e)
        {
            string url = TargetURL.Text;
            if (url != lastURL) {
                if (lastURL != "") {
                    YAPI.UnregisterHub(lastURL);
                }
                string errmsg = "";
                int res = YAPI.RegisterHub(url, ref errmsg);
                if (res != YAPI.SUCCESS) {
                    DisplayAlert("Erreur", errmsg, "OK");
                    return;
                }
                lastURL = url;
            }
            Inventory.Clear();
            YModule module = YModule.FirstModule();
            while (module != null) {
                Inventory.Add(module.get_serialNumber());
                module = module.nextModule();
            }
        }
    }
}