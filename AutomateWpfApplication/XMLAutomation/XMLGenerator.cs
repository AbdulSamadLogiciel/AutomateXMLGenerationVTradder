using AutomateWpfApplication.XMLAutomation.Strategies;
using AutomateWpfApplication.XMLAutomation.Strategies.Interfaces;
using System.Diagnostics;
using System.Text;
using System.Windows.Automation;
using System.Xml;

namespace AutomateWpfApplication.Classes
{
    public class XMLGenerator
    {

        private static bool isFirstText = true;
        private static bool isFirstWindow = true;
        private static bool isFirstButton = true;
     
        

        public static void GenerateXML(string applicationName)
        {

            // Specify the process name of the application you want to target
            string processName = applicationName;

            // Find the main window of the specified application
            AutomationElement mainWindow = FindMainWindow(processName);


            StringBuilder xmlBuilder = new StringBuilder();
            xmlBuilder.AppendLine("<EmbeddedControlBase>\n<SubControls>");
            // Parse and print UI elements recursively
            if (mainWindow != null)
            {
                Console.WriteLine("UI Elements:");
                ParseUIElements(mainWindow, 0, xmlBuilder);
            }
            else
            {
                Console.WriteLine($"Failed to find the main window of the '{processName}' application.");
            }
            if (!isFirstWindow)
            {
                xmlBuilder.AppendLine("\r\n</SubControls>\r\n</WindowEmbeddedControl>");

            }


            xmlBuilder.AppendLine("</SubControls>\n</EmbeddedControlBase>");


            string xmlString = xmlBuilder.ToString();
            string beautifiedXml = BeautifyXml(xmlString.ToString());

            // Print the beautified XML
            Console.WriteLine(beautifiedXml);
            // Console.WriteLine(xmlString);

        }

        static AutomationElement FindMainWindow(string processName)
        {
            // Find the main window of the specified application by its process name
            Process[] processes = Process.GetProcessesByName(processName);
            if (processes.Length > 0)
            {
                IntPtr mainWindowHandle = processes[0].MainWindowHandle;
                AutomationElement mainWindow = AutomationElement.FromHandle(mainWindowHandle);
                return mainWindow;
            }
            else
            {
                return null;
            }
        }

        static void ParseUIElements(AutomationElement element, int depth, StringBuilder xmlBuilder)
        {
            // Print the current element's details
            PrintElementDetails(element, depth, xmlBuilder);

            // Get the children of the current element
            AutomationElementCollection children = element.FindAll(TreeScope.Children, Condition.TrueCondition);

            // Recursively parse each child element
            foreach (AutomationElement child in children)
            {
                ParseUIElements(child, depth + 1, xmlBuilder);
            }
        }

        static void PrintElementDetails(AutomationElement element, int depth, StringBuilder xmlBuilder)
        {
            string indent = new string(' ', depth * 4);
            Console.WriteLine($"{indent}{element.Current.ControlType.LocalizedControlType} - {element.Current.Name} - AutomationId:{element.Current.AutomationId}");
            GenerateXML(element,  xmlBuilder);
        }


        static void GenerateXML(AutomationElement element,  StringBuilder xmlBuilder)
        {

            var previousUIElement = UIElements.GetPreviousSiblingElement(element)?.Current.ControlType.LocalizedControlType ?? "";
            if (previousUIElement != element.Current.ControlType.LocalizedControlType && (element.Current.ControlType.LocalizedControlType == "button"))
            {
                isFirstButton = true;
            }
            
            // Revive the state for text fields 
            if (previousUIElement != element.Current.ControlType.LocalizedControlType && (element.Current.ControlType.LocalizedControlType == "edit"))
            {
                isFirstText = true;  // setting the default value for other texts             
            }

            if (element.Current.ControlType.LocalizedControlType == "window")
            {
                IXMLGenerationStrategy strategy = new WindowStrategy();
                strategy.StrategicXMLGeneration(element, ref xmlBuilder, ref isFirstWindow);
            }
            if(element.Current.ControlType.LocalizedControlType == "button")
            {
                IXMLGenerationStrategy strategy = new ButtonStrategy();
                strategy.StrategicXMLGeneration(element, ref xmlBuilder, ref isFirstButton);
            }
            else if (element.Current.ControlType.LocalizedControlType == "edit")
            {
                IXMLGenerationStrategy strategy = new TextFieldStrategy();
                strategy.StrategicXMLGeneration(element, ref xmlBuilder, ref isFirstText);
            }
        }

        static string BeautifyXml(string xml)
        {
            try
            {
                // Load the XML string into an XmlDocument
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                // Create a string writer to store the formatted XML
                StringBuilder stringBuilder = new StringBuilder();
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.NewLineChars = "\n"; // Optional: You can change the newline characters to your preference

                // Write the XML to the string writer with indentation
                using (XmlWriter writer = XmlWriter.Create(stringBuilder, settings))
                {
                    doc.Save(writer);
                }

                // Return the formatted XML string
                return stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                // Handle any exceptions (e.g., invalid XML format)
                Console.WriteLine("Error beautifying XML: " + ex.Message);
                return xml; // Return the original XML if an error occurs
            }
        }

    }


}
