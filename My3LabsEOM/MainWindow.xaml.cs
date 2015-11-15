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
using lab1;

namespace My3LabsEOM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double[][] data = new double[4][];       
        private double[] b = new double[] {1.02,1.00,1.43,5.6 };
        private Label diffLabelNM;
        private Label integralInitialResult = new Label();
        private Label integralResult = new Label();
        private Label integralMistake = new Label();
        private Label diffMistake = new Label();
            
                
        
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            data[0] = new double[] { 0.31, 0.14, 0.30, 0.27 };
            data[1] = new double[] { 0.26, 0.32, 0.18, 0.24 };
            data[2] = new double[] { 0.61, 0.22, 0.20, 0.31 };
            data[3] = new double[] { 0.40, 0.34, 0.36, 0.17 };

            
            //init matrix
            for (int j = 0; j < data[0].Length; j++ )
                for (int i = 0; i < data[0].Length; i++)
                    ((TextBox)matrixGrid.Children[i + j*4]).Text = data[j][i].ToString();

            //init vector b
            for (int i = 0; i < b.Length; i++)
                ((TextBox)vectorB.Children[i]).Text = b[i].ToString();

            Lab2Integral l = new Lab2Integral(0.2, 1, 80, 0.01);
            
            try
            {
                //l.FillValueTable();
                var values = l.All();
                valuesGrid.ItemsSource = values;
                var count = valuesGrid.Columns.Count;
                
            }catch(Exception ex)
            {
                MessageBox.Show("Problem with database occurs");
            }

            diffLabelNM = new Label();
            resultPanel.Children.Add(diffLabelNM);
            resultPanel.Children.Add(integralInitialResult);
            resultPanel.Children.Add(integralResult);
            resultPanel.Children.Add(integralMistake);
            resultPanel.Children.Add(diffMistake);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < matrixGrid.Children.Count; i++)
            {
                try
                {                    
                    data[(int)i / 4][i % 4] = Double.Parse(((TextBox)matrixGrid.Children[i]).Text);
                }
                catch (Exception ex)
                {
                    result.Content = ex.Message + " at [" + ((int)i / 4 + 1) + "], [" + (i % 4 + 1) + "] element.";
                    return;
                }
            }

            for(int i = 0; i < vectorB.Children.Count; i++)
            {
                try
                {
                    b[i] = Double.Parse(((TextBox)vectorB.Children[i]).Text);
                }
                catch (Exception ex)
                {
                    result.Content = ex.Message + " at [" + (i + 1) + "] element in vector b.";
                    return;
                    
                }
            }

            Gauss g = new Gauss(4,4);
            double []res = g.Solve(data,b);

            StringBuilder resStrBldr = new StringBuilder();
            for (int i = 0; i < res.Length; i++)
            {
                resStrBldr.Append(res[i]);
                if(i != res.Length - 1)
                    resStrBldr.Append(";\n ");
            }
            result.Content = "Result is \n[ " + resStrBldr + " ]";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            double left_bound = 0;
            double right_bound = 0;
            double interval = 0;
            int num_of_points = 0;
            double freePoint = 0;
            try
            {
                left_bound = Double.Parse(left.Text);
                right_bound = Double.Parse(right.Text);
                interval = Double.Parse(intervalTextBox.Text);
                num_of_points = int.Parse(numOfPoints.Text);
                freePoint = Double.Parse(freePointTextBox.Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Very bad result: " + ex.Message);
                return;
            }

            Lab2Integral lab2 = new Lab2Integral(left_bound, right_bound, num_of_points, interval);

            
            integralResult.Content = "This is result of integration: " + lab2.RightRectangle();

            try
            {
                integralInitialResult.Content = "This is result of Newton formula: " + lab2.InitialSum();
                
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            diffLabelNM.Content = "This is result of differentiation: " + lab2.NumericDifferentiate(freePoint);

            integralMistake.Content = "This is integration fault: "+Math.Abs(lab2.RightRectangle() - lab2.InitialSum());

            diffMistake.Content = "This is differentiation fault: " + Math.Abs(lab2.NumericDifferentiate(freePoint) -
                                                                            lab2.underIntegralFunction(freePoint));


        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            double interval = 0;
            double a = 0;
            double b = 0;
            try{
                a = Double.Parse(aTextBox.Text);
                b = Double.Parse(bTextBox.Text);
                interval = Double.Parse(intervalLab3TextBox.Text);

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            Lab3 lab3 = new Lab3(a, b, interval);
            var solution = lab3.Solve();
            var result = solution.Select(s => new { X = s.Key, Y = s.Value });
            lab3ResultDataGrid.ItemsSource = result;
        }
    }
}
