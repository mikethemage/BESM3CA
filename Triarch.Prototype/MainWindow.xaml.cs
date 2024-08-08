using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Triarch.BusinessLogic.Models.Definitions;
using Triarch.BusinessLogic.Models.Entities;
using Triarch.BusinessLogic.Services;
using Triarch.Dtos.Definitions;
using Triarch.Dtos.Entities;
using Triarch.Prototype.ViewModels;
using Triarch.Prototype.Views;

namespace Triarch.Prototype;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void button_Click(object sender, RoutedEventArgs e)
    {
        if(SystemSelector.Text=="BESM3E")
        {
            MessageBox.Show("Test");

            string fileData = File.ReadAllText("DataFiles" + Path.DirectorySeparatorChar + SystemSelector.Text + ".json");
            RPGSystemDto? systemData = JsonSerializer.Deserialize<RPGSystemDto>(fileData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true});

            if (systemData != null)
            {
                RPGSystemMapper mapper =  new RPGSystemMapper();
                RPGSystem loadedSystem = mapper.Deserialize(systemData);

                if (loadedSystem != null)
                {

                    RPGSystemProvider rPGSystemProvider = new RPGSystemProvider();
                    rPGSystemProvider.AddSystem("BESM 3rd Edition", loadedSystem);

                    string characterData = File.ReadAllText("D:\\Stuff\\RPGs\\BESM\\BESM 3rd Edition\\BESM Characters\\Converted\\test9.json");
                    EntityDto? characterDto = JsonSerializer.Deserialize<EntityDto>(characterData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (characterDto != null)
                    {

                        RPGEntityMapper entityMapper = new RPGEntityMapper(rPGSystemProvider);
                        RPGEntity entity = entityMapper.Deserialize(characterDto);


                        EntityController controller = new EntityController();
                        controller.AddElement(entity.RootElement, loadedSystem.ElementDefinitions.Where(x => x.ElementName == "Companion").First());

                        

                        EntityDto outputDto = entityMapper.Serialize(entity);
                        string outputText = JsonSerializer.Serialize(outputDto, new JsonSerializerOptions { PropertyNameCaseInsensitive = true, DefaultIgnoreCondition=System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull });

                        File.WriteAllText("D:\\Stuff\\RPGs\\BESM\\BESM 3rd Edition\\BESM Characters\\Converted\\test10.json", outputText);

                        EntityViewModel entityViewModel = new EntityViewModel(entity, "D:\\Stuff\\RPGs\\BESM\\BESM 3rd Edition\\BESM Characters\\Converted\\test10.json");

                        EntityEditor editorWindow = new EntityEditor(entityViewModel);
                        editorWindow.ShowDialog();

                    }
                }

            }


        }
    }
}