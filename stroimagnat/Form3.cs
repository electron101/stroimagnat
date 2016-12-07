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

        static public BindingSource bs_product = new BindingSource();
        static public BindingSource bs_post = new BindingSource();
        static public BindingSource bs_mol = new BindingSource();
        static public BindingSource bs_prihod = new BindingSource();
        static public BindingSource bs_otpusk = new BindingSource();
        static public BindingSource bs_spid_vid = new BindingSource();
        static public BindingSource bs_rashod = new BindingSource();
     
        // для перемещиния формы ----------
        private Point mouseOffset;
        private bool isMouseDown = false;
        // --------------------------------

        public Form3()
        {
            InitializeComponent();
        }
      
        static public void load_prihod()                   // функция для отображения информации
        {
            //if (ds.Tables["PRIHOD"] == null)
            //    ds.Tables.Add("PRIHOD");
            ds.Tables["PRIHOD"].Clear();
            strSQL = " SELECT prihod.id_prihod AS '№_Прихода', postav.name AS 'Поставщик', " +
                           " mol.name AS 'Ответственный', product.name AS 'Материал', " +
                           " prihod.kolvo AS 'Количество', " +
                           " prihod.date_time AS 'Дата_прихода' FROM prihod JOIN postav ON " +
                           " prihod.id_post = postav.id_post JOIN mol ON prihod.id_mol = " +
                           " mol.id_mol JOIN product ON prihod.id_product = product.id_product ";// +
                           //" WHERE prihod.date_time >= @DATE_S AND prihod.date_time < @DATE_PO ";

            SqlCommand cm0 = new SqlCommand(strSQL, cn);
            cm0.Parameters.Add("@DATE_S", SqlDbType.Date).Value = Program.F1.dateTimePicker4.Value;
            cm0.Parameters.Add("@DATE_PO", SqlDbType.Date).Value = Program.F1.dateTimePicker3.Value.AddDays(1);

            SQLAdapter = new SqlDataAdapter(cm0);
            SQLAdapter.Fill(ds, "PRIHOD");

            bs_prihod.DataSource = ds.Tables["PRIHOD"];
            Program.F1.dataGridView3.DataSource = bs_prihod;
        }


        public void Form3_Load(object sender, EventArgs e)
        {
            Program.center_form(Program.F3);

            SqlConnectionStringBuilder bdr = new SqlConnectionStringBuilder();
            bdr.DataSource = @".\SQLExpress";
            bdr.InitialCatalog = "vit";
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
            //dateTimePicker1.Value = DateTime.Now.AddMonths(-1);
            //dateTimePicker2.Value = DateTime.Now;

            Program.F1.dateTimePicker4.Value = DateTime.Now.AddMonths(-1);
            Program.F1.dateTimePicker3.Value = DateTime.Now;
            //
            // --- [ ЗАГРУЗКА ] ---   ПРИХОД ПРОДУКТОВ ----------------------------------------------
            ds.Tables.Add("PRIHOD");
            load_prihod();
            Program.F1.dataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            Program.F1.dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Program.F1.comboBox_prod_pri.DataBindings.Add(new Binding("Text", bs_prihod, "Материал", false, DataSourceUpdateMode.Never));
            Program.F1.comboBox_post_pri.DataBindings.Add(new Binding("Text", bs_prihod, "Поставщик", false, DataSourceUpdateMode.Never));
            Program.F1.comboBox_mol_pri.DataBindings.Add(new Binding("Text", bs_prihod, "Ответственный", false, DataSourceUpdateMode.Never));
            Program.F1.numericUpDown_pri_kolvo.DataBindings.Add(new Binding("Value", bs_prihod, "Количество", false, DataSourceUpdateMode.Never));
            //textBox_pri_cena.DataBindings.Add(new Binding("Text", bs_prihod, "Цена закупочная", false, DataSourceUpdateMode.Never));
            // --------------------------------------------------------------------------------------

            //Program.F1.comboBox_prod_pri.DataSource = bs_product;
            //Program.F1.comboBox_prod_pri.DisplayMember = "Наименование";
            //Program.F1.comboBox_prod_pri.ValueMember = "№_Материала";

            //Program.F1.comboBox_post_pri.DataSource = bs_post;
            //Program.F1.comboBox_post_pri.DisplayMember = "Наименование";
            //Program.F1.comboBox_post_pri.ValueMember = "№_Поставщика";

            //Program.F1.comboBox_mol_pri.DataSource = bs_mol;
            //Program.F1.comboBox_mol_pri.DisplayMember = "ФИО";
            //Program.F1.comboBox_mol_pri.ValueMember = "№_Ответсвенного лица";
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
            Program.F1 = new Form1();   // создаёт экземпляр формы 
            Program.F3.Hide();          // скрывает форму входа
            Program.F1.Show();          // отображает форму 
            Program.F3.Close();         // закрывает форму входа
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // запустить форму материалов
            Program.F4 = new Form4();   // создаёт экземпляр формы 
            Program.F3.Hide();          // скрывает форму входа
            Program.F4.Show();          // отображает форму 
            Program.F3.Close();         // закрывает форму входа
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            // запустить форму отпуска материалов
            Program.F2 = new Form2();   // создаёт экземпляр формы 
            Program.F3.Hide();          // скрывает форму входа
            Program.F2.Show();          // отображает форму 
            Program.F3.Close();         // закрывает форму входа
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // запустить форму истории выдачи
            Program.F5 = new Form5();   // создаёт экземпляр формы 
            Program.F3.Hide();          // скрывает форму входа
            Program.F5.Show();          // отображает форму 
            Program.F3.Close();         // закрывает форму входа

        }

        private void button6_Click(object sender, EventArgs e)
        {
            // запустить форму поставщиков
            Program.F6 = new Form6();   // создаёт экземпляр формы 
            Program.F3.Hide();          // скрывает форму входа
            Program.F6.Show();          // отображает форму 
            Program.F3.Close();         // закрывает форму входа
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // запустить форму ответственных
            Program.F7 = new Form7();   // создаёт экземпляр формы 
            Program.F3.Hide();          // скрывает форму входа
            Program.F7.Show();          // отображает форму 
            Program.F3.Close();         // закрывает форму входа
        }
    }
}