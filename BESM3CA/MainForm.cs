using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Linq;

namespace BESM3CA
{
    public partial class MainForm : Form
    {
        private string FileName;
        private bool checkMaxLevel;

        private List<AttributeListing> AttributeList;
        private List<VariantListing> VariantList;
        private List<TypeListing> TypeList;

        public MainForm()
        {
            InitializeComponent();
        }


        private void BESM3CA_Load(object sender, EventArgs e)
        {
            checkMaxLevel = false;




            AttributeList = new List<AttributeListing>();
            VariantList = new List<VariantListing>();
            TypeList = new List<TypeListing>();

            DatabaseLoader.LoadDatabase(AttributeList, VariantList, TypeList);
            JSONyStuff.createJSON(AttributeList);

            AttributeList = new List<AttributeListing>();
            JSONyStuff.JSONLoader(out AttributeList, VariantList, TypeList);

            ResetAll();
        }

        private void ResetAll()
        {
            FileName = "";
            this.Text = "BESM3CA";
            listBox1.DataSource = null;
            treeView1.Nodes.Clear();
            TreeNode Root;
            Root = treeView1.Nodes.Add("Character");
            Root.Tag = new CharacterData("");
            ((NodeData)Root.Tag).NodeOrder = 1;
            treeView1.SelectedNode = Root;
            RefreshFilter();
            RefreshList();
            refreshTree(treeView1.Nodes);
            RefreshTextBoxes();
            treeView1.TreeViewNodeSorter = new NodeSorter();
            treeView1.SelectedNode = Root;
        }

        private void RefreshFilter()
        {
            comboBox1.Items.Add("All");
            comboBox1.SelectedIndex = 0;

            //LINQ Version:
            var FilteredTypeList = from AttType in TypeList
                                   orderby AttType.Name
                                   select AttType.Name;

            foreach (var item in FilteredTypeList)
            {
                comboBox1.Items.Add(item);
            }

        }

        private void RefreshVariants()
        {
            if (treeView1.SelectedNode.Tag.GetType() == typeof(BESM3CA.AttributeData))
            {
                //LINQ Version:
                var FilteredVarList = from Att in AttributeList
                                      where Att.ID == ((AttributeData)treeView1.SelectedNode.Tag).AttributeID
                                      from Vari in VariantList
                                      where Att.ID == Vari.AttributeID
                                      orderby Vari.DefaultVariant descending, Vari.Name
                                      select (Att.ID, AttributeName: Att.Name, VariantID: Vari.ID, VariantName: Vari.Name, Vari.CostperLevel, Vari.Desc, Vari.DefaultVariant);

                listBox2.Items.Clear();

                if (FilteredVarList.Any())
                {
                    listBox2.Visible = true;
                    label4.Visible = true;
                    comboBox1.Top = 77;

                    if (listBox1.Top == 26)
                    {
                        listBox1.Height -= 104 - 26;
                    }
                    listBox1.Top = 104;

                    foreach (var item in FilteredVarList)
                    {
                        listBox2.Items.Add(new ListItems(item.AttributeName + " [" + item.VariantName + "]", item.VariantID));
                    }
                }
                else
                {
                    comboBox1.Top = 3;

                    if (listBox1.Top == 104)
                    {
                        listBox1.Height += 104 - 26;
                    }
                    listBox1.Top = 26;
                    listBox2.Visible = false;
                    label4.Visible = false;
                }

                listBox2.DisplayMember = "DisplayMember";
                listBox2.ValueMember = "ValueMember";
            }
            else
            {
                comboBox1.Top = 3;
                if (listBox1.Top == 104)
                {
                    listBox1.Height += 104 - 26;
                }
                listBox1.Top = 26;

                listBox2.Items.Clear();
                listBox2.Visible = false;
                label4.Visible = false;
            }
        }

