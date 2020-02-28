using System;
using System.Windows;
using System.Windows.Media;

namespace InstructionInput
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //ControlPlaceholder.Children.Add(new InputControl("1,\"Rd = \",1A,\" + \",1A")
            //ControlPlaceholder.Children.Add(new InputControl("2,\"Rd = \",1A,\" + \",1A,\" + C\"")
            //ControlPlaceholder.Children.Add(new InputControl("3,\"Rdh: Rdl = Rdh:\",1A,\" + \",9A")
            //ControlPlaceholder.Children.Add(new InputControl("4,\"Rd = \",1A,\" - \",1A")
            ControlPlaceholder.Children.Add(new InputControl("5,\"Rd = \",1A,\" - \",9B")
            {
                OnDone = (instruction) =>
                {
                    Console.WriteLine("La instrucción generada fue: " + instruction);
                }
            });
        }
    }
}
