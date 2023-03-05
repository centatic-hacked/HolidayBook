using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;
using HolidayBook.ViewModels;
using System.Windows.Media.Animation;
using Model.Infrastructure;

namespace HolidayBook.Overview
{
    public class WPFRuntimeElements
    {
        public MainWindow MW = default!;

        public WPFRuntimeElements(MainWindow mw, HolidayBookContext db)
        {
            MW = mw;
            Database = db;
        }

        public HolidayBookContext Database { get; private set; } = default!;

        public void childrenItemCreator(int newAmount, int oldAmount)
        {
            ListBox lb = default!;
            foreach (UIElement en in MW.childrenCare.Children)
            {
                if (en is ListBox)
                {
                    lb = (ListBox)en;
                }
            }
            if (lb == default!)
            {
                lb = new ListBox();
                lb.BorderThickness = new Thickness(0);
                lb.MaxHeight = 250;
                lb.MaxWidth = 420;
                lb.IsTabStop = false;
            }
            if (newAmount > oldAmount && newAmount == 1)
            {
                Border br = new Border();
                br.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#eeeeee");
                br.BorderThickness = new Thickness(0.8);
                br.Margin = new Thickness(2, 15, 5, 0.3);
                MW.childrenCare.Height += 140;
                MW.childrenCare.Children.Add(br);
                lb.Items.Add(itemListCreator(newAmount));
                MW.childrenCare.Children.Add(lb);
            }
            else if (newAmount > oldAmount && newAmount == 2)
            {
                MW.childrenCare.Height += 140;
                lb.Items.Add(itemListCreator(newAmount));
            }
            else if (newAmount > oldAmount)
            {
                lb.Items.Add(itemListCreator(newAmount));
            }
            if (newAmount == 0)
            {
                List<UIElement> toDel = new List<UIElement>();
                foreach (UIElement i in MW.childrenCare.Children)
                {
                    if (i is ListBox)
                    {
                        toDel.Add(i);

                    }
                    if (i is Border)
                    {
                        toDel.Add(i);

                    }
                }
                foreach (UIElement i in toDel)
                {
                    MW.childrenCare.Children.Remove(i);
                }
                MW.childrenCare.Height -= 140;
            }
            else if (oldAmount > newAmount)
            {
                foreach (UIElement i in MW.childrenCare.Children)
                {
                    if (i is ListBox)
                    {
                        ListBox delIt = (ListBox)i;
                        delIt.Items.RemoveAt(delIt.Items.Count - 1);
                    }
                }
                if (newAmount == 1) MW.childrenCare.Height -= 140;
            }
        }
        private ListBoxItem itemListCreator(int number)
        {
            ListBoxItem lbi = new ListBoxItem();
            WrapPanel wrp = new WrapPanel();
            wrp.Orientation = Orientation.Vertical;
            wrp.Margin = new Thickness(2, 15, 5, 0);
            TextBlock block = new TextBlock();
            block.Text = $"{number} child's age";
            block.FontSize = 15;
            block.FontWeight = FontWeights.Bold;
            Button btn = new Button();
            btn.Name = $"childrenBtn{number}";
            btn.Content = "Select age at time of flying";
            Popup popup = new Popup();
            popup.Name = $"childrenPopup{number}";
            popup.IsEnabled = true;
            popup.LostFocus += (s, e) => popup.IsOpen = false;
            popup.StaysOpen = false;
            btn.Click += (s, e) => popup.IsOpen = true;

            Binding binding = new Binding
            {
                Source = btn
            };

            BindingOperations.SetBinding(popup, Popup.PlacementTargetProperty, binding);
            ListBox lb = new ListBox();
            lb.Height = 200;
            lb.Width = 350;
            lb.SelectionChanged += (s, e) =>
            {
                TextBlock tb = (TextBlock)lb.SelectedItem;
                btn.Content = tb.Text;
            };
            for (int i = 0; i <= 17; i++)
            {
                TextBlock tb = new TextBlock();
                tb.Text = $"{i}";
                lb.Items.Add(tb);
            }
            popup.Child = lb;
            TextBlock block1 = new TextBlock();
            block1.Text = "Select the age this child will be on the date of your final flight";
            block1.FontSize = 13;
            block1.Foreground = (Brush)new BrushConverter().ConvertFrom("#474747");
            wrp.Children.Add(block);
            wrp.Children.Add(btn);
            wrp.Children.Add(popup);
            wrp.Children.Add(block1);
            lbi.Content = wrp;
            return lbi;
        }