        private void RefreshList()
        {
            RefreshVariants();


            List<AttributeListing> SelectedAttributeChildren;

            if (treeView1.SelectedNode.Tag.GetType() == typeof(BESM3CA.AttributeData))
            {
                SelectedAttributeChildren = AttributeList.Where(n => n.ID == ((AttributeData)treeView1.SelectedNode.Tag).ID).First().Children.Values.ToList<AttributeListing>();
                

            }
            else
            {
                SelectedAttributeChildren = AttributeList;
            }

            //LINQ Version:
            var FilteredAttList = from Att in AttributeList
                                  where
                                  (comboBox1.SelectedIndex == -1 || comboBox1.Items[comboBox1.SelectedIndex].ToString() == "All" || comboBox1.Items[comboBox1.SelectedIndex].ToString() == "" || Att.Type == comboBox1.Items[comboBox1.SelectedIndex].ToString())
                                  && 
                                  (treeView1.SelectedNode.Tag.GetType() == typeof(BESM3CA.AttributeData) || Att.Type == "Attribute" || Att.Type == "Defect" || Att.Type == "Skill")
                                  &&
                                  Att.Name!="Character"
                                  from Children in SelectedAttributeChildren
                                  where
                                  Att.ID==Children.ID
                                  orderby Att.Type, Att.Name
                                  select (Att.ID, Att.Name, Att.Type);

           


            /* "Select Attribute.AttributeID, Attribute.AttributeName, Type from "
             + "Attribute, AttChildren,Types Where Attribute.Type=Types.TypeName " + FilterAtts + " and AttributeName<>'Character' and ChildID=Attribute.AttributeID and ParentID=" 
             + ((AttributeData)treeView1.SelectedNode.Tag).AttributeID + " Order By Types.TypeOrder, AttributeName;"
              */

                //**

                listBox1.Items.Clear();
            string Type = "";

            foreach (var item in FilteredAttList)
            {
                if (Type != item.Type)
                {
                    if (Type != "")
                    {
                        listBox1.Items.Add(new ListItems("-------------------------", 0));
                    }
                    Type = item.Type;
                    listBox1.Items.Add(new ListItems(Type + ":", 0));
                    listBox1.Items.Add(new ListItems("-------------------------", 0));
                }
                listBox1.Items.Add(new ListItems(item.Name, item.ID));
            }

            listBox1.DisplayMember = "DisplayMember";
            listBox1.ValueMember = "ValueMember";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            add_attr();
        }

        private void add_attr()
        {
            if (listBox1.SelectedIndex >= 0 && ((ListItems)listBox1.SelectedItem).ValueMember > 0)
            {

                TreeNode NewNode;
                NewNode = treeView1.SelectedNode.Nodes.Add(((ListItems)listBox1.SelectedItem).DisplayMember.ToString());

                var CostPerLevel = from Att in AttributeList
                                   where Att.ID == ((ListItems)listBox1.SelectedItem).ValueMember
                                   select Att.CostperLevel;

                int baselevel = 0;
                if (NewNode.Text == "Weapon")
                {
                    baselevel = 0;

                }
                else
                {
                    baselevel = 1;
                }

                if (CostPerLevel.First() == 0)
                {
                    var RequiresVariant = from Att in AttributeList
                                          where Att.ID == ((ListItems)listBox1.SelectedItem).ValueMember
                                          select Att.RequiresVariant;

                    if (RequiresVariant.First() == true)
                    {
                        NewNode.Tag = new AttributeData(NewNode.Text, ((ListItems)listBox1.SelectedItem).ValueMember, "", baselevel, 0);

                    }
                    else
                    {
                        NewNode.Tag = new AttributeData(NewNode.Text, ((ListItems)listBox1.SelectedItem).ValueMember, "", 0);
                    }
                }
                else
                {
                    //int pointsPerLevel = (int)cmd.ExecuteScalar();
                    NewNode.Tag = new AttributeData(NewNode.Text, ((ListItems)listBox1.SelectedItem).ValueMember, "", baselevel, CostPerLevel.First());
                }

                                    ((NodeData)NewNode.Tag).NodeOrder = NewNode.Parent.Nodes.Count;

                if (((ListItems)listBox1.SelectedItem).DisplayMember == "Companion")
                {
                    NewNode = NewNode.Nodes.Add("Character");
                    NewNode.Tag = new CharacterData("");
                    NewNode.Parent.Expand();
                }

                if (((ListItems)listBox1.SelectedItem).DisplayMember == "Mind Control")
                {
                    TreeNode NewSubNode;
                    NewSubNode = NewNode.Nodes.Add("Range");
                    NewSubNode.Tag = new AttributeData(NewSubNode.Text, 167, "", 3, 1, -3);
                    NewSubNode.Parent.Expand();
                }

                refreshTree(treeView1.Nodes);

                treeView1.SelectedNode.Expand();
                //treeView1.Focus();

            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            add_attr();
            listBox1.Focus();
        }


        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            RefreshList();
            RefreshTextBoxes();

        }

