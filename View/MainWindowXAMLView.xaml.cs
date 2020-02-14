#region Copyright Syncfusion Inc. 2001-2019.
// Copyright Syncfusion Inc. 2001-2019. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
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


using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Caliburn.Micro;

namespace DockingManagerMVVMCaliburnMicro
{


    public static class InstructionDB
    {
        public static TableMap tableMap = new TableMap();
        public static List<Rule> rules = new List<Rule>
        {
            new Rule
            (
                id: 0,
                format: new List<ITokenType>
                {
                    new FixedString("if"),
                    new FixedString("("),
                    new AsmPlaceHolder(new PlaceHolder(0, "Rr", new TableID(1, 'A'))),
                    new FixedString("("),
                    new AsmPlaceHolder(new PlaceHolder(1, "b", new TableID(5, 'A'))),
                    new FixedString(")"),
                    new FixedString("="),
                    new BasicPlaceHolder(new PlaceHolder(0, "Bit", new TableID(6, 'A'))),
                    new FixedString(")"),
                    new FixedString("Skipline")
                }
            ),
            new Rule
            (
                id: 1,
                format: new List<ITokenType>
                {
                    new AsmPlaceHolder(new PlaceHolder(0, "Rd", new TableID(1, 'A'))),
                    new FixedString("+"),
                    new AsmPlaceHolder(new PlaceHolder(1, "Rr", new TableID(1, 'A')))
                }
            ),
            new Rule
            (
                id: 2,
                format: new List<ITokenType>
                {
                    new AsmPlaceHolder(new PlaceHolder(0, "Rd", new TableID(1, 'A'))),
                    new FixedString("^"),
                    new AsmPlaceHolder(new PlaceHolder(1, "Rr", new TableID(1, 'A')))
                }
             ),
            new Rule
            (
                id: 3,
                format: new List<ITokenType>
                {
                    new FixedString("~"),
                    new AsmPlaceHolder(new PlaceHolder(0, "Rd", new TableID(1, 'A')))
                }
            ),

        };
        public static InstructionMap instructionMap =
        new InstructionMap(
            new List<Instruction>
            {
                    new Instruction
                    (
                        Id: 0,
                        Name: "ADD",
                        AsmParams: new List<AsmParameter>
                        {
                            new AsmParameter(new Parameter("Rd", new RangeConstraint(new Range(0, 32)))),
                            new AsmParameter(new Parameter("Rr", new RangeConstraint(new Range(0, 32))))
                        },
                        BasicParams: new List<BasicParameter>(),
                        Rule: rules[1]
                    ),
                    new Instruction
                    (
                        Id: 1,
                        Name: "SBRC",
                        AsmParams: new List<AsmParameter>
                        {
                            new AsmParameter(new Parameter("Rr", new RangeConstraint(new Range(0, 32)))),
                            new AsmParameter(new Parameter("b", new RangeConstraint(new Range(0, 8))))
                        },
                        BasicParams: new List<BasicParameter>
                        {
                            new BasicParameter(new Parameter("Bit", new ConstantConstraint("0")))
                        },
                        Rule: rules[0]
                    ),
                    new Instruction
                    (
                        Id: 2,
                        Name: "SBRS",
                        AsmParams: new List<AsmParameter>
                        {
                            new AsmParameter(new Parameter("Rr", new RangeConstraint(new Range(0, 32)))),
                            new AsmParameter(new Parameter("b", new RangeConstraint(new Range(0, 8))))
                        },
                        BasicParams: new List<BasicParameter>
                        {
                            new BasicParameter(new Parameter("Bit", new ConstantConstraint("1")))
                        },
                        Rule: rules[0]
                    ),
                    new Instruction
                    (
                        Id: 3,
                        Name: "OR",
                        AsmParams: new List<AsmParameter>
                        {
                            new AsmParameter(new Parameter("Rd", new RangeConstraint(new Range(0, 32)))),
                            new AsmParameter(new Parameter("Rr", new RangeConstraint(new Range(0, 32))))
                        },
                        BasicParams: new List<BasicParameter>(),
                        Rule: rules[2]
                     ),
                    new Instruction
                    (
                        Id: 4,
                        Name: "NOT",
                        AsmParams: new List<AsmParameter>
                        {
                            new AsmParameter(new Parameter("Rd", new RangeConstraint(new Range(0, 32)))),
                        },
                        BasicParams: new List<BasicParameter>(),
                        Rule: rules[3]
                     )
            }
    );
    }