        public void checkFinishTraveller()
        {
            if (int.Parse(MW.travellerCount.Text) > 0)
            {
                List<bool> bools = new List<bool>();
                bool noDefine = false;
                foreach (UIElement ui in MW.childrenCare.Children)
                {
                    if (ui is ListBox)
                    {
                        ListBox lb = (ListBox)ui;
                        foreach (ListBoxItem it in lb.Items)
                        {
                            WrapPanel wrp = (WrapPanel)it.Content;
                            foreach (UIElement btn in wrp.Children)
                            {
                                if (btn is Button)
                                {
                                    Button button = (Button)btn;
                                    int i = 0;
                                    bools.Add(int.TryParse(button.Content.ToString(), out i));
                                }
                            }
                        }
                    }
                }
                foreach (bool ages in bools)
                {
                    if (ages == false)
                    {
                        noDefine = true;
                    }
                }
                if (noDefine == true)
                {
                    TextBlock tbx = new TextBlock();
                    tbx.Text = "You have to define every childrens age";
                    tbx.Foreground = (Brush)new BrushConverter().ConvertFrom("#d4111e");
                    tbx.Margin = new Thickness(20, 10, 0, 0);
                    tbx.Name = "err";
                    if (MW.showErr.Children.Count > 1)
                    {
                        MW.showErr.Children.RemoveAt(1);
                    }
                    MW.showErr.Children.Add(tbx);

                }
                else
                {
                    if (MW.showErr.Children.Count > 1)
                    {
                        MW.showErr.Children.RemoveAt(1);
                    }
                    MW.popupTraveller.IsOpen = false;
                }
            }
            else
            {
                MW.popupTraveller.IsOpen = false;
            }
        }