        private void RefreshTextBoxes()
        {
            // textBox1.Text = ((NodeData)treeView1.SelectedNode.Tag).Name + " " + GetPoints(treeView1.SelectedNode).ToString();
            textBox1.Text = ((NodeData)treeView1.SelectedNode.Tag).Notes;
            if (treeView1.SelectedNode.Tag.GetType() == typeof(BESM3CA.CharacterData))
            {
                tbBody.Text = ((CharacterData)treeView1.SelectedNode.Tag).Body.ToString();
                tbMind.Text = ((CharacterData)treeView1.SelectedNode.Tag).Mind.ToString();
                tbSoul.Text = ((CharacterData)treeView1.SelectedNode.Tag).Soul.ToString();
                tbBody.Visible = true;
                tbMind.Visible = true;
                tbSoul.Visible = true;
                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
                tbLevel.Text = "";
                tbDesc.Text = "";
                tbLevel.Visible = false;
                tbDesc.Visible = false;
                label5.Visible = false;
                label6.Visible = true;
                label7.Visible = true;
                label10.Visible = false;
                label11.Visible = false;

                CalcStats stats = GetStats(treeView1.SelectedNode);
                tbHealth.Visible = true;
                tbEnergy.Visible = true;
                tbHealth.Text = stats.Health.ToString();
                tbEnergy.Text = stats.Energy.ToString();

                label9.Visible = true;
                label8.Visible = true;
                tbACV.Visible = true;
                tbDCV.Visible = true;
                tbACV.Text = stats.ACV.ToString();
                tbDCV.Text = stats.DCV.ToString();

                label15.Visible = false;
                tbPPL.Visible = false;
                tbPoints.Visible = false;

            }
            else
            {
                tbBody.Text = "";
                tbMind.Text = "";
                tbSoul.Text = "";
                tbBody.Visible = false;
                tbMind.Visible = false;
                tbSoul.Visible = false;
                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;

                label6.Visible = false;
                label7.Visible = false;
                tbHealth.Visible = false;
                tbEnergy.Visible = false;

                label9.Visible = false;
                label8.Visible = false;
                tbACV.Visible = false;
                tbDCV.Visible = false;

                if (((AttributeData)treeView1.SelectedNode.Tag).Name != "Item")
                {
                    tbLevel.Text = ((AttributeData)treeView1.SelectedNode.Tag).Level.ToString();
                    tbLevel.Visible = true;
                    tbDesc.Visible = true;
                    label15.Visible = true;

                    var Description = from Att in AttributeList
                                      where Att.ID == ((AttributeData)treeView1.SelectedNode.Tag).ID
                                      select Att.Description;

                    tbDesc.Text = Description.First();

                    label5.Visible = true;
                    if (((AttributeData)treeView1.SelectedNode.Tag).Name != "Companion")
                    {
                        tbPPL.Visible = true;

                        tbPPL.Text = ((AttributeData)treeView1.SelectedNode.Tag).PointsPerLevel.ToString();
                        tbPoints.Text = ((((AttributeData)treeView1.SelectedNode.Tag).PointsPerLevel * ((AttributeData)treeView1.SelectedNode.Tag).Level) + ((AttributeData)treeView1.SelectedNode.Tag).PointAdj).ToString();
                        tbPoints.Visible = true;
                        label10.Visible = true;
                        label11.Visible = true;
                    }
                    else
                    {
                        tbPPL.Visible = false;

                        tbPoints.Visible = false;
                        label10.Visible = false;
                        label11.Visible = false;
                    }
                }
                else
                {

                    tbLevel.Visible = false;
                    label5.Visible = false;
                    tbPPL.Visible = false;

                    tbPoints.Visible = false;
                    label10.Visible = false;
                    label11.Visible = false;
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RaiseLevel();
            RefreshTextBoxes();
        }

        private void RaiseLevel()
        {
            if (treeView1.SelectedNode.Tag.GetType() == typeof(BESM3CA.AttributeData))
            {
                AttributeListing SelectedAttribute = AttributeList.Where(n => n.ID == ((AttributeData)treeView1.SelectedNode.Tag).ID).First();

                if ((checkMaxLevel == false && SelectedAttribute.EnforceMaxLevel == false) ||
                   (SelectedAttribute.MaxLevel != int.MaxValue && SelectedAttribute.MaxLevel > ((AttributeData)treeView1.SelectedNode.Tag).Level))
                {
                    ((AttributeData)treeView1.SelectedNode.Tag).raiseLevel();

                }
                refreshTree(treeView1.Nodes);

            }
        }

        private void LowerLevel()
        {
            if (treeView1.SelectedNode.Tag.GetType() == typeof(BESM3CA.AttributeData))
            {

                AttributeListing SelectedAttribute = AttributeList.Where(n => n.ID == ((AttributeData)treeView1.SelectedNode.Tag).ID).First();

                if (((AttributeData)treeView1.SelectedNode.Tag).Level > 1 || (((AttributeData)treeView1.SelectedNode.Tag).Level > 0 && SelectedAttribute.Name == "Weapon"))
                {
                    ((AttributeData)treeView1.SelectedNode.Tag).lowerLevel();

                }
                refreshTree(treeView1.Nodes);

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
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                saveFileDialog1.Filter = "BESM3CA Files (*.xml)|*.xml|All Files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 1;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    FileName = saveFileDialog1.FileName;
                    this.Text = "BESM3CA - " + FileName;
                }
                else
                {
                    return; //User Pressed Cancel
                }
            }
            SaveLoad Saver = new SaveLoad();
            Saver.SerializeTreeView(treeView1, FileName);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog1.Filter = "BESM3CA Files (*.xml)|*.xml|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ResetAll();
                treeView1.Nodes.Clear();
                SaveLoad Loader = new SaveLoad();
                Loader.DeserializeTreeView(treeView1, openFileDialog1.FileName);
                FileName = openFileDialog1.FileName;
                this.Text = "BESM3CA - " + FileName;
                if (treeView1.Nodes.Count > 0)
                {
                    treeView1.SelectedNode = treeView1.Nodes[0];
                }

            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to start a new character?  Any unsaved data will be lost!", "New", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ResetAll();
            }
        }

