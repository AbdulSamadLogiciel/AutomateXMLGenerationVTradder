using AutomateWpfApplication.Classes;
using AutomateWpfApplication.XMLAutomation.Strategies.Interfaces;
using System.Text;
using System.Windows.Automation;

namespace AutomateWpfApplication.XMLAutomation.Strategies
{
    public class TextFieldStrategy : IXMLGenerationStrategy
    {

        public static bool IsText(AutomationElement element)
        {
            return element.Current.ControlType.LocalizedControlType == UIElementDescriptor.Text;
        }
        public void StrategicXMLGeneration(AutomationElement element, StringBuilder xmlBuilder, ref bool isFirstText)
        {
          
             if (IsText(element) && isFirstText && !UIElements.ISNextSiblingElementExists(element))
            {
                xmlBuilder.Append($"\r\n<FieldsEmbeddedControlBase Key=\"Fields\" AutomationID=\"LayoutControl\">\r\n<SubControls>\r\n<TextEditEmbeddedControl Key=\"User\" ControlType=\"Edit\">\r\n<listOfElementHopper>\r\n<ElementHopper AutomationID=\"User\"/>\r\n</listOfElementHopper>\r\n</TextEditEmbeddedControl>");
                xmlBuilder.Append($"\r\n</SubControls>\r\n</FieldsEmbeddedControlBase>");
                isFirstText = false;

            }
            else if (IsText(element) && isFirstText && UIElements.ISNextSiblingElementExists(element))
            {
                xmlBuilder.Append($"\r\n<FieldsEmbeddedControlBase Key=\"Fields\" AutomationID=\"LayoutControl\">\r\n<SubControls>\r\n<TextEditEmbeddedControl Key=\"User\" ControlType=\"Edit\">\r\n<listOfElementHopper>\r\n<ElementHopper AutomationID=\"User\"/>\r\n</listOfElementHopper>\r\n</TextEditEmbeddedControl>");
                isFirstText = false;

            }
            else if (IsText(element) && !isFirstText && UIElements.ISNextSiblingElementExists(element))
            {
                xmlBuilder.Append($"\r\n<TextEditEmbeddedControl Key=\"User\" ControlType=\"Edit\">\r\n<listOfElementHopper>\r\n<ElementHopper AutomationID=\"User\"/>\r\n</listOfElementHopper>\r\n</TextEditEmbeddedControl>");

            }

            else if (IsText(element) && !isFirstText && !UIElements.ISNextSiblingElementExists(element))
            {
                xmlBuilder.Append($"\r\n<TextEditEmbeddedControl Key=\"User\" ControlType=\"Edit\">\r\n<listOfElementHopper>\r\n<ElementHopper AutomationID=\"User\"/>\r\n</listOfElementHopper>\r\n</TextEditEmbeddedControl>");
                xmlBuilder.Append($"\r\n</SubControls>\r\n</FieldsEmbeddedControlBase>");

            }
        }
    }



}
