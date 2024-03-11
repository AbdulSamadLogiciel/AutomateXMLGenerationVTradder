using AutomateWpfApplication.Classes;
using AutomateWpfApplication.XMLAutomation.Strategies.Interfaces;
using System.Text;
using System.Windows.Automation;

namespace AutomateWpfApplication.XMLAutomation.Strategies
{
    public class ButtonStrategy:IXMLGenerationStrategy
    {


        public static bool IsButton(AutomationElement element)
        {
            return element.Current.ControlType.LocalizedControlType == UIElementDescriptor.Button;
        }
        public void StrategicXMLGeneration(AutomationElement element,  StringBuilder xmlBuilder, ref bool isFirstButton)
        {
            var defaultValue = (element.Current.Name != "" ? element.Current.Name : element.Current.AutomationId != "" ? element.Current.AutomationId : element.Current.ControlType != null ? element.Current.ControlType.ToString() : "");

            if (IsButton(element)  && isFirstButton && !UIElements.ISNextSiblingElementExists(element))
            {
                xmlBuilder.Append($"\r\n<ButtonEmbeddedControlBase Key=\"{defaultValue}\" Name=\"{defaultValue}\">\r\n<SubControls>\r\n<ButtonEmbeddedControl Key=\"{defaultValue}\" Name=\"{defaultValue}\">\r\n<ExtraInfo>\r\n<Info Key=\"ActionType\" Value=\"Click\"/>\r\n</ExtraInfo>\r\n</ButtonEmbeddedControl>");
                xmlBuilder.Append($"\r\n</SubControls>\r\n</ButtonEmbeddedControlBase>");
                isFirstButton = false;

            }
            else if (IsButton(element) && isFirstButton && UIElements.ISNextSiblingElementExists(element))
            {
                xmlBuilder.Append($"\r\n<ButtonEmbeddedControlBase Key=\"{defaultValue}\" Name=\"{defaultValue}\">\r\n<SubControls>\r\n<ButtonEmbeddedControl Key=\"{defaultValue}\" Name=\"{defaultValue}\">\r\n<ExtraInfo>\r\n<Info Key=\"ActionType\" Value=\"Click\"/>\r\n</ExtraInfo>\r\n</ButtonEmbeddedControl>");
                isFirstButton = false;

            }
            else if (IsButton(element) && !isFirstButton && UIElements.ISNextSiblingElementExists(element))
            {
                xmlBuilder.Append($"\r\n<ButtonEmbeddedControl Key=\"{defaultValue}\" Name=\"{defaultValue}\">\r\n<ExtraInfo>\r\n<Info Key=\"ActionType\" Value=\"Click\"/>\r\n</ExtraInfo>\r\n</ButtonEmbeddedControl>");

            }

            else if (IsButton(element) && !isFirstButton && !UIElements.ISNextSiblingElementExists(element))
            {
                xmlBuilder.Append($"\r\n<ButtonEmbeddedControl Key=\"{defaultValue}\" Name=\"{defaultValue}\">\r\n<ExtraInfo>\r\n<Info Key=\"ActionType\" Value=\"Click\"/>\r\n</ExtraInfo>\r\n</ButtonEmbeddedControl>");
                xmlBuilder.Append($"\r\n</SubControls>\r\n</ButtonEmbeddedControlBase>");

            }
        }
    }
}
