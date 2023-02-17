using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Learning_1.Model;

namespace Learning_1.ViewModel
{

    public class SinhVienViewModel
    {
        private ObservableCollection<SinhVien> listsv = new ObservableCollection<SinhVien>();
        public ICommand DeleteCommand;
        public ICommand AddCommand;
        public ObservableCollection<SinhVien> Listsv
        {
            get { return listsv; }
            set { listsv = value; }
        }
        public SinhVienViewModel()
        {
            Listsv.Add(new SinhVien() { Id = 1, Name = "ABC" });
            Listsv.Add(new SinhVien() { Id = 2, Name = "DEF" });
            Listsv.Add(new SinhVien() { Id = 3, Name = "GHJ" });
            DeleteCommand = new RelayCommand<object>((p) => p!=null, (p) =>
            {
                Listsv.Remove(p as SinhVien);
            });
            AddCommand = new RelayCommand<UIElementCollection>((p) => true, p => {
                int id = 0;
                string name = "";
                bool isIDInt = false;
                foreach(var item in p)
                {
                    TextBox tb = item as TextBox;
                    if (tb == null) continue;
                    switch(tb.Name)
                    {
                        case "tb_id":
                            isIDInt = Int32.TryParse(tb.Text, out id);
                            break;
                        case "tb_name":
                            name = tb.Text;
                            break;
                    }
                }
                if(!isIDInt || string.IsNullOrEmpty(name))
                {
                    return;
                }
                SinhVien a = new SinhVien() { Id = id, Name = name };
                Listsv.Add(a);
            });
        }
        
    }
}
