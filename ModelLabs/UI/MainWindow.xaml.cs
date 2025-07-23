using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private GDA Gda;                                                                                        //instanca GDA service client-a
        private Dictionary<ModelCode, string> propertiesDesc = new Dictionary<ModelCode, string>();
        private Dictionary<ModelCode, string> propertiesDescRelated = new Dictionary<ModelCode, string>();
        private Dictionary<ModelCode, string> propertiesDescExtended = new Dictionary<ModelCode, string>();

        private ModelResourcesDesc modelResourcesDesc = new ModelResourcesDesc();                               // Helper klasa za mapiranje DMSType enuma na ModelCode-ove, za opis modela


        private static List<long> gids = new List<long>();                                                      // staticna lista za store-ovanje svih GID-ova retrivovanih iz modela
        private long selectedDMSType = -1;                                                                      // za pracenje selektovanih stvari...
        private long selectedGIDRelated = -1;
        private long selectedRelatedProperty = -1;
        private long selectedGID = -1;

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                Gda = new GDA();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "GDA has exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            int count = 0;
            foreach (DMSType dmsType in Enum.GetValues(typeof(DMSType)))
            {
                //prolazimo kroz sve DMSType vrednosi osim MASK_TYPE on je bitmask place holder
                if (dmsType == DMSType.MASK_TYPE)
                    continue;
                try
                {
                    //convertujemo svaki DMSType ka odgovarajucem ModelCode-u
                    ModelCode dmsTypesModelCode = modelResourcesDesc.GetModelCodeFromType(dmsType);
                    CommonTrace.WriteTrace(true, dmsTypesModelCode.ToString());

                    //Dodamo konvertovani DMSType koji je sad ModelCode kao opciju u ComboBox-u
                    DMSTypes.Items.Add(dmsTypesModelCode);

                    //uzmemo sve GID-ove od trenutnog ModelCode-a i sacuvamo njihove GID-ove u onoj static listi
                    Gda.GetExtentValues(dmsTypesModelCode, new List<ModelCode> { ModelCode.IDOBJ_GID }, null).ForEach(g => gids.Add(g));
                }
                catch (Exception e)
                {
                    CommonTrace.WriteTrace(true, e.Message + e.StackTrace);
                    throw new Exception();
                }
                count++;
            }

            // ovde populatujemo ComboBox opcije
            if (gids.Count > 0)
            {
                gids.ForEach(g =>
                {
                    GIDs.Items.Add(string.Format("{0:X16}", g));
                    GIDsRelated.Items.Add(string.Format("{0:X16}", g));
                });
            }
            else
                CommonTrace.WriteTrace(true, $"NO GIDS were loaded into gids list, {count}");

            selectedGID = -1;
            selectedDMSType = -1;


        }

        private void PopulateProperties(StackPanel propertiesPanel, Dictionary<ModelCode, string> propContainer, DMSType DMStype)
        {
            //ako ga nema vracamo se
            if ((long)DMStype == -1)
                return;


            //ucistimo prethodni
            propertiesPanel.Children.Clear();

            //uzemmo sve propertije za izbrani DMSType iz modelResourceDesc

            List<ModelCode> properties = modelResourcesDesc.GetAllPropertyIds(DMStype);

            propContainer.Clear();

            //dodamo u dictionary i dodamo checkbox za njega na UI sa hex-a repreznaticjom
            foreach (ModelCode property in properties)
            {
                propContainer.Add(property, property.ToString());

                var checkBox = new CheckBox()
                {
                    Content = string.Format("{0:X}", property.ToString()),
                };

                propertiesPanel.Children.Add(checkBox);
            }
        }

        //akcija za combobox gde se bira GID

        private void GIDs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            long.TryParse(GIDs.SelectedItem.ToString(), out selectedGID);

            selectedGID = gids[GIDs.SelectedIndex];

            var DMSType = ModelCodeHelper.ExtractTypeFromGlobalId(selectedGID);

            //pozovemo funkciju od gore da bi mogli da prikazemo sve property-ije od selectovanog GID-a
            PopulateProperties(Properties, propertiesDesc, (DMSType)DMSType);
        }


        //na prvoj stranici akcija kada se pritisne dugme za GetValues
        private void Button_Click_GetValues(object sender, RoutedEventArgs e)
        {
            if (selectedGID == -1)
                return;

            List<ModelCode> selectedProperties = new List<ModelCode>();

            foreach (var child in Properties.Children)
            {
                if (!(child is CheckBox checkBox))
                    continue;
                if (!checkBox.IsChecked.Value)
                    continue;

                foreach (KeyValuePair<ModelCode, string> keyValuePair in propertiesDesc)
                {
                    if (keyValuePair.Value.Equals(checkBox.Content))
                        selectedProperties.Add(keyValuePair.Key);
                }
            }

            ResourceDescription rd = null;
            try
            {
                //Zovemo Gda.GetValues(selectedGID, selectedProperties) da dobijemo vrednosti od servera

                rd = Gda.GetValues(selectedGID, selectedProperties);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "GetValues", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (rd == null)
                return;

            var sb = new StringBuilder();
            sb.Append("Returned entity" + Environment.NewLine + Environment.NewLine);
            sb.Append($"Entity with gid: 0x{rd.Id:X16}" + Environment.NewLine);


            // Parsiramo ResourceDescription returned (rd)
            // Za svaki property na osnovy type-a koristimo helper funk (StringAppend) da formatiramo output
            foreach (Property property in rd.Properties)
            {
                switch (property.Type)
                {
                    case PropertyType.Int64:
                        StringHelper.AppendLong(sb, property);
                        break;
                    case PropertyType.Float:
                        StringHelper.AppendFloat(sb, property);
                        break;
                    case PropertyType.String:
                        StringHelper.AppendString(sb, property);
                        break;
                    case PropertyType.Reference:
                        StringHelper.AppendReference(sb, property);
                        break;
                    case PropertyType.ReferenceVector:
                        StringHelper.AppendReferenceVector(sb, property);
                        break;
                    case PropertyType.DateTime:
                        StringHelper.AppendDateTime(sb, property);
                        break;

                    default:
                        sb.Append($"\t{property.Id}: {property.PropertyValue.LongValue}{Environment.NewLine}");
                        break;
                }
            }

            Values.Clear();
            Values.AppendText(sb.ToString());
        }

        /*
         *  Data Types & Helpers Involved
                ModelCode: Enum representing a property on an entity (e.g. BREAKER_MRID).
                DMSType: Enum representing entity types (e.g. BREAKER, TRANSFORMER).
                GDA (Generic Data Access): Used to fetch values for properties by GID.
                ResourceDescription: Contains properties for a specific GID.
                Property: Represents a single property with a value and type.

                This UI component allows the user to:
                Select an entity by GID.
                View all properties applicable to its type.
                Select which properties they want to query.
                Click a button to fetch and display their values.

         
         */
        private void DMSTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedDMSType = (long)DMSTypes.SelectedItem;
            PopulateProperties(PropertiesExtent, propertiesDescExtended, ModelCodeHelper.GetTypeFromModelCode((ModelCode)selectedDMSType));
        }

        private void Button_Click_GetExtentValues(object sender, RoutedEventArgs e)
        {
            if (selectedDMSType == -1)
                return;

            List<ModelCode> selectedProperties = new List<ModelCode>();

            foreach (var child in PropertiesExtent.Children)
            {
                if (!(child is CheckBox checkBox))
                    continue;
                if (!checkBox.IsChecked.Value)
                    continue;

                foreach (KeyValuePair<ModelCode, string> keyValuePair in propertiesDescExtended)
                {
                    if (keyValuePair.Value.Equals(checkBox.Content))
                        selectedProperties.Add(keyValuePair.Key);
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("Returned entities" + Environment.NewLine + Environment.NewLine);

            try
            {
                Gda.GetExtentValues((ModelCode)selectedDMSType, selectedProperties, sb);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "GetValues", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            ValuesExtent.Clear();
            ValuesExtent.AppendText(sb.ToString());
        }

        private void RelatedGIDs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RelationalProps.Items.Clear();
            RelationalTypes.Items.Clear();
            PropertiesRelated.Children.Clear();

            selectedGIDRelated = gids[GIDsRelated.SelectedIndex];
            short type = ModelCodeHelper.ExtractTypeFromGlobalId(selectedGIDRelated);
            List<ModelCode> properties = modelResourcesDesc.GetAllPropertyIds((DMSType)type);

            foreach (ModelCode property in properties)
            {
                var prop = new Property(property);
                if (prop.Type != PropertyType.Reference && prop.Type != PropertyType.ReferenceVector)
                    continue;

                RelationalProps.Items.Add(property);
            }
        }

        private void RelationalProps_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RelationalProps.SelectedItem == null)
                return;

            RelationalTypes.Items.Clear();

            selectedRelatedProperty = (long)RelationalProps.SelectedItem;

            var targetEntity = GetTypeFromReferenceModelCode(selectedRelatedProperty);

            RelationalTypes.Items.Add(targetEntity);

            PopulateProperties(PropertiesRelated, propertiesDescRelated, targetEntity);
        }


        private DMSType GetTypeFromReferenceModelCode(long modelCode)
        {
            var rd = Gda.GetValues(selectedGIDRelated, new List<ModelCode>() { (ModelCode)modelCode });
            var prop = rd.GetProperty((ModelCode)modelCode);

            long gid = -1;
            if (prop.IsCompatibleWith(PropertyType.ReferenceVector))
            {
                if (prop.AsReferences().Count > 0)
                    gid = prop.AsReferences()[0];
            }
            else
            {
                gid = prop.AsReference();
            }

            if (gid == -1)
                return DMSType.MASK_TYPE;

            var targetEntity = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(gid);
            return targetEntity;
        }

        private void Button_Click_GetRelatedValues(object sender, RoutedEventArgs e)
        {
            if (selectedRelatedProperty == -1)
                return;

            List<ModelCode> selectedProperties = new List<ModelCode>();

            foreach (var child in PropertiesRelated.Children)
            {
                if (child is CheckBox checkBox && checkBox.IsChecked.Value)
                {
                    foreach (KeyValuePair<ModelCode, string> keyValuePair in propertiesDescRelated)
                    {
                        if (keyValuePair.Value.Equals(checkBox.Content))
                        {
                            selectedProperties.Add(keyValuePair.Key);
                        }
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("Returned entities" + Environment.NewLine + Environment.NewLine);

            // Property is of reference type, but value not set so skip it.
            var type = GetTypeFromReferenceModelCode(selectedRelatedProperty);
            if (type == DMSType.MASK_TYPE)
                return;

            var association = new Association((ModelCode)selectedRelatedProperty, modelResourcesDesc.GetModelCodeFromType(type));
            List<long> gids = Gda.GetRelatedValues(selectedGIDRelated, selectedProperties, association, sb);

            ValuesRelated.Clear();
            ValuesRelated.AppendText(sb.ToString());
        }
    }
}
