// 2021.06.06 / 08 :33 AM
//텍스트 박스 포커스 및 로그인 엔터기능


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace pj1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
           
        }
        MySqlConnection conn;
        MySqlCommand cmd;
        string str_id, str_pw,str_name;

        private void button1_Click(object sender, EventArgs e)
        {
            //this.button1.BackColor = Color.CornflowerBlue;
            String sql = "SELECT * FROM user";
            cmd.CommandText = sql; //트럭에 짐 싣기
            MySqlDataReader reader; //짐을 연결한 끈, 서버에서 데이터를 가져오도록 실행
            reader = cmd.ExecuteReader();
            reader.Read();
          
           
            
                str_id = reader["id"].ToString();
                str_pw = reader["pw"].ToString();
                str_name = reader["name"].ToString();

                
                if (tb_user.Text.ToString() == str_id && tb_pw.Text.ToString() == str_pw)
                    {

                        Form2 f2 = new Form2(this);
                        f2.Show();
                        MessageBox.Show($"{str_name}님 환영합니다!");
                        f2.label17.Text = $"관리자 : {str_name}님 /";
                        this.Hide();
                        

                    }
                    else
                    {
                        MessageBox.Show("다시 입력해 주세요!");
                    }
                
            
           
                reader.Close();
            
        }

        private void tb_pw_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
            }

       
        }

        private void label1_MouseHover(object sender, EventArgs e)
        {
            
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            this.button1.BackColor = Color.CornflowerBlue;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            this.button1.BackColor = Color.Gainsboro;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
           
            conn.Close();
            
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
             String connStr = "Server=192.168.100.56;Port=3306;Uid=pjuser;Pwd=1234;Database=project;CHARSET=UTF8";
            conn = new MySqlConnection(connStr);
            conn.Open();
            cmd = new MySqlCommand("", conn);
            this.ActiveControl = tb_user;

        }
    }
}
