using Microsoft.CSharp;
using ScintillaNET;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Run_Live_CSharp
{
    public partial class CodeForm : Form
    {
        public static Color IntToColor(int rgb)
        {
            return Color.FromArgb(255, (byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb);
        }

        private string defaultString = @"using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public class Program{
    public static void Main(){
        
    }
}";

        private Scintilla TextArea;

        public CodeForm()
        {
            InitializeComponent();

            TextArea = new Scintilla();
            inputDock.Controls.Add(TextArea);
            TextArea.Dock = DockStyle.Fill;
            TextArea.WrapMode = WrapMode.Char;
            TextArea.IndentationGuides = IndentView.LookBoth;
            TextArea.CharAdded += OnCharAdded;

            InitSyntaxColoring();
            InitNumberMargin();
            InitCodeFolding();
            InitDragDropFile();

            InitHotkeys();

            var args = Environment.GetCommandLineArgs();

            if(args.Length <= 1)
            {
                if(Properties.Settings.Default.CurrentCode == "")
                {
                    TextArea.Text = defaultString;
                }
                else
                {
                    TextArea.Text = Properties.Settings.Default.CurrentCode;
                }
            }
            else
            {
                //we assume args is path to a .cs file

                string filePath = "";
                for (int i = 0; i < args.Length; i++)
                {
                    string str = args[i];

                    if (i == 0) continue;

                    filePath += str + ' ';
                }

                TextArea.Text = File.ReadAllText(filePath);
            }
        }

        private void InitHotkeys()
        {
            // remove conflicting hotkeys from scintilla
            TextArea.ClearCmdKey(Keys.Control | Keys.F);
            TextArea.ClearCmdKey(Keys.Control | Keys.R);
            TextArea.ClearCmdKey(Keys.Control | Keys.H);
            TextArea.ClearCmdKey(Keys.Control | Keys.L);
            TextArea.ClearCmdKey(Keys.Control | Keys.U);
            TextArea.ClearCmdKey(Keys.Control | Keys.S);
        }

        private void OnCharAdded(object sender, CharAddedEventArgs e)
        {
            if(e.Char == 10) //created new line
            {
                string previousLine = TextArea.Lines[TextArea.CurrentLine - 1].Text;
                float tabCount = 0;
                foreach (var character in previousLine)
                {
                    if (character == ' ')
                    {
                        tabCount += 0.25f; //a tab is four spaces
                    }
                    else if (character == '\t')
                    {
                        tabCount++;
                    }
                    else
                    {
                        break;
                    }
                }
                TextArea.AddText(RepeatCharacter('\t', (int)tabCount));
            }

            // Find the word start
            var currentPos = TextArea.CurrentPosition;
            var wordStartPos = TextArea.WordStartPosition(currentPos, true);

            // Display the autocompletion list
            var lenEntered = currentPos - wordStartPos;
            if (lenEntered > 0)
            {
                if (!TextArea.AutoCActive)
                    TextArea.AutoCShow(lenEntered, "abstract as base break case catch checked continue default delegate do else event explicit extern false finally fixed for foreach goto if implicit in interface internal is lock namespace new null object operator out override params private protected public readonly ref return sealed sizeof stackalloc switch this throw true try typeof unchecked unsafe using virtual while");
            }
        }

        private string RepeatCharacter(char target, int times)
        {
            string output = "";

            for (int i = 0; i < times; i++)
            {
                output += target;
            }

            return output;
        }

        private void InitSyntaxColoring()
        {
            TextArea.SetSelectionBackColor(true, IntToColor(0x114D9C));

            // Configure the default style
            TextArea.StyleResetDefault();
            TextArea.Styles[Style.Default].Font = "Consolas";
            TextArea.Styles[Style.Default].Size = 10;
            TextArea.Styles[Style.Default].BackColor = IntToColor(0x212121);
            TextArea.Styles[Style.Default].ForeColor = IntToColor(0xFFFFFF);
            TextArea.StyleClearAll();

            // Configure the CPP (C#) lexer styles
            TextArea.Styles[Style.Cpp.Identifier].ForeColor = IntToColor(0xD0DAE2);
            TextArea.Styles[Style.Cpp.Comment].ForeColor = IntToColor(0xBD758B);
            TextArea.Styles[Style.Cpp.CommentLine].ForeColor = IntToColor(0x40BF57);
            TextArea.Styles[Style.Cpp.CommentDoc].ForeColor = IntToColor(0x2FAE35);
            TextArea.Styles[Style.Cpp.Number].ForeColor = IntToColor(0xFFFF00);
            TextArea.Styles[Style.Cpp.String].ForeColor = IntToColor(0xFFFF00);
            TextArea.Styles[Style.Cpp.Character].ForeColor = IntToColor(0xE95454);
            TextArea.Styles[Style.Cpp.Preprocessor].ForeColor = IntToColor(0x8AAFEE);
            TextArea.Styles[Style.Cpp.Operator].ForeColor = IntToColor(0xE0E0E0);
            TextArea.Styles[Style.Cpp.Regex].ForeColor = IntToColor(0xff00ff);
            TextArea.Styles[Style.Cpp.CommentLineDoc].ForeColor = IntToColor(0x77A7DB);
            TextArea.Styles[Style.Cpp.Word].ForeColor = IntToColor(0x48A8EE);
            TextArea.Styles[Style.Cpp.Word2].ForeColor = IntToColor(0xF98906);
            TextArea.Styles[Style.Cpp.CommentDocKeyword].ForeColor = IntToColor(0xB3D991);
            TextArea.Styles[Style.Cpp.CommentDocKeywordError].ForeColor = IntToColor(0xFF0000);
            TextArea.Styles[Style.Cpp.GlobalClass].ForeColor = IntToColor(0x48A8EE);
            TextArea.CaretForeColor = Color.White;

            TextArea.Lexer = Lexer.Cpp;

            TextArea.SetKeywords(0, "class extends implements import interface new case do while else if for in switch throw get set function var try catch finally while with default break continue delete return each const namespace package include use is as instanceof typeof author copy default deprecated eventType example exampleText exception haxe inheritDoc internal link mtasc mxmlc param private return see serial serialData serialField since throws usage version langversion playerversion productversion dynamic private public partial static intrinsic internal native override protected AS3 final super this arguments null Infinity NaN undefined true false abstract as base bool break by byte case catch char checked class const continue decimal default delegate do double descending explicit event extern else enum false finally fixed float for foreach from goto group if implicit in int interface internal into is lock long new null namespace object operator out override orderby params private protected public readonly ref return switch struct sbyte sealed short sizeof stackalloc static string select this throw true try typeof uint ulong unchecked unsafe ushort using var virtual volatile void while where yield");
            TextArea.SetKeywords(1, "void Null ArgumentError arguments Array Boolean Class Date DefinitionError Error EvalError Function int Math Namespace Number Object RangeError ReferenceError RegExp SecurityError String SyntaxError TypeError uint XML XMLList Boolean Byte Char DateTime Decimal Double Int16 Int32 Int64 IntPtr SByte Single UInt16 UInt32 UInt64 UIntPtr Void Path File System Windows Forms ScintillaNET");
        }

        #region Numbers and code folding

        /// <summary>
        /// the background color of the text area
        /// </summary>
        private const int BACK_COLOR = 0x2A211C;

        /// <summary>
        /// default text color of the text area
        /// </summary>
        private const int FORE_COLOR = 0xB7B7B7;

        /// <summary>
        /// change this to whatever margin you want the line numbers to show in
        /// </summary>
        private const int NUMBER_MARGIN = 1;

        /// <summary>
        /// change this to whatever margin you want the code folding tree (+/-) to show in
        /// </summary>
        private const int FOLDING_MARGIN = 3;

        /// <summary>
        /// set this true to show circular buttons for code folding (the [+] and [-] buttons on the margin)
        /// </summary>
        private const bool CODEFOLDING_CIRCULAR = true;

        private void InitNumberMargin()
        {

            TextArea.Styles[Style.LineNumber].BackColor = IntToColor(BACK_COLOR);
            TextArea.Styles[Style.LineNumber].ForeColor = IntToColor(FORE_COLOR);
            TextArea.Styles[Style.IndentGuide].ForeColor = IntToColor(FORE_COLOR);
            TextArea.Styles[Style.IndentGuide].BackColor = IntToColor(BACK_COLOR);

            var nums = TextArea.Margins[NUMBER_MARGIN];
            nums.Width = 30;
            nums.Type = MarginType.Number;
            nums.Sensitive = true;
            nums.Mask = 0;
        }
        
        private void InitCodeFolding()
        {

            TextArea.SetFoldMarginColor(true, IntToColor(BACK_COLOR));
            TextArea.SetFoldMarginHighlightColor(true, IntToColor(BACK_COLOR));

            // Enable code folding
            TextArea.SetProperty("fold", "1");
            TextArea.SetProperty("fold.compact", "1");

            // Configure a margin to display folding symbols
            TextArea.Margins[FOLDING_MARGIN].Type = MarginType.Symbol;
            TextArea.Margins[FOLDING_MARGIN].Mask = Marker.MaskFolders;
            TextArea.Margins[FOLDING_MARGIN].Sensitive = true;
            TextArea.Margins[FOLDING_MARGIN].Width = 20;

            // Set colors for all folding markers
            for (int i = 25; i <= 31; i++)
            {
                TextArea.Markers[i].SetForeColor(IntToColor(BACK_COLOR)); // styles for [+] and [-]
                TextArea.Markers[i].SetBackColor(IntToColor(FORE_COLOR)); // styles for [+] and [-]
            }

            // Configure folding markers with respective symbols
            TextArea.Markers[Marker.Folder].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CirclePlus : MarkerSymbol.BoxPlus;
            TextArea.Markers[Marker.FolderOpen].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CircleMinus : MarkerSymbol.BoxMinus;
            TextArea.Markers[Marker.FolderEnd].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CirclePlusConnected : MarkerSymbol.BoxPlusConnected;
            TextArea.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            TextArea.Markers[Marker.FolderOpenMid].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CircleMinusConnected : MarkerSymbol.BoxMinusConnected;
            TextArea.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            TextArea.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            // Enable automatic folding
            TextArea.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);

        }
        #endregion

        public void InitDragDropFile()
        {

            TextArea.AllowDrop = true;
            TextArea.DragEnter += delegate (object sender, DragEventArgs e) {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                    e.Effect = DragDropEffects.Copy;
                else
                    e.Effect = DragDropEffects.None;
            };
            TextArea.DragDrop += delegate (object sender, DragEventArgs e) {

                // get file drop
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {

                    Array a = (Array)e.Data.GetData(DataFormats.FileDrop);
                    if (a != null)
                    {

                        string path = a.GetValue(0).ToString();

                        TextArea.Text = File.ReadAllText(path);

                    }
                }
            };

        }

        #region Indent / Outdent
        private void Indent()
        {
            // we use this hack to send "Shift+Tab" to scintilla, since there is no known API to indent,
            // although the indentation function exists. Pressing TAB with the editor focused confirms this.
            GenerateKeystrokes("{TAB}");
        }

        private void Outdent()
        {
            // we use this hack to send "Shift+Tab" to scintilla, since there is no known API to outdent,
            // although the indentation function exists. Pressing Shift+Tab with the editor focused confirms this.
            GenerateKeystrokes("+{TAB}");
        }

        private void GenerateKeystrokes(string keys)
        {
            TextArea.Focus();
            SendKeys.Send(keys);
        }
        #endregion


        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
        }

        private void openFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            TextArea.Text = File.ReadAllText(openFileDialog.FileName);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.ShowDialog();
        }

        private void saveFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            File.ReadAllText(saveFileDialog.FileName);
        }

        private void compileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> provOptions =
            new Dictionary<string, string>();

            provOptions.Add("CompilerVersion", "v4.0");

            CSharpCodeProvider provider = new CSharpCodeProvider(provOptions);
            CompilerParameters parameters = new CompilerParameters();

            //parameters
            foreach(var line in Properties.Settings.Default.ReferenceAssemblies.Split('\n'))
            {
                parameters.ReferencedAssemblies.Add(line);
            }
            parameters.GenerateInMemory = false;
            parameters.GenerateExecutable = true;

            //get results
            CompilerResults results = provider.CompileAssemblyFromSource(parameters, TextArea.Text);

            //check errors
            if (results.Errors.HasErrors)
            {
                StringBuilder sb = new StringBuilder();

                foreach (CompilerError error in results.Errors)
                {
                    sb.AppendLine(string.Format("Error ({0}): {1}", error.ErrorNumber, error.ToString()));
                }

                var outputFormError = new CodeOutput(sb.ToString());
                outputFormError.ShowDialog();
                return;
            }
            
            var startInfo = new ProcessStartInfo(results.PathToAssembly);
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            var process = Process.Start(startInfo);

            process.WaitForExit(1000);
            var outputForm = new CodeOutput(process.StandardOutput);
            outputForm.ShowDialog();

            /*
            //find assembly and main method to execute
            Assembly assembly = results.CompiledAssembly;
            Type program = assembly.GetType("Program");
            MethodInfo main = program.GetMethod("Main");

            main.Invoke(null, null);

            var outputForm = new CodeOutput(results.Output);
            outputForm.ShowDialog();*/
        }

        private void defaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextArea.Text = defaultString;
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextArea.Text = "";
        }

        private void codeInput_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData == Keys.Tab)
            {
                e.IsInputKey = true;
            }
            if (e.KeyData == (Keys.Tab | Keys.Shift))
            {
                e.IsInputKey = true;
            }
        }

        private void CodeForm_Deactivate(object sender, EventArgs e)
        {
            Properties.Settings.Default.CurrentCode = TextArea.Text;
            Properties.Settings.Default.Save();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SettingsForm().ShowDialog();
        }
    }
}
