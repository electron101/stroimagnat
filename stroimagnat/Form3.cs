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
        BindingSource bs_post = new BindingSource();
        BindingSource bs_mol = new BindingSource();
        BindingSource bs_prihod = new BindingSource();
        BindingSource bs_otpusk = new BindingSource();
        BindingSource bs_spid_vid = new BindingSource();
        BindingSource bs_rashod = new BindingSource();
        
        // для перемещиния формы ----------
        private Point mouseOffset;
        private bool isMouseDown = false;
        // --------------------------------

        public Form3()
        {
            InitializeComponent();
        }

        void load_product()                   // функция для отображения информации о продукте в datagridview
        {
            ds.Tables["PRODUCT"].Clear();
            strSQL = "SELECT id_product AS '№_Продукта', name AS 'Наименование', ed_izmer AS 'Единица измерения', cena AS 'Цена' FROM product";
            SQLAdapter = new SqlDataAdapter(strSQL, cn);

            SQLAdapter.Fill(ds, "PRODUCT");

            bs_product.DataSource = ds.Tables["PRODUCT"];
            dataGridView1.DataSource = bs_product;
        }

        void load_post()                   // функция для отображения информации
        {
            ds.Tables["POST"].Clear();
            strSQL = " SELECT id_post AS '№_Поставщика', name AS 'Наименование', adres AS 'Адрес', " +
                     " tel AS 'Телефон', bank_schet AS 'Банковский счёт' FROM postav";
            SQLAdapter = new SqlDataAdapter(strSQL, cn);

            SQLAdapter.Fill(ds, "POST");

            bs_post.DataSource = ds.Tables["POST"];
            dataGridView2.DataSource = bs_post;
        }

        void load_mol()                   // функция для отображения информации
        {
            ds.Tables["MOL"].Clear();
            strSQL = " SELECT id_mol AS '№_Ответсвенного лица', name AS 'ФИО', adres AS 'Адрес', tel AS 'Телефон' FROM mol";
            SQLAdapter = new SqlDataAdapter(strSQL, cn);

            SQLAdapter.Fill(ds, "MOL");

            bs_mol.DataSource = ds.Tables["MOL"];
            dataGridView4.DataSource = bs_mol;
        }

        void load_prihod()                   // функция для отображения информации
        {
            ds.Tables["PRIHOD"].Clear();
            strSQL = " SELECT prihod.id_prihod AS '№_Прихода', postav.name AS 'Поставщик', mol.name AS 'Ответственный', " +
                     " product.name AS 'Продукт', prihod.kolvo AS 'Количество', prihod.cena_zakup AS 'Цена закупочная', " +
                     " prihod.date_time AS 'Дата_прихода' FROM prihod JOIN postav ON prihod.id_post = postav.id_post " +
                     " JOIN mol ON prihod.id_mol = mol.id_mol JOIN product ON prihod.id_product = product.id_product";
            SQLAdapter = new SqlDataAdapter(strSQL, cn);

            SQLAdapter.Fill(ds, "PRIHOD");

            bs_prihod.DataSource = ds.Tables["PRIHOD"];
            dataGridView3.DataSource = bs_prihod;
        }


        void load_otpusk()                   // функция для отображения информации
        {
            ds.Tables["OTPUSK"].Clear();
            strSQL = " SELECT nalichie.id_product AS '№_Продукта', product.name AS 'Наименование', " +
                     " nalichie.kolvo AS 'Количество', product.ed_izmer AS 'Единица измерения' FROM nalichie " +
                     " JOIN product ON nalichie.id_product = product.id_product WHERE nalichie.kolvo > 0";
            SQLAdapter = new SqlDataAdapter(strSQL, cn);

            SQLAdapter.Fill(ds, "OTPUSK");

            bs_otpusk.DataSource = ds.Tables["OTPUSK"];
            dataGridView5.DataSource = bs_otpusk;
        }

        void load_rashod()                   // функция для отображения информации
        {
            ds.Tables["RASHOD"].Clear();
            strSQL = " SELECT rashod.id_rashod AS '№_Расхода', product.id_product AS 'Продукт', " +
                     " rashod.kolvo AS 'Количество', rashod.date_time AS 'Дата_выдачи' FROM rashod " +
                     " JOIN product ON rashod.id_product = product.id_product";
            SQLAdapter = new SqlDataAdapter(strSQL, cn);

            SQLAdapter.Fill(ds, "RASHOD");

            bs_rashod.DataSource = ds.Tables["RASHOD"];
            dataGridView7.DataSource = bs_rashod;
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
            panel3.Dock = DockStyle.Fill;

            //
            // --- [ ЗАГРУЗКА ] ---   ПРОДУКТЫ ------------------------------------------------------
            ds.Tables.Add("PRODUCT");
            load_product();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            textBox_prod_name.DataBindings.Add(new Binding("Text", bs_product, "Наименование", false, DataSourceUpdateMode.Never));
            textBox_prod_ediz.DataBindings.Add(new Binding("Text", bs_product, "Единица измерения", false, DataSourceUpdateMode.Never));
            textBox_prod_cena.DataBindings.Add(new Binding("Text", bs_product, "Цена", false, DataSourceUpdateMode.Never));

            // --------------------------------------------------------------------------------------

            //
            // --- [ ЗАГРУЗКА ] ---   ПОСТАВЩИКИ ----------------------------------------------------
            ds.Tables.Add("POST");
            load_post();
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            textBox_post_name.DataBindings.Add(new Binding("Text", bs_post, "Наименование", false, DataSourceUpdateMode.Never));
            textBox_post_adres.DataBindings.Add(new Binding("Text", bs_post, "Адрес", false, DataSourceUpdateMode.Never));
            textBox_post_tel.DataBindings.Add(new Binding("Text", bs_post, "Телефон", false, DataSourceUpdateMode.Never));
            textBox_post_bank.DataBindings.Add(new Binding("Text", bs_post, "Банковский счёт", false, DataSourceUpdateMode.Never));

            // --------------------------------------------------------------------------------------

            //
            // --- [ ЗАГРУЗКА ] ---   ЛИЦА (МОЛ) ----------------------------------------------------
            ds.Tables.Add("MOL");
            load_mol();
            dataGridView4.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView4.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            textBox_mol_fio.DataBindings.Add(new Binding("Text", bs_mol, "ФИО", false, DataSourceUpdateMode.Never));
            textBox_mol_adres.DataBindings.Add(new Binding("Text", bs_mol, "Адрес", false, DataSourceUpdateMode.Never));
            textBox_mol_tel.DataBindings.Add(new Binding("Text", bs_mol, "Телефон", false, DataSourceUpdateMode.Never));

            // --------------------------------------------------------------------------------------

            //
            // --- [ ЗАГРУЗКА ] ---   ПРИХОД ПРОДУКТОВ ----------------------------------------------
            ds.Tables.Add("PRIHOD");
            load_prihod();
            dataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            comboBox_prod_pri.DataBindings.Add(new Binding("Text", bs_prihod, "Продукт", false, DataSourceUpdateMode.Never));
            comboBox_post_pri.DataBindings.Add(new Binding("Text", bs_prihod, "Поставщик", false, DataSourceUpdateMode.Never));
            comboBox_mol_pri.DataBindings.Add(new Binding("Text", bs_prihod, "Ответственный", false, DataSourceUpdateMode.Never));
            numericUpDown_pri_kolvo.DataBindings.Add(new Binding("Value", bs_prihod, "Количество", false, DataSourceUpdateMode.Never));
            textBox_pri_cena.DataBindings.Add(new Binding("Text", bs_prihod, "Цена закупочная", false, DataSourceUpdateMode.Never));

            // --------------------------------------------------------------------------------------

            //
            // --- [ ЗАГРУЗКА ] ---   ОТПУСК ПРОДУКТОВ ----------------------------------------------
            try
            {
                ds.Tables.Add("OTPUSK");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            load_otpusk();
            dataGridView5.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView5.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // --------------------------------------------------------------------------------------

            //
            // --- [ ЗАГРУЗКА ] ---   СПИСОК ВЫДАЧИ -------------------------------------------------
            ds.Tables.Add("SPIS_VID");
            dataGridView6.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView6.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // добавляем нужные столбцы в ручную
            ds.Tables["SPIS_VID"].Columns.Add("№_Продукта");
            ds.Tables["SPIS_VID"].Columns.Add("Наименование");
            ds.Tables["SPIS_VID"].Columns.Add("Колличество");
            ds.Tables["SPIS_VID"].Columns.Add("Единица измерения");

            bs_spid_vid.DataSource = ds.Tables["SPIS_VID"];
            dataGridView6.DataSource = bs_spid_vid;

            // --------------------------------------------------------------------------------------

            //
            // --- [ ЗАГРУЗКА ] ---   ВЫДАННЫЕ ПРОДУКТЫ ---------------------------------------------
            ds.Tables.Add("RASHOD");
            load_rashod();
            dataGridView7.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView7.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // --------------------------------------------------------------------------------------

            comboBox_prod_pri.DataSource = bs_product;
            comboBox_prod_pri.DisplayMember = "Наименование";
            comboBox_prod_pri.ValueMember = "№_Продукта";

            comboBox_post_pri.DataSource = bs_post;
            comboBox_post_pri.DisplayMember = "Наименование";
            comboBox_post_pri.ValueMember = "№_Поставщика";

            comboBox_mol_pri.DataSource = bs_mol;
            comboBox_mol_pri.DisplayMember = "ФИО";
            comboBox_mol_pri.ValueMember = "№_Ответсвенного лица";


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
            panel3.Visible = false;
            panel1.Dock = DockStyle.Fill;
            panel1.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // запустить форму материалов
            panel3.Visible = false;
            panel4.Dock = DockStyle.Fill;
            panel4.Visible = true;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            // запустить форму отпуска материалов
            panel3.Visible = false;
            panel2.Dock = DockStyle.Fill;
            panel2.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // запустить форму истории выдачи
            panel3.Visible = false;
            panel5.Dock = DockStyle.Fill;
            panel5.Visible = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // запустить форму поставщиков
            panel3.Visible = false;
            panel6.Dock = DockStyle.Fill;
            panel6.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // запустить форму ответственных
            panel3.Visible = false;
            panel7.Dock = DockStyle.Fill;
            panel7.Visible = true;
        }
    }
}