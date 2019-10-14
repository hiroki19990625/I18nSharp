using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BinaryIO;
using I18nSharp.Writer;

namespace I18nSharp.WinformEditor
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

            if (File.Exists("Data_00.bin"))
            {
                BinaryStream stream = new BinaryStream(File.ReadAllBytes("Data_00.bin"));
                textBox1.Text = stream.ReadStringUtf8();
            }

            CultureInfo[] cultureInfos = CultureInfo.GetCultures(CultureTypes.AllCultures);
            foreach (CultureInfo cultureInfo in cultureInfos)
            {
                comboBox1.Items.Add(cultureInfo.Name);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = textBox1.Text;
            dialog.ShowDialog();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("The folder is not selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Directory.CreateDirectory(textBox1.Text);

            LanguageFile languageFile = new LanguageFile();
            languageFile.LanguageFileDictionary.CultureString = comboBox1.Text;
            languageFile.SaveFile(
                new FileInfo(textBox1.Text + "/" + comboBox1.Text + ".json"));

            Close();
        }
    }
}