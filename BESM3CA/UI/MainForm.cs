using BESM3CA.Model;
using BESM3CA.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace BESM3CA
{
    public partial class MainForm : Form
    {
        //Title bar:
        private const string ApplicationName = "BESM3CA";

        //Data:        
        private Controller CurrentController;


        //Constructor:
        public MainForm()
        {
            InitializeComponent();
        }


        //Initialisation code:
        private void BESM3CA_Load(object sender, EventArgs e)
        {
            //Initialise Controller:
            CurrentController = new Controller();            
            ResetAll();
        }


        //UI member functions:
        private TreeNode AddNodeDataToTree(NodeData nodeData, TreeNodeCollection insertionPoint)
        {
            TreeNode AddedNode;
            AddedNode = insertionPoint.Add(nodeData.Name);
            AddedNode.Tag = nodeData;

            return AddedNode;
        }

        private void ResetAll()
        {           
            Text = ApplicationName;

            //Reset root character:
            CurrentController.ResetAll();
            
            //reset Treeview 
            CharacterTreeView.Nodes.Clear();

            //link to root:
            CharacterTreeView.SelectedNode = AddNodeDataToTree(CurrentController.RootCharacter, CharacterTreeView.Nodes);                        

            //Refresh right hand boxes:
            RefreshFilter();
            RefreshList();

            //Refresh tree/data:
            RefreshTree(CharacterTreeView.Nodes);
            RefreshTextBoxes();

            //Sort visually by Node order
            CharacterTreeView.TreeViewNodeSorter = new NodeSorter();            
        }

        private void RefreshFilter()
        {
            //Reset Attribute filter listbox:
            FilterComboBox.DataSource = CurrentController.SelectedTemplate.GetTypesForFilter();            
        }

        private void RefreshVariants()
        {
            //Constants for adjusting right hand list and combo boxes:
            const int HeightAdjust1 = 125;
            const int HeightAdjust2 = 27;
            const int HeightAdjust3 = 101;
            const int HeightAdjust4 = 3;
            //****

            if (CharacterTreeView.SelectedNode.Tag.GetType() == typeof(AttributeData) && ((AttributeData)CharacterTreeView.SelectedNode.Tag).HasVariants)
            {
                List<ListItems> FilteredVarList = ((AttributeData)CharacterTreeView.SelectedNode.Tag).GetVariants();
                if (FilteredVarList != null)
                {
                    VariantListBox.Visible = true;
                    lbVariant.Visible = true;
                    FilterComboBox.Top = HeightAdjust3;

                    if (AttributeListBox.Top == HeightAdjust2)
                    {
                        AttributeListBox.Height -= (HeightAdjust1 - HeightAdjust2);
                    }

                    AttributeListBox.Top = HeightAdjust1;

                    VariantListBox.SelectedIndexChanged -= lbVariantList_SelectedIndexChanged;   //Temporarily disable event
                    VariantListBox.DisplayMember = "Name";
                    VariantListBox.ValueMember = "ID";
                    VariantListBox.DataSource = FilteredVarList;

                    if( ((AttributeData)CharacterTreeView.SelectedNode.Tag).Variant>0)
                    {
                        VariantListBox.SelectedValue = ((AttributeData)CharacterTreeView.SelectedNode.Tag).Variant;  //Load in saved variant
                    }
                    else
                    {
                        VariantListBox.SelectedIndex = -1; // This optional line keeps the first item from being selected.
                    }                    
                    VariantListBox.SelectedIndexChanged += lbVariantList_SelectedIndexChanged;   //Re-enable event           
                }
            }
            else
            {                
                VariantListBox.Visible = false;
                lbVariant.Visible = false;
                FilterComboBox.Top = HeightAdjust4;
                if (AttributeListBox.Top == HeightAdjust1)
                {
                    AttributeListBox.Height += (HeightAdjust1 - HeightAdjust2);
                }
                AttributeListBox.Top = HeightAdjust2;
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
            
            AttributeListBox.DisplayMember = "Name";
            AttributeListBox.ValueMember = "ID"; 
            AttributeListBox.DataSource = ((NodeData)CharacterTreeView.SelectedNode.Tag).GetFilteredPotentialChildren(Filter);            
        }

        private void AddAttr()
        {
            if (CharacterTreeView.SelectedNode!=null && AttributeListBox.SelectedIndex >= 0 && (int)AttributeListBox.SelectedValue>0)          
            {
                NodeData FirstNewNodeData = ((NodeData)CharacterTreeView.SelectedNode.Tag).AddChildAttribute(AttributeListBox.Text, (int)AttributeListBox.SelectedValue);
                UpdateTreeFromNodes(CharacterTreeView.SelectedNode, FirstNewNodeData);
                RefreshTextBoxes();
            }
        }

        private void DisableLevelButtons()
        {
            bnDecreaseLevel.Enabled = false;
            bnIncreaseLevel.Enabled = false;
        }

        private void EnableLevelButtons()
        {
            bnDecreaseLevel.Enabled = true;
            bnIncreaseLevel.Enabled = true;
        }

        private void RefreshTextBoxes()
        {
            NotesTextBox.Text = ((NodeData)CharacterTreeView.SelectedNode.Tag).Notes;

            if (((NodeData)CharacterTreeView.SelectedNode.Tag).HasCharacterStats)
            {
                //Get Character Stats:
                CalcStats stats = ((NodeData)CharacterTreeView.SelectedNode.Tag).GetStats();

                BodyTextBox.Text = ((CharacterData)CharacterTreeView.SelectedNode.Tag).Body.ToString();
                MindTextBox.Text = ((CharacterData)CharacterTreeView.SelectedNode.Tag).Mind.ToString();
                SoulTextBox.Text = ((CharacterData)CharacterTreeView.SelectedNode.Tag).Soul.ToString();
                HealthTextBox.Text = stats.Health.ToString();
                EnergyTextBox.Text = stats.Energy.ToString();
                ACVTextBox.Text = stats.ACV.ToString();
                DCVTextBox.Text = stats.DCV.ToString();
                BodyTextBox.Visible = true;
                MindTextBox.Visible = true;
                SoulTextBox.Visible = true;
                HealthTextBox.Visible = true;
                EnergyTextBox.Visible = true;
                ACVTextBox.Visible = true;
                DCVTextBox.Visible = true;
                BodyLabel.Visible = true;
                MindLabel.Visible = true;
                SoulLabel.Visible = true;
                HealthLabel.Visible = true;
                EnergyLabel.Visible = true;
                ACVLabel.Visible = true;
                DCVLabel.Visible = true;
            }
            else
            {
                BodyTextBox.Text = "";
                MindTextBox.Text = "";
                SoulTextBox.Text = "";
                BodyTextBox.Visible = false;
                MindTextBox.Visible = false;
                SoulTextBox.Visible = false;
                HealthTextBox.Visible = false;
                EnergyTextBox.Visible = false;
                BodyLabel.Visible = false;
                MindLabel.Visible = false;
                SoulLabel.Visible = false;
                HealthLabel.Visible = false;
                EnergyLabel.Visible = false;
                ACVLabel.Visible = false;
                DCVLabel.Visible = false;
                ACVTextBox.Visible = false;
                DCVTextBox.Visible = false;
            }

            if (((NodeData)CharacterTreeView.SelectedNode.Tag).HasLevelStats)
            {
                if (CharacterTreeView.SelectedNode.Tag.GetType() == typeof(AttributeData))
                {
                    LevelTextBox.Text = ((AttributeData)CharacterTreeView.SelectedNode.Tag).Level.ToString();
                    DescriptionTextBox.Text = ((AttributeData)CharacterTreeView.SelectedNode.Tag).AttributeDescription;
                }
                else
                {
                    LevelTextBox.Text = "";
                    DescriptionTextBox.Text = "";
                }
                LevelTextBox.Visible = true;
                DescriptionTextBox.Visible = true;
                LevelLabel.Visible = true;
                DescriptionLabel.Visible = true;
            }
            else
            {
                LevelTextBox.Text = "";
                DescriptionTextBox.Text = "";
                LevelTextBox.Visible = false;
                DescriptionTextBox.Visible = false;
                LevelLabel.Visible = false;
                DescriptionLabel.Visible = false;
            }

            if (((NodeData)CharacterTreeView.SelectedNode.Tag).HasPointsStats)
            {
                if (CharacterTreeView.SelectedNode.Tag.GetType() == typeof(AttributeData))
                {
                    PointsPerLevelTextBox.Text = ((AttributeData)CharacterTreeView.SelectedNode.Tag).PointsPerLevel.ToString();
                    PointCostTextBox.Text = ((AttributeData)CharacterTreeView.SelectedNode.Tag).BaseCost.ToString();
                }
                else
                {
                    LevelTextBox.Text = "";
                    DescriptionTextBox.Text = "";
                }
                PointsPerLevelTextBox.Visible = true;
                PointCostTextBox.Visible = true;
                PointsPerLevelLabel.Visible = true;
                PointCostLabel.Visible = true;
            }
            else
            {
                PointsPerLevelTextBox.Text = "";
                PointCostTextBox.Text = "";
                PointsPerLevelTextBox.Visible = false;
                PointCostTextBox.Visible = false;
                PointsPerLevelLabel.Visible = false;
                PointCostLabel.Visible = false;
            }
        }

        private void RaiseLevel()
        {
            if (CharacterTreeView.SelectedNode.Tag.GetType() == typeof(AttributeData))
            {
                ((AttributeData)CharacterTreeView.SelectedNode.Tag).RaiseLevel();
                RefreshTree(CharacterTreeView.Nodes);
            }
        }

        private void LowerLevel()
        {
            if (CharacterTreeView.SelectedNode.Tag.GetType() == typeof(AttributeData))
            {
                ((AttributeData)CharacterTreeView.SelectedNode.Tag).LowerLevel();
                RefreshTree(CharacterTreeView.Nodes);
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

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    CurrentController.SaveAs(saveFileDialog1.FileName);
                    
                    Text = ApplicationName + " - " + CurrentController.FileName;
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

        private void RefreshTree(TreeNodeCollection Nodes)
        {
            foreach (TreeNode Node in Nodes)
            {
                RefreshTree(Node.Nodes);
                Node.Text = ((NodeData)Node.Tag).DisplayText;
            }
        }

        private void DelAttr()
        {
            if (CharacterTreeView.SelectedNode != null && CharacterTreeView.SelectedNode.Tag.GetType() != typeof(CharacterData)) //do not allow manual deletion of Character nodes
            {
                if (((AttributeData)CharacterTreeView.SelectedNode.Tag).PointAdj >= 0)  //do not delete "freebies"
                {
                    TreeNode tempNode;
                    if (CharacterTreeView.SelectedNode.NextNode != null)
                    {
                        tempNode = CharacterTreeView.SelectedNode.NextNode;
                    }
                    else
                    {
                        tempNode = CharacterTreeView.SelectedNode.PrevNode;
                    }

                    ((NodeData)CharacterTreeView.SelectedNode.Tag).Delete();
                    CharacterTreeView.SelectedNode.Remove();
                    CharacterTreeView.SelectedNode = tempNode;
                    RefreshTree(CharacterTreeView.Nodes);
                    RefreshTextBoxes();
                }
            }
        }


        //Events:
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            RefreshList();
            RefreshTextBoxes();

            if (CharacterTreeView.SelectedNode.Tag.GetType() == typeof(CharacterData))
            {
                DisableLevelButtons();
            }
            else if (CharacterTreeView.SelectedNode.Tag.GetType() == typeof(AttributeData))
            {
                if (((AttributeData)CharacterTreeView.SelectedNode.Tag).HasLevel)
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

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile(true);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                //InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                RestoreDirectory = false,
                Filter = ApplicationName + " Files(*.xml)|*.xml|All Files (*.*)|*.*",
                FilterIndex = 1
            };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ResetAll();
                CharacterTreeView.Nodes.Clear();

                CurrentController.Load(openFileDialog1.FileName);

                //***

                UpdateTreeFromNodes(CharacterTreeView.Nodes.Add(CurrentController.RootCharacter.Name), CurrentController.RootCharacter);
                
                Text = ApplicationName + " - " + CurrentController.FileName;
                if (CharacterTreeView.Nodes.Count > 0)
                {
                    CharacterTreeView.SelectedNode = CharacterTreeView.Nodes[0];
                }
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to start a new character?  Any unsaved data will be lost!", "New", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ResetAll();
            }
        }

        private void tbBody_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = false;
            if (CharacterTreeView.SelectedNode.Tag.GetType() == typeof(CharacterData))
            {
                if (int.TryParse(BodyTextBox.Text, out int temp) && temp > 0)
                {
                    ((CharacterData)CharacterTreeView.SelectedNode.Tag).Body = temp;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
        private void tbMind_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = false;
            if (CharacterTreeView.SelectedNode.Tag.GetType() == typeof(CharacterData))
            {
                if (int.TryParse(MindTextBox.Text, out int temp) && temp > 0)
                {
                    ((CharacterData)CharacterTreeView.SelectedNode.Tag).Mind = temp;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
        private void tbSoul_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = false;
            if (CharacterTreeView.SelectedNode.Tag.GetType() == typeof(CharacterData))
            {
                if (int.TryParse(SoulTextBox.Text, out int temp) && temp > 0)
                {
                    ((CharacterData)CharacterTreeView.SelectedNode.Tag).Soul = temp;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void tbBody_Validated(object sender, EventArgs e)
        {
            RefreshTree(CharacterTreeView.Nodes);
            RefreshTextBoxes();
        }
        private void tbMind_Validated(object sender, EventArgs e)
        {
            RefreshTree(CharacterTreeView.Nodes);
            RefreshTextBoxes();
        }

        private void tbSoul_Validated(object sender, EventArgs e)
        {
            RefreshTree(CharacterTreeView.Nodes);
            RefreshTextBoxes();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile(false);
        }

        private void exportToTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                //InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                RestoreDirectory = false,
                Filter = "Export Files (*.txt)|*.txt|All Files (*.*)|*.*",
                FilterIndex = 1
            };

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                CurrentController.ExportToText(saveFileDialog1.FileName);                
            }
            else
            {
                return; //User Pressed Cancel
            }
        }

        private void tbBody_ValueChanged(object sender, EventArgs e)
        {
            if (CharacterTreeView.SelectedNode.Tag.GetType() == typeof(CharacterData))
            {
                ((CharacterData)CharacterTreeView.SelectedNode.Tag).Body = (int)BodyTextBox.Value;
                RefreshTree(CharacterTreeView.Nodes);
                RefreshTextBoxes();
            }
        }

        private void tbMind_ValueChanged(object sender, EventArgs e)
        {
            if (CharacterTreeView.SelectedNode.Tag.GetType() == typeof(CharacterData))
            {
                ((CharacterData)CharacterTreeView.SelectedNode.Tag).Mind = (int)MindTextBox.Value;
                RefreshTree(CharacterTreeView.Nodes);
                RefreshTextBoxes();
            }
        }

        private void tbSoul_ValueChanged(object sender, EventArgs e)
        {
            if (CharacterTreeView.SelectedNode.Tag.GetType() == typeof(CharacterData))
            {
                ((CharacterData)CharacterTreeView.SelectedNode.Tag).Soul = (int)SoulTextBox.Value;
                RefreshTree(CharacterTreeView.Nodes);
                RefreshTextBoxes();
            }
        }

        private void bnAdd_Click(object sender, EventArgs e)
        {
            AddAttr();
        }

        private void bnDelete_Click(object sender, EventArgs e)
        {
            DelAttr();
        }

        private void bnMoveUp_Click(object sender, EventArgs e)
        {
            if (CharacterTreeView.SelectedNode != CharacterTreeView.Nodes[0] && CharacterTreeView.SelectedNode.Parent.Nodes.Count > 1 && CharacterTreeView.SelectedNode.PrevNode != null)
            {
                TreeNode tempnode = CharacterTreeView.SelectedNode;
                ((NodeData)tempnode.Tag).MoveUp();
                CharacterTreeView.Sort();
                CharacterTreeView.SelectedNode = tempnode;
                RefreshTree(tempnode.Parent.Nodes);
            }
        }

        private void bnMoveDown_Click(object sender, EventArgs e)
        {
            if (CharacterTreeView.SelectedNode != CharacterTreeView.Nodes[0] && CharacterTreeView.SelectedNode.Parent.Nodes.Count > 1 && CharacterTreeView.SelectedNode.NextNode != null)
            {
                TreeNode tempnode = CharacterTreeView.SelectedNode;
                ((NodeData)tempnode.Tag).MoveDown();

                CharacterTreeView.Sort();
                CharacterTreeView.SelectedNode = tempnode;

                RefreshTree(tempnode.Parent.Nodes);
            }
        }

        private void bnIncreaseLevel_Click(object sender, EventArgs e)
        {
            RaiseLevel();
            RefreshTextBoxes();
        }

        private void bnDecreaseLevel_Click(object sender, EventArgs e)
        {
            LowerLevel();
            RefreshTextBoxes();
        }

        private void lbVariantList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (VariantListBox.SelectedIndex >= 0)
            {
                if ((int)VariantListBox.SelectedValue > 0) 
                {
                    ((AttributeData)CharacterTreeView.SelectedNode.Tag).Variant = (int)VariantListBox.SelectedValue;
                    ((AttributeData)CharacterTreeView.SelectedNode.Tag).Name = VariantListBox.Text;

                    RefreshTree(CharacterTreeView.Nodes);
                }
            }
        }

        private void lbAttributeList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            AddAttr();
            AttributeListBox.Focus();
        }

        private void lbAttributeList_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                AddAttr();
            }
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void tbNotes_Validating(object sender, CancelEventArgs e)
        {
            ((NodeData)CharacterTreeView.SelectedNode.Tag).Notes = NotesTextBox.Text;
        }

        private void UpdateTreeFromNodes(TreeNode StartingTreePoint, NodeData StartingNodeData)
        {
            //Version for loading code:                                                     //Version for adding attribs:
            //StartingTreePoint == tvCharacterTree.Nodes.Add()                               //StartingTreePoint == tvCharacterTree.SelectedNode
            //StartingNodeData == RootCharacter                                              //StartingNodeData == FirstNewNodeData

            TreeNode TreeInsertionPoint = StartingTreePoint;
            NodeData CurrentNewNodeData = StartingNodeData;

            while (CurrentNewNodeData != null)
            {
                if (CurrentNewNodeData != CurrentController.RootCharacter)
                {
                    TreeInsertionPoint = TreeInsertionPoint.Nodes.Add(CurrentNewNodeData.Name);      //Add node to treeview
                }

                if (TreeInsertionPoint.Parent != null)
                {
                    TreeInsertionPoint.Parent.Expand();
                }

                TreeInsertionPoint.Tag = CurrentNewNodeData;

                if (CurrentNewNodeData.Children != null)
                {
                    CurrentNewNodeData = CurrentNewNodeData.Children;             //Now check for any children
                }
                else if (CurrentNewNodeData.Next != null)
                {
                    CurrentNewNodeData = CurrentNewNodeData.Next;                 //if no children add all siblings
                    TreeInsertionPoint = TreeInsertionPoint.Parent;               //move insertion point back up one
                }
                else if (CurrentNewNodeData.Parent == null)
                {
                    CurrentNewNodeData = null; //drop out of the loop                            
                }
                else if (CurrentNewNodeData.Parent != StartingNodeData && CurrentNewNodeData.Parent.Next != null) // make sure we are not back to original node
                {
                    CurrentNewNodeData = CurrentNewNodeData.Parent.Next;          //no children or siblings so add next sibling of the parent node
                    TreeInsertionPoint = TreeInsertionPoint.Parent.Parent;        //move insertion point back up two
                }
                else
                {
                    //either only one node to add, or we have gone through all of the above and ended up at the last immediate child of the original new node
                    CurrentNewNodeData = null; //drop out of the loop                            
                }
            }

            RefreshTree(CharacterTreeView.Nodes);  //Refresh the whole tree as can have impact both up and down the tree  
        }
    }
}