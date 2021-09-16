using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using BESM3CA.Model;
using BESM3CA;
//using BESM3CA_WPF.UI;
using Microsoft.Win32;

namespace BESM3CA_WPF
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
            ResetAll();
        }


        //UI member functions:
        private TreeViewItem AddNodeDataToTree(NodeData nodeData, ItemCollection insertionPoint)
        {
            TreeViewItem AddedNode = new TreeViewItem();            
            AddedNode.Header = nodeData.DisplayText;
            AddedNode.Tag = nodeData;
            
            insertionPoint.Add(AddedNode);
            
            return AddedNode;
        }

        private void ResetAll()
        {
            Title = ApplicationName;

            //Reset root character:
            CurrentController.ResetAll();

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

        private void RefreshFilter()
        {
            //Reset Attribute filter listbox:            
            FilterComboBox.ItemsSource = CurrentController.SelectedTemplate.GetTypesForFilter();
            FilterComboBox.SelectedIndex = 0;
        }

        private void RefreshVariants()
        {
            

            if (CharacterTreeView.SelectedItem!=null && ((TreeViewItem)CharacterTreeView.SelectedItem).Tag.GetType() == typeof(AttributeData) && ((AttributeData)((TreeViewItem)CharacterTreeView.SelectedItem).Tag).HasVariants)
            {
                List<ListItems> FilteredVarList = ((AttributeData)((TreeViewItem)CharacterTreeView.SelectedItem).Tag).GetVariants();
                if (FilteredVarList != null)
                {
                    VariantListBox.Visibility = Visibility.Visible;
                    VariantLabel.Visibility = Visibility.Visible;
                    //FilterComboBox.Top = HeightAdjust3;

                    //if (AttributeListBox.Top == HeightAdjust2)
                    //{
                    //    AttributeListBox.Height -= (HeightAdjust1 - HeightAdjust2);
                    //}

                    //AttributeListBox.Top = HeightAdjust1;

                    //VariantListBox.SelectedIndexChanged -= lbVariantList_SelectedIndexChanged;   //Temporarily disable event
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
                    //VariantListBox.SelectedIndexChanged += lbVariantList_SelectedIndexChanged;   //Re-enable event           
                }
            }
            else
            {
                VariantListBox.Visibility = Visibility.Collapsed;
                VariantLabel.Visibility = Visibility.Collapsed;
                //FilterComboBox.Top = HeightAdjust4;
                //if (AttributeListBox.Top == HeightAdjust1)
                //{
                    //AttributeListBox.Height += (HeightAdjust1 - HeightAdjust2);
                //}
                //AttributeListBox.Top = HeightAdjust2;
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
                UpdateTreeFromNodes(((TreeViewItem)CharacterTreeView.SelectedItem), FirstNewNodeData);
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

        private void RefreshTree(ItemCollection Nodes) //Needs Fixing
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
                    /*TreeViewItem tempNode;
                    if (CharacterTreeView.SelectedNode.NextNode != null)
                    {
                        tempNode = CharacterTreeView.SelectedNode.NextNode;
                    }
                    else
                    {
                        tempNode = CharacterTreeView.SelectedNode.PrevNode;
                    }*/

                    ((NodeData)((TreeViewItem)CharacterTreeView.SelectedItem).Tag).Delete();
                    CharacterTreeView.Items.Remove(((TreeViewItem)CharacterTreeView.SelectedItem).Tag);
                    //CharacterTreeView.SelectedNode = tempNode;
                    //RefreshTree(CharacterTreeView.Nodes);
                    RefreshTextBoxes();
                }
            }
        }


        //Events:
        private void CharacterTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            RefreshList();
            RefreshTextBoxes();

            if (((TreeViewItem)CharacterTreeView.SelectedItem).Tag.GetType() == typeof(CharacterData))
            {
                DisableLevelButtons();
            }
            else if (((TreeViewItem)CharacterTreeView.SelectedItem).Tag.GetType() == typeof(AttributeData))
            {
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

        /***
        Todo: Tool strip items

        ***/





































        /*Todo: text box validation
         */

        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
                
        
               
        
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
                
                ((TreeViewItem)tempnode.Parent).Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Tag.NodeOrder", System.ComponentModel.ListSortDirection.Ascending));
                ((TreeViewItem)tempnode.Parent).Items.Refresh();

                tempnode.IsSelected = true;
            }
        }       

        private void MoveDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (CharacterTreeView.SelectedItem != CharacterTreeView.Items[0] && ((TreeViewItem)((TreeViewItem)CharacterTreeView.SelectedItem).Parent).Items.Count > 1) // && CharacterTreeView.SelectedNode.NextNode != null)
            {
                TreeViewItem tempnode = (TreeViewItem)CharacterTreeView.SelectedItem;
                ((NodeData)tempnode.Tag).MoveDown();

                ((TreeViewItem)tempnode.Parent).Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Tag.NodeOrder", System.ComponentModel.ListSortDirection.Ascending));
                ((TreeViewItem)tempnode.Parent).Items.Refresh();
                tempnode.IsSelected = true;               
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
                }
            }
        }

        private void AttributeListBox_PreviewMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            AddAttr();
            AttributeListBox.Focus();
        }

        /*Todo: Keypress events*/







        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshList();
        }

        
        
        
        
        
        private void UpdateTreeFromNodes(TreeViewItem StartingTreePoint, NodeData StartingNodeData)
        {
            //Version for loading code:                                                     //Version for adding attribs:
            //StartingTreePoint == tvCharacterTree.Nodes.Add()                               //StartingTreePoint == tvCharacterTree.SelectedNode
            //StartingNodeData == RootCharacter                                              //StartingNodeData == FirstNewNodeData

            TreeViewItem TreeInsertionPoint = StartingTreePoint;
            NodeData CurrentNewNodeData = StartingNodeData;

            while (CurrentNewNodeData != null)
            {
                if (CurrentNewNodeData != CurrentController.RootCharacter)
                {
                    TreeInsertionPoint = AddNodeDataToTree(CurrentNewNodeData, TreeInsertionPoint.Items);//Add node to treeview
                    
                }

                if (TreeInsertionPoint.Parent != null)
                {
                    ((TreeViewItem)TreeInsertionPoint.Parent).ExpandSubtree();
                }

                TreeInsertionPoint.Tag = CurrentNewNodeData;

                if (CurrentNewNodeData.Children != null)
                {
                    CurrentNewNodeData = CurrentNewNodeData.Children;             //Now check for any children
                }
                else if (CurrentNewNodeData.Next != null)
                {
                    CurrentNewNodeData = CurrentNewNodeData.Next;                 //if no children add all siblings
                    TreeInsertionPoint = (TreeViewItem)TreeInsertionPoint.Parent;               //move insertion point back up one
                }
                else if (CurrentNewNodeData.Parent == null)
                {
                    CurrentNewNodeData = null; //drop out of the loop                            
                }
                else if (CurrentNewNodeData.Parent != StartingNodeData && CurrentNewNodeData.Parent.Next != null) // make sure we are not back to original node
                {
                    CurrentNewNodeData = CurrentNewNodeData.Parent.Next;          //no children or siblings so add next sibling of the parent node
                    TreeInsertionPoint = (TreeViewItem)((TreeViewItem)TreeInsertionPoint.Parent).Parent;        //move insertion point back up two
                }
                else
                {
                    //either only one node to add, or we have gone through all of the above and ended up at the last immediate child of the original new node
                    CurrentNewNodeData = null; //drop out of the loop                            
                }
            }

            RefreshTree(CharacterTreeView.Items);  //Refresh the whole tree as can have impact both up and down the tree  
        }
    }
}