        private int GetPoints(TreeNode Node)
        {
            int basepoints = 0;
            int level = 1;
            bool isItem = false;
            bool isCompanion = false;
            bool isAlternateAttack = false;
            bool isAlternateForm = false;
            int PointAdj = 0;


            if (Node.Tag.GetType() == typeof(BESM3CA.AttributeData))
            {
                basepoints = ((AttributeData)Node.Tag).PointsPerLevel;
                level = ((AttributeData)Node.Tag).Level;
                isItem = ((AttributeData)Node.Tag).Name == "Item";
                isCompanion = ((AttributeData)Node.Tag).Name == "Companion";
                isAlternateForm = ((AttributeData)Node.Tag).Name == "Alternate Form";

                if (((AttributeData)Node.Tag).Variant > 0)
                {
                    VariantListing SelectedVariant = VariantList.Where(n => n.ID == ((AttributeData)Node.Tag).Variant).First();

                    if (SelectedVariant.Name == "Alternate Attack")
                    {
                        isAlternateAttack = true;
                    }
                }

                AttributeListing SelectedAttribute = AttributeList.Where(n => n.ID == ((AttributeData)Node.Tag).ID).First();

                if (SelectedAttribute.Type == "Special")
                {
                    basepoints = 0;
                    level = 0;

                }
                PointAdj = ((AttributeData)Node.Tag).PointAdj;
            }
            else
            {
                basepoints = ((CharacterData)Node.Tag).basecost;
                level = 1;

            }

            int Extra = 0;
            int itempoints = 0;
            foreach (TreeNode Child in Node.Nodes)
            {
                if (isItem == false && isCompanion == false && isAlternateForm == false)
                {
                    Extra += GetPoints(Child);
                }
                else if (isCompanion == true && Child.Tag.GetType() == typeof(BESM3CA.AttributeData))
                {
                    AttributeListing SelectedAttribute = AttributeList.Where(n => n.ID == ((AttributeData)Child.Tag).ID).First();

                    if (SelectedAttribute.Type == "Restriction")
                    {
                        Extra += GetPoints(Child);
                    }
                }
                else if (isItem == true)
                {
                    itempoints += GetPoints(Child);
                }
                else if (isCompanion == true)
                {
                    if (Child.Tag.GetType() == typeof(BESM3CA.CharacterData))
                    {
                        int temp = GetPoints(Child);
                        if (temp > 120)
                        {
                            basepoints += 2 + ((GetPoints(Child) - 120) / 10);
                        }
                        else
                        {
                            basepoints += 2;
                        }
                    }
                    else
                    {
                        Extra += GetPoints(Child);
                    }
                }
            }
            if (isItem)
            {
                if (itempoints < 1)
                {
                    basepoints = 1;
                }
                else
                {
                    basepoints += itempoints / 2;
                }
            }
            //MessageBox.Show(((NodeData)Node.Tag).Name + " " + basepoints.ToString() + " " + Extra.ToString());

            //***
            //if alternate weapon attack half points
            if (isAlternateAttack)
            {
                return ((basepoints * level) + Extra + 1) / 2;
            }
            //***

            return (basepoints * level) + Extra + PointAdj;

        }

