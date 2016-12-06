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
        static public SqlConnection cn;
        static public DataSet ds = new DataSet();
        static public SqlDataAdapter SQLAdapter = new SqlDataAdapter(strSQL, cn);
        static public string strSQL;

        BindingSource bs_prepod = new BindingSource();
        BindingSource bs_polzov = new BindingSource();
        
        public Form1()
        {
            InitializeComponent();
        }
    }
}
