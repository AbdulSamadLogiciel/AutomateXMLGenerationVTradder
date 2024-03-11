using AutomateWpfApplication.XMLAutomation.Strategies.Interfaces;
using System.Text;
using System.Windows.Automation;

namespace AutomateWpfApplication.XMLAutomation.Strategies
{
    public class WindowStrategy : IXMLGenerationStrategy
    {

        public static bool IsWindow(AutomationElement element)
        {
            return element.Current.ControlType.LocalizedControlType == UIElementDescriptor.Window;
        }
        public void StrategicXMLGeneration(AutomationElement element,  StringBuilder xmlBuilder, ref bool isFirstWindow)
        {
            var defaultValue = (element.Current.Name != "" ? element.Current.Name : element.Current.AutomationId != "" ? element.Current.AutomationId : element.Current.ControlType != null ? element.Current.ControlType.ToString() : "");
            if (IsWindow(element) && isFirstWindow)
            {
                xmlBuilder.Append($"<WindowEmbeddedControl Key=\"{defaultValue}\" Name=\"{defaultValue}\">\r\n<SubControls>");
                isFirstWindow = false;

            }
            else if (IsWindow(element) && !isFirstWindow)
            {
                xmlBuilder.Append($"\r\n</SubControls>\r\n</WindowEmbeddedControl>");
                xmlBuilder.Append($"\r\n<WindowEmbeddedControl Key=\"{defaultValue}\" Name=\"{defaultValue}\">\r\n<SubControls>");
            }
        }
    }
}
