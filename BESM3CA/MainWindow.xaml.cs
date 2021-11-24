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
        public DataController CurrentController;


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
            DataContext = CurrentController;
            CurrentController.SelectedListingData.CreateJSON(@"C:\Users\Mike\Documents\BESM3E.json");
            ResetAll();
        }                

        private void ResetAll()
        {
            Title = ApplicationName;

            //Reset root character:
            CurrentController.ResetAll();

            //Refresh Genre list:
            RefreshGenreList();

            //Refresh right hand boxes:
            RefreshFilter();
            RefreshList();
        }

        private void RefreshGenreList()
        {
            if (CurrentController.SelectedListingData.Genres != null && CurrentController.SelectedListingData.Genres.Count > 0)
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

            if (CharacterTreeView.SelectedItem is BaseNode baseNode)
            {
                //Reset Attribute filter listbox basded off selected node:    
                string OriginalValue = (string)FilterComboBox.SelectedValue;

                FilterComboBox.ItemsSource = baseNode.GetTypesForFilter();

                if (FilterComboBox.Items.Contains(OriginalValue))
                {
                    FilterComboBox.SelectedValue = OriginalValue;
                }
                else
                {
                    FilterComboBox.SelectedIndex = 0;
                }
            }

        }

        private void RefreshVariants()
        {
            if (CharacterTreeView.SelectedItem is IVariantDataNode selectedVariantNode)
            {
                List<VariantListing> FilteredVarList = selectedVariantNode.GetVariants();
                if (FilteredVarList != null)
                {
                    VariantListBox.Visibility = Visibility.Visible;
                    VariantLabel.Visibility = Visibility.Visible;

                    VariantListBox.DisplayMemberPath = "FullName";
                    VariantListBox.ItemsSource = FilteredVarList;

                    if (selectedVariantNode.Variant != null)
                    {
                        VariantListBox.SelectedValue = selectedVariantNode.Variant;  //Load in saved variant
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

                ((BaseNode)CharacterTreeView.SelectedItem).AssociatedListing.RefreshFilteredPotentialChildren(Filter);
                
               
            }
        }
        
        private void SaveFile(bool SaveExisting)
        {
            if (SaveExisting == false || CurrentController.CurrentEntity.FileName == "")
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog
                {
                    RestoreDirectory = false,
                    Filter = ApplicationName + " Files (*.xml)|*.xml|All Files (*.*)|*.*",
                    FilterIndex = 1
                };

                if (saveFileDialog1.ShowDialog() == true)
                {
                    CurrentController.CurrentEntity.SaveAs(saveFileDialog1.FileName);

                    Title = ApplicationName + " - " + CurrentController.CurrentEntity.FileName;
                }
                else
                {
                    return; //User Pressed Cancel
                }
            }
            else
            {
                CurrentController.CurrentEntity.Save();
            }
        }
                


        //Events:       
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
                
                CurrentController.Load(openFileDialog1.FileName);

                if (CurrentController.CurrentEntity.RootCharacter == null)
                {
                    //load failed, reset:
                    ResetAll();
                    MessageBox.Show("Unable to load file: invalid format", "Error loading file", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (CurrentController.CurrentEntity.SelectedGenreIndex > -1)
                    {
                        GenreComboBox.SelectedIndex = CurrentController.CurrentEntity.SelectedGenreIndex;
                    }

                    TreeViewItem newRoot = new TreeViewItem
                    {
                        Header = CurrentController.CurrentEntity.RootCharacter.DisplayText
                    };
                                        
                    NewUpdateTreeFromNodes(newRoot, CurrentController.CurrentEntity.RootCharacter);

                    Title = ApplicationName + " - " + CurrentController.CurrentEntity.FileName;
                    if (CharacterTreeView.Items.Count > 0)
                    {
                        ((BaseNode)CharacterTreeView.Items[0]).IsSelected = true;
                    }
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
                CurrentController.CurrentEntity.ExportToText(saveFileDialog1.FileName);
            }
            else
            {
                return; //User Pressed Cancel
            }
        }


        private void VariantListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VariantListBox.SelectedValue is VariantListing selectedVariant)
            {
                ((IVariantDataNode)CharacterTreeView.SelectedItem).Variant = selectedVariant;   
            }
        }

        private void AttributeListBox_PreviewMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (CurrentController.CurrentEntity.SelectedNode is BaseNode SelectedTreeNode && SelectedTreeNode.CanAddSelectedChild())
            {
                SelectedTreeNode.AddSelectedChild();
            }         
            
            AttributeListBox.Focus();
        }

        private void AttributeListBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Return)
            {
                if (CurrentController.CurrentEntity.SelectedNode is BaseNode SelectedTreeNode && SelectedTreeNode.CanAddSelectedChild())
                {
                    SelectedTreeNode.AddSelectedChild();
                }
            }
        }

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshList();
        }

        private void NotesTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (CharacterTreeView.SelectedItem is BaseNode selectedTreeNode)
            {
                selectedTreeNode.Notes = NotesTextBox.Text;
            }
        }

        private void NewUpdateTreeFromNodes(TreeViewItem insertionPoint, BaseNode nodeDataToAdd)
        {
            if (nodeDataToAdd == CurrentController.CurrentEntity.RootCharacter)
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
            if (CharacterTreeView.SelectedItem is BaseNode && AttributeListBox.SelectedValue is DataListing)
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
                CurrentController.CurrentEntity.SelectedGenreIndex = GenreComboBox.SelectedIndex;
                CurrentController.CurrentEntity.RootCharacter.InvalidateGenrePoints();
                
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
                CurrentController.CurrentEntity.ExportToHTML(saveFileDialog1.FileName);
            }
            else
            {
                return; //User Pressed Cancel
            }
        }

        private void CharacterTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

            if (CharacterTreeView.SelectedItem is BaseNode selectedTreeNode)
            {
                RefreshFilter();
                RefreshList();
                
                if (AttributeListBox.Items.Count > 0)
                {
                    AttributeListBox.ScrollIntoView(AttributeListBox.Items[0]);
                }

                if (selectedTreeNode is CharacterNode)
                {
                    
                    DelAttButton.IsEnabled = false;
                    
                }
                else if (selectedTreeNode is DataNode)
                {
                    if (selectedTreeNode.Parent != null)
                    {
                        DelAttButton.IsEnabled = true;
                        
                    }
                    else
                    {
                        //Do not allow deletion/moving of root node regardless:
                        DelAttButton.IsEnabled = false;
                        
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
    }
}