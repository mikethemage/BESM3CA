using BESM3CAData.Control;
using BESM3CAData.Listings;
using BESM3CAData.Model;
using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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
        private DataController CurrentController;


        //Constructor:
        public MainWindow()
        {
            InitializeComponent();
        }


        //Initialisation code:
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Initialise Controller:
            CurrentController = new DataController();
            //CurrentController.SelectedListingData.CreateJSON(@"C:\Users\Mike\Documents\TestBESM.json");
            ResetAll();
        }


        //UI methods:
        private static TreeViewItem AddNodeDataToTree(BaseNode nodeData, ItemCollection insertionPoint)
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
            if (CurrentController.SelectedListingData.Genres != null)
            {
                GenreComboBox.Visibility = Visibility.Visible;
                GenreComboBox.ItemsSource = CurrentController.SelectedListingData.Genres;
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
            //Reset Attribute filter listbox basded off selected node:    
            string OriginalValue = (string)FilterComboBox.SelectedValue;

            FilterComboBox.ItemsSource = ((BaseNode)((TreeViewItem)CharacterTreeView.SelectedItem).Tag).GetTypesForFilter();

            if (FilterComboBox.Items.Contains(OriginalValue))
            {
                FilterComboBox.SelectedValue = OriginalValue;
            }
            else
            {
                FilterComboBox.SelectedIndex = 0;
            }
        }

        private void RefreshVariants()
        {
            AttributeNode selectedAttribute = null;
            if (CharacterTreeView.SelectedItem != null)
            {
                if (CharacterTreeView.SelectedItem is TreeViewItem selectedTreeNode)
                {
                    selectedAttribute = selectedTreeNode.Tag as AttributeNode;
                }
            }

            if (selectedAttribute != null && selectedAttribute.HasVariants)
            {
                List<VariantListing> FilteredVarList = selectedAttribute.GetVariants();
                if (FilteredVarList != null)
                {
                    VariantListBox.Visibility = Visibility.Visible;
                    VariantLabel.Visibility = Visibility.Visible;

                    VariantListBox.DisplayMemberPath = "FullName";
                    VariantListBox.ItemsSource = FilteredVarList;

                    if (selectedAttribute.Variant != null)
                    {
                        VariantListBox.SelectedValue = selectedAttribute.Variant;  //Load in saved variant
                    }
                    else
                    {
                        VariantListBox.SelectedIndex = -1; // This line keeps the first item from being selected.
                    }
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

            if (CharacterTreeView.SelectedItem != null)
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(((BaseNode)((TreeViewItem)CharacterTreeView.SelectedItem).Tag).GetFilteredPotentialChildren(Filter));
                view.GroupDescriptions.Add(new PropertyGroupDescription("Type"));
                AttributeListBox.ItemsSource = view;
            }
        }

        private void AddAttr()
        {
            if (CharacterTreeView.SelectedItem is TreeViewItem SelectedTreeNode && AttributeListBox.SelectedIndex >= 0 && AttributeListBox.SelectedValue != null)
            {
                BaseNode FirstNewNodeData = ((BaseNode)SelectedTreeNode.Tag).AddChildAttribute((AttributeListing)AttributeListBox.SelectedItem);
                NewUpdateTreeFromNodes(SelectedTreeNode, FirstNewNodeData);
                RefreshTree(CharacterTreeView.Items);
                RefreshTextBoxes();
                ((TreeViewItem)SelectedTreeNode.Items[^1]).BringIntoView();
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
            if (CharacterTreeView.SelectedItem is TreeViewItem selectedTreeNode)
            {
                NotesTextBox.Text = ((BaseNode)selectedTreeNode.Tag).Notes;

                if (selectedTreeNode.Tag is CharacterNode selectedCharacter)
                {
                    //Get Character Stats:
                    CalcStats stats = selectedCharacter.GetStats();

                    BodyTextBox.Text = selectedCharacter.Body.ToString();
                    MindTextBox.Text = selectedCharacter.Mind.ToString();
                    SoulTextBox.Text = selectedCharacter.Soul.ToString();
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

                if (((BaseNode)selectedTreeNode.Tag).HasLevelStats)
                {
                    if (selectedTreeNode.Tag is AttributeNode selectedAttribute)
                    {
                        LevelTextBox.Text = selectedAttribute.Level.ToString();
                        DescriptionTextBox.Text = selectedAttribute.AttributeDescription;
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

                if (((BaseNode)selectedTreeNode.Tag).HasPointsStats)
                {
                    if (selectedTreeNode.Tag is AttributeNode selectedAttribute)
                    {
                        PointsPerLevelTextBox.Text = selectedAttribute.PointsPerLevel.ToString();
                        PointCostTextBox.Text = selectedAttribute.BaseCost.ToString();
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
            if (((TreeViewItem)CharacterTreeView.SelectedItem).Tag is AttributeNode selectedAttribute)
            {
                if (selectedAttribute.RaiseLevel())
                {
                    RefreshTree(CharacterTreeView.Items);
                }
            }
        }

        private void LowerLevel()
        {
            if (((TreeViewItem)CharacterTreeView.SelectedItem).Tag is AttributeNode selectedAttribute)
            {
                if (selectedAttribute.LowerLevel())
                {
                    RefreshTree(CharacterTreeView.Items);
                }
            }
        }

        private void SaveFile(bool SaveExisting)
        {
            if (SaveExisting == false || CurrentController.FileName == "")
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog
                {
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
                Node.Header = ((BaseNode)Node.Tag).DisplayText;
            }
        }

        private void DelAttr()
        {
            if (CharacterTreeView.SelectedItem is TreeViewItem selectedTreeNode && selectedTreeNode.Tag is not CharacterNode)  //do not allow manual deletion of Character nodes
            {
                if (((AttributeNode)selectedTreeNode.Tag).PointAdj >= 0)  //do not delete "freebies"
                {
                    ((BaseNode)selectedTreeNode.Tag).Delete();
                    TreeViewItem selectedParent = (TreeViewItem)selectedTreeNode.Parent;
                    int selectedIndex = selectedParent.Items.IndexOf(selectedTreeNode);
                    TreeViewItem newSelectedItem;
                    if (selectedIndex > 0)
                    {
                        //not the first item
                        newSelectedItem = (TreeViewItem)selectedParent.Items[selectedIndex - 1];
                    }
                    else if (selectedIndex < selectedParent.Items.Count - 1)
                    {
                        //not the last item
                        newSelectedItem = (TreeViewItem)selectedParent.Items[selectedIndex + 1];
                    }
                    else
                    {
                        newSelectedItem = selectedParent;
                    }

                    selectedParent.Items.Remove(selectedTreeNode);

                    newSelectedItem.IsSelected = true;

                    RefreshTree(CharacterTreeView.Items);
                    RefreshTextBoxes();
                }
            }
        }

        private void CheckMoveUpDown()
        {
            if (CharacterTreeView.SelectedItem is TreeViewItem selectedTreeNode && selectedTreeNode.Parent is TreeViewItem selectedParent)
            {
                if (selectedParent.Items[0] == selectedTreeNode) //First Item
                {
                    MoveUpButton.IsEnabled = false;
                }
                else
                {
                    MoveUpButton.IsEnabled = true;
                }

                if (selectedParent.Items[^1] == selectedTreeNode) //Last Item
                {
                    MoveDownButton.IsEnabled = false;
                }
                else
                {
                    MoveDownButton.IsEnabled = true;
                }
            }
        }


        //Events:
        private void CharacterTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (CharacterTreeView.SelectedItem is TreeViewItem selectedTreeNode)
            {
                RefreshFilter();
                RefreshList();
                RefreshTextBoxes();
                if (AttributeListBox.Items.Count > 0)
                {
                    AttributeListBox.ScrollIntoView(AttributeListBox.Items[0]);
                }

                if (selectedTreeNode.Tag is CharacterNode)
                {
                    DisableLevelButtons();
                    DelAttButton.IsEnabled = false;
                    MoveUpButton.IsEnabled = false;
                    MoveDownButton.IsEnabled = false;
                }
                else if (selectedTreeNode.Tag is AttributeNode selectedAttribute)
                {
                    DelAttButton.IsEnabled = true;
                    MoveDownButton.IsEnabled = true;
                    CheckMoveUpDown();

                    if (selectedAttribute.HasLevel)
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

        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFile(true);
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
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
            if (CharacterTreeView.SelectedItem is TreeViewItem selectedTreeNode && selectedTreeNode.Tag is CharacterNode selectedCharacter)
            {
                if (int.TryParse(BodyTextBox.Text, out int temp) && temp > 0)
                {
                    selectedCharacter.Body = temp;
                    RefreshTree(CharacterTreeView.Items);
                    RefreshTextBoxes();
                }
            }
        }

        private void MindTextBox_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (CharacterTreeView.SelectedItem is TreeViewItem selectedTreeNode && selectedTreeNode.Tag is CharacterNode selectedCharacter)
            {
                if (int.TryParse(MindTextBox.Text, out int temp) && temp > 0)
                {
                    selectedCharacter.Mind = temp;
                    RefreshTree(CharacterTreeView.Items);
                    RefreshTextBoxes();
                }
            }
        }

        private void SoulTextBox_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (CharacterTreeView.SelectedItem is TreeViewItem selectedTreeNode && selectedTreeNode.Tag is CharacterNode selectedCharacter)
            {
                if (int.TryParse(SoulTextBox.Text, out int temp) && temp > 0)
                {
                    selectedCharacter.Soul = temp;
                    RefreshTree(CharacterTreeView.Items);
                    RefreshTextBoxes();
                }
            }
        }

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
            if (CharacterTreeView.SelectedItem != CharacterTreeView.Items[0] && CharacterTreeView.SelectedItem is TreeViewItem selectedTreeNode && selectedTreeNode.Parent is TreeViewItem selectedParent && selectedParent.Items.Count > 1)
            {
                ((BaseNode)selectedTreeNode.Tag).MoveUp();

                selectedParent.Items.SortDescriptions.Clear();
                selectedParent.Items.SortDescriptions.Add(new SortDescription("Tag.NodeOrder", ListSortDirection.Ascending));
                selectedParent.Items.Refresh();

                selectedTreeNode.IsSelected = true;
                selectedTreeNode.BringIntoView();

                CheckMoveUpDown();
            }
        }

        private void MoveDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (CharacterTreeView.SelectedItem != CharacterTreeView.Items[0] && CharacterTreeView.SelectedItem is TreeViewItem selectedTreeNode && selectedTreeNode.Parent is TreeViewItem selectedParent && selectedParent.Items.Count > 1)
            {
                ((BaseNode)selectedTreeNode.Tag).MoveDown();

                selectedParent.Items.SortDescriptions.Clear();
                selectedParent.Items.SortDescriptions.Add(new SortDescription("Tag.NodeOrder", ListSortDirection.Ascending));
                selectedParent.Items.Refresh();

                selectedTreeNode.IsSelected = true;
                selectedTreeNode.BringIntoView();

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
            if (VariantListBox.SelectedValue is VariantListing selectedVariant)
            {
                ((AttributeNode)((TreeViewItem)CharacterTreeView.SelectedItem).Tag).Variant = selectedVariant;

                RefreshTree(CharacterTreeView.Items);
                RefreshTextBoxes();
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
            if (CharacterTreeView.SelectedItem is TreeViewItem selectedTreeNode)
            {
                ((BaseNode)selectedTreeNode.Tag).Notes = NotesTextBox.Text;
            }
        }

        private void NewUpdateTreeFromNodes(TreeViewItem insertionPoint, BaseNode nodeDataToAdd)
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
            BaseNode child = nodeDataToAdd.FirstChild;
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
            if (CharacterTreeView.SelectedItem is TreeViewItem && AttributeListBox.SelectedValue is AttributeListing)
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

        private void ExportToHTMLMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                RestoreDirectory = false,
                Filter = "Export Files (*.html)|*.html|All Files (*.*)|*.*",
                FilterIndex = 1
            };

            if (saveFileDialog1.ShowDialog() == true)
            {
                CurrentController.ExportToHTML(saveFileDialog1.FileName);
            }
            else
            {
                return; //User Pressed Cancel
            }
        }
    }
}