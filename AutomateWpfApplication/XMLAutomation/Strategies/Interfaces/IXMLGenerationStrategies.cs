using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace AutomateWpfApplication.XMLAutomation.Strategies.Interfaces
{
    public interface IXMLGenerationStrategy
    {

        public void StrategicXMLGeneration( AutomationElement element,  StringBuilder xmlBuilder , ref bool isFirst);

    }
}
