using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using I18nSharp.Writer;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using Color = System.Windows.Media.Color;

namespace I18nSharp.WinformEditor
{
    public partial class Form3 : Form
    {
        private readonly TextEditor _editor;
        private readonly LanguageFile[] _files;

        public Form3(string[] cultureList, LanguageFile[] files)
        {
            InitializeComponent();

            foreach (string culture in cultureList)
            {
                comboBox1.Items.Add(culture);
            }

            if (cultureList.Length > 0)
                comboBox1.SelectedIndex = 0;

            _editor = new TextEditor();
            _editor.IsReadOnly = true;
            _editor.ShowLineNumbers = true;
            _editor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");
            _editor.BorderBrush = new SolidColorBrush(Colors.Black);
            _editor.BorderThickness = new Thickness(0.5);
            elementHost1.Child = _editor;

            _files = files;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            _editor.Text = CodeGenerator.Generate(_files, comboBox1.Text);
        }
    }
}