    public enum State
    {
        Start,
        SelectingRule,
        SelectingValues
    }


     public static class Editor
    {
        public static State currentState = State.Start;
        public static int counter = 0;
        public static Dictionary<int, InstructionCell> map = new Dictionary<int, InstructionCell>();
        public static Dictionary<InstructionCell, int> invMap;
        public static InstructionCell lastSelected { get; set; }
        //public static View[] lastAdded { get; set; }


        public static void Add(InstructionCell cell)
        {
            map.Add(counter, cell);
            counter++;
        }
        public static void Remove(int id)
        {
            map.Remove(id);
        }
    }
    public class ParamListToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var instance = value as InstructionInstance;
            return instance.ToString();
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class InstructionCell : GridView
    {
        public StackPanel StackPanel { get; set; }
        public InstructionCell()
        {
            StackPanel = new StackPanel();
            Label lbl = new Label();
            Label lblParams = new Label();
            lbl.SetBinding(Label.ContentProperty, new Binding("Instance.Instruction.Name"));
            //lblParams.SetBinding(Label.ContentProperty, new Binding("Instance", BindingMode.Default, new ParamListToStringConverter(), null));
            var stkLabel = new StackPanel()
            {
                Children = { lbl, lblParams }
            };
            stkLabel.Orientation = Orientation.Horizontal;
            var stk = new StackPanel()
            {
                Children = { stkLabel, StackPanel }
            };
            //View = stk;
            Editor.Add(this);
         
            //Data.mapping.Add(this);
        }
    }
    public class InstructionCellData
    {
        public InstructionInstance Instance { get; }
        public int Id { get; }
        public InstructionCellData(InstructionInstance Instance, int Id)
        {
            this.Instance = Instance;
            this.Id = Id;
        }
    }


    /// <summary>
    /// Interaction logic for MainWindowXAMLView.xaml
    /// </summary>
    public partial class MainWindowXAMLView : UserControl
    {
        public ListView listview { get; set; }
        StackPanel MainStack;
        StackPanel MenuStack;





        //protected  async void OnAppearing()
        //{
            
        //    //var editor = await App.Database.GetEditorAsync();
        //    //var list = editor.Select(x => new InstructionCellData(x, x.Id));
        //    //Editor.counter = App.Database.Counter;
        //    listview.ItemTemplate = new DataTemplate(typeof(InstructionCell));
        //   // listview.SelectedItem += Listview_ItemSelected;
        //    listview.ItemsSource = new ObservableCollection<InstructionCellData>();

        //}


