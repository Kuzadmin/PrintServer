using FastReport;
using FastReportDesigner;
using Microsoft.Win32;
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

namespace FastReportClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string path_template;
        private string path_xml;

        private void Update()
        {
            lblTemplate.ToolTip = path_template;
            lblTemplate.Content = path_template;
            lblFile.ToolTip = path_xml;
            lblFile.Content = path_xml;
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnOpenOrCreate_Click(object sender, RoutedEventArgs e)
        {
            ObjectClass obj = new ObjectClass(path_template, path_xml);
            obj.OpenDoc();
        }

        private void btnTemplateOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myDialog = new OpenFileDialog();
            myDialog.Filter = "Шаблон(*.frx)|*.frx" + "|Все файлы (*.*)|*.*";
            myDialog.CheckFileExists = true;
            myDialog.Multiselect = true;
            if (myDialog.ShowDialog() == true)
            {
                path_template = myDialog.FileName;
                Update();
            }
        }

        private void btnFileOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myDialog = new OpenFileDialog();
            myDialog.Filter = "XML(*.xml)|*.xml" + "|Все файлы (*.*)|*.*";
            myDialog.CheckFileExists = true;
            myDialog.Multiselect = true;
            if (myDialog.ShowDialog() == true)
            {
                path_xml = myDialog.FileName;
                Update();
            }
        }

        private void btnTemplateClear_Click(object sender, RoutedEventArgs e)
        {
            path_template = "";
            Update();
        }

        private void btnFileClear_Click(object sender, RoutedEventArgs e)
        {
            path_xml = "";
            Update();
        }
    }
}
