using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using BESM3CA.Model;
using BESM3CA;


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
        /*private TreeNode AddNodeDataToTree(NodeData nodeData, TreeNodeCollection insertionPoint)//probably isn't needed anymore
        {
            TreeNode AddedNode;
            AddedNode = insertionPoint.Add(nodeData.Name);
            AddedNode.Tag = nodeData;

            return AddedNode;
        }*/

        private void ResetAll()
        {
            Title = ApplicationName;

            //Reset root character:
            CurrentController.ResetAll();

            //reset Treeview 
            //CharacterTreeView.Nodes.Clear();  //Possible version needs testing:
            CharacterTreeView.Items.Clear();

            //link to root:
            //CharacterTreeView.SelectedNode = AddNodeDataToTree(CurrentController.RootCharacter, CharacterTreeView.Nodes); //Needs Fixing

            CharacterTreeView.DisplayMemberPath = "DisplayText";           

            CharacterTreeView.Items.Add(CurrentController.RootCharacter);

            //CharacterTreeView.SelectedIndex = 0;

            //haracterTreeView.Items.c



            //Refresh right hand boxes:
            RefreshFilter();
            RefreshList();

            //Refresh tree/data:
            //RefreshTree(CharacterTreeView.Nodes);  //Needs Fixing
            RefreshTextBoxes();
        }

        private void RefreshFilter()
        {
            //Reset Attribute filter listbox:            
            FilterComboBox.ItemsSource = CurrentController.SelectedTemplate.GetTypesForFilter();
        }

        private void RefreshVariants()
        {
            //Constants for adjusting right hand list and combo boxes:
            const int HeightAdjust1 = 125;
            const int HeightAdjust2 = 27;
            const int HeightAdjust3 = 101;
            const int HeightAdjust4 = 3;
            //****

            if (CharacterTreeView.SelectedItem!=null && CharacterTreeView.SelectedItem.GetType() == typeof(AttributeData) && ((AttributeData)CharacterTreeView.SelectedItem).HasVariants)
            {
                List<ListItems> FilteredVarList = ((AttributeData)CharacterTreeView.SelectedItem).GetVariants();
                if (FilteredVarList != null)
                {
                    //VariantListBox.Visibility = "Visible";
                    //lbVariant.Visible = true;
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

                    if (((AttributeData)CharacterTreeView.SelectedItem).Variant > 0)
                    {
                        VariantListBox.SelectedValue = ((AttributeData)CharacterTreeView.SelectedItem).Variant;  //Load in saved variant
                    }
                    else
                    {
                        VariantListBox.SelectedIndex = -1; // This optional line keeps the first item from being selected.
                    }
                    //VariantListBox.SelectedIndexChanged += lbVariantList_SelectedIndexChanged;   //Re-enable event           
                }
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

            //Need fixing:
            AttributeListBox.DisplayMemberPath = "Name";
            AttributeListBox.SelectedValuePath = "ID";
            if (CharacterTreeView.SelectedValue != null)
            {
                AttributeListBox.ItemsSource = ((NodeData)CharacterTreeView.SelectedValue).GetFilteredPotentialChildren(Filter);
            }
        }

        private void AddAttr()
        {
            if (CharacterTreeView.SelectedItem != null && AttributeListBox.SelectedIndex >= 0 && (int)AttributeListBox.SelectedValue > 0)
            {
                NodeData FirstNewNodeData = ((NodeData)CharacterTreeView.SelectedItem).AddChildAttribute(((ListItems)AttributeListBox.SelectedItem).Name, (int)AttributeListBox.SelectedValue);
                UpdateTreeFromNodes((TreeViewItem)CharacterTreeView.SelectedItem, FirstNewNodeData);
                RefreshTextBoxes();
            }
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
                    //TreeInsertionPoint = 
                        TreeInsertionPoint.Items.Add(CurrentNewNodeData.Name);      //Add node to treeview
                }

                if (TreeInsertionPoint.Parent != null)
                {
                    //TreeInsertionPoint.Parent.Expand();
                }

                TreeInsertionPoint.Tag = CurrentNewNodeData;

                if (CurrentNewNodeData.Children != null)
                {
                    CurrentNewNodeData = CurrentNewNodeData.Children;             //Now check for any children
                }
                else if (CurrentNewNodeData.Next != null)
                {
                    CurrentNewNodeData = CurrentNewNodeData.Next;                 //if no children add all siblings
                    //TreeInsertionPoint = TreeInsertionPoint.Parent;               //move insertion point back up one
                }
                else if (CurrentNewNodeData.Parent == null)
                {
                    CurrentNewNodeData = null; //drop out of the loop                            
                }
                else if (CurrentNewNodeData.Parent != StartingNodeData && CurrentNewNodeData.Parent.Next != null) // make sure we are not back to original node
                {
                    CurrentNewNodeData = CurrentNewNodeData.Parent.Next;          //no children or siblings so add next sibling of the parent node
                   // TreeInsertionPoint = TreeInsertionPoint.Parent.Parent;        //move insertion point back up two
                }
                else
                {
                    //either only one node to add, or we have gone through all of the above and ended up at the last immediate child of the original new node
                    CurrentNewNodeData = null; //drop out of the loop                            
                }
            }

            //RefreshTree(CharacterTreeView.Nodes);  //Refresh the whole tree as can have impact both up and down the tree  
        }

        /*private void RefreshTree() //Needs Fixing
        {

        }*/

        private void RefreshTextBoxes()
        {
            //if (CharacterTreeView.SelectedNode.Tag.GetType() == typeof(AttributeData) && ((AttributeData)CharacterTreeView.SelectedNode.Tag).HasVariants)
        }

        private void CharacterTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            RefreshList();
            RefreshTextBoxes();

            if (CharacterTreeView.SelectedItem.GetType() == typeof(CharacterData))
            {
                DisableLevelButtons();
            }
            else if (CharacterTreeView.SelectedItem.GetType() == typeof(AttributeData))
            {
                if (((AttributeData)CharacterTreeView.SelectedItem).HasLevel)
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

        private void DisableLevelButtons()
        {

        }

        private void EnableLevelButtons()
        {

        }

        private void AddAttButton_Click(object sender, RoutedEventArgs e)
        {
            AddAttr();
        }
    }
}
