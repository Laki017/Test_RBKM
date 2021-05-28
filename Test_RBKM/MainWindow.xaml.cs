using System;
using System.IO;
using System.Reflection;
using System.Windows;

namespace Test_RBKM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            PathTextBlock.Text = Directory.GetCurrentDirectory();
        }

        private string SearchClassMethods(DirectoryInfo directory)
        {
            string outData = "";
            FileInfo[] files = directory.GetFiles();

            foreach(FileInfo file in files)
            {
                if (file.Extension == ".dll")
                {
                    try
                    {
                        Assembly asm = Assembly.LoadFrom(file.FullName);
                        Type[] types = asm.GetTypes();
                        outData += file.Name + "\n";
                        foreach (Type t in types)
                        {
                            if (t.IsClass)
                                outData += "\t" + t.Name + "\n";
                            foreach (MethodInfo method in t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.NonPublic))
                            {
                                if (method.IsPublic) outData += "\t\t- " + method.Name + "\n";
                                if (method.IsFamily) outData += "\t\t- " + method.Name + "\n";

                            }
                        }
                    }
                    catch (Exception) { }
                }
            }
            return outData;
        }

        private void EnterPath_Click(object sender, RoutedEventArgs e)
        {
            if (PathTextBlock.Text == "") { return; }      
            DirectoryInfo directory = new DirectoryInfo(PathTextBlock.Text);

            if (directory.Exists)
            {
                OutData.Text = SearchClassMethods(directory);
            }
            else
            {
                OutData.Text = "Path entry error!";
            }
        }
    }
}
