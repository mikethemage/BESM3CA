using BESM3CA.Model;
using BESM3CA.Templates;
using BESM3CA.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BESM3CA
{
    public partial class MainForm : Form
    {
        //Constants for adjusting right hand list and combo boxes:
        private const int HeightAdjust1 = 125;
        private const int HeightAdjust2 = 27;
        private const int HeightAdjust3 = 101;
        private const int HeightAdjust4 = 3;
        //****

        private const string ApplicationName = "BESM3CA";

        private TemplateData templateData;
        private CharacterData RootCharacter;

        private string FileName;        

        public MainForm()
        {
            InitializeComponent();
        }

        private void BESM3CA_Load(object sender, EventArgs e)
        {
            //checkMaxLevel = false;
            //Load template from file:
            templateData = TemplateData.JSONLoader();
            ResetAll();
        }

        private void ResetAll()
        {
            FileName = "";
            Text = ApplicationName;

            //Reset root character:
            RootCharacter = new CharacterData("", templateData);

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
            cbFilter.Items.Clear();
            cbFilter.Items.Add("All");
            cbFilter.SelectedIndex = 0;

            foreach (string item in templateData.GetTypesForFilter())
            {
                cbFilter.Items.Add(item);
            }
        }

        private void RefreshVariants()
        {
            if (tvCharacterTree.SelectedNode.Tag.GetType() == typeof(AttributeData) && ((AttributeData)tvCharacterTree.SelectedNode.Tag).HasVariants)
            {
                //LINQ Version:
                IEnumerable<ListItems> FilteredVarList = from Att in templateData.AttributeList
                                                         where Att.ID == ((AttributeData)tvCharacterTree.SelectedNode.Tag).AttributeID
                                                         from Vari in templateData.VariantList
                                                         where Att.ID == Vari.AttributeID
                                                         orderby Vari.DefaultVariant descending, Vari.Name
                                                         select new ListItems
                                                         (
                                                             Att.Name + " [" + Vari.Name + "]",
                                                             Vari.ID
                                                         );

                lbVariantList.Items.Clear();
                lbVariantList.Visible = true;
                lbVariant.Visible = true;
                cbFilter.Top = HeightAdjust3;

                if (lbAttributeList.Top == HeightAdjust2)
                {
                    lbAttributeList.Height -= (HeightAdjust1 - HeightAdjust2);
                }
                lbAttributeList.Top = HeightAdjust1;

                lbVariantList.Items.AddRange(FilteredVarList.ToArray());

                lbVariantList.DisplayMember = "DisplayMember";
                lbVariantList.ValueMember = "ValueMember";
            }
            else
            {
                lbVariantList.Items.Clear();
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

            List<ListItems> FilteredAttList = ((NodeData)tvCharacterTree.SelectedNode.Tag).GetFilteredPotentialChildren(Filter);

            if (FilteredAttList != null)
            {
                lbAttributeList.Items.Clear();

                string Type = "";
                foreach (ListItems item in FilteredAttList)
                {
                    if (Type != item.OptionalMember)
                    {
                        if (Type != "")
                        {
                            lbAttributeList.Items.Add(new ListItems("-------------------------", 0));
                        }
                        Type = item.OptionalMember;
                        lbAttributeList.Items.Add(new ListItems(Type + ":", 0));
                        lbAttributeList.Items.Add(new ListItems("-------------------------", 0));
                    }
                    lbAttributeList.Items.Add(item);
                }

                lbAttributeList.DisplayMember = "DisplayMember";
                lbAttributeList.ValueMember = "ValueMember";
            }
        }

        private void AddAttr()
        {
            if (lbAttributeList.SelectedIndex >= 0 && ((ListItems)lbAttributeList.SelectedItem).ValueMember > 0)
            {
                TreeNode NewNode;
                NewNode = tvCharacterTree.SelectedNode.Nodes.Add(((ListItems)lbAttributeList.SelectedItem).DisplayMember.ToString());

                AttributeListing Att = templateData.AttributeList.FirstOrDefault(n => n.ID == ((ListItems)lbAttributeList.SelectedItem).ValueMember);

                //Temp code for subbing in decoupler:                   
                NewNode.Tag = new AttributeData(NewNode.Text, Att.ID, "", Att.CostperLevel, templateData);
                ((NodeData)NewNode.Parent.Tag).AddChild((NodeData)NewNode.Tag);
                //***

                TreeNode NewSubNode;
                if (((NodeData)NewNode.Tag).Children != null)
                {
                    //Required children now created in class, just need to check for them and update treeview accordingly:
                    NodeData RequiredChildren = ((NodeData)NewNode.Tag).Children;
                    while (RequiredChildren != null)
                    {
                        NewSubNode = NewNode.Nodes.Add(((NodeData)NewNode.Tag).Children.Name);
                        NewSubNode.Tag = ((NodeData)NewNode.Tag).Children;
                        NewSubNode.Parent.Expand();
                        RequiredChildren = RequiredChildren.Next;
                    }
                }

                RefreshTree(tvCharacterTree.Nodes);
                tvCharacterTree.SelectedNode.Expand();
            }
        }

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

            CalcStats stats = CalcStats.GetStats((NodeData)tvCharacterTree.SelectedNode.Tag);//, templateData);

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

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile(true);
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
            SaveLoad Saver = new SaveLoad();
            Saver.SerializeTreeView(tvCharacterTree, FileName, templateData);
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
                //SaveLoad Loader = new SaveLoad();
                SaveLoad.DeserializeTreeView(tvCharacterTree, openFileDialog1.FileName, templateData);
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

        private void RefreshTree(TreeNodeCollection Nodes)
        {
            foreach (TreeNode Node in Nodes)
            {
                RefreshTree(Node.Nodes);
                Node.Text = ((NodeData)Node.Tag).DisplayText;
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
                    ExportNode(tvCharacterTree.Nodes, 0, tw);
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

        private void ExportNode(TreeNodeCollection nodes, int tabdepth, TextWriter tw)
        {
            string tabstring = "";
            for (int i = 0; i < tabdepth; i++)
            {
                tabstring += ("\t");
            }
            bool isAttrib = false;
            foreach (TreeNode current in nodes)
            {
                string nexttabstring;

                if (current.Tag.GetType() == typeof(CharacterData))
                {
                    //write stuff
                    // write a line of text to the file
                    tw.WriteLine(tabstring + current.Text);

                    nexttabstring = tabstring + "\t";

                    tw.WriteLine(nexttabstring + "Mind: " + ((CharacterData)current.Tag).Mind);

                    tw.WriteLine(nexttabstring + "Body: " + ((CharacterData)current.Tag).Body);

                    tw.WriteLine(nexttabstring + "Soul: " + ((CharacterData)current.Tag).Soul);

                    tw.WriteLine();

                    CalcStats stats = CalcStats.GetStats((NodeData)current.Tag);

                    tw.WriteLine(nexttabstring + "ACV: " + stats.ACV);
                    tw.WriteLine(nexttabstring + "DCV: " + stats.DCV);
                    tw.WriteLine(nexttabstring + "Health: " + stats.Health);
                    tw.WriteLine(nexttabstring + "Energy: " + stats.Energy);

                    tw.WriteLine();
                }
                else
                {
                    AttributeListing SelectedAttribute = templateData.AttributeList.Where(n => n.ID == ((AttributeData)current.Tag).ID).First();

                    if (SelectedAttribute.Type == "Attribute")
                    {
                        //write stuff
                        // write a line of text to the file
                        tw.WriteLine(tabstring + current.Text);

                        nexttabstring = tabstring + "\t";

                        if (((AttributeData)current.Tag).Name == "Item")
                        {

                            tw.WriteLine(tabstring + "(");
                        }
                        else
                        {
                            tw.WriteLine(nexttabstring + "Level " + ((AttributeData)current.Tag).Level + " x " + ((AttributeData)current.Tag).PointsPerLevel + " = " + (((AttributeData)current.Tag).Level * ((AttributeData)current.Tag).PointsPerLevel));
                        }

                        tw.WriteLine(nexttabstring + "Description: " + SelectedAttribute.Description);

                        isAttrib = true;
                    }
                    else
                    {
                        //write stuff
                        // write a line of text to the file
                        tw.WriteLine(tabstring + current.Text + " Level " + ((AttributeData)current.Tag).Level);

                        nexttabstring = tabstring + "\t";
                    }

                }
                if (((NodeData)current.Tag).Notes != "")
                {
                    tw.WriteLine(nexttabstring + "[Notes: " + (((NodeData)current.Tag).Notes).Replace("\n", "\n" + nexttabstring) + "]");
                }

                ExportNode(current.Nodes, tabdepth + 1, tw);

                if (isAttrib)
                {
                    if (((AttributeData)current.Tag).Name == "Item")
                    {
                        tw.WriteLine(tabstring + ") / 2");
                    }

                    tw.WriteLine();
                }
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

        private void DelAttr()
        {
            if (tvCharacterTree.SelectedNode.Tag.GetType() != typeof(CharacterData))
            {
                if (((AttributeData)tvCharacterTree.SelectedNode.Tag).PointAdj >= 0)  //do not delete "freebies"
                {
                    TreeNode tempNode = tvCharacterTree.SelectedNode.NextNode;
                    ((NodeData)tvCharacterTree.SelectedNode.Tag).Delete();
                    tvCharacterTree.SelectedNode.Remove();
                    tvCharacterTree.SelectedNode = tempNode;
                    RefreshTree(tvCharacterTree.Nodes);
                }
            }
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
                if (((ListItems)lbVariantList.SelectedItem).ValueMember > 0)
                {
                    ((AttributeData)tvCharacterTree.SelectedNode.Tag).Variant = ((ListItems)lbVariantList.SelectedItem).ValueMember;
                    ((AttributeData)tvCharacterTree.SelectedNode.Tag).Name = ((ListItems)lbVariantList.SelectedItem).DisplayMember;

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
    }
}