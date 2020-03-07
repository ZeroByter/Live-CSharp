using System.Collections.Specialized;
using System.IO;
using System.Windows.Forms;

namespace Run_Live_CSharp
{
    public partial class CodeOutput : Form
    {
        public CodeOutput(string output)
        {
            InitializeComponent();

            codeOutputField.Text = output.ToString();
        }

        public CodeOutput(StreamReader stream)
        {
            InitializeComponent();

            string line;
            while((line = stream.ReadLine()) != null){
                codeOutputField.Text += line + '\n';
            }
        }
    }
}