        private void refreshTree(TreeNodeCollection Nodes)
        {

            foreach (TreeNode Node in Nodes)
            {
                refreshTree(Node.Nodes);

                if (Node.Tag.GetType() == typeof(BESM3CA.AttributeData))
                {
                    bool altform = false;
                    if (((AttributeData)Node.Tag).Name == "Alternate Form")
                    {
                        altform = true;
                    }

                    AttributeListing SelectedAttribute = AttributeList.Where(n => n.ID == ((AttributeData)Node.Tag).ID).First();

                    if (SelectedAttribute.SpecialContainer || altform)
                    {
                        int LevelsUsed = 0;
                        foreach (TreeNode child in Node.Nodes)
                        {
                            SelectedAttribute = AttributeList.Where(n => n.ID == ((AttributeData)child.Tag).ID).First();

                            if (SelectedAttribute.Type == "Special" || altform)
                            {
                                LevelsUsed += ((AttributeData)child.Tag).Level * ((AttributeData)child.Tag).PointsPerLevel;
                            }
                        }
                        int specialpoints;
                        if (altform)
                        {
                            specialpoints = ((AttributeData)Node.Tag).Level * 10;
                        }
                        else
                        {
                            specialpoints = ((AttributeData)Node.Tag).Level;
                        }
                        Node.Text = ((AttributeData)Node.Tag).Name + " (" + (specialpoints - LevelsUsed).ToString() + " Left)" + " (" + GetPoints(Node) + " Points)";
                    }
                    else
                    {
                        SelectedAttribute = AttributeList.Where(n => n.ID == ((AttributeData)Node.Tag).ID).First();

                        if (SelectedAttribute.Type == "Special")
                        {
                            Node.Text = ((AttributeData)Node.Tag).Name;

                        }
                        else
                        {
                            Node.Text = ((AttributeData)Node.Tag).Name + " (" + GetPoints(Node) + " Points)";
                        }
                    }
                }
                else
                {
                    Node.Text = ((CharacterData)Node.Tag).Name + " (" + GetPoints(Node) + " Points)";
                }

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Tag.GetType() != typeof(BESM3CA.CharacterData))
            {
                if (((BESM3CA.AttributeData)treeView1.SelectedNode.Tag).PointAdj >= 0)
                {

                    TreeNode tempNode = treeView1.SelectedNode.NextNode;
                    treeView1.SelectedNode.Remove();

                    while (tempNode != null)
                    {
                        ((NodeData)tempNode.Tag).NodeOrder -= 1;
                        tempNode = tempNode.NextNode;
                    }

                    refreshTree(treeView1.Nodes);
                }

            }
        }

        private void tbBody_Validating(object sender, CancelEventArgs e)
        {
            
        }

