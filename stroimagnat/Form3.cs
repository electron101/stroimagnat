using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace stroimagnat
{
    public partial class Form3 : Form
    {
        static public SqlConnection cn;
        static public DataSet ds = new DataSet();
        static public SqlDataAdapter SQLAdapter = new SqlDataAdapter(strSQL, cn);
        static public string strSQL;

        BindingSource bs_prepod = new BindingSource();
        BindingSource bs_polzov = new BindingSource();

        // для перемещиния формы ----------
        private Point mouseOffset;
        private bool isMouseDown = false;
        // --------------------------------

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            Program.center_form(Program.F3);

            SqlConnectionStringBuilder bdr = new SqlConnectionStringBuilder();
            bdr.DataSource = @".\SQLExpress";
            bdr.InitialCatalog = "bank_karta";
            bdr.IntegratedSecurity = true;

            cn = new SqlConnection(bdr.ConnectionString);
            try
            {
                cn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            panel3.Dock = DockStyle.Top;
        }
                
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
        // для перемещения формы мышью ----------------------------------------------
        //
        private void Form3_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = false;
            }
        }
        private void Form3_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouseOffset.X, mouseOffset.Y);
                Location = mousePos;
            }
        }
        private void Form3_MouseDown(object sender, MouseEventArgs e)
        {
            int xOffset;
            int yOffset;

            if (e.Button == MouseButtons.Left)
            {
                xOffset = -e.X - SystemInformation.FrameBorderSize.Width;
                yOffset = -e.Y - SystemInformation.CaptionHeight -
                    SystemInformation.FrameBorderSize.Height;
                mouseOffset = new Point(xOffset, yOffset);
                isMouseDown = true;
            }
        }
        private void Form3_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // запустить форму прихода материалов
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // запустить форму материалов
           
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            // запустить форму отпуска материалов
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // запустить форму истории выдачи
                  }

        private void button6_Click(object sender, EventArgs e)
        {
            // запустить форму поставщиков
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // запустить форму ответственных
           
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            panel3.Visible = false;
            panel1.Dock = DockStyle.Top;
            panel1.Visible = true;
        }
    }
}