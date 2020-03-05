using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Libraries;
namespace InstructionInput
{
    public partial class InputControl : UserControl
    {
        private int InstructionID = -1;
        private Rule rule;
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
        public InputControl(Rule rule)
        {
            InitializeComponent();
            this.rule = rule;
            foreach (var token in rule.Format)
            {
                if (token is FixedString)
                {
                    var strToken = (FixedString)token;
                    TextBlock txt = new TextBlock()
                    {
                        Text = strToken.Str,
                        VerticalAlignment = System.Windows.VerticalAlignment.Center
                    };
                    InstructionPlaceholder.Children.Add(txt);
                }
                else // if (token is AsmPlaceHolder || token is BasicPlaceHolder)
                {
                    (var name, var tableId) = token.Match(
                        str => (null, null), 
                        basic => (basic.name, basic.tableId), 
                        asm => (asm.name, asm.tableId));
                    InstructionPlaceholder.Children.Add(GenerateTable(name, tableId));
                }
            }
            // Define el resto del comportamiento del control.
            AcceptControl.Click += AcceptControl_ClickRule;
        }
        // TODO: Mostrar tablas correspondientes al parámetro.
        private UIElement GenerateTable(string text, TableID id)
        {
            return new ComboBox
            {
                Text = text,
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
        private void AcceptControl_ClickRule(object sender, RoutedEventArgs e)
        {
            string result =
                rule.Id + ",\"" +
                LabelControl.Text + "\"," +
                IndentControl.Value.ToString() + ",\"" +
                // TODO: Registros seleccionados y valores para mostrar ("variables").
                CommentControl.Text + "\"";
            OnDone(result);
        }
        public Action<string> OnDone = (instruction) => { };
    }
}
