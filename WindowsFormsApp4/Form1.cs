using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aspose.Cells;
using System.IO;
using ExcelDataReader;
using NPOI.XSSF.Model;
using System.Xml.Linq;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int RCnt = dGridView_Base.Rows.Count - 1;

            dGridView_Base.Visible = false;
            for (int TCnt = 0; TCnt <= RCnt; TCnt++)
                dGridView_Base.Rows.Remove(dGridView_Base.Rows[0]);
            dGridView_Base.Visible = true;

            txtFilePath.Text = "";

            LoadNewFile();
        }


        private void LoadNewFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            DialogResult dr = ofd.ShowDialog();

            if (dr == DialogResult.OK)
            {

                txtFilePath.Text = ofd.FileName;

                var extension = Path.GetExtension(ofd.FileName).ToLower();

                var workbook = new Workbook(ofd.FileName);

                var path = ofd.FileName.Replace(extension, ".xlsx");

                workbook.Save(path);

                Worksheet worksheet = workbook.Worksheets[0];
                DataTable dt = new DataTable(worksheet.Name);

                dt = worksheet.Cells.ExportDataTableAsString(0, 0, worksheet.Cells.MaxDataRow + 1
                    , worksheet.Cells.MaxDataColumn + 1, true);

                    DataBase db = new DataBase();
                db.Connect_DB(dt);

                dGridView_Base.DataSource = dt;
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                }

                catch (Exception theException)
                {

                    String errorMessage;
                    errorMessage = "Error: ";
                    errorMessage = String.Concat(errorMessage, theException.Message);
                    errorMessage = String.Concat(errorMessage, " Line: ");
                    errorMessage = String.Concat(errorMessage, theException.Source);

                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void dGridView_Base_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }


        private void Form1_Load(object sender, EventArgs e)
        {


        }

   
    }
}