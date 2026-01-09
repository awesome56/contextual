using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Contextual.Properties;
using FontAwesome.Sharp;
using GlobalVariable;

namespace Contextual
{
    public partial class frmResult : Form
    {
        public frmResult()
        {
            InitializeComponent();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + GlobalVariable.Globals.databasePath + ";Integrated Security=True;MultipleActiveResultSets=True;Connect Timeout=30");

        private PrintDocument printDocument1 = new PrintDocument();

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void frmResult_Load(object sender, EventArgs e)
        {
            /* printing command for old printing
            PrintDialog pd = new PrintDialog();
            PrintDocument doc = new PrintDocument();
            doc.PrintPage += myPrintPage;
            pd.Document = doc;
            if (pd.ShowDialog() == DialogResult.OK)
            {
                doc.Print();
            }
            */
            // Define your logo image and header text
        }

        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            Con.Open();

            string header = "";

            string query = "SELECT * FROM UserTbl";
            SqlCommand cmd = new SqlCommand(query, Con);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                header = reader.GetString("Header");
            }

            Con.Close();
            // Define your logo image and header text
            Image logo = Resources.lautech_letter_head;

            // Set up the font and margins
            Font headerFont = new Font("Arial", 10.8f, FontStyle.Bold);
            float y = 10;  // logo vertical position
            float y2 = 23;  // header vertical
            float lineHeight = headerFont.GetHeight();
            float logoWidth = 150;
            float logoHeight = 150;

            // Draw the logo and header
            e.Graphics.DrawImage(logo, 10, y, logoWidth, logoHeight);
            e.Graphics.DrawString(header, headerFont, Brushes.Black, logoWidth + 20, y2);
        }

        private void Print(Panel pnl)
        {
            /*
            PrinterSettings ps = new PrinterSettings();
            PrintDocument printDocument = new PrintDocument();
            //printDocument.DefaultPageSettings.Landscape = true;
            //printDocument.DefaultPageSettings.Margins = new Margins(10, 10, 10, 10);
            panel1 = pnl; 

            getprintarea(panel1);

            // Set up the print preview dialog
            PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
            printPreviewDialog.Document = printDocument;

            // Set up the print event handler
            printDocument.PrintPage += new PrintPageEventHandler(PrintPage);

            // Display the print preview dialog
            printPreviewDialog.ShowDialog();

            // Print the document
            //printDocument.Print();

            //Bitmap panelContent = LoadPanelContent();
            //printDocument.Print();
            */
        }
        /*
        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            Rectangle pagearea = e.PageBounds;

            e.Graphics.DrawImage(memoryimg, (pagearea.Width / 2) - (this.panel1.Width /2), this.panel1.Location.Y);
        }
        */
        // Event handler for the print event
        public Bitmap memoryimg;
        /*
        private void getprintarea(Panel pnl)
        {
            memoryimg = new Bitmap(pnl.Width, pnl.Height);
            pnl.DrawToBitmap(memoryimg, new Rectangle(0, 0, pnl.Width, pnl.Height));
        }
        */

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridResult_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtName_Click(object sender, EventArgs e)
        {

        }
        Bitmap bitmap;
        private void label22_Click(object sender, EventArgs e)
        {
            
           // Print(this.panel1);
           /*
           Panel panel = new Panel();
            this.Controls.Add(panel);
            Graphics graphics = panel.CreateGraphics();
            Size size = this.ClientSize;
            bitmap = new Bitmap(size.Width, size.Height, graphics);
            graphics = Graphics.FromImage(bitmap);

            Point point = PointToScreen(panel.Location);
            graphics.CopyFromScreen(point.X, point.Y, 0, 0, size);

            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
           */


        }

        private void myPrintPage(object sender, PrintPageEventArgs e)
        {
            Bitmap bm = new Bitmap(panel1.Width, panel1.Height);
            panel1.DrawToBitmap(bm, new Rectangle(0,0,panel1.Width,panel1.Height));
            
            e.Graphics.DrawImage(bm,0,0);

            //e.HasMorePages = true;

            bm.Dispose();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        /*
        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(bitmap, 0, 0);
        }
        */
    }
}