        private void tbMind_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = false;
            if (treeView1.SelectedNode.Tag.GetType() == typeof(BESM3CA.CharacterData))
            {
                int temp = 0;

                if (int.TryParse(tbMind.Text, out temp) && temp > 0)
                {
                    ((CharacterData)treeView1.SelectedNode.Tag).Mind = temp;
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
            if (treeView1.SelectedNode.Tag.GetType() == typeof(BESM3CA.CharacterData))
            {
                int temp = 0;

                if (int.TryParse(tbSoul.Text, out temp) && temp > 0)
                {
                    ((CharacterData)treeView1.SelectedNode.Tag).Soul = temp;
                }
                else
                {
                    e.Cancel = true;
                }

            }
        }

        private void tbBody_Validated(object sender, EventArgs e)
        {
            
        }

        private void tbMind_Validated(object sender, EventArgs e)
        {
            refreshTree(treeView1.Nodes);
            RefreshTextBoxes();
        }

        private void tbSoul_Validated(object sender, EventArgs e)
        {
            refreshTree(treeView1.Nodes);
            RefreshTextBoxes();
        }


        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex >= 0)
            {
                if (((ListItems)listBox2.SelectedItem).ValueMember > 0)
                {
                    ((AttributeData)treeView1.SelectedNode.Tag).Variant = ((ListItems)listBox2.SelectedItem).ValueMember;
                    ((AttributeData)treeView1.SelectedNode.Tag).Name = ((ListItems)listBox2.SelectedItem).DisplayMember;

                    VariantListing SelectedVariant = VariantList.Where(n => n.ID == ((ListItems)listBox2.SelectedItem).ValueMember).First();

                    ((AttributeData)treeView1.SelectedNode.Tag).PointsPerLevel = SelectedVariant.CostperLevel;
                    refreshTree(treeView1.SelectedNode.Parent.Nodes);
                }

            }
        }

