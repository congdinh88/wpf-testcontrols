using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<TestSuggestItem> testSuggestItems { get; set; }
        public ObservableCollection<TestDatagrid> testDatagrids { get; set; }=new ObservableCollection<TestDatagrid>();
        public MainWindow()
        {
            InitializeComponent();
            testSuggestItems = new ObservableCollection<TestSuggestItem>() { 
                new TestSuggestItem{Col1="1ad", Col2="2",Col3="3"},
                new TestSuggestItem{Col1="abc", Col2="b", Col3="c"},
            };
            dataGrid.ItemsSource=testDatagrids;
            DataContext = this;
        }
    }

    public class TestDatagrid
    {
        public string Txt1 { get; set; }
    }
    public class TestSuggestItem
    {
        public string Col1 { get; set; }
        public string Col2 { get; set; }
        public string Col3 { get; set; }
    }
}