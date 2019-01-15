using LiveCharts;
using LiveCharts.Defaults;
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

namespace STOLP
{
    public partial class MainWindow : Window
    {


        public ChartValues<ObservablePoint> ValuesA { get; set; }
        public ChartValues<ObservablePoint> ValuesB { get; set; }
        public ChartValues<ObservablePoint> ValuesC { get; set; }
        public ChartValues<ObservablePoint> ValuesD { get; set; }

        private Stolp stolp = new Stolp();
        private List<Data> data;

        public MainWindow()
        {
            InitializeComponent();

            var r = new Random();
            ValuesA = new ChartValues<ObservablePoint>();
            ValuesB = new ChartValues<ObservablePoint>();
            ValuesC= new ChartValues<ObservablePoint>();
            ValuesD= new ChartValues<ObservablePoint>();

            DataContext = this;

        }

        private void LoadLearningDataSetBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = System.IO.Directory.GetParent(Environment.CurrentDirectory).FullName;

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string fileName = dlg.FileName;
                learningDataSetFileNameLabel.Content = System.IO.Path.GetFileName(fileName);
                data = Parser.ParseJson(fileName);
                
            }
        }

        private void ClassificateButton_Click(object sender, RoutedEventArgs e)
        {
            if (data != null)
            {
                ValuesA.Clear();
                ValuesB.Clear();
                ValuesC.Clear();
                ValuesD.Clear();

                List<Data> newData = new List<Data>();
                List<Data> omega = stolp.stolp(data, int.Parse(deltaTextBox.Text), int.Parse(l0TextBox.Text));
                int maxItemCount = int.Parse(count.Text);

                for (int i = 0; i < maxItemCount; i++)
                {
                    for (int j = 0; j < maxItemCount; j++)
                    {
                        Data obj = new Data(new double[] { i / (double)maxItemCount, j / (double)maxItemCount }, -1);
                        obj.ObjClass = stolp.classifier(omega, obj);
                        switch (obj.ObjClass)
                        {
                            case 0: ValuesA.Add(new ObservablePoint(obj.Attributes[0], obj.Attributes[1])); break;
                            case 1: ValuesB.Add(new ObservablePoint(obj.Attributes[0], obj.Attributes[1])); break;
                        }
                        newData.Add(obj);
                    }
                }

                stolp.stolp(data, int.Parse(deltaTextBox.Text), int.Parse(l0TextBox.Text)).ForEach(i =>
                {

                    ValuesC.Add(new ObservablePoint(i.Attributes[0], i.Attributes[1]));

                });

                List<Data> standarts = new List<Data>();
                //standarts.AddRange(stolp.findStandard(data));
                standarts.AddRange(stolp.findStandard(stolp.emissionСutOff(data, int.Parse(deltaTextBox.Text))));
                standarts.ForEach(i =>
                {

                    ValuesD.Add(new ObservablePoint(i.Attributes[0], i.Attributes[1]));

                });
            } else
            {
                MessageBox.Show("Сперва Выберите Файл c обучающей выборкой");
            }
                /*data.ForEach(i =>
                {
                    switch (i.ObjClass)
                    {
                        case 0: ValuesA.Add(new ObservablePoint(i.Attributes[0], i.Attributes[1])); break;
                        case 1: ValuesB.Add(new ObservablePoint(i.Attributes[0], i.Attributes[1])); break;
                    }
                });

                */

        }

        private void DrawLearningDataSetButton_Click(object sender, RoutedEventArgs e)
        {
            if (data != null)
            {
                ValuesA.Clear();
                ValuesB.Clear();
                ValuesC.Clear();
                ValuesD.Clear();

                data.ForEach(i =>
                {
                    switch (i.ObjClass)
                    {
                        case 0: ValuesA.Add(new ObservablePoint(i.Attributes[0], i.Attributes[1])); break;
                        case 1: ValuesB.Add(new ObservablePoint(i.Attributes[0], i.Attributes[1])); break;
                    }
                });

                stolp.stolp(data, int.Parse(deltaTextBox.Text), int.Parse(l0TextBox.Text)).ForEach(i =>
                {

                    ValuesC.Add(new ObservablePoint(i.Attributes[0], i.Attributes[1]));

                });

                List<Data> standarts = new List<Data>();
                standarts.AddRange(stolp.findStandard(stolp.emissionСutOff(data, int.Parse(deltaTextBox.Text))));
                //standarts.AddRange(stolp.findStandard(data));

                standarts.ForEach(i =>
                {

                    ValuesD.Add(new ObservablePoint(i.Attributes[0], i.Attributes[1]));

                });
            }
        }
    }
}