        public MainWindowXAMLView()
        {
            InitializeComponent();
            listview = new ListView();



            listview.ItemTemplate = new DataTemplate(typeof(InstructionCell));
            listview.ItemsSource = new ObservableCollection<InstructionCellData>();

            var btn = new Button()
            {
                Content = "+",
                VerticalAlignment = VerticalAlignment.Center,
                Height=100,Width=100
               
                
            };
            // listview.Footer = new Label { Text = "footer" };
           // listview.Footer = btn;
            //listview.HasUnevenRows = true;
            listview.VerticalAlignment = VerticalAlignment.Center;
            btn.Click += AddMenu;
            MenuStack = new StackPanel();
            MainStack = new StackPanel()
            {
                Children = { listview, MenuStack },
                VerticalAlignment = VerticalAlignment.Center

            };
            grid.Children.Add(MainStack);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void AddMenu(object sender, EventArgs e)
        {
           
            
            //var cell = Editor.map[insInstance.Id];
            switch (Editor.currentState)
            {
                case State.Start:
                    //if (Editor.lastSelected != null) ClearLayout(Editor.lastSelected.StackPanel);
                    var rulePicker = new ComboBox { Name = "select", ItemsSource = InstructionDB.rules };
                    
                    rulePicker.SelectionChanged += RulePicker_SelectedIndexChanged;
                    AddLayout(MenuStack, new List<UIElement> { rulePicker });
                    //Editor.lastSelected = cell;
                    Editor.currentState = State.SelectingRule;
                    break;

            }
        }

        private void Listview_ItemSelected(object sender, SelectedCellsChangedEventArgs e)
        {
            /*
            //if (Editor.lastSelected != null) RemoveLayout(Editor.lastSelected.StackPanel, Editor.lastAdded);
            var insInstance = e.SelectedItem as InstructionCellData;
            var cell = Editor.map[insInstance.Id];
            switch (Editor.currentState)
            {
                case State.Start:
                    if (Editor.lastSelected != null) ClearLayout(Editor.lastSelected.StackPanel);
                    var rulePicker = new Picker { Title = "select", ItemsSource = InstructionDB.rules};
                    rulePicker.SelectedIndexChanged += RulePicker_SelectedIndexChanged;
                    AddLayout(Editor.map[insInstance.Id].StackPanel, new List<View> {rulePicker});
                    Editor.lastSelected = cell;
                    Editor.currentState = State.SelectingRule;
                    break;
            }
            */
            // generate new layout
        }
        private void RulePicker_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            var picker = sender as ComboBox;
            if (picker.SelectedItem is Rule rule)
            {
                //ClearLayout(Editor.lastSelected.StackPanel);
                ClearLayout(MenuStack);
                (var layout, var asmList, var basicList) = GenerateLayout(rule);
                var btn = new Button { Content = "Agregar" };
                btn.Click += new RoutedEventHandler(AddInstance(rule, asmList, basicList));
                layout.Add(btn);
                //Editor.lastSelected.StackPanel.Orientation = StackOrientation.Horizontal;
                MenuStack.Orientation = Orientation.Horizontal;
                //AddLayout(Editor.lastSelected.StackPanel, layout);
                AddLayout(MenuStack, layout);
                Editor.currentState = State.SelectingValues;
            }
        }
        private Action<object, RoutedEventArgs> AddInstance(Rule rule, List<ComboBox> asmList, List<ComboBox> basicList)
        {
            //TODO: Fix possibly memory leak
            return
                (object sender, RoutedEventArgs e) =>
                {
                    List<IExprValue> asmValues = asmList.Select(ComboBox => ComboBox.SelectedItem as IExprValue).ToList();
                    List<IExprValue> basicValues = basicList.Select(ComboBox => ComboBox.SelectedItem as IExprValue).ToList();
                    var instruction = InstructionDB.instructionMap.FindMatch(rule, basicValues, asmValues);
                    InstructionInstance instance = new InstructionInstance(instruction, asmValues, basicValues);

                    IEnumerable source = listview.ItemsSource;

                    var list = source.Cast<InstructionCellData>().ToList();
                    list.Add(new InstructionCellData(instance, Editor.counter));
                    list.Sort((x, y) => x.Id.CompareTo(y.Id));
                    listview.ItemsSource = list;
                   // App.Database.AddToSave(instance);
                    Editor.currentState = State.Start;
                    //ClearLayout(Editor.lastSelected.StackPanel);
                    ClearLayout(MenuStack);
                };
        }
        private void Btn_AddInstructionInstance(object sender, EventArgs e)
        {
            //ListView
        }


