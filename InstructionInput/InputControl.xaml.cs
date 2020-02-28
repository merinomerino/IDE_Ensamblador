using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace InstructionInput
{
    public partial class InputControl : UserControl
    {
        private int InstructionID = -1;

        /**
         * Toma el string [template] como plantilla para generar 
         * dinámicamente el contenido del control.
         * 
         * Esta cadena de texto debe tener la estructura 
         * especificada en la tercera diapositiva del 
         * archivo [Estructura IDE Basisc 1-1.ptx] al 24 de 
         * febrero del 2020. 
         */
        public InputControl(String template)
        {
            InitializeComponent();

            // Genera el contenido del control.
            String[] individualParams = template.Split(',');

            InstructionID = int.Parse(individualParams[0]);
            for (int i = 1; i < individualParams.Length; ++i)
            {
                String param = individualParams[i];

                if (Regex.Match(param, "\".*\"").Success)
                {
                    // Parámetro actual es una cadena de texto.
                    TextBlock txt = new TextBlock()
                    {
                        Text = param.Substring(1, param.Length - 2),
                        VerticalAlignment = System.Windows.VerticalAlignment.Center
                    };

                    InstructionPlaceholder.Children.Add(txt);
                    continue;
                }

                // TODO: Mostrar tablas correspondientes al parámetro.
                // Parámetro es una referencia a una tabla.
                ComboBox cmb = new ComboBox 
                {
                    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                    ItemsSource = Enumerable.Range(1, 32).Select(v =>
                    {
                        ComboBoxItem item = new ComboBoxItem
                        {
                            Content = v
                        };
                        return item;
                    })
                };

                InstructionPlaceholder.Children.Add(cmb);
            }
            // Define el resto del comportamiento del control.
            AcceptControl.Click += AcceptControl_Click;
        }

        private void AcceptControl_Click(object sender, RoutedEventArgs e)
        {
            string result =
                InstructionID.ToString() + ",\"" +
                LabelControl.Text + "\"," +
                IndentControl.Value.ToString() + ",\"" +
                // TODO: Registros seleccionados y valores para mostrar ("variables").
                CommentControl.Text + "\"";
            OnDone(result);
        }

        public Action<string> OnDone = (instruction) => { };
    }
}