        public void airportLookUp(string txt, string eventName)
        {
            List<ListBox> ls = new List<ListBox>();
            if (eventName == "arrLookUp")
            {
                foreach (UIElement ui in MW.arrAirportPanel.Children)
                {
                    if (ui is ListBox)
                    {
                        ls.Add((ListBox)ui);
                    }
                }
                foreach (ListBox l in ls)
                {
                    MW.arrAirportPanel.Children.Remove(l);
                }
                //.Foreground= Brushes.Red;
                ListBox vislist = new ListBox();
                vislist.Name = "airportList";
                vislist.MaxHeight = 200;
                vislist.MaxWidth = 350;
                vislist.BorderThickness = new Thickness(0);

                var list = Database.Airports.Where(a => a.IATA.Contains($"{txt}")).ToList();
                list.AddRange(Database.Airports.Where(a => a.Name.Contains($"{txt}")).ToList());
                list.AddRange(Database.Airports.Where(a => a.Country.Contains($"{txt}")).ToList());
                list.AddRange(Database.Airports.Where(a => a.City.Contains($"{txt}")).ToList());
                list = list.Distinct().ToList();
                int i = 0;
                foreach (var a in list)
                {
                    Grid gridListItem = new();
                    gridListItem.Margin = new Thickness(0, 10, 0, 0);
                    gridListItem.Width = 310;
                    ColumnDefinition columnDefinition = new ColumnDefinition();
                    columnDefinition.Width = GridLength.Auto;
                    gridListItem.ColumnDefinitions.Add(columnDefinition);
                    ColumnDefinition columnDefinition2 = new ColumnDefinition();
                    columnDefinition2.Width = new GridLength(1, GridUnitType.Star);
                    gridListItem.ColumnDefinitions.Add(columnDefinition2);
                    ColumnDefinition columnDefinition3 = new ColumnDefinition();
                    columnDefinition3.Width = GridLength.Auto;
                    gridListItem.ColumnDefinitions.Add(columnDefinition3);
                    WrapPanel wrapPanel = new WrapPanel();
                    wrapPanel.HorizontalAlignment = HorizontalAlignment.Center;
                    TextBlock iataBlock = new TextBlock();
                    CheckBox checkBox = new CheckBox();
                    checkBox.Name = $"arr{i}";
                    checkBox.Checked += (s, e) => show_Place(s, e, a.IATA, a.Name);
                    checkBox.HorizontalAlignment = HorizontalAlignment.Right;
                    checkBox.VerticalAlignment = VerticalAlignment.Center;
                    iataBlock.Text = a.IATA.Trim() + " ";
                    TextBlock nameBlock = new TextBlock();
                    nameBlock.Text = a.Name;
                    wrapPanel.Children.Add(iataBlock);
                    wrapPanel.Children.Add(nameBlock);
                    TextBlock destinationBlock = new TextBlock();
                    destinationBlock.Text = $"{a.City}, {a.Country}";
                    WrapPanel wrapPanelAllText = new WrapPanel();
                    wrapPanelAllText.HorizontalAlignment = HorizontalAlignment.Left;
                    wrapPanelAllText.Orientation = Orientation.Vertical;
                    wrapPanelAllText.Children.Add(wrapPanel);
                    wrapPanelAllText.Children.Add(destinationBlock);
                    Grid.SetColumn(wrapPanelAllText, 0);
                    WrapPanel checkPanel = new();
                    checkPanel.HorizontalAlignment = HorizontalAlignment.Right;
                    checkPanel.Children.Add(checkBox);
                    Grid.SetColumn(checkPanel, 2);
                    gridListItem.Children.Add(wrapPanelAllText);
                    gridListItem.Children.Add(checkPanel);
                    vislist.Items.Add(gridListItem);
                    i++;
                }
                MW.arrAirportPanel.Children.Add(vislist);
            }
            else if (eventName == "depLookUp")
            {
                foreach (UIElement ui in MW.depAirportPanel.Children)
                {
                    if (ui is ListBox)
                    {
                        ls.Add((ListBox)ui);
                    }
                }
                foreach (ListBox l in ls)
                {
                    MW.depAirportPanel.Children.Remove(l);
                }
                //.Foreground= Brushes.Red;
                ListBox vislist = new ListBox();
                vislist.Name = "airportList";
                vislist.MaxHeight = 200;
                vislist.MaxWidth = 350;
                vislist.BorderThickness = new Thickness(0);

                var list = Database.Airports.Where(a => a.IATA.Contains($"{txt}")).ToList();
                list.AddRange(Database.Airports.Where(a => a.Name.Contains($"{txt}")).ToList());
                list.AddRange(Database.Airports.Where(a => a.Country.Contains($"{txt}")).ToList());
                list.AddRange(Database.Airports.Where(a => a.City.Contains($"{txt}")).ToList());
                list = list.Distinct().ToList();
                int i = 0;
                foreach (var a in list)
                {
                    //WrapPanel wrapListItem = new();
                    //wrapListItem.Orientation = Orientation.Horizontal;
                    //wrapListItem.Margin = new Thickness(0, 10, 0, 0);
                    Grid gridListItem = new();
                    gridListItem.Margin = new Thickness(0, 10, 0, 0);
                    gridListItem.Width = 310;
                    ColumnDefinition columnDefinition= new ColumnDefinition();
                    columnDefinition.Width = GridLength.Auto;
                    gridListItem.ColumnDefinitions.Add(columnDefinition);
                    ColumnDefinition columnDefinition2 = new ColumnDefinition();
                    columnDefinition2.Width = new GridLength(1, GridUnitType.Star);
                    gridListItem.ColumnDefinitions.Add(columnDefinition2);
                    ColumnDefinition columnDefinition3 = new ColumnDefinition();
                    columnDefinition3.Width = GridLength.Auto;
                    gridListItem.ColumnDefinitions.Add(columnDefinition3);
                    WrapPanel wrapPanel = new WrapPanel();
                    wrapPanel.HorizontalAlignment = HorizontalAlignment.Center;
                    TextBlock iataBlock = new TextBlock();
                    CheckBox checkBox = new CheckBox();
                    checkBox.Name = $"dep{i}";
                    checkBox.Checked += (s, e) => show_Place(s, e, a.IATA, a.Name);
                    checkBox.HorizontalAlignment = HorizontalAlignment.Right;
                    //checkBox.Margin = new Thickness(0, 0, 20, 0);
                    checkBox.VerticalAlignment = VerticalAlignment.Center;
                    iataBlock.Text = a.IATA.Trim() + " ";
                    TextBlock nameBlock = new TextBlock();
                    nameBlock.Text = a.Name;
                    wrapPanel.Children.Add(iataBlock);
                    wrapPanel.Children.Add(nameBlock);
                    TextBlock destinationBlock = new TextBlock();
                    destinationBlock.Text = $"{a.City}, {a.Country}";
                    WrapPanel wrapPanelAllText = new WrapPanel();
                    wrapPanelAllText.HorizontalAlignment = HorizontalAlignment.Left;
                    wrapPanelAllText.Orientation = Orientation.Vertical;
                    wrapPanelAllText.Children.Add(wrapPanel);
                    wrapPanelAllText.Children.Add(destinationBlock);
                    Grid.SetColumn(wrapPanelAllText, 0);
                    WrapPanel checkPanel = new();
                    checkPanel.HorizontalAlignment = HorizontalAlignment.Right;
                    checkPanel.Children.Add(checkBox);
                    Grid.SetColumn(checkPanel, 2);

                    gridListItem.Children.Add(wrapPanelAllText);
                    gridListItem.Children.Add(checkPanel);
                    vislist.Items.Add(gridListItem);
                    i++;
                }
                MW.depAirportPanel.Children.Add(vislist);
            }
        }

        private void show_Place(object sender, RoutedEventArgs e, string airporticao, string airportname)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (checkBox.Name.StartsWith("dep"))
            {
                MW.txtDepartureAirport.Content = airporticao + " " + airportname;
                MW.txtDepartureAirport.Foreground = Brushes.Black;
            }
            else if (checkBox.Name.StartsWith("arr"))
            {
                MW.txtArrivalAirport.Content = airporticao + " " + airportname;
                MW.txtArrivalAirport.Foreground = Brushes.Black;
            }
        }
    }
}
