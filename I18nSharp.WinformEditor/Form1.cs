using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BinaryIO;
using I18nSharp.Writer;
using I18nSharp.Writer.Content;

namespace I18nSharp.WinformEditor
{
    public partial class Form1 : Form
    {
        private readonly Dictionary<string, LanguageFile> _languageFiles = new Dictionary<string, LanguageFile>();
        private readonly Dictionary<string, FileInfo> _files = new Dictionary<string, FileInfo>();
        private DirectoryInfo _directoryInfo;

        private string _folder = "";

        private bool _ignoreEvent;
        private LanguageFile _selectedLanguageFile;

        public Form1()
        {
            InitializeComponent();

            if (File.Exists("Data_00.bin"))
            {
                BinaryStream stream = new BinaryStream(File.ReadAllBytes("Data_00.bin"));
                _folder = stream.ReadStringUtf8();
            }

            LoadLanguages();
            UpdateList();
        }

        private void LoadLanguages()
        {
            _languageFiles.Clear();
            _files.Clear();

            _directoryInfo = new DirectoryInfo(_folder);

            BinaryStream stream = new BinaryStream();
            stream.WriteStringUtf8(_folder);
            File.WriteAllBytes("Data_00.bin", stream.GetBuffer());

            foreach (FileInfo fileInfo in _directoryInfo.GetFiles())
            {
                LanguageFile languageFile = new LanguageFile(fileInfo);
                _languageFiles.Add(languageFile.GetCulture(), languageFile);
                _files.Add(languageFile.GetCulture(), fileInfo);
            }
        }

        private void LoadLanguageLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = _folder;
            dialog.ShowDialog();

            try
            {
                _folder = dialog.SelectedPath;
                LoadLanguages();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show("Reading LanguageFile failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            UpdateList();
        }

        private void UpdateList()
        {
            listBox1.Items.Clear();

            foreach (KeyValuePair<string, LanguageFile> languageFile in _languageFiles)
            {
                listBox1.Items.Add(languageFile.Key);
            }

            if (listBox1.Items.Count > 0)
                listBox1.SelectedIndex = 0;
        }

        private void CreateLanguageCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();

            LoadLanguages();
            UpdateList();
        }

        private void SaveSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, LanguageFile> languageFile in _languageFiles)
            {
                languageFile.Value.SaveFile(_files[languageFile.Key]);
            }
        }

        private void ExitEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            if (index == -1)
                return;

            string culture = (string) listBox1.Items[index];
            _selectedLanguageFile = _languageFiles[culture];

            UpdateGrid();
        }

        public void UpdateGrid()
        {
            _ignoreEvent = true;
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("key", "key");
            dataGridView1.Columns.Add("value", "value");

            foreach (KeyValuePair<string, LanguageFileContent> keyValue in _selectedLanguageFile.LanguageFileDictionary
                .LanguageFileContents)
            {
                dataGridView1.Rows.Add(keyValue.Key, ((LanguageFileText) keyValue.Value).Content);
            }

            _ignoreEvent = false;
        }

        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            _selectedLanguageFile.LanguageFileDictionary.LanguageFileContents.Clear();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    _selectedLanguageFile.LanguageFileDictionary.LanguageFileContents.Add((string) row.Cells[0].Value,
                        new LanguageFileText((string) row.Cells[1].Value));
                }
            }
        }

        private void DataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "key" &&
                _selectedLanguageFile.LanguageFileDictionary.LanguageFileContents.ContainsKey(
                    e.FormattedValue.ToString()))
            {
                //dataGridView1.Rows[e.RowIndex].ErrorText = "Duplicate key.";
                dataGridView1.CancelEdit();
                e.Cancel = true;
            }

            if (dataGridView1.Columns[e.ColumnIndex].Name == "value" &&
                string.IsNullOrWhiteSpace(dataGridView1.Rows[e.RowIndex].Cells[0].Value?.ToString()))
            {
                //dataGridView1.Rows[e.RowIndex].ErrorText = "Empty key.";
                dataGridView1.CancelEdit();
                e.Cancel = true;
            }
        }

        private void DataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            // dataGridView1.Rows[e.RowIndex].ErrorText = null;
        }

        private void DataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (_ignoreEvent)
                return;

            _selectedLanguageFile.LanguageFileDictionary.LanguageFileContents.Clear();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    _selectedLanguageFile.LanguageFileDictionary.LanguageFileContents.Add((string) row.Cells[0].Value,
                        new LanguageFileText((string) row.Cells[1].Value));
                }
            }
        }

        private void GenerateGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3(_languageFiles.Keys.ToArray(), _languageFiles.Values.ToArray());
            form3.ShowDialog();
        }

        private void CodeCToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            codeCToolStripMenuItem.Enabled = _languageFiles.Count > 0;
        }
    }
}