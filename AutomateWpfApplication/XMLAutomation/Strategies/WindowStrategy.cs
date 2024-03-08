using AutomateWpfApplication.XMLAutomation.Strategies.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace AutomateWpfApplication.XMLAutomation.Strategies
{
    public class WindowStrategy : IXMLGenerationStrategy
    {
        public void StrategicXMLGeneration(AutomationElement element, ref StringBuilder xmlBuilder, ref bool isFirstWindow)
        {
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
        }
    }
}
