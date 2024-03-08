using AutomateWpfApplication.Classes;
using AutomateWpfApplication.XMLAutomation.Strategies.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;

namespace AutomateWpfApplication.XMLAutomation.Strategies
{
    public class ButtonStrategy:IXMLGenerationStrategy
    {

        public void StrategicXMLGeneration(AutomationElement element, ref StringBuilder xmlBuilder, ref bool isFirstButton)
        {
          
             if (element.Current.ControlType.LocalizedControlType == "button" && isFirstButton && UIElements.GetNextSiblingElement(element) == null)
            {
                xmlBuilder.Append($"\r\n<ButtonEmbeddedControlBase Key=\"MainButtons\" Name=\"Right\">\r\n<SubControls>\r\n<ButtonEmbeddedControl Key=\"Login\" Name=\"Login\">\r\n<ExtraInfo>\r\n<Info Key=\"ActionType\" Value=\"Click\"/>\r\n</ExtraInfo>\r\n</ButtonEmbeddedControl>");
                xmlBuilder.Append($"\r\n</SubControls>\r\n</ButtonEmbeddedControlBase>");
                isFirstButton = false;

            }
            else if (element.Current.ControlType.LocalizedControlType == "button" && isFirstButton && UIElements.GetNextSiblingElement(element) != null)
            {
                xmlBuilder.Append($"\r\n<ButtonEmbeddedControlBase Key=\"MainButtons\" Name=\"Right\">\r\n<SubControls>\r\n<ButtonEmbeddedControl Key=\"Login\" Name=\"Login\">\r\n<ExtraInfo>\r\n<Info Key=\"ActionType\" Value=\"Click\"/>\r\n</ExtraInfo>\r\n</ButtonEmbeddedControl>");
                isFirstButton = false;

            }
            else if (element.Current.ControlType.LocalizedControlType == "button" && !isFirstButton && UIElements.GetNextSiblingElement(element) != null)
            {
                xmlBuilder.Append($"\r\n<ButtonEmbeddedControl Key=\"Login\" Name=\"Login\">\r\n<ExtraInfo>\r\n<Info Key=\"ActionType\" Value=\"Click\"/>\r\n</ExtraInfo>\r\n</ButtonEmbeddedControl>");

            }

            else if (element.Current.ControlType.LocalizedControlType == "button" && !isFirstButton && UIElements.GetNextSiblingElement(element) == null)
            {
                xmlBuilder.Append($"\r\n<ButtonEmbeddedControl Key=\"Login\" Name=\"Login\">\r\n<ExtraInfo>\r\n<Info Key=\"ActionType\" Value=\"Click\"/>\r\n</ExtraInfo>\r\n</ButtonEmbeddedControl>");
                xmlBuilder.Append($"\r\n</SubControls>\r\n</ButtonEmbeddedControlBase>");

            }
        }
    }
}
