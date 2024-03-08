using AutomateWpfApplication.Classes;
using AutomateWpfApplication.XMLAutomation.Strategies.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace AutomateWpfApplication.XMLAutomation.Strategies
{
    public class TextFieldStrategy : IXMLGenerationStrategy
    {
        public void StrategicXMLGeneration(AutomationElement element,ref StringBuilder xmlBuilder, ref bool isFirstText)
        {
          
             if (element.Current.ControlType.LocalizedControlType == "edit" && isFirstText && UIElements.GetNextSiblingElement(element) == null)
            {
                xmlBuilder.Append($"\r\n<FieldsEmbeddedControlBase Key=\"Fields\" AutomationID=\"LayoutControl\">\r\n<SubControls>\r\n<TextEditEmbeddedControl Key=\"User\" ControlType=\"Edit\">\r\n<listOfElementHopper>\r\n<ElementHopper AutomationID=\"User\"/>\r\n</listOfElementHopper>\r\n</TextEditEmbeddedControl>");
                xmlBuilder.Append($"\r\n</SubControls>\r\n</FieldsEmbeddedControlBase>");
                isFirstText = false;

            }
            else if (element.Current.ControlType.LocalizedControlType == "edit" && isFirstText && UIElements.GetNextSiblingElement(element) != null)
            {
                xmlBuilder.Append($"\r\n<FieldsEmbeddedControlBase Key=\"Fields\" AutomationID=\"LayoutControl\">\r\n<SubControls>\r\n<TextEditEmbeddedControl Key=\"User\" ControlType=\"Edit\">\r\n<listOfElementHopper>\r\n<ElementHopper AutomationID=\"User\"/>\r\n</listOfElementHopper>\r\n</TextEditEmbeddedControl>");
                isFirstText = false;

            }
            else if (element.Current.ControlType.LocalizedControlType == "edit" && !isFirstText && UIElements.GetNextSiblingElement(element) != null)
            {
                xmlBuilder.Append($"\r\n<TextEditEmbeddedControl Key=\"User\" ControlType=\"Edit\">\r\n<listOfElementHopper>\r\n<ElementHopper AutomationID=\"User\"/>\r\n</listOfElementHopper>\r\n</TextEditEmbeddedControl>");

            }

            else if (element.Current.ControlType.LocalizedControlType == "edit" && !isFirstText && UIElements.GetNextSiblingElement(element) == null)
            {
                xmlBuilder.Append($"\r\n<TextEditEmbeddedControl Key=\"User\" ControlType=\"Edit\">\r\n<listOfElementHopper>\r\n<ElementHopper AutomationID=\"User\"/>\r\n</listOfElementHopper>\r\n</TextEditEmbeddedControl>");
                xmlBuilder.Append($"\r\n</SubControls>\r\n</FieldsEmbeddedControlBase>");

            }
        }
    }



}
