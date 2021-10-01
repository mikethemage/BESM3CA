using BESM3CAData;
using BESM3CAData.Model;
using BESM3CAData.Templates;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System;

namespace BESM3CA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Title bar:
        private const string ApplicationName = "BESM3CA";

        //Data:        
        private Controller CurrentController;


        //Constructor:
        public MainWindow()
        {
            InitializeComponent();
        }


        //Initialisation code:
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Initialise Controller:
            CurrentController = new Controller();


            //CurrentController.SelectedTemplate.CreateJSON(@"C:\Users\Mike\Documents\TestBESM.json");

            ResetAll();
        }


        //UI member functions:
        private static TreeViewItem AddNodeDataToTree(NodeData nodeData, ItemCollection insertionPoint)
        {
            TreeViewItem AddedNode = new TreeViewItem
            {
                Header = nodeData.DisplayText,
                Tag = nodeData
            };

            insertionPoint.Add(AddedNode);

            return AddedNode;
        }

        private void ResetAll()
        {
            Title = ApplicationName;

            //Reset root character:
            CurrentController.ResetAll();

            //Refresh Genre list:
            RefreshGenreList();

            //reset Treeview             
            CharacterTreeView.Items.Clear();

            //link to root:
            AddNodeDataToTree(CurrentController.RootCharacter, CharacterTreeView.Items).IsSelected = true;

            //Refresh right hand boxes:
            RefreshFilter();
            RefreshList();

            //Refresh tree/data:
            RefreshTree(CharacterTreeView.Items);
            RefreshTextBoxes();



        }

        private void RefreshGenreList()
        {
            if (CurrentController.SelectedTemplate.Genres != null)
            {
                GenreComboBox.Visibility = Visibility.Visible;
                GenreComboBox.ItemsSource = CurrentController.SelectedTemplate.Genres;
                GenreComboBox.SelectedIndex = 0;
            }
            else
            {
                GenreComboBox.Visibility = Visibility.Collapsed;
                GenreComboBox.ItemsSource = null;
            }
        }

        private void RefreshFilter()
        {
            //Reset Attribute filter listbox:            
            FilterComboBox.ItemsSource = CurrentController.SelectedTemplate.GetTypesForFilter();
            FilterComboBox.SelectedIndex = 0;
        }

        private void RefreshVariants()
        {







            if (CharacterTreeView.SelectedItem != null && ((TreeViewItem)CharacterTreeView.SelectedItem).Tag.GetType() == typeof(AttributeData) && ((AttributeData)((TreeViewItem)CharacterTreeView.SelectedItem).Tag).HasVariants)
            {
                List<ListItems> FilteredVarList = ((AttributeData)((TreeViewItem)CharacterTreeView.SelectedItem).Tag).GetVariants();
                if (FilteredVarList != null)
                {
                    VariantListBox.Visibility = Visibility.Visible;
                    VariantLabel.Visibility = Visibility.Visible;









                    //VariantListBox.SelectedIndexChanged -= lbVariantList_SelectedIndexChanged;   //Temporarily disable event  - not currently needed for WPF version
                    VariantListBox.DisplayMemberPath = "Name";
                    VariantListBox.SelectedValuePath = "ID";
                    VariantListBox.ItemsSource = FilteredVarList;

                    if (((AttributeData)((TreeViewItem)CharacterTreeView.SelectedItem).Tag).Variant > 0)
                    {
                        VariantListBox.SelectedValue = ((AttributeData)((TreeViewItem)CharacterTreeView.SelectedItem).Tag).Variant;  //Load in saved variant
                    }
                    else
                    {
                        VariantListBox.SelectedIndex = -1; // This optional line keeps the first item from being selected.
                    }
                    //VariantListBox.SelectedIndexChanged += lbVariantList_SelectedIndexChanged;   //Re-enable event   - not currently needed for WPF version        
                }
            }
            else
            {
                VariantListBox.Visibility = Visibility.Collapsed;
                VariantLabel.Visibility = Visibility.Collapsed;






            }
        }

        private void RefreshList()
        {
            RefreshVariants();

            string Filter;
            if (FilterComboBox.SelectedIndex == -1)
            {
                Filter = "";
            }
            else
            {
                Filter = FilterComboBox.Items[FilterComboBox.SelectedIndex].ToString();
            }

            AttributeListBox.DisplayMemberPath = "Name";
            AttributeListBox.SelectedValuePath = "ID";
            if (CharacterTreeView.SelectedItem != null)
            {
                AttributeListBox.ItemsSource = ((NodeData)((TreeViewItem)CharacterTreeView.SelectedItem).Tag).GetFilteredPotentialChildren(Filter);
            }
        }

        private void AddAttr()
        {
            if (CharacterTreeView.SelectedItem != null && AttributeListBox.SelectedIndex >= 0 && (int)AttributeListBox.SelectedValue > 0)
            {
                NodeData FirstNewNodeData = ((NodeData)((TreeViewItem)CharacterTreeView.SelectedItem).Tag).AddChildAttribute(((ListItems)AttributeListBox.SelectedItem).Name, (int)AttributeListBox.SelectedValue);
                NewUpdateTreeFromNodes((TreeViewItem)CharacterTreeView.SelectedItem, FirstNewNodeData);
                RefreshTree(CharacterTreeView.Items);
                RefreshTextBoxes();
            }
        }

        private void DisableLevelButtons()
        {
            LowerLevelButton.IsEnabled = false;
            RaiseLevelButton.IsEnabled = false;
        }

        private void EnableLevelButtons()
        {
            LowerLevelButton.IsEnabled = true;
            RaiseLevelButton.IsEnabled = true;
        }

        private void RefreshTextBoxes()
        {
            if (CharacterTreeView.SelectedItem != null)
            {
                NotesTextBox.Text = ((NodeData)((TreeViewItem)CharacterTreeView.SelectedItem).Tag).Notes;

                if (((NodeData)((TreeViewItem)CharacterTreeView.SelectedItem).Tag).HasCharacterStats)
                {
                    //Get Character Stats:
                    CalcStats stats = ((NodeData)((TreeViewItem)CharacterTreeView.SelectedItem).Tag).GetStats();

                    BodyTextBox.Text = ((CharacterData)((TreeViewItem)CharacterTreeView.SelectedItem).Tag).Body.ToString();
                    MindTextBox.Text = ((CharacterData)((TreeViewItem)CharacterTreeView.SelectedItem).Tag).Mind.ToString();
                    SoulTextBox.Text = ((CharacterData)((TreeViewItem)CharacterTreeView.SelectedItem).Tag).Soul.ToString();
                    HealthTextBox.Text = stats.Health.ToString();
                    EnergyTextBox.Text = stats.Energy.ToString();
                    ACVTextBox.Text = stats.ACV.ToString();
                    DCVTextBox.Text = stats.DCV.ToString();
                    BodyTextBox.Visibility = Visibility.Visible;
                    MindTextBox.Visibility = Visibility.Visible;
                    SoulTextBox.Visibility = Visibility.Visible;
                    HealthTextBox.Visibility = Visibility.Visible;
                    EnergyTextBox.Visibility = Visibility.Visible;
                    ACVTextBox.Visibility = Visibility.Visible;
                    DCVTextBox.Visibility = Visibility.Visible;
                    BodyLabel.Visibility = Visibility.Visible;
                    MindLabel.Visibility = Visibility.Visible;
                    SoulLabel.Visibility = Visibility.Visible;
                    HealthLabel.Visibility = Visibility.Visible;
                    EnergyLabel.Visibility = Visibility.Visible;
                    ACVLabel.Visibility = Visibility.Visible;
                    DCVLabel.Visibility = Visibility.Visible;
                }
                else
                {
                    BodyTextBox.Text = "";
                    MindTextBox.Text = "";
                    SoulTextBox.Text = "";
                    BodyTextBox.Visibility = Visibility.Hidden;
                    MindTextBox.Visibility = Visibility.Hidden;
                    SoulTextBox.Visibility = Visibility.Hidden;
                    HealthTextBox.Visibility = Visibility.Hidden;
                    EnergyTextBox.Visibility = Visibility.Hidden;
                    BodyLabel.Visibility = Visibility.Hidden;
                    MindLabel.Visibility = Visibility.Hidden;
                    SoulLabel.Visibility = Visibility.Hidden;
                    HealthLabel.Visibility = Visibility.Hidden;
                    EnergyLabel.Visibility = Visibility.Hidden;
                    ACVLabel.Visibility = Visibility.Hidden;
                    DCVLabel.Visibility = Visibility.Hidden;
                    ACVTextBox.Visibility = Visibility.Hidden;
                    DCVTextBox.Visibility = Visibility.Hidden;
                }

                if (((NodeData)((TreeViewItem)CharacterTreeView.SelectedItem).Tag).HasLevelStats)
                {
                    if (((TreeViewItem)CharacterTreeView.SelectedItem).Tag.GetType() == typeof(AttributeData))
                    {
                        LevelTextBox.Text = ((AttributeData)((TreeViewItem)CharacterTreeView.SelectedItem).Tag).Level.ToString();
                        DescriptionTextBox.Text = ((AttributeData)((TreeViewItem)CharacterTreeView.SelectedItem).Tag).AttributeDescription;
                    }
                    else
                    {
                        LevelTextBox.Text = "";
                        DescriptionTextBox.Text = "";
                    }
                    LevelTextBox.Visibility = Visibility.Visible;
                    DescriptionTextBox.Visibility = Visibility.Visible;
                    LevelLabel.Visibility = Visibility.Visible;
                    DescriptionLabel.Visibility = Visibility.Visible;
                }
                else
                {
                    LevelTextBox.Text = "";
                    DescriptionTextBox.Text = "";
                    LevelTextBox.Visibility = Visibility.Hidden;
                    DescriptionTextBox.Visibility = Visibility.Hidden;
                    LevelLabel.Visibility = Visibility.Hidden;
                    DescriptionLabel.Visibility = Visibility.Hidden;
                }

                if (((NodeData)((TreeViewItem)CharacterTreeView.SelectedItem).Tag).HasPointsStats)
                {
                    if (((TreeViewItem)CharacterTreeView.SelectedItem).Tag.GetType() == typeof(AttributeData))
                    {
                        PointsPerLevelTextBox.Text = ((AttributeData)((TreeViewItem)CharacterTreeView.SelectedItem).Tag).PointsPerLevel.ToString();
                        PointCostTextBox.Text = ((AttributeData)((TreeViewItem)CharacterTreeView.SelectedItem).Tag).BaseCost.ToString();
                    }
                    else
                    {
                        LevelTextBox.Text = "";
                        DescriptionTextBox.Text = "";
                    }
                    PointsPerLevelTextBox.Visibility = Visibility.Visible;
                    PointCostTextBox.Visibility = Visibility.Visible;
                    PointsPerLevelLabel.Visibility = Visibility.Visible;
                    PointCostLabel.Visibility = Visibility.Visible;
                }
                else
                {
                    PointsPerLevelTextBox.Text = "";
                    PointCostTextBox.Text = "";
                    PointsPerLevelTextBox.Visibility = Visibility.Hidden;
                    PointCostTextBox.Visibility = Visibility.Hidden;
                    PointsPerLevelLabel.Visibility = Visibility.Hidden;
                    PointCostLabel.Visibility = Visibility.Hidden;
                }
            }
        }

        private void RaiseLevel()
        {
            if (((TreeViewItem)CharacterTreeView.SelectedItem).Tag.GetType() == typeof(AttributeData))
            {
                ((AttributeData)((TreeViewItem)CharacterTreeView.SelectedItem).Tag).RaiseLevel();
                RefreshTree(CharacterTreeView.Items);
            }
        }

        private void LowerLevel()
        {
            if (((TreeViewItem)CharacterTreeView.SelectedItem).Tag.GetType() == typeof(AttributeData))
            {
                ((AttributeData)((TreeViewItem)CharacterTreeView.SelectedItem).Tag).LowerLevel();
                RefreshTree(CharacterTreeView.Items);
            }
        }

        private void SaveFile(bool SaveExisting)
        {
            if (SaveExisting == false || CurrentController.FileName == "")
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog
                {
                    //InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                    RestoreDirectory = false,
                    Filter = ApplicationName + " Files (*.xml)|*.xml|All Files (*.*)|*.*",
                    FilterIndex = 1
                };

                if (saveFileDialog1.ShowDialog() == true)
                {
                    CurrentController.SaveAs(saveFileDialog1.FileName);

                    Title = ApplicationName + " - " + CurrentController.FileName;
                }
                else
                {
                    return; //User Pressed Cancel
                }
            }
            else
            {
                CurrentController.Save();
            }
        }

        private void RefreshTree(ItemCollection Nodes)
        {
            foreach (TreeViewItem Node in Nodes)
            {
                RefreshTree(Node.Items);
                Node.Header = ((NodeData)Node.Tag).DisplayText;
            }
        }

        private void DelAttr()
        {
            if ((TreeViewItem)CharacterTreeView.SelectedItem != null && ((TreeViewItem)CharacterTreeView.SelectedItem).Tag.GetType() != typeof(CharacterData))  //do not allow manual deletion of Character nodes
            {
                if (((AttributeData)((TreeViewItem)CharacterTreeView.SelectedItem).Tag).PointAdj >= 0)  //do not delete "freebies"
                {
                    ((NodeData)((TreeViewItem)CharacterTreeView.SelectedItem).Tag).Delete();

                    int selectedIndex = ((TreeViewItem)((TreeViewItem)CharacterTreeView.SelectedItem).Parent).Items.IndexOf(CharacterTreeView.SelectedItem);
                    TreeViewItem newSelectedItem;
                    if (selectedIndex > 0)
                    {
                        //not the first item
                        newSelectedItem = (TreeViewItem)((TreeViewItem)((TreeViewItem)CharacterTreeView.SelectedItem).Parent).Items[selectedIndex - 1];
                    }
                    else if (selectedIndex < ((TreeViewItem)((TreeViewItem)CharacterTreeView.SelectedItem).Parent).Items.Count - 1)
                    {
                        //not the last item
                        newSelectedItem = (TreeViewItem)((TreeViewItem)((TreeViewItem)CharacterTreeView.SelectedItem).Parent).Items[selectedIndex + 1];
                    }
                    else
                    {
                        newSelectedItem = (TreeViewItem)((TreeViewItem)CharacterTreeView.SelectedItem).Parent;
                    }

                    ((TreeViewItem)((TreeViewItem)CharacterTreeView.SelectedItem).Parent).Items.Remove((TreeViewItem)CharacterTreeView.SelectedItem);

                    newSelectedItem.IsSelected = true;

                    RefreshTree(CharacterTreeView.Items);
                    RefreshTextBoxes();
                }
            }
        }


        //Events:
        private void CharacterTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (CharacterTreeView.SelectedItem != null)
            {
                RefreshList();
                RefreshTextBoxes();
                if (AttributeListBox.Items.Count > 0)
                {
                    AttributeListBox.ScrollIntoView(AttributeListBox.Items[0]);
                }

                if (((TreeViewItem)CharacterTreeView.SelectedItem).Tag.GetType() == typeof(CharacterData))
                {
                    DisableLevelButtons();
                    DelAttButton.IsEnabled = false;
                    MoveUpButton.IsEnabled = false;
                    MoveDownButton.IsEnabled = false;
                }
                else if (((TreeViewItem)CharacterTreeView.SelectedItem).Tag.GetType() == typeof(AttributeData))
                {
                    DelAttButton.IsEnabled = true;

                    MoveDownButton.IsEnabled = true;

                    CheckMoveUpDown();

                    if (((AttributeData)((TreeViewItem)CharacterTreeView.SelectedItem).Tag).HasLevel)
                    {
                        EnableLevelButtons();
                    }
                    else
                    {
                        DisableLevelButtons();
                    }
                }
                else
                {
                    //Error
                    Debug.Assert(false);
                }
            }

            CheckValidAttributeForAddButton();
        }

        private void CheckMoveUpDown()
        {
            if (((TreeViewItem)CharacterTreeView.SelectedItem).Parent != null)
            {
                if (((TreeViewItem)((TreeViewItem)CharacterTreeView.SelectedItem).Parent).Items[0] == CharacterTreeView.SelectedItem)
                {
                    //First Item
                    MoveUpButton.IsEnabled = false;
                }
                else
                {
                    MoveUpButton.IsEnabled = true;
                }



                if (((TreeViewItem)((TreeViewItem)CharacterTreeView.SelectedItem).Parent).Items[^1] == CharacterTreeView.SelectedItem)
                {
                    //Last Item
                    MoveDownButton.IsEnabled = false;
                }
                else
                {
                    MoveDownButton.IsEnabled = true;
                }
            }
        }

        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFile(true);
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                //InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                RestoreDirectory = false,
                Filter = ApplicationName + " Files(*.xml)|*.xml|All Files (*.*)|*.*",
                FilterIndex = 1
            };
            if (openFileDialog1.ShowDialog() == true)
            {
                ResetAll();
                CharacterTreeView.Items.Clear();

                CurrentController.Load(openFileDialog1.FileName);

                if (CurrentController.SelectedGenreIndex > -1)
                {
                    GenreComboBox.SelectedIndex = CurrentController.SelectedGenreIndex;
                }

                //***
                TreeViewItem newRoot = new TreeViewItem
                {
                    Header = CurrentController.RootCharacter.DisplayText
                };

                CharacterTreeView.Items.Add(newRoot);
                NewUpdateTreeFromNodes(newRoot, CurrentController.RootCharacter);

                Title = ApplicationName + " - " + CurrentController.FileName;
                if (CharacterTreeView.Items.Count > 0)
                {
                    ((TreeViewItem)CharacterTreeView.Items[0]).IsSelected = true;
                }
            }
        }

        private void NewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to start a new character?  Any unsaved data will be lost!", "New", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                ResetAll();
            }
        }

        private void BodyTextBox_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (CharacterTreeView.SelectedItem != null && ((TreeViewItem)CharacterTreeView.SelectedItem).Tag.GetType() == typeof(CharacterData))
            {
                if (int.TryParse(BodyTextBox.Text, out int temp) && temp > 0)
                {
                    ((CharacterData)((TreeViewItem)CharacterTreeView.SelectedItem).Tag).Body = temp;
                    RefreshTree(CharacterTreeView.Items);
                    RefreshTextBoxes();
                }
            }
        }



        private void MindTextBox_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (CharacterTreeView.SelectedItem != null && ((TreeViewItem)CharacterTreeView.SelectedItem).Tag.GetType() == typeof(CharacterData))
            {
                if (int.TryParse(MindTextBox.Text, out int temp) && temp > 0)
                {
                    ((CharacterData)((TreeViewItem)CharacterTreeView.SelectedItem).Tag).Mind = temp;
                    RefreshTree(CharacterTreeView.Items);
                    RefreshTextBoxes();
                }
            }
        }



        private void SoulTextBox_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (CharacterTreeView.SelectedItem != null && ((TreeViewItem)CharacterTreeView.SelectedItem).Tag.GetType() == typeof(CharacterData))
            {
                if (int.TryParse(SoulTextBox.Text, out int temp) && temp > 0)
                {
                    ((CharacterData)((TreeViewItem)CharacterTreeView.SelectedItem).Tag).Soul = temp;
                    RefreshTree(CharacterTreeView.Items);
                    RefreshTextBoxes();
                }
            }
        }




        /*Todo: check if validation code required (control should force integer value on front end???*/














        /*******/

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveAsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFile(false);
        }

        private void ExportToTextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                //InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                RestoreDirectory = false,
                Filter = "Export Files (*.txt)|*.txt|All Files (*.*)|*.*",
                FilterIndex = 1
            };

            if (saveFileDialog1.ShowDialog() == true)
            {
                CurrentController.ExportToText(saveFileDialog1.FileName);
            }
            else
            {
                return; //User Pressed Cancel
            }
        }

        /*Todo: check if validation code required (control should force integer value on front end???*/



























        /******/

        private void AddAttButton_Click(object sender, RoutedEventArgs e)
        {
            AddAttr();
        }

        private void DelAttButton_Click(object sender, RoutedEventArgs e)
        {
            DelAttr();
        }

        private void MoveUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (CharacterTreeView.SelectedItem != CharacterTreeView.Items[0] && ((TreeViewItem)((TreeViewItem)CharacterTreeView.SelectedItem).Parent).Items.Count > 1)// && ((TreeViewItem)CharacterTreeView.SelectedItem).PrevNode != null)
            {
                TreeViewItem tempnode = (TreeViewItem)CharacterTreeView.SelectedItem;
                ((NodeData)tempnode.Tag).MoveUp();

                ((TreeViewItem)tempnode.Parent).Items.SortDescriptions.Clear();
                ((TreeViewItem)tempnode.Parent).Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Tag.NodeOrder", System.ComponentModel.ListSortDirection.Ascending));
                ((TreeViewItem)tempnode.Parent).Items.Refresh();

                tempnode.IsSelected = true;

                CheckMoveUpDown();
            }
        }

        private void MoveDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (CharacterTreeView.SelectedItem != CharacterTreeView.Items[0] && ((TreeViewItem)((TreeViewItem)CharacterTreeView.SelectedItem).Parent).Items.Count > 1) // && CharacterTreeView.SelectedNode.NextNode != null)
            {
                TreeViewItem tempnode = (TreeViewItem)CharacterTreeView.SelectedItem;
                ((NodeData)tempnode.Tag).MoveDown();

                ((TreeViewItem)tempnode.Parent).Items.SortDescriptions.Clear();
                ((TreeViewItem)tempnode.Parent).Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Tag.NodeOrder", System.ComponentModel.ListSortDirection.Ascending));
                ((TreeViewItem)tempnode.Parent).Items.Refresh();

                tempnode.IsSelected = true;

                CheckMoveUpDown();
            }
        }

        private void RaiseLevelButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseLevel();
            RefreshTextBoxes();
        }

        private void LowerLevelButton_Click(object sender, RoutedEventArgs e)
        {
            LowerLevel();
            RefreshTextBoxes();
        }

        private void VariantListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VariantListBox.SelectedIndex >= 0)
            {
                if ((int)VariantListBox.SelectedValue > 0)
                {
                    ((AttributeData)((TreeViewItem)CharacterTreeView.SelectedItem).Tag).Variant = (int)VariantListBox.SelectedValue;
                    ((AttributeData)((TreeViewItem)CharacterTreeView.SelectedItem).Tag).Name = ((ListItems)VariantListBox.SelectedItem).Name;

                    RefreshTree(CharacterTreeView.Items);
                    RefreshTextBoxes();
                }
            }
        }

        private void AttributeListBox_PreviewMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            AddAttr();
            AttributeListBox.Focus();
        }

        private void AttributeListBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Return)
            {
                AddAttr();
            }
        }

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshList();
        }

        private void NotesTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (CharacterTreeView.SelectedItem != null)
            {
                ((NodeData)((TreeViewItem)CharacterTreeView.SelectedItem).Tag).Notes = NotesTextBox.Text;
            }
        }

        private void NewUpdateTreeFromNodes(TreeViewItem insertionPoint, NodeData nodeDataToAdd)
        {
            if (nodeDataToAdd == CurrentController.RootCharacter)
            {
                insertionPoint.Tag = nodeDataToAdd;
            }
            else
            {
                TreeViewItem temp = new TreeViewItem() { Header = nodeDataToAdd.DisplayText, Tag = nodeDataToAdd };
                insertionPoint.Items.Add(temp);
                insertionPoint.IsExpanded = true;
                insertionPoint = temp;
            }
            var child = nodeDataToAdd.Children;
            while (child != null)
            {
                NewUpdateTreeFromNodes(insertionPoint, child);
                child = child.Next;
            }

        }


        private void AttributeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckValidAttributeForAddButton();
        }

        private void CheckValidAttributeForAddButton()
        {
            if (CharacterTreeView.SelectedItem != null && AttributeListBox.SelectedIndex >= 0 && (int)AttributeListBox.SelectedValue > 0)
            {
                AddAttButton.IsEnabled = true;
            }
            else
            {
                AddAttButton.IsEnabled = false;
            }
        }

        private void GenreComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GenreComboBox.SelectedIndex > -1)
            {
                CurrentController.SelectedGenreIndex = GenreComboBox.SelectedIndex;
                CurrentController.RootCharacter.InvalidateGenrePoints();
                RefreshTree(CharacterTreeView.Items);
            }
        }
    }
}