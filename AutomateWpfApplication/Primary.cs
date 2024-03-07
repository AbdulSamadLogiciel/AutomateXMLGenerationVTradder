using System.Diagnostics;
using System.Text;
using System.Windows.Automation;

namespace AutomateWpfApplication
{ 
    class Program
    {
        private static bool isFirstText = true;
        private static string previousUIElement = "";
        private static bool isFirstWindow = true;
        static void Main(string[] args)
        {
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
            Console.WriteLine(xmlString);
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
            previousUIElement = element.Current.ControlType.LocalizedControlType;
            Console.WriteLine($"{element.Current.ControlType.LocalizedControlType} - {element.Current.Name} - AutomationId:{element.Current.AutomationId}");
            GenerateXML(element, depth, xmlBuilder, indent);
        }

        static void GenerateXML(AutomationElement element, int depth,StringBuilder xmlBuilder,string indent)
        {

            if(previousUIElement != element.Current.ControlType.LocalizedControlType) 
            {
                if (element.Current.ControlType.LocalizedControlType == "edit")
                {
                    isFirstText = true;
                }
               
            }
        
            if(element.Current.ControlType.LocalizedControlType == "window" && isFirstWindow)
            {
                
                xmlBuilder.Append($"<WindowEmbeddedControl Key=\"Login\" Name=\"Login\">\r\n<SubControls>");
                isFirstWindow = false;
              
                return;
            }else if (element.Current.ControlType.LocalizedControlType == "window" && !isFirstWindow)
            {
                xmlBuilder.Append($"\r\n</SubControls>\r\n</WindowEmbeddedControl>");
                xmlBuilder.Append($"\r\n<WindowEmbeddedControl Key=\"Login\" Name=\"Login\">\r\n<SubControls>");
            }

            else if(element.Current.ControlType.LocalizedControlType == "edit" && isFirstText && PrintElementDetails(element, depth) == null)
            {
                xmlBuilder.Append($"\r\n<FieldsEmbeddedControlBase Key=\"Fields\" AutomationID=\"LayoutControl\">\r\n<SubControls>\r\n<TextEditEmbeddedControl Key=\"User\" ControlType=\"Edit\">\r\n<listOfElementHopper>\r\n<ElementHopper AutomationID=\"User\"/>\r\n</listOfElementHopper>\r\n</TextEditEmbeddedControl>");
                xmlBuilder.Append($"\r\n</SubControls>\r\n</FieldsEmbeddedControlBase>");
                isFirstText = false;
                return;
            }
            else if (element.Current.ControlType.LocalizedControlType == "edit" && isFirstText && PrintElementDetails(element, depth) != null)
            {
                xmlBuilder.Append($"\r\n<FieldsEmbeddedControlBase Key=\"Fields\" AutomationID=\"LayoutControl\">\r\n<SubControls>\r\n<TextEditEmbeddedControl Key=\"User\" ControlType=\"Edit\">\r\n<listOfElementHopper>\r\n<ElementHopper AutomationID=\"User\"/>\r\n</listOfElementHopper>\r\n</TextEditEmbeddedControl>");
                isFirstText = false;
                return;
            }
            else if(element.Current.ControlType.LocalizedControlType == "edit" && !isFirstText && PrintElementDetails (element, depth) != null)
            {
                xmlBuilder.Append($"\r\n<TextEditEmbeddedControl Key=\"User\" ControlType=\"Edit\">\r\n<listOfElementHopper>\r\n<ElementHopper AutomationID=\"User\"/>\r\n</listOfElementHopper>\r\n</TextEditEmbeddedControl>");
                return;
            
            }
            
            else if(element.Current.ControlType.LocalizedControlType == "edit" && !isFirstText && PrintElementDetails(element, depth) == null)
            {
                
                xmlBuilder.Append($"\r\n</SubControls>\r\n</FieldsEmbeddedControlBase>");
                return;
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
                    if(nextSibling.Current.ControlType.LocalizedControlType == "edit")
                    {
                         return nextSibling;
                    }
                   
                   
                }
              
            }
            return null;
        }
    }



}