        /*
        private void AddInstance(Rule rule, List<View> layout)
        {
            
            (List<AsmPlaceHolder>, List<BasicPlaceHolder>) PlaceHolders (Rule r)
            {
                var listAsm = new List<AsmPlaceHolder>();
                var listBasic = new List<BasicPlaceHolder>();
                foreach(ITokenType t in r.format)
                {
                    if (t is BasicPlaceHolder b)
                    {
                        listBasic.Add(b);
                    }
                    else if (t is AsmPlaceHolder a)
                    {
                        listAsm.Add(a);
                    }
                }
                return (listAsm, listBasic);
            }

            int indexAsm = 0;
            int indexBasic = 0;
            foreach(var v in layout)
            {
                if (v is Picker picker)
                {
                    var value = picker.SelectedItem as IExprValue;
                }
            }
            
        }
        */

        /*
private void Listview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
{
   if (Data.lastSelected != null) RemoveLayout(Data.lastSelected.Stk, Data.lastAdded);

   Elem el = e.SelectedItem as Elem;
   ElemCell elcell = Data.mapping[el.index];
   Label lbl = new Label() { Text = "added" };

   List<string> dropdown = new List<string>() { "a", "b", "c" };
   Picker picker = new Picker() { Title = "Dropdown" };
   picker.ItemsSource = dropdown;

   View[] layout = new View[] { lbl, picker };
   GenerateLayout(elcell.Stk, layout);
   Data.lastAdded = layout;
   Data.lastSelected = elcell;

}
*/
        /*
private void btnOrientation_Clicked(object sender, EventArgs e)
{
   var newOrientation = layout.Orientation == StackOrientation.Vertical ? StackOrientation.Horizontal : StackOrientation.Vertical;
   layout.Orientation = newOrientation;
}
*/
        private (List<UIElement>, List<ComboBox>, List<ComboBox>) GenerateLayout(Rule rule)
        {
            Func<List<ComboBox>, ComboBox, ComboBox> addAndReturn = (list, p) => { list.Add(p); return p; };
            List<ComboBox> basicPicker = new List<ComboBox>();
            List<ComboBox> asmPicker = new List<ComboBox>();
            var layout =
                rule.format.Select(t => t.Match(
                    fixedString: str => (UIElement)new Label { Content = str },
                    asmPlaceHolder: ph => (UIElement)addAndReturn(asmPicker, new ComboBox { Name = ph.name, ItemsSource = InstructionDB.tableMap[ph.tableId] }),
                    basicPlaceHolder: ph => (UIElement)addAndReturn(basicPicker, new ComboBox { Name = ph.name, ItemsSource = InstructionDB.tableMap[ph.tableId] })))
                .ToList();
            return (layout, asmPicker, basicPicker);
        }
        private List<UIElement> GenerateLayout(InstructionInstance instructionInstance)
        {
            var basicValues = instructionInstance.BasicValues;
            var asmValues = instructionInstance.AsmValues;
            List<UIElement> layout = new List<UIElement>();
            layout.Add(new Label { Content = instructionInstance.Instruction.Name });
            for (int k = 0; k < basicValues.Count; k++)
            {
                var v = basicValues[k];
                layout.Add(v.Match(str => (UIElement)new Label { Content = str }, i => new Label { Content = i.ToString() }));
            }
            for (int k = 0; k < asmValues.Count; k++)
            {
                var v = asmValues[k];
                layout.Add(v.Match(str => (UIElement)new Label { Content = str }, i => (UIElement)new Label { Content = i.ToString() }));
            }
            return layout;
        }
        private void AddLayout(StackPanel root, IList<UIElement> views)
        {
            foreach (var v in views)
            {
                root.Children.Add(v);
             
            }
        }

        private void ClearLayout(StackPanel root)
        {
            root.Children.Clear();
          
        }

        private void btnRemove_Clicked(object sender, EventArgs e)
        {
            //RemoveLayout(layout, generated);
        }

        private void Lista_ItemSelected(object sender, SelectedCellsChangedEventArgs e)
        {

        }
        /*
        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            var label = new Label { Text = "added" };
            ViewCell v = sender as ViewCell;
            Lista.
            stk.Children.Add(label);
        }
        */
    }
}

