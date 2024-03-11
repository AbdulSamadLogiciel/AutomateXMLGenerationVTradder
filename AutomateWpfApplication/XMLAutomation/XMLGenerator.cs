using AutomateWpfApplication.XMLAutomation.Strategies;
using AutomateWpfApplication.XMLAutomation.Strategies.Interfaces;
using System.Diagnostics;
using System.IO;
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
     

        public static void SaveXML(StringBuilder xmlString)
        {
            string beautifiedXml = BeautifyXml(xmlString.ToString());
            string filePath = "C:\\Users\\abdul.samad\\source\\repos\\AutomateWpfApplication\\AutomateWpfApplication\\UIAutomation.xml";
            File.WriteAllText(filePath, beautifiedXml);
            Console.WriteLine("XML data has been written to the file successfully.");
        }
        public static void GenerateXML(string applicationName)
        {
            try
            {
                
                string processName = applicationName;

                AutomationElement mainWindow = FindMainWindow(processName);
                StringBuilder xmlBuilder = new StringBuilder();
                xmlBuilder.AppendLine("<EmbeddedControlBase>\n<SubControls>");
            
                    Console.WriteLine("UI Elements:");
                    ParseUIElements(mainWindow, 0, xmlBuilder);

                    if (!isFirstWindow)
                    {
                        xmlBuilder.AppendLine("\r\n</SubControls>\r\n</WindowEmbeddedControl>");

                    }
                    xmlBuilder.AppendLine("</SubControls>\n</EmbeddedControlBase>");
                    SaveXML(xmlBuilder);
                    
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"Something went wrong: ${ex.Message}");
            }


               

        }

        static AutomationElement FindMainWindow(string processName)
        {
            
            Process[] processes = Process.GetProcessesByName(processName);
            if (processes.Length > 0)
            {
                IntPtr mainWindowHandle = processes[0].MainWindowHandle;
                AutomationElement mainWindow = AutomationElement.FromHandle(mainWindowHandle);
                return mainWindow;
            }
            throw new Exception("Target Application Not found");
        }

        static void ParseUIElements(AutomationElement element, int depth, StringBuilder xmlBuilder)
        {
            
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

           

            if (!UIElements.IsPreviousAndCurrentElementSame(element) && ButtonStrategy.IsButton(element))
            {
                isFirstButton = true;
            }
            
        
            if (!UIElements.IsPreviousAndCurrentElementSame(element) && TextFieldStrategy.IsText(element))
            {
                isFirstText = true;              
            }

            if (WindowStrategy.IsWindow(element))
            {
                IXMLGenerationStrategy strategy = new WindowStrategy();
                strategy.StrategicXMLGeneration(element,  xmlBuilder, ref isFirstWindow);
            }
            if(ButtonStrategy.IsButton(element))
            {
                IXMLGenerationStrategy strategy = new ButtonStrategy();
                strategy.StrategicXMLGeneration(element,  xmlBuilder, ref isFirstButton);
            }
            else if (TextFieldStrategy.IsText(element))
            {
                IXMLGenerationStrategy strategy = new TextFieldStrategy();
                strategy.StrategicXMLGeneration(element,  xmlBuilder, ref isFirstText);
            }
        }

        static string BeautifyXml(string xml)
        {
            try
            {
                
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                
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
              
                Console.WriteLine("Error beautifying XML: " + ex.Message);
                return xml; 
            }
        }

    }


}
