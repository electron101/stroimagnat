﻿using System;
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

        static public void load_post()                   // функция для отображения информации
        {
            ds.Tables["POST"].Clear();
            strSQL = " SELECT id_post AS '№_Поставщика', name AS 'Наименование', " +
                           " adres AS 'Адрес', tel AS 'Телефон', bank_schet AS " +
                           " 'Банковский счёт' FROM postav ";
            SQLAdapter = new SqlDataAdapter(strSQL, cn);

            SQLAdapter.Fill(ds, "POST");

            bs_post.DataSource = ds.Tables["POST"];
        }

        private void Form3_Load(object sender, EventArgs e)
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

            //
            // --- [ ЗАГРУЗКА ] ---   ПОСТАВЩИКИ ----------------------------------------------------
            ds.Tables.Add("POST");
            load_post();
            Program.F6.dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            Program.F6.dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Program.F6.textBox_post_name.DataBindings.Add(new Binding("Text", Form3.bs_post, "Наименование", false, DataSourceUpdateMode.Never));
            Program.F6.textBox_post_adres.DataBindings.Add(new Binding("Text", Form3.bs_post, "Адрес", false, DataSourceUpdateMode.Never));
            Program.F6.textBox_post_tel.DataBindings.Add(new Binding("Text", Form3.bs_post, "Телефон", false, DataSourceUpdateMode.Never));
            Program.F6.textBox_post_bank.DataBindings.Add(new Binding("Text", Form3.bs_post, "Банковский счёт", false, DataSourceUpdateMode.Never));
            // --------------------------------------------------------------------------------------

            Program.F6.dataGridView2.DataSource = bs_post;
            //Form4.load_product();
            //Form6.load_post();
            //Form7.load_mol();
            //Program.F1.comboBox_prod_pri.DataSource = Form3.bs_product;
            //Program.F1.comboBox_prod_pri.DisplayMember = "Наименование";
            //Program.F1.comboBox_prod_pri.ValueMember = "№_Материала";

            Program.F1.comboBox_post_pri.DataSource = Form3.bs_post;
            Program.F1.comboBox_post_pri.DisplayMember = "Наименование";
            Program.F1.comboBox_post_pri.ValueMember = "№_Поставщика";

            //Program.F1.comboBox_mol_pri.DataSource = Form3.bs_mol;
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