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

namespace stroimagnat
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        void load_prihod()                   // функция для отображения информации
        {
            Form3.ds.Tables["PRIHOD"].Clear();
            Form3.strSQL = " SELECT prihod.id_prihod AS '№_Прихода', postav.name AS 'Поставщик', " +
                           " mol.name AS 'Ответственный', product.name AS 'Материал', " +
                           " prihod.kolvo AS 'Количество', " +
                           " prihod.date_time AS 'Дата_прихода' FROM prihod JOIN postav ON " +
                           " prihod.id_post = postav.id_post JOIN mol ON prihod.id_mol = " +
                           " mol.id_mol JOIN product ON prihod.id_product = product.id_product " +
                           " WHERE prihod.date_time >= @DATE_S AND prihod.date_time < @DATE_PO ";

            SqlCommand cm0 = new SqlCommand(Form3.strSQL, Form3.cn);
            cm0.Parameters.Add("@DATE_S", SqlDbType.Date).Value = dateTimePicker4.Value;
            cm0.Parameters.Add("@DATE_PO", SqlDbType.Date).Value = dateTimePicker3.Value.AddDays(1);

            Form3.SQLAdapter = new SqlDataAdapter(cm0);
            Form3.SQLAdapter.Fill(Form3.ds, "PRIHOD");

            Form3.bs_prihod.DataSource = Form3.ds.Tables["PRIHOD"];
            dataGridView3.DataSource = Form3.bs_prihod;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dateTimePicker4.Value = DateTime.Now.AddMonths(-1);
            dateTimePicker3.Value = DateTime.Now;
            Form6.Form6_Load(sender, e);
            //
            // --- [ ЗАГРУЗКА ] ---   ПРИХОД ПРОДУКТОВ ----------------------------------------------
            Form3.ds.Tables.Add("PRIHOD");
            load_prihod();
            dataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            comboBox_prod_pri.DataBindings.Add(new Binding("Text", Form3.bs_prihod, "Материал", false, DataSourceUpdateMode.Never));
            comboBox_post_pri.DataBindings.Add(new Binding("Text", Form3.bs_prihod, "Поставщик", false, DataSourceUpdateMode.Never));
            comboBox_mol_pri.DataBindings.Add(new Binding("Text", Form3.bs_prihod, "Ответственный", false, DataSourceUpdateMode.Never));
            numericUpDown_pri_kolvo.DataBindings.Add(new Binding("Value", Form3.bs_prihod, "Количество", false, DataSourceUpdateMode.Never));
            //textBox_pri_cena.DataBindings.Add(new Binding("Text", Form3.bs_prihod, "Цена закупочная", false, DataSourceUpdateMode.Never));
            // --------------------------------------------------------------------------------------

            comboBox_prod_pri.DataSource = Form3.bs_product;
            comboBox_prod_pri.DisplayMember = "Наименование";
            comboBox_prod_pri.ValueMember = "№_Материала";

            comboBox_post_pri.DataSource = Form3.bs_post;
            comboBox_post_pri.DisplayMember = "Наименование";
            comboBox_post_pri.ValueMember = "№_Поставщика";

            comboBox_mol_pri.DataSource = Form3.bs_mol;
            comboBox_mol_pri.DisplayMember = "ФИО";
            comboBox_mol_pri.ValueMember = "№_Ответсвенного лица";
        }

        private void button23_Click(object sender, EventArgs e)
        {
            load_prihod();
        }
    }
}