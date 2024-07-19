using BESM3CAData.Control;
using BESM3CAData.Model;
using Microsoft.Win32;
using System;
using System.Windows;

namespace BESM3CA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Title bar:
        private const string _applicationName = "BESM3CA";

        //Data:
        public DataController CurrentController { get; set; } = null!;


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
            if (CurrentController.SelectedListingData != null)
            {
                CurrentController.SelectedListingData.CreateJSON(@"C:\Users\Mike\Documents\NEW_BESM3E.json");
                ResetAll();
                GenreComboBox.SelectedIndex = 0;
                FilterComboBox.SelectedIndex = 0;
            }
        }


        //Helper Methods:
        private void ResetAll()
        {
            Title = _applicationName;

            //Reset root character:
            CurrentController.ResetAll();

            //Refresh Genre list:           
        }

        

        private void SaveFile(bool SaveExisting)
        {
            if(CurrentController.CurrentEntity==null)
            {
                throw new Exception("Nothing to save!");
            }

            if (SaveExisting == false || CurrentController.CurrentEntity.FileNameAndPath == "")
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    RestoreDirectory = false,
                    Filter = _applicationName + " Files (*.json)|*.json|All Files (*.*)|*.*",
                    FilterIndex = 1
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    
                    CurrentController.CurrentEntity.SaveAs(saveFileDialog.FileName, saveFileDialog.SafeFileName);

                    Title = _applicationName + " - " + CurrentController.CurrentEntity.FileNameAndPath;
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

        private void ImportOldMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                RestoreDirectory = false,
                Filter = _applicationName + " Files(*.xml)|*.xml|All Files (*.*)|*.*",
                FilterIndex = 1
            };
            if (openFileDialog.ShowDialog() == true)
            {
                ResetAll();

                CurrentController.ImportOldXml(openFileDialog.FileName);

                if (CurrentController.CurrentEntity?.RootCharacter == null)
                {
                    //load failed, reset:
                    ResetAll();
                    MessageBox.Show("Unable to load file: invalid format", "Error loading file", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    Title = _applicationName + " - " + CurrentController.CurrentEntity.FileNameAndPath;
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
            if (CurrentController.CurrentEntity == null)
            {
                throw new Exception("Nothing to Export!");
            }

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

        private void AttributeListBox_PreviewMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            AddNodeIfSelected();

            AttributeListBox.Focus();
        }

        private void AddNodeIfSelected()
        {
            if (CurrentController.CurrentEntity?.SelectedNode != null && CurrentController.CurrentEntity.SelectedNode.CanAddSelectedChild())
            {
                CurrentController.CurrentEntity.SelectedNode.AddSelectedChild();
            }
        }

        private void AttributeListBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Return)
            {
                AddNodeIfSelected();
            }
        }

        

        private void ExportToHTMLMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if(CurrentController.CurrentEntity== null)
            {
                throw new Exception("Nothing to export!");
            }

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
            if (AttributeListBox.Items.Count > 0)
            {
                AttributeListBox.ScrollIntoView(AttributeListBox.Items[0]);
            }
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                RestoreDirectory = false,
                Filter = _applicationName + " Files(*.json)|*.json|All Files (*.*)|*.*",
                FilterIndex = 1
            };
            if (openFileDialog.ShowDialog() == true)
            {
                ResetAll();

                CurrentController.Load(openFileDialog.FileName);

                if (CurrentController.CurrentEntity?.RootCharacter == null)
                {
                    //load failed, reset:
                    ResetAll();
                    MessageBox.Show("Unable to load file: invalid format", "Error loading file", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    Title = _applicationName + " - " + CurrentController.CurrentEntity.FileNameAndPath;
                    if (CharacterTreeView.Items.Count > 0)
                    {
                        ((BaseNode)CharacterTreeView.Items[0]).IsSelected = true;
                    }
                }
            }
        }
    }
}