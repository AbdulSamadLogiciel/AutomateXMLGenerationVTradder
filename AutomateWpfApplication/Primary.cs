using AutomateWpfApplication.Classes;
using System.Diagnostics;
using System.Text;
using System.Windows.Automation;
using System.Xml;

namespace AutomateWpfApplication
{ 
    public class Program
    {
        /*
        private static bool isFirstText = true;
        private static string previousUIElement = "";
        private static bool isFirstWindow = true;
        private static bool isFirstButton = true;
        */
        static void Main(string[] args)
        {

            XMLGenerator.GenerateXML("DesktopApplication");
            /*
            // Specify the process name of the application you want to target
            string processName = "DesktopApplication";
            
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
            previousUIElement = GetPreviousSiblingElement(element)?.Current.ControlType.LocalizedControlType?? "";
            Console.WriteLine($"{indent}{element.Current.ControlType.LocalizedControlType} - {element.Current.Name} - AutomationId:{element.Current.AutomationId}");
            GenerateXML(element, depth, xmlBuilder, indent);
        }

        static void GenerateXML(AutomationElement element, int depth,StringBuilder xmlBuilder,string indent)
        {

            // Revive the state for text fields 
            if(previousUIElement != element.Current.ControlType.LocalizedControlType && (element.Current.ControlType.LocalizedControlType == "edit")) 
            {
                isFirstText = true;  // setting the default value for other texts             
            }
            // Revive the state for buttons
            if (previousUIElement != element.Current.ControlType.LocalizedControlType && (element.Current.ControlType.LocalizedControlType == "button"))
            {
                isFirstButton = true;
            }

            // Generate the XML for window tag

            if (element.Current.ControlType.LocalizedControlType == "window" && isFirstWindow)
            {
                xmlBuilder.Append($"<WindowEmbeddedControl Key=\"Login\" Name=\"Login\">\r\n<SubControls>");
                isFirstWindow = false;
              
            }
            else if (element.Current.ControlType.LocalizedControlType == "window" && !isFirstWindow)
            {
                xmlBuilder.Append($"\r\n</SubControls>\r\n</WindowEmbeddedControl>");
                xmlBuilder.Append($"\r\n<WindowEmbeddedControl Key=\"Login\" Name=\"Login\">\r\n<SubControls>");
            }

            // Generate the XML for text field tag
            else if(element.Current.ControlType.LocalizedControlType == "edit" && isFirstText && PrintElementDetails(element, depth) == null)
            {
                xmlBuilder.Append($"\r\n<FieldsEmbeddedControlBase Key=\"Fields\" AutomationID=\"LayoutControl\">\r\n<SubControls>\r\n<TextEditEmbeddedControl Key=\"User\" ControlType=\"Edit\">\r\n<listOfElementHopper>\r\n<ElementHopper AutomationID=\"User\"/>\r\n</listOfElementHopper>\r\n</TextEditEmbeddedControl>");
                xmlBuilder.Append($"\r\n</SubControls>\r\n</FieldsEmbeddedControlBase>");
                isFirstText = false;
               
            }
            else if (element.Current.ControlType.LocalizedControlType == "edit" && isFirstText && PrintElementDetails(element, depth) != null)
            {
                xmlBuilder.Append($"\r\n<FieldsEmbeddedControlBase Key=\"Fields\" AutomationID=\"LayoutControl\">\r\n<SubControls>\r\n<TextEditEmbeddedControl Key=\"User\" ControlType=\"Edit\">\r\n<listOfElementHopper>\r\n<ElementHopper AutomationID=\"User\"/>\r\n</listOfElementHopper>\r\n</TextEditEmbeddedControl>");
                isFirstText = false;
               
            }
            else if(element.Current.ControlType.LocalizedControlType == "edit" && !isFirstText && PrintElementDetails (element, depth) != null)
            {
                xmlBuilder.Append($"\r\n<TextEditEmbeddedControl Key=\"User\" ControlType=\"Edit\">\r\n<listOfElementHopper>\r\n<ElementHopper AutomationID=\"User\"/>\r\n</listOfElementHopper>\r\n</TextEditEmbeddedControl>");
            
            }
            
            else if(element.Current.ControlType.LocalizedControlType == "edit" && !isFirstText && PrintElementDetails(element, depth) == null)
            { 
                xmlBuilder.Append($"\r\n<TextEditEmbeddedControl Key=\"User\" ControlType=\"Edit\">\r\n<listOfElementHopper>\r\n<ElementHopper AutomationID=\"User\"/>\r\n</listOfElementHopper>\r\n</TextEditEmbeddedControl>");
                xmlBuilder.Append($"\r\n</SubControls>\r\n</FieldsEmbeddedControlBase>");
           
            }

            // Generate the XML for Button tag
            
            else if (element.Current.ControlType.LocalizedControlType == "button" && isFirstButton && PrintElementDetails(element, depth) == null)
            {
                xmlBuilder.Append($"\r\n<ButtonEmbeddedControlBase Key=\"MainButtons\" Name=\"Right\">\r\n<SubControls>\r\n<ButtonEmbeddedControl Key=\"Login\" Name=\"Login\">\r\n<ExtraInfo>\r\n<Info Key=\"ActionType\" Value=\"Click\"/>\r\n</ExtraInfo>\r\n</ButtonEmbeddedControl>");
                xmlBuilder.Append($"\r\n</SubControls>\r\n</ButtonEmbeddedControlBase>");
                isFirstButton = false;

            }
            else if (element.Current.ControlType.LocalizedControlType == "button" && isFirstButton && PrintElementDetails(element, depth) != null)
            {
                xmlBuilder.Append($"\r\n<ButtonEmbeddedControlBase Key=\"MainButtons\" Name=\"Right\">\r\n<SubControls>\r\n<ButtonEmbeddedControl Key=\"Login\" Name=\"Login\">\r\n<ExtraInfo>\r\n<Info Key=\"ActionType\" Value=\"Click\"/>\r\n</ExtraInfo>\r\n</ButtonEmbeddedControl>");
                isFirstButton = false;

            }
            else if (element.Current.ControlType.LocalizedControlType == "button" && !isFirstButton && PrintElementDetails(element, depth) != null)
            {
                xmlBuilder.Append($"\r\n<ButtonEmbeddedControl Key=\"Login\" Name=\"Login\">\r\n<ExtraInfo>\r\n<Info Key=\"ActionType\" Value=\"Click\"/>\r\n</ExtraInfo>\r\n</ButtonEmbeddedControl>");

            }

            else if (element.Current.ControlType.LocalizedControlType == "button" && !isFirstButton && PrintElementDetails(element, depth) == null)
            {
                xmlBuilder.Append($"\r\n<ButtonEmbeddedControl Key=\"Login\" Name=\"Login\">\r\n<ExtraInfo>\r\n<Info Key=\"ActionType\" Value=\"Click\"/>\r\n</ExtraInfo>\r\n</ButtonEmbeddedControl>");
                xmlBuilder.Append($"\r\n</SubControls>\r\n</ButtonEmbeddedControlBase>");

            }

        }


        static AutomationElement PrintElementDetails(AutomationElement element, int depth)
        {

            // Get the parent element
            AutomationElement parentElement = TreeWalker.ControlViewWalker.GetParent(element);

            if (parentElement != null)
            {
                // Find all the sibling elements of the parent element
                AutomationElementCollection siblingElements = parentElement.FindAll(TreeScope.Children, Condition.TrueCondition);

                // Find the index of the current element
                int currentIndex = -1;
                for (int i = 0; i < siblingElements.Count; i++)
                {
                    if (siblingElements[i] == element)
                    {
                        currentIndex = i;
                        break;
                    }
                }

                // If the current element is found and it's not the last one, print details of the next sibling element
                if (currentIndex != -1 && currentIndex < siblingElements.Count - 1)
                {
                    AutomationElement nextSibling = siblingElements[currentIndex + 1];
                    if(nextSibling.Current.ControlType.LocalizedControlType == element.Current.ControlType.LocalizedControlType)
                    {
                         return nextSibling;
                    }
                   
                   
                }
              
            }
            return null;
        }



        static AutomationElement GetPreviousSiblingElement(AutomationElement element)
        {
            // Get the parent element
            AutomationElement parentElement = TreeWalker.ControlViewWalker.GetParent(element);

            if (parentElement != null)
            {
                // Find all the sibling elements of the parent element
                AutomationElementCollection siblingElements = parentElement.FindAll(TreeScope.Children, Condition.TrueCondition);

                // Find the index of the current element
                int currentIndex = -1;
                for (int i = 0; i < siblingElements.Count; i++)
                {
                    if (siblingElements[i] == element)
                    {
                        currentIndex = i;
                        break;
                    }
                }

                // If the current element is found and it's not the first one, return the previous sibling element
                if (currentIndex != -1 && currentIndex > 0 )
                {
                    return siblingElements[currentIndex - 1];
                    
                }
            }
            return null;
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
            */
        }
    }

   

}