        private void tbSoul_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            LowerLevel();
            RefreshTextBoxes();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != treeView1.Nodes[0] && treeView1.SelectedNode.Parent.Nodes.Count > 1 && treeView1.SelectedNode.PrevNode != null)
            {
                TreeNode tempnode = treeView1.SelectedNode;
                int temp = ((NodeData)treeView1.SelectedNode.Tag).NodeOrder;
                int temp2 = ((NodeData)treeView1.SelectedNode.PrevNode.Tag).NodeOrder;
                ((NodeData)treeView1.SelectedNode.Tag).NodeOrder = temp2;
                ((NodeData)treeView1.SelectedNode.PrevNode.Tag).NodeOrder = temp;
                treeView1.Sort();
                treeView1.SelectedNode = tempnode;

            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != treeView1.Nodes[0] && treeView1.SelectedNode.Parent.Nodes.Count > 1 && treeView1.SelectedNode.NextNode != null)
            {
                TreeNode tempnode = treeView1.SelectedNode;
                int temp = ((NodeData)treeView1.SelectedNode.Tag).NodeOrder;
                int temp2 = ((NodeData)treeView1.SelectedNode.NextNode.Tag).NodeOrder;
                ((NodeData)treeView1.SelectedNode.Tag).NodeOrder = temp2;
                ((NodeData)treeView1.SelectedNode.NextNode.Tag).NodeOrder = temp;
                treeView1.Sort();
                treeView1.SelectedNode = tempnode;

            }
        }

        private void listBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                add_attr();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile(false);
        }

        private void exportToTextToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog1.Filter = "Export Files (*.txt)|*.txt|All Files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //open file saveFileDialog1.FileName
                TextWriter tw;
                try
                {
                    tw = new StreamWriter(saveFileDialog1.FileName);
                    exportNode(treeView1.Nodes, 0, tw);
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

        private void exportNode(TreeNodeCollection nodes, int tabdepth, TextWriter tw)
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

                    CalcStats stats = GetStats(current);
                    tw.WriteLine(nexttabstring + "ACV: " + stats.ACV);
                    tw.WriteLine(nexttabstring + "DCV: " + stats.DCV);

                    tw.WriteLine(nexttabstring + "Health: " + stats.Health);

                    tw.WriteLine(nexttabstring + "Energy: " + stats.Energy);

                    tw.WriteLine();

                }
                else
                {
                    AttributeListing SelectedAttribute = AttributeList.Where(n => n.ID == ((AttributeData)current.Tag).ID).First();

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

                exportNode(current.Nodes, tabdepth + 1, tw);

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

        private CalcStats GetStats(TreeNode Node)
        {
            CalcStats stats;

            if (Node.Tag.GetType() == typeof(BESM3CA.AttributeData))
            {
                stats = new CalcStats(0, 0, 0, 0);

            }
            else
            {
                stats = new CalcStats(((CharacterData)Node.Tag).basehealth,
                 ((CharacterData)Node.Tag).baseenergy,
                ((CharacterData)Node.Tag).baseCV,
                ((CharacterData)Node.Tag).baseCV);
            }

            CalcStats temp;
            foreach (TreeNode current in Node.Nodes)
            {

                if (current.Tag.GetType() == typeof(BESM3CA.AttributeData))
                {
                    if (((AttributeData)current.Tag).Name == "Tough")
                    {
                        temp = new CalcStats(((AttributeData)current.Tag).Level * 5, 0, 0, 0);
                    }
                    else if (((AttributeData)current.Tag).Name == "Energy Bonus")
                    {
                        temp = new CalcStats(0, ((AttributeData)current.Tag).Level * 5, 0, 0);
                    }
                    else if (((AttributeData)current.Tag).Name == "Attack Combat Mastery")
                    {
                        temp = new CalcStats(0, 0, ((AttributeData)current.Tag).Level, 0);
                    }
                    else if (((AttributeData)current.Tag).Name == "Defence Combat Mastery")
                    {
                        temp = new CalcStats(0, 0, 0, ((AttributeData)current.Tag).Level);
                    }
                    else
                    {
                        temp = new CalcStats(0, 0, 0, 0);
                    }

                    if (temp.ACV > 0 || temp.DCV > 0 || temp.Energy > 0 || temp.Health > 0)
                    {
                        foreach (TreeNode child in current.Nodes)
                        {
                            if (child.Tag.GetType() == typeof(BESM3CA.AttributeData))
                            {
                                AttributeListing SelectedAttribute = AttributeList.Where(n => n.ID == ((AttributeData)child.Tag).ID).First();


                                if (SelectedAttribute.Type == "Restriction")
                                {
                                    temp = new CalcStats(0, 0, 0, 0);
                                    break;
                                }
                            }
                        }

                        stats.Health += temp.Health;
                        stats.Energy += temp.Energy;
                        stats.ACV += temp.ACV;
                        stats.DCV += temp.DCV;

                    }
                }

            }
            return stats;
        }


        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            ((NodeData)treeView1.SelectedNode.Tag).Notes = textBox1.Text;
        }

        private void tbPoints_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbBody_ValueChanged(object sender, EventArgs e)
        {
            
            if (treeView1.SelectedNode.Tag.GetType() == typeof(BESM3CA.CharacterData))
            {
                ((CharacterData)treeView1.SelectedNode.Tag).Body = (int)tbBody.Value;
                
            }
            refreshTree(treeView1.Nodes);
            RefreshTextBoxes();
        }

        private void tbMind_ValueChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Tag.GetType() == typeof(BESM3CA.CharacterData))
            {
                ((CharacterData)treeView1.SelectedNode.Tag).Mind = (int)tbMind.Value;

            }
            refreshTree(treeView1.Nodes);
            RefreshTextBoxes();
        }

        private void tbSoul_ValueChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Tag.GetType() == typeof(BESM3CA.CharacterData))
            {
                ((CharacterData)treeView1.SelectedNode.Tag).Soul = (int)tbSoul.Value;

            }
            refreshTree(treeView1.Nodes);
            RefreshTextBoxes();
        }
    }


    // Create a node sorter that implements the IComparer interface.
    public class NodeSorter : IComparer
    {
        // Compare the length of the strings, or the strings
        // themselves, if they are the same length.
        public int Compare(object x, object y)
        {
            TreeNode tx = x as TreeNode;
            TreeNode ty = y as TreeNode;

            if (tx.Tag == null && ty.Tag == null)
                return string.Compare(tx.Text, ty.Text);

            if (tx.Tag == null)
                return int.MaxValue;

            if (ty.Tag == null)
                return int.MinValue;

            // Compare the length of the strings, returning the difference.
            if (((NodeData)tx.Tag).NodeOrder != ((NodeData)ty.Tag).NodeOrder)
                return (((NodeData)tx.Tag).NodeOrder - ((NodeData)ty.Tag).NodeOrder);

            // If they are the same length, call Compare.
            return string.Compare(tx.Text, ty.Text);
        }
    }

    public class CalcStats
    {
        public int Health;
        public int Energy;
        public int ACV;
        public int DCV;

        public CalcStats(int h, int e, int a, int d)
        {
            Health = h;
            Energy = e;
            ACV = a;
            DCV = d;
        }
    }

}
