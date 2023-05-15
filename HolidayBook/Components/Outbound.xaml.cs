using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HolidayBook.ViewModels;

namespace HolidayBook.Components
{
    /// <summary>
    /// Interaction logic for Outbound.xaml
    /// </summary>
    public partial class Outbound : UserControl
    {
        public Outbound()
        {
            InitializeComponent();
            this.DataContext = new OutboundViewModel(this);
        }
    }
}
