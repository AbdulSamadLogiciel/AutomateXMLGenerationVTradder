using System.Text;
using System.Windows.Automation;

namespace AutomateWpfApplication.XMLAutomation.Strategies.Interfaces
{
    public interface IXMLGenerationStrategy
    {

        public void StrategicXMLGeneration( AutomationElement element,  StringBuilder xmlBuilder , ref bool isFirst);

    }
}
