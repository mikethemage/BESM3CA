using BESM3CA.Model;
using BESM3CA.Templates;
using BESM3CA.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace BESM3CA
{
    public partial class MainForm : Form
    {
        //Fields:
        //Title bar:
        private const string ApplicationName = "BESM3CA";
        private string FileName;

        //Data:
        private TemplateData templateData;
        private CharacterData RootCharacter;


        //Constructor:
        public MainForm()
        {
            InitializeComponent();
        }


        //Initialisation code:
        private void BESM3CA_Load(object sender, EventArgs e)
        {
            //Load template from file:
            templateData = TemplateData.JSONLoader();
            ResetAll();
        }


        //UI member functions:
        private void ResetAll()
        {
            FileName = "";
            Text = ApplicationName;

            //Reset root character:
            RootCharacter = new CharacterData(templateData);

            //reset Treeview and link to root:
            tvCharacterTree.Nodes.Clear();
            TreeNode Root;
            Root = tvCharacterTree.Nodes.Add("Character");
            Root.Tag = RootCharacter;
            tvCharacterTree.SelectedNode = Root;
            //***

            //Refresh right hand boxes:
            RefreshFilter();
            RefreshList();

            //Todo: update refresh data code:
            RefreshTree(tvCharacterTree.Nodes);
            RefreshTextBoxes();
            tvCharacterTree.TreeViewNodeSorter = new NodeSorter();
            tvCharacterTree.SelectedNode = Root;
            //***            
        }

        private void RefreshFilter()
        {
            cbFilter.DataSource = templateData.GetTypesForFilter();
            cbFilter.SelectedIndex = 0;
        }

        private void RefreshVariants()
        {
            //Constants for adjusting right hand list and combo boxes:
            const int HeightAdjust1 = 125;
            const int HeightAdjust2 = 27;
            const int HeightAdjust3 = 101;
            const int HeightAdjust4 = 3;
            //****

            if (tvCharacterTree.SelectedNode.Tag.GetType() == typeof(AttributeData) && ((AttributeData)tvCharacterTree.SelectedNode.Tag).HasVariants)
            {
                List<ListItems> FilteredVarList = ((AttributeData)tvCharacterTree.SelectedNode.Tag).GetVariants();
                if (FilteredVarList != null)
                {
                    lbVariantList.Visible = true;
                    lbVariant.Visible = true;
                    cbFilter.Top = HeightAdjust3;

                    if (lbAttributeList.Top == HeightAdjust2)
                    {
                        lbAttributeList.Height -= (HeightAdjust1 - HeightAdjust2);
                    }

                    lbAttributeList.Top = HeightAdjust1;

                    lbVariantList.SelectedIndexChanged -= lbVariantList_SelectedIndexChanged;   //Temporarily disable event
                    lbVariantList.DisplayMember = "Name";
                    lbVariantList.ValueMember = "ID";
                    lbVariantList.DataSource = FilteredVarList;

                    if( ((AttributeData)tvCharacterTree.SelectedNode.Tag).Variant>0)
                    {
                        lbVariantList.SelectedValue = ((AttributeData)tvCharacterTree.SelectedNode.Tag).Variant;  //Load in saved variant
                    }
                    else
                    {
                        lbVariantList.SelectedIndex = -1; // This optional line keeps the first item from being selected.
                    }                    
                    lbVariantList.SelectedIndexChanged += lbVariantList_SelectedIndexChanged;   //Re-enable event           
                }
            }
            else
            {                
                lbVariantList.Visible = false;
                lbVariant.Visible = false;
                cbFilter.Top = HeightAdjust4;
                if (lbAttributeList.Top == HeightAdjust1)
                {
                    lbAttributeList.Height += (HeightAdjust1 - HeightAdjust2);
                }
                lbAttributeList.Top = HeightAdjust2;
            }
        }

        private void RefreshList()
        {
            RefreshVariants();

            string Filter;
            if (cbFilter.SelectedIndex == -1)
            {
                Filter = "";
            }
            else
            {
                Filter = cbFilter.Items[cbFilter.SelectedIndex].ToString();
            }         
            
            lbAttributeList.DisplayMember = "Name";
            lbAttributeList.ValueMember = "ID"; 
            lbAttributeList.DataSource = ((NodeData)tvCharacterTree.SelectedNode.Tag).GetFilteredPotentialChildren(Filter);            
        }

        private void AddAttr()
        {
            if (tvCharacterTree.SelectedNode!=null && lbAttributeList.SelectedIndex >= 0 && (int)lbAttributeList.SelectedValue>0)          
            {
                NodeData FirstNewNodeData = ((NodeData)tvCharacterTree.SelectedNode.Tag).AddChildAttribute(lbAttributeList.Text, (int)lbAttributeList.SelectedValue);
                UpdateTreeFromNodes(tvCharacterTree.SelectedNode, FirstNewNodeData);
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
            tbNotes.Text = ((NodeData)tvCharacterTree.SelectedNode.Tag).Notes;

            CalcStats stats = CalcStats.GetStats((NodeData)tvCharacterTree.SelectedNode.Tag);

            if (((NodeData)tvCharacterTree.SelectedNode.Tag).HasCharacterStats)
            {
                tbBody.Text = ((CharacterData)tvCharacterTree.SelectedNode.Tag).Body.ToString();
                tbMind.Text = ((CharacterData)tvCharacterTree.SelectedNode.Tag).Mind.ToString();
                tbSoul.Text = ((CharacterData)tvCharacterTree.SelectedNode.Tag).Soul.ToString();
                tbHealth.Text = stats.Health.ToString();
                tbEnergy.Text = stats.Energy.ToString();
                tbACV.Text = stats.ACV.ToString();
                tbDCV.Text = stats.DCV.ToString();
                tbBody.Visible = true;
                tbMind.Visible = true;
                tbSoul.Visible = true;
                tbHealth.Visible = true;
                tbEnergy.Visible = true;
                tbACV.Visible = true;
                tbDCV.Visible = true;
                lbBody.Visible = true;
                lbMind.Visible = true;
                lbSoul.Visible = true;
                lbHealth.Visible = true;
                lbEnergy.Visible = true;
                lbACV.Visible = true;
                lbDCV.Visible = true;
            }
            else
            {
                tbBody.Text = "";
                tbMind.Text = "";
                tbSoul.Text = "";
                tbBody.Visible = false;
                tbMind.Visible = false;
                tbSoul.Visible = false;
                tbHealth.Visible = false;
                tbEnergy.Visible = false;
                lbBody.Visible = false;
                lbMind.Visible = false;
                lbSoul.Visible = false;
                lbHealth.Visible = false;
                lbEnergy.Visible = false;
                lbACV.Visible = false;
                lbDCV.Visible = false;
                tbACV.Visible = false;
                tbDCV.Visible = false;
            }

            if (((NodeData)tvCharacterTree.SelectedNode.Tag).HasLevelStats)
            {
                if (tvCharacterTree.SelectedNode.Tag.GetType() == typeof(AttributeData))
                {
                    tbLevel.Text = ((AttributeData)tvCharacterTree.SelectedNode.Tag).Level.ToString();
                    tbDesc.Text = ((AttributeData)tvCharacterTree.SelectedNode.Tag).AttributeDescription;
                }
                else
                {
                    tbLevel.Text = "";
                    tbDesc.Text = "";
                }
                tbLevel.Visible = true;
                tbDesc.Visible = true;
                lbLevel.Visible = true;
                lbDescription.Visible = true;
            }
            else
            {
                tbLevel.Text = "";
                tbDesc.Text = "";
                tbLevel.Visible = false;
                tbDesc.Visible = false;
                lbLevel.Visible = false;
                lbDescription.Visible = false;
            }

            if (((NodeData)tvCharacterTree.SelectedNode.Tag).HasPointsStats)
            {
                if (tvCharacterTree.SelectedNode.Tag.GetType() == typeof(AttributeData))
                {
                    tbPPL.Text = ((AttributeData)tvCharacterTree.SelectedNode.Tag).PointsPerLevel.ToString();
                    tbPoints.Text = ((AttributeData)tvCharacterTree.SelectedNode.Tag).BaseCost.ToString();
                }
                else
                {
                    tbLevel.Text = "";
                    tbDesc.Text = "";
                }
                tbPPL.Visible = true;
                tbPoints.Visible = true;
                lbPointsPerLevel.Visible = true;
                lbPointCost.Visible = true;
            }
            else
            {
                tbPPL.Text = "";
                tbPoints.Text = "";
                tbPPL.Visible = false;
                tbPoints.Visible = false;
                lbPointsPerLevel.Visible = false;
                lbPointCost.Visible = false;
            }
        }

        private void RaiseLevel()
        {
            if (tvCharacterTree.SelectedNode.Tag.GetType() == typeof(AttributeData))
            {
                ((AttributeData)tvCharacterTree.SelectedNode.Tag).RaiseLevel();
                RefreshTree(tvCharacterTree.Nodes);
            }
        }

        private void LowerLevel()
        {
            if (tvCharacterTree.SelectedNode.Tag.GetType() == typeof(AttributeData))
            {
                ((AttributeData)tvCharacterTree.SelectedNode.Tag).LowerLevel();
                RefreshTree(tvCharacterTree.Nodes);
            }
        }

        private void SaveFile(bool SaveExisting)
        {
            if (SaveExisting == false || FileName == "")
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
                    FileName = saveFileDialog1.FileName;
                    Text = ApplicationName + " - " + FileName;
                }
                else
                {
                    return; //User Pressed Cancel
                }
            }

            SaveLoad.SerializeXML(RootCharacter, FileName, templateData);
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
            if (tvCharacterTree.SelectedNode != null && tvCharacterTree.SelectedNode.Tag.GetType() != typeof(CharacterData)) //do not allow manual deletion of Character nodes
            {
                if (((AttributeData)tvCharacterTree.SelectedNode.Tag).PointAdj >= 0)  //do not delete "freebies"
                {
                    TreeNode tempNode;
                    if (tvCharacterTree.SelectedNode.NextNode != null)
                    {
                        tempNode = tvCharacterTree.SelectedNode.NextNode;
                    }
                    else
                    {
                        tempNode = tvCharacterTree.SelectedNode.PrevNode;
                    }

                    ((NodeData)tvCharacterTree.SelectedNode.Tag).Delete();
                    tvCharacterTree.SelectedNode.Remove();
                    tvCharacterTree.SelectedNode = tempNode;
                    RefreshTree(tvCharacterTree.Nodes);
                }
            }
        }


        //Events:
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            RefreshList();
            RefreshTextBoxes();

            if (tvCharacterTree.SelectedNode.Tag.GetType() == typeof(CharacterData))
            {
                DisableLevelButtons();
            }
            else if (tvCharacterTree.SelectedNode.Tag.GetType() == typeof(AttributeData))
            {
                if (((AttributeData)tvCharacterTree.SelectedNode.Tag).HasLevel)
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
                tvCharacterTree.Nodes.Clear();

                RootCharacter = (CharacterData)SaveLoad.DeserializeXML(openFileDialog1.FileName, templateData);

                UpdateTreeFromNodes(tvCharacterTree.Nodes.Add("Character"), RootCharacter);

                FileName = openFileDialog1.FileName;
                Text = ApplicationName + " - " + FileName;
                if (tvCharacterTree.Nodes.Count > 0)
                {
                    tvCharacterTree.SelectedNode = tvCharacterTree.Nodes[0];
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
            if (tvCharacterTree.SelectedNode.Tag.GetType() == typeof(CharacterData))
            {
                if (int.TryParse(tbBody.Text, out int temp) && temp > 0)
                {
                    ((CharacterData)tvCharacterTree.SelectedNode.Tag).Body = temp;
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
            if (tvCharacterTree.SelectedNode.Tag.GetType() == typeof(CharacterData))
            {
                if (int.TryParse(tbMind.Text, out int temp) && temp > 0)
                {
                    ((CharacterData)tvCharacterTree.SelectedNode.Tag).Mind = temp;
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
            if (tvCharacterTree.SelectedNode.Tag.GetType() == typeof(CharacterData))
            {
                if (int.TryParse(tbSoul.Text, out int temp) && temp > 0)
                {
                    ((CharacterData)tvCharacterTree.SelectedNode.Tag).Soul = temp;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void tbBody_Validated(object sender, EventArgs e)
        {
            RefreshTree(tvCharacterTree.Nodes);
            RefreshTextBoxes();
        }
        private void tbMind_Validated(object sender, EventArgs e)
        {
            RefreshTree(tvCharacterTree.Nodes);
            RefreshTextBoxes();
        }

        private void tbSoul_Validated(object sender, EventArgs e)
        {
            RefreshTree(tvCharacterTree.Nodes);
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
                //open file saveFileDialog1.FileName
                TextWriter tw;
                try
                {
                    tw = new StreamWriter(saveFileDialog1.FileName);
                    SaveLoad.ExportNode(RootCharacter, 0, tw);
                }
                catch
                {
                    MessageBox.Show("Error Opening file: " + saveFileDialog1.FileName);
                    return;
                }

                //close file
                tw.Close();
            }
            else
            {
                return; //User Pressed Cancel
            }
        }

        private void tbBody_ValueChanged(object sender, EventArgs e)
        {
            if (tvCharacterTree.SelectedNode.Tag.GetType() == typeof(CharacterData))
            {
                ((CharacterData)tvCharacterTree.SelectedNode.Tag).Body = (int)tbBody.Value;
                RefreshTree(tvCharacterTree.Nodes);
                RefreshTextBoxes();
            }
        }

        private void tbMind_ValueChanged(object sender, EventArgs e)
        {
            if (tvCharacterTree.SelectedNode.Tag.GetType() == typeof(CharacterData))
            {
                ((CharacterData)tvCharacterTree.SelectedNode.Tag).Mind = (int)tbMind.Value;
                RefreshTree(tvCharacterTree.Nodes);
                RefreshTextBoxes();
            }
        }

        private void tbSoul_ValueChanged(object sender, EventArgs e)
        {
            if (tvCharacterTree.SelectedNode.Tag.GetType() == typeof(CharacterData))
            {
                ((CharacterData)tvCharacterTree.SelectedNode.Tag).Soul = (int)tbSoul.Value;
                RefreshTree(tvCharacterTree.Nodes);
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
            if (tvCharacterTree.SelectedNode != tvCharacterTree.Nodes[0] && tvCharacterTree.SelectedNode.Parent.Nodes.Count > 1 && tvCharacterTree.SelectedNode.PrevNode != null)
            {
                TreeNode tempnode = tvCharacterTree.SelectedNode;
                ((NodeData)tempnode.Tag).MoveUp();
                tvCharacterTree.Sort();
                tvCharacterTree.SelectedNode = tempnode;
                RefreshTree(tempnode.Parent.Nodes);
            }
        }

        private void bnMoveDown_Click(object sender, EventArgs e)
        {
            if (tvCharacterTree.SelectedNode != tvCharacterTree.Nodes[0] && tvCharacterTree.SelectedNode.Parent.Nodes.Count > 1 && tvCharacterTree.SelectedNode.NextNode != null)
            {
                TreeNode tempnode = tvCharacterTree.SelectedNode;
                ((NodeData)tempnode.Tag).MoveDown();

                tvCharacterTree.Sort();
                tvCharacterTree.SelectedNode = tempnode;

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
            if (lbVariantList.SelectedIndex >= 0)
            {
                if ((int)lbVariantList.SelectedValue > 0) 
                {
                    ((AttributeData)tvCharacterTree.SelectedNode.Tag).Variant = (int)lbVariantList.SelectedValue;
                    ((AttributeData)tvCharacterTree.SelectedNode.Tag).Name = lbVariantList.Text;

                    RefreshTree(tvCharacterTree.Nodes);
                }
            }
        }

        private void lbAttributeList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            AddAttr();
            lbAttributeList.Focus();
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
            ((NodeData)tvCharacterTree.SelectedNode.Tag).Notes = tbNotes.Text;
        }

        private void UpdateTreeFromNodes(TreeNode StartingTreePoint, NodeData StartingNodeData)
        {
            //Version for loading code:                                                                 //Version for adding attribs:
            //StartingTreePoint = tvCharacterTree.Nodes.Add("Character");                               //StartingTreePoint = tvCharacterTree.SelectedNode;
            //StartingNodeData = RootCharacter;                                                         //StartingNodeData = FirstNewNodeData;

            TreeNode TreeInsertionPoint = StartingTreePoint;
            NodeData CurrentNewNodeData = StartingNodeData;

            while (CurrentNewNodeData != null)
            {
                if (CurrentNewNodeData != RootCharacter)
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

            RefreshTree(tvCharacterTree.Nodes);  //Refresh the whole tree as can have impact both up and down the tree  
        }
    }
}