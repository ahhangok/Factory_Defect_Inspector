// 2021.06.06 / 1:10 PM
// 그래프 리프레쉬 최종

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
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;


namespace pj1
{
    public partial class Form2 : Form
    {
        Form1 f1;
        FileInfo _fileInfo = null;        // 파일 정보
        string _strFilePath = "";        // 파일 경로
        public Form2(Form1 f)
        {
            InitializeComponent();
            f1 = f;
            //f1.Close();


        }
        MySqlConnection conn;
        MySqlCommand cmd;
        string str_id;
        string str_name;
        string str_model;
        string str_date;
        string str_money;
        string str_amount;
        string str_place;
        string str_bid, str_bname, str_bmodel, str_bdate, str_bamount, str_bmoney, str_ps;

        //등록 버튼 클릭시
        private void button4_Click(object sender, EventArgs e)
        {

            try
            {
                int f_id;
                string f_name;
                string f_model;
                string f_date;
                string f_money;
                int f_amount;
                string f_place;

                f_id = int.Parse(tb_id.Text.ToString());
                f_name = tb_name.Text.ToString();
                f_model = tb_model.Text.ToString();
                f_date = tb_date.Text.ToString();
                f_money = tb_money.Text.ToString();
                f_amount = int.Parse(tb_amount.Text.ToString());
                f_place = tb_place.Text.ToString();

                if (f_amount > 0)
                {


                    //쿼리문을 준비
                    String sql = "INSERT INTO output(공장순번,장비명,모델명,생산일자,생산량,생산금액,장소)VALUES(" + f_id + ",'" + f_name + "','" + f_model + "', '" + f_date + "'," + f_amount + ",'" + f_money + "','" + f_place + "' );";
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery(); //insert, update등 실행갯수 반환
                    MessageBox.Show("입력 성공~");

                    String sql1 = "SELECT 공장순번,장비명,모델명,생산일자,생산량,생산금액,장소 FROM output ORDER BY 공장순번 DESC LIMIT 1";
                    cmd.CommandText = sql1; //트럭에 짐 싣기
                    MySqlDataReader reader; //짐을 연결한 끈, 서버에서 데이터를 가져오도록 실행
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {

                        str_id = reader["공장순번"].ToString();
                        str_name = reader["장비명"].ToString();
                        str_model = reader["모델명"].ToString();
                        str_date = reader["생산일자"].ToString();
                        str_amount = reader["생산량"].ToString();
                        str_money = reader["생산금액"].ToString();
                        str_place = reader["장소"].ToString();


                        ListViewItem lvi = new ListViewItem(new string[] { str_id, str_name, str_model, str_date, str_amount, str_money, str_place });
                        ls_view.Items.Add(lvi); chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;

                        chart1.Series[0].LegendText = "100초과는 파랑";
                        int id = int.Parse(reader["공장순번"].ToString());
                        int amount = int.Parse(reader["생산량"].ToString());
                        chart1.Series[0].Points.AddXY(id, amount);

                        chart1.ChartAreas[0].AxisX.Minimum = 0;
                        //chart1.Series[0].Color = Color.Red; // 전체 차트 색상을 전부 변경시킴

                        if (amount > 100)
                        {
                            chart1.Series[0].Points[id - 1].Color = Color.CornflowerBlue;
                        }
                        else
                        {
                            chart1.Series[0].Points[id - 1].Color = Color.IndianRed;
                        }


                    }
                    reader.Close();
                }
                else
                {
                    MessageBox.Show("수량은 0이하일 수 없습니다.");
                }
            
            }
            catch (MySqlException)
            {
                MessageBox.Show("죄송합니다. 공장순번은 같을 수 없습니다.\n다시입력 부탁드립니다.");
            }

        }

        //폼 시작시
        private void Form2_Load(object sender, EventArgs e)
        {
            gb1.Show();
            menuStrip1.Visible = true;
            menuStrip2.Visible = false;
            String connStr = "Server=192.168.100.56;Port=3306;Uid=pjuser;Pwd=1234;Database=project;CHARSET=UTF8";
            conn = new MySqlConnection(connStr);
            conn.Open();
            cmd = new MySqlCommand("", conn);

            this.button10.BackColor = Color.CornflowerBlue;
            this.button10.ForeColor = Color.White;
            this.button5.BackColor = Color.Gainsboro;
            this.button5.ForeColor = Color.Black;

            timer1.Interval = 1000;
            timer1.Enabled = true;
            timer1.Tick += timer1_Tick;

            label15.Text = DateTime.Now.ToString();
            label15.TextAlign = ContentAlignment.MiddleCenter;



            String sql = "SELECT 공장순번,장비명,모델명,생산일자,생산금액,생산량,장소 FROM output";
            cmd.CommandText = sql; 
            MySqlDataReader reader; 
            reader = cmd.ExecuteReader();
            chart1.Series[0].Points.Clear();

            int index = 0;
            while (reader.Read())
            {

                str_id = reader["공장순번"].ToString();
                str_amount = reader["생산량"].ToString();
                int id = int.Parse(reader["공장순번"].ToString());
                int amount = int.Parse(reader["생산량"].ToString());
                str_name = reader["장비명"].ToString();
                str_model = reader["모델명"].ToString();
                str_date = reader["생산일자"].ToString();
                str_money = reader["생산금액"].ToString();
                str_place = reader["장소"].ToString();


                chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                
                chart1.Series[0].LegendText = "100초과는 파랑";
                chart1.Series[0].Points.AddXY(id, amount);


                chart1.ChartAreas[0].AxisX.Minimum = 0;
                chart1.Series[0].Color = Color.CornflowerBlue;

                if (amount <= 100)
                {
                    chart1.Series[0].Points[index].Color = Color.IndianRed;
                }

                index++;
            }
            reader.Close();

        }


        //폼 닫을시
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Close();
            f1.Close();
        }

        //생산량 리스트뷰 한줄 선택하게 하기
        private void ls_view_Click(object sender, EventArgs e)
        {
            if (ls_view.SelectedItems.Count > 0)
            {
                tb_id.Text = ls_view.SelectedItems[0].SubItems[0].Text;
                tb_name.Text = ls_view.SelectedItems[0].SubItems[1].Text;
                tb_model.Text = ls_view.SelectedItems[0].SubItems[2].Text;
                tb_date.Text = ls_view.SelectedItems[0].SubItems[3].Text;
                tb_amount.Text = ls_view.SelectedItems[0].SubItems[4].Text;
                tb_money.Text = ls_view.SelectedItems[0].SubItems[5].Text;
                tb_place.Text = ls_view.SelectedItems[0].SubItems[6].Text;


            }

        }

        //생산량 추출 버튼 클릭시
        private void button2_Click(object sender, EventArgs e)
        {
            ListViewItem lvi;
            ls_view.Items.Clear(); //폼의 listview 초기화
                                   //쿼리문 준비
            String sql = "SELECT 공장순번,장비명,모델명,생산일자,생산금액,생산량,장소 FROM output";
            cmd.CommandText = sql; //트럭에 짐 싣기
            MySqlDataReader reader; //짐을 연결한 끈, 서버에서 데이터를 가져오도록 실행
            reader = cmd.ExecuteReader();
            //끈 당기기
            while (reader.Read())
            {
                str_id = reader["공장순번"].ToString();
                str_name = reader["장비명"].ToString();
                str_model = reader["모델명"].ToString();
                str_date = reader["생산일자"].ToString();
                str_amount = reader["생산량"].ToString();
                str_money = reader["생산금액"].ToString();
                str_place = reader["장소"].ToString();

                lvi = new ListViewItem(new string[] { str_id, str_name, str_model, str_date,  str_amount, str_money, str_place });
                ls_view.Items.Add(lvi);
            }
            reader.Close();

        }

        //생산량 취소 버튼
        private void button1_Click(object sender, EventArgs e)
        {
            tb_id.Text = "";
            tb_name.Text = "";
            tb_model.Text = "";
            tb_date.Text = "";
            tb_money.Text = "";
            tb_amount.Text = "";
            tb_place.Text = "";
            ls_view.Items.Clear();
        }

        //생산량 삭제 버튼
        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("삭제하시겠습니까?", "삭제여부",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                str_id = ls_view.SelectedItems[0].SubItems[0].Text;
                str_name = ls_view.SelectedItems[0].SubItems[1].Text;
                str_model = ls_view.SelectedItems[0].SubItems[2].Text;
                str_date = ls_view.SelectedItems[0].SubItems[3].Text;
                str_money = ls_view.SelectedItems[0].SubItems[4].Text;
                str_amount = ls_view.SelectedItems[0].SubItems[5].Text;
                str_place = ls_view.SelectedItems[0].SubItems[6].Text;

                String sql = "DELETE FROM output WHERE 공장순번 ='" + str_id + "' && 장비명 = '" + str_name + "'";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                ls_view.SelectedItems[0].Remove();

                String sql1 = "SELECT 공장순번,장비명,모델명,생산일자,생산금액,생산량,장소 FROM output";
                cmd.CommandText = sql1;
                MySqlDataReader reader; 
                reader = cmd.ExecuteReader();
                chart1.Series[0].Points.Clear();

                int index = 0;
                while (reader.Read())
                {

                    int id = int.Parse(reader["공장순번"].ToString());
                    int amount = int.Parse(reader["생산량"].ToString());
                    str_name = reader["장비명"].ToString();
                    str_model = reader["모델명"].ToString();
                    str_date = reader["생산일자"].ToString();
                    str_money = reader["생산금액"].ToString();
                    str_place = reader["장소"].ToString();


                    chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                    //chart1.Series[0].Points.Clear();
                    chart1.Series[0].LegendText = "100이하는  빨강";
                    chart1.Series[0].Points.AddXY(id, amount);

                    chart1.ChartAreas[0].AxisX.Minimum = 0;
                    chart1.Series[0].Color = Color.IndianRed;

                    if (amount > 100)
                    {
                        chart1.Series[0].Points[index].Color = Color.CornflowerBlue;
                    }

                   
                    index++;
                }
                reader.Close();
                tb_id.Text = "";
                tb_name.Text = "";
                tb_model.Text = "";
                tb_date.Text = "";
                tb_money.Text = "";
                tb_amount.Text = "";
                tb_place.Text = "";
            }
        }

        //파일 저장 메뉴 클릭시
        private void 파일저장ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile(ls_view);
        }

        //파일 불러오기 버튼 클릭시
        private void 불러오기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BringFile(ls_view); 
        }

        private void BringFile(ListView listview)
        {
            openFileDialog1.InitialDirectory = @"C:\"; //열기 창이 표시될 때 나타날 디렉토리지정
            openFileDialog1.Title = "열기";
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "Text Files (*.txt)|*.txt|모든파일 (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            if (openFileDialog1.ShowDialog() != DialogResult.Cancel)
            {
                System.IO.StreamReader objFileRead = new System.IO.StreamReader(openFileDialog1.FileName);

                string strCard;
                strCard = objFileRead.ReadLine();
                while (strCard != null) //텍스트파일의 텍스트들을 ' '띄어쓰기로 구분해서 서브아이템에 하나씩 넣어줌
                {
                    string[] str1 = strCard.Split('|');
                    ListViewItem total = new ListViewItem(str1[0]);
                    for (int i = 1; i <= 7; i++)
                    {
                        total.SubItems.Add(str1[i]);
                    }

                    listview.Items.Add(total);
                    strCard = objFileRead.ReadLine();
                }
                objFileRead.Close();
                objFileRead.Dispose();
            }
        }

        //불량품 삭제 버튼 클릭시
        private void button7_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("삭제하시겠습니까?", "삭제여부",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                str_bid = ls_view2.SelectedItems[0].SubItems[0].Text;
                str_bname = ls_view2.SelectedItems[0].SubItems[1].Text;
                str_bmodel = ls_view2.SelectedItems[0].SubItems[2].Text;
                str_bdate = ls_view2.SelectedItems[0].SubItems[3].Text;
                str_bamount = ls_view2.SelectedItems[0].SubItems[4].Text;
                str_ps = ls_view2.SelectedItems[0].SubItems[5].Text;
                str_bmoney = ls_view2.SelectedItems[0].SubItems[6].Text;

                String sql = "DELETE FROM bad WHERE 불량품순번 ='" + str_bid + "' && 장비명 = '" + str_bname + "'";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                ls_view2.SelectedItems[0].Remove();

                String sql1 = "SELECT 불량품순번,장비명,모델명,생산일자,불량수량,불량률,손실금액 FROM bad";
                cmd.CommandText = sql1; 
                MySqlDataReader reader; 
                reader = cmd.ExecuteReader();
                chart2.Series[0].Points.Clear();

                int index = 0;
                while (reader.Read())
                {

                    int bid = int.Parse(reader["불량품순번"].ToString());
                    int bamount = int.Parse(reader["불량수량"].ToString());
                    str_bname = reader["장비명"].ToString();
                    str_bmodel = reader["모델명"].ToString();
                    str_bdate = reader["생산일자"].ToString();
                    str_bmoney = reader["손실금액"].ToString();
                    str_ps = reader["불량률"].ToString();

                    chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                    
                    chart2.Series[0].LegendText = "100이상은 빨강";
                    chart2.Series[0].Points.AddXY(bid, bamount);

                    chart2.ChartAreas[0].AxisX.Minimum = 0;

                    chart2.Series[0].Color = Color.IndianRed;

                    if (bamount < 100)
                    {
                        chart2.Series[0].Points[index].Color = Color.CornflowerBlue;
                    }

                    
                    index++;

                }
                reader.Close();
                tb_bid.Text = "";
                tb_bname.Text = "";
                tb_bmodel.Text = "";
                tb_bdate.Text = "";
                tb_bmoney.Text = "";
                tb_bamount.Text = "";
                tb_ps.Text = "";
                
            }
        }

        //Search클릭시
        private void button11_Click(object sender, EventArgs e)
        {
            ListViewItem lvi;
            ls_view2.Items.Clear();

            string std_bname = tb_bname.Text.ToString();

            String sql = "SELECT * FROM bad where 장비명 ='" + std_bname + "'";
            cmd.CommandText = sql;
            MySqlDataReader reader;
            reader = cmd.ExecuteReader();

            //끈 당기기
            while (reader.Read())
            {
                str_bid = reader["불량품순번"].ToString();
                str_bname = reader["장비명"].ToString();
                str_bmodel = reader["모델명"].ToString();
                str_bdate = reader["생산일자"].ToString();
                str_bamount = reader["불량수량"].ToString();
                str_ps = reader["불량률"].ToString();
                str_bmoney = reader["손실금액"].ToString();

                lvi = new ListViewItem(new string[] { str_bid, str_bname, str_bmodel, str_bdate, str_bamount, str_ps,str_bmoney });
                ls_view2.Items.Add(lvi).Selected = true;
            }
            reader.Close();


        }


        //메뉴스트립1 도움말 클릭시
        private void 도움말ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3(this);
            f3.Show();
        }

        //현재시간 타이머
        private void timer1_Tick(object sender, EventArgs e)
        {
            label15.Text = DateTime.Now.ToString();
        }

        //메뉴스트립1 엑셀저장
        private void 엑셀저장ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (this.ls_view.Items.Count != 0)
            {
                this.saveFileDialog1.Filter = "엑셀 파일(*.xlsx) | *.xlsx";
                if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    _strFilePath = this.saveFileDialog1.FileName;
                    ExcelFileSave();
                }
            }
        }

        //메뉴스트립1 엑셀파일저장 함수
        private void ExcelFileSave()
        {
            Excel.Application application;
            Excel.Workbook workbook;
            Excel.Worksheet workSheet;

            string[,] data;

            application = new Excel.Application();
            workbook = application.Workbooks.Add(true);
            workSheet = (Excel.Worksheet)workbook.Sheets[1];

            int nRow = this.ls_view.Items.Count + 1;
            int nCol = 7;

            data = new string[nRow, nCol];
            data[0, 0] = "공장순번";
            data[0, 1] = "장비명";
            data[0, 2] = "모델명";
            data[0, 3] = "생산일자";
            data[0, 4] = "생산량";
            data[0, 5] = "생산금액";
            data[0, 6] = "장소";

            for (int i = 0; i < this.ls_view.Items.Count; ++i)
            {
                for (int j = 0; j < this.ls_view.Items[i].SubItems.Count; ++j)
                {
                    data[i + 1, j] = this.ls_view.Items[i].SubItems[j].Text;
                }
            }

            string EndCell = "G" + nRow.ToString();
            workSheet.Range["A1:" + EndCell].Value = data;
            workbook.SaveAs(_strFilePath, workbook.FileFormat, Type.Missing, Type.Missing, false, false,
                Excel.XlSaveAsAccessMode.xlShared, false, false, Type.Missing, Type.Missing, Type.Missing);
            workbook.Close(false, Type.Missing, Type.Missing);
            application.Quit();

        }

        //메뉴스트립1 엑셀 불러오기
        private void 엑셀불러오기ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            BringExcel(ls_view);
            //쿼리문을 준비
        }

        //엑셀불러오기
        private void BringExcel(ListView listview)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (string strInfo in this.openFileDialog1.FileNames)
                {
                    _fileInfo = new FileInfo(strInfo);
                    Excel.Application application = new Excel.Application();
                    Excel.Workbook workbook = application.Workbooks.Open(strInfo);
                    Excel.Worksheet worksheet1 = workbook.Worksheets.get_Item(1);
                    application.Visible = false;

                    Excel.Range range = worksheet1.UsedRange;

                    for (int row = 2; row <= range.Rows.Count; row++) // Excel 파일 row 2번 항목부터 시작
                    {
                        string[] item = new string[range.Columns.Count];
                        for (int column = 0; column < range.Columns.Count; column++) //가져온 열 만큼 반복
                        {
                            // 배열 인덱스는 0부터 시작하므로 colum은 0부터
                            item[column] = (string)(range.Cells[row, column + 1] as Excel.Range).Value2; //셀 데이터 가져옴
                        }
                        ListViewItem lvi = new ListViewItem(item);
                        listview.Items.Add(lvi);
                    }

                    
                }
            }



        }

        //메뉴스트립1 파일 사이즈 구하기
        private string GetFileSize(double byteLength)
        {
            
            string rtSize = "";
            if (byteLength >= 1073741824.0)
            {
                rtSize = String.Format("{0:##.##}", byteLength / 1073741824.0) + " GB";
            }
            else if (byteLength >= 1048576.0)
            {
                rtSize = String.Format("{0:##.##}", byteLength / 1048576.0) + " MB";
            }
            else if (byteLength >= 1024.0)
            {
                rtSize = String.Format("{0:##.##}", byteLength / 1024.0) + " KB";

            }
            else
            {
                rtSize = byteLength.ToString() + "Bytes";
            }
            return rtSize;
        }

        private void 파일저장ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFile(ls_view2);
        }

        //파일저장 함수
        private void SaveFile(ListView listview)
        {
            saveFileDialog1.Title = "저장";
            saveFileDialog1.Filter = "Text Files (*.txt)|*.txt";  //텍스트파일로 저장
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.OverwritePrompt = true;     //덮어쓰기
            if (saveFileDialog1.ShowDialog() != DialogResult.Cancel)
            {
                System.IO.StreamWriter objFile = new System.IO.StreamWriter(saveFileDialog1.FileName);
                //리스트뷰의 아이템 개수 만큼 for문 실행
                for (int intCounter = 0; intCounter <= listview.Items.Count - 1; intCounter++)
                {
                    // 서브아이템 개수 만큼 for문 실행,인적사항들 사이에 ' '띄어쓰기로 구분시킴
                    for (int i = 0; i <= listview.Items[intCounter].SubItems.Count - 1; i++)
                    {
                        objFile.Write(Convert.ToString(listview.Items[intCounter].SubItems[i].Text));
                        objFile.Write('|');
                    }
                    objFile.WriteLine();
                }
                objFile.Close();
                objFile.Dispose();
            }
        }

        // 불량품 텍스트파일 불러오기
        private void 파일저장ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            BringFile(ls_view2);
        }

        //메뉴스트립2 엑셀저장 클릭 시
        private void 엑셀저장ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.ls_view2.Items.Count != 0)
            {
                this.saveFileDialog1.Filter = "엑셀 파일(*.xlsx) | *.xlsx";
                if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    _strFilePath = this.saveFileDialog1.FileName;
                }
            }


            Excel.Application application;
            Excel.Workbook workbook;
            Excel.Worksheet workSheet;

            string[,] data;

            application = new Excel.Application();
            workbook = application.Workbooks.Add(true);
            workSheet = (Excel.Worksheet)workbook.Sheets[1];

            int nRow = this.ls_view2.Items.Count + 1;
            int nCol = 7;

            data = new string[nRow, nCol];
            data[0, 0] = "불량순번";
            data[0, 1] = "장비명";
            data[0, 2] = "모델명";
            data[0, 3] = "생산일자";
            data[0, 4] = "불량품량";
            data[0, 5] = "불량률";
            data[0, 6] = "손실금액";
            
            for (int i = 0; i < this.ls_view2.Items.Count; ++i)
            {
                for (int j = 0; j < this.ls_view2.Items[i].SubItems.Count; ++j)
                {
                    data[i + 1, j] = this.ls_view2.Items[i].SubItems[j].Text;
                }
            }

            string EndCell = "G" + nRow.ToString();
            workSheet.Range["A1:" + EndCell].Value = data;
            workbook.SaveAs(_strFilePath, workbook.FileFormat, Type.Missing, Type.Missing, false, false,
                Excel.XlSaveAsAccessMode.xlShared, false, false, Type.Missing, Type.Missing, Type.Missing);
            workbook.Close(false, Type.Missing, Type.Missing);
            application.Quit();


        }



        private void 엑셀불러오기ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            BringExcel(ls_view2);
        }

        private void 도움말ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3(this);
            f3.Show();
        }

        //생산품의 모델명 클릭시
        private void button14_Click(object sender, EventArgs e)
        {
            ListViewItem lvi;
            ls_view.Items.Clear(); 

            string std_model = tb_model.Text.ToString();
            
                String sql = "SELECT * FROM output where 모델명 ='" + std_model + "'";
                cmd.CommandText = sql; 
                MySqlDataReader reader; 
                reader = cmd.ExecuteReader();

                //끈 당기기
                while (reader.Read())
                {
                    str_id = reader["공장순번"].ToString();
                    str_name = reader["장비명"].ToString();
                    str_model = reader["모델명"].ToString();
                    str_date = reader["생산일자"].ToString();
                    str_money = reader["생산금액"].ToString();
                    str_amount = reader["생산량"].ToString();
                    str_place = reader["장소"].ToString();

                    lvi = new ListViewItem(new string[] { str_id, str_name, str_model, str_date, str_money, str_amount, str_place });
                    ls_view.Items.Add(lvi).Selected =true;

                }
                reader.Close();
            

        }

        //잘못된 더블클릭
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            

        }

        //불량품 모델명 클릭시
        private void button12_Click(object sender, EventArgs e)
        {
            ListViewItem lvi;
            ls_view2.Items.Clear();

            string std_bmodel = tb_bmodel.Text.ToString();

            String sql = "SELECT * FROM bad where 모델명 ='" + std_bmodel + "'";
            cmd.CommandText = sql;
            MySqlDataReader reader;
            reader = cmd.ExecuteReader();

            //끈 당기기
            while (reader.Read())
            {
                str_bid = reader["불량품순번"].ToString();
                str_bname = reader["장비명"].ToString();
                str_bmodel = reader["모델명"].ToString();
                str_bdate = reader["생산일자"].ToString();
                str_bamount = reader["불량수량"].ToString();
                str_ps = reader["불량률"].ToString();
                str_bmoney = reader["손실금액"].ToString();

                lvi = new ListViewItem(new string[] { str_bid, str_bname, str_bmodel, str_bdate, str_bamount, str_ps, str_bmoney });
                ls_view2.Items.Add(lvi).Selected = true;
            }
            reader.Close();

        }

        //생산품의 장소 클릭시
        private void button15_Click(object sender, EventArgs e)
        {
             
            if(tb_place.Text.ToString() == ls_view2.SelectedItems[0].SubItems[6].Text)
            {
                ls_view.SelectedItems[0].SubItems[6].BackColor = Color.Red;
            } 
        }

        //생산일자 검색버튼 클릭시
        private void button13_Click(object sender, EventArgs e)
        {
            ListViewItem lvi;
            ls_view.Items.Clear();

            string std_date = tb_date.Text.ToString();

            String sql = "SELECT * FROM output where 생산일자 ='" + std_date + "'";
            cmd.CommandText = sql;
            MySqlDataReader reader;
            reader = cmd.ExecuteReader();

            //끈 당기기
            while (reader.Read())
            {
                str_id = reader["공장순번"].ToString();
                str_name = reader["장비명"].ToString();
                str_model = reader["모델명"].ToString();
                str_date = reader["생산일자"].ToString();
                str_money = reader["생산금액"].ToString();
                str_amount = reader["생산량"].ToString();
                str_place = reader["장소"].ToString();

                lvi = new ListViewItem(new string[] { str_id, str_name, str_model, str_date, str_money, str_amount, str_place });
                ls_view.Items.Add(lvi).Selected = true;
            }
            reader.Close();

        }

        //불량 리스트뷰 클릭 시
        private void ls_view2_Click(object sender, EventArgs e)
        {
            if (ls_view2.SelectedItems.Count > 0)
            {
                tb_bid.Text = ls_view2.SelectedItems[0].SubItems[0].Text;
                tb_bname.Text = ls_view2.SelectedItems[0].SubItems[1].Text;
                tb_bmodel.Text = ls_view2.SelectedItems[0].SubItems[2].Text;
                tb_bdate.Text = ls_view2.SelectedItems[0].SubItems[3].Text;
                tb_bamount.Text = ls_view2.SelectedItems[0].SubItems[4].Text;
                tb_ps.Text = ls_view2.SelectedItems[0].SubItems[5].Text;
                tb_bmoney.Text = ls_view2.SelectedItems[0].SubItems[6].Text;

            }

        }

        //불량품 생산일자 클릭시
        private void button15_Click_1(object sender, EventArgs e)
        {
            ListViewItem lvi;
            ls_view2.Items.Clear();

            string std_bdate = tb_bdate.Text.ToString();

            String sql = "SELECT * FROM bad where 생산일자 ='" + std_bdate + "'";
            cmd.CommandText = sql;
            MySqlDataReader reader;
            reader = cmd.ExecuteReader();

            //끈 당기기
            while (reader.Read())
            {
                str_bid = reader["불량품순번"].ToString();
                str_bname = reader["장비명"].ToString();
                str_bmodel = reader["모델명"].ToString();
                str_bdate = reader["생산일자"].ToString();
                str_bamount = reader["불량수량"].ToString();
                str_ps = reader["불량률"].ToString();
                str_bmoney = reader["손실금액"].ToString();

                lvi = new ListViewItem(new string[] { str_bid, str_bname, str_bmodel, str_bdate, str_bamount, str_ps, str_bmoney });
                ls_view2.Items.Add(lvi).Selected = true;
            }
            reader.Close();

        }

        //생산량 장비명 검색버튼 클릭 시
        private void button16_Click(object sender, EventArgs e)
        {
            ListViewItem lvi;
            ls_view.Items.Clear();

            string std_name = tb_name.Text.ToString();

            String sql = "SELECT * FROM output where 장비명 ='" + std_name + "'";
            cmd.CommandText = sql;
            MySqlDataReader reader;
            reader = cmd.ExecuteReader();

            //끈 당기기
            while (reader.Read())
            {
                str_id = reader["공장순번"].ToString();
                str_name = reader["장비명"].ToString();
                str_model = reader["모델명"].ToString();
                str_date = reader["생산일자"].ToString();
                str_money = reader["생산금액"].ToString();
                str_amount = reader["생산량"].ToString();
                str_place = reader["장소"].ToString();

                lvi = new ListViewItem(new string[] { str_id, str_name, str_model, str_date, str_money, str_amount, str_place });
                ls_view.Items.Add(lvi).Selected =true;

            }
            reader.Close();

        }



        //불량 버튼 클릭시
        private void button5_Click(object sender, EventArgs e)
        {
            gb1.Visible = false;
            gb2.Visible = true;
            menuStrip1.Visible = false;
            menuStrip2.Visible = true;

            this.button5.BackColor = Color.IndianRed;
            this.button5.ForeColor = Color.White;

            this.button10.BackColor = Color.Gainsboro;
            this.button10.ForeColor = Color.Black;
            

            String sql = "SELECT 불량품순번,장비명,모델명,생산일자,불량수량,불량률,손실금액 FROM bad";
            cmd.CommandText = sql; //트럭에 짐 싣기
            MySqlDataReader reader; //짐을 연결한 끈, 서버에서 데이터를 가져오도록 실행
            reader = cmd.ExecuteReader();
            chart2.Series[0].Points.Clear();

            int index = 0;
            while (reader.Read())
            {

                int bid = int.Parse(reader["불량품순번"].ToString());
                int bamount = int.Parse(reader["불량수량"].ToString());
                str_bname = reader["장비명"].ToString();
                str_bmodel = reader["모델명"].ToString();
                str_bdate = reader["생산일자"].ToString();
                str_bmoney = reader["손실금액"].ToString();
                str_ps = reader["불량률"].ToString();

                chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                //chart1.Series[0].Points.Clear();
                chart2.Series[0].LegendText = "100미만은 파랑";
                chart2.Series[0].Points.AddXY(bid, bamount);

                chart2.ChartAreas[0].AxisX.Minimum = 0;

                chart2.Series[0].Color = Color.CornflowerBlue;

                if (bamount >= 100)
                {
                    chart2.Series[0].Points[index].Color = Color.IndianRed;
                }

               
                index++;

            }
            reader.Close();

        }

        //안씀 건드리지X
        private void button10_Click(object sender, EventArgs e)
        {
           
        }

        private void gb1_Enter(object sender, EventArgs e)
        {

        }

        private void menuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void gb2_Enter(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        //생산버튼 클릭시
        private void button10_Click_1(object sender, EventArgs e)
        {
            gb1.Visible = true;
            gb2.Visible = false;

            menuStrip1.Visible = true;
            menuStrip2.Visible = false;

            this.button10.BackColor = Color.CornflowerBlue;
            this.button10.ForeColor = Color.White;
            this.button5.BackColor = Color.Gainsboro;
            this.button5.ForeColor = Color.Black;

            String sql = "SELECT 공장순번,장비명,모델명,생산일자,생산금액,생산량,장소 FROM output";
            cmd.CommandText = sql; //트럭에 짐 싣기
            MySqlDataReader reader; //짐을 연결한 끈, 서버에서 데이터를 가져오도록 실행
            reader = cmd.ExecuteReader();
            chart1.Series[0].Points.Clear();

            int index = 0;
            while (reader.Read())
            {

                int id = int.Parse(reader["공장순번"].ToString());
                int amount = int.Parse(reader["생산량"].ToString());
                str_name = reader["장비명"].ToString();
                str_model = reader["모델명"].ToString();
                str_date = reader["생산일자"].ToString();
                str_money = reader["생산금액"].ToString();
                str_place = reader["장소"].ToString();


                chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                
                chart1.Series[0].LegendText = "100초과는 파랑";
                chart1.Series[0].Points.AddXY(id, amount);

                chart1.ChartAreas[0].AxisX.Minimum = 0;
                chart1.Series[0].Color = Color.CornflowerBlue;

                if (amount <= 100)
                {
                    chart1.Series[0].Points[index].Color = Color.IndianRed;

                }

                index++;

            }
            reader.Close();

        }


        //불량률 취소 버튼
        private void button9_Click(object sender, EventArgs e)
        {
            tb_bid.Text = "";
            tb_bname.Text = "";
            tb_bmodel.Text = "";
            tb_bdate.Text = "";
            tb_bmoney.Text = "";
            tb_bamount.Text = "";
            tb_ps.Text = "";
            ls_view2.Items.Clear();
        }

        //불량률 추출 버튼
        private void button8_Click(object sender, EventArgs e)
        {
            ListViewItem lvi;
            ls_view.Items.Clear(); //폼의 listview 초기화
                                   //쿼리문 준비
            String sql = "SELECT 불량품순번,장비명,모델명,생산일자,불량수량,불량률,손실금액 FROM bad ";
            cmd.CommandText = sql; //트럭에 짐 싣기
            MySqlDataReader reader; //짐을 연결한 끈, 서버에서 데이터를 가져오도록 실행
            reader = cmd.ExecuteReader();

  
            //끈 당기기
            while (reader.Read())
            {
                str_bid = reader["불량품순번"].ToString();
                str_bname = reader["장비명"].ToString();
                str_bmodel = reader["모델명"].ToString();
                str_bdate = reader["생산일자"].ToString();
                str_bmoney = reader["불량수량"].ToString();
                str_bamount = reader["불량률"].ToString();
                str_ps = reader["손실금액"].ToString();


                lvi = new ListViewItem(new string[] { str_bid, str_bname, str_bmodel, str_bdate, str_bmoney, str_bamount, str_ps});
                ls_view2.Items.Add(lvi);
            }
            reader.Close();

        }

        //불량률 등록 버튼 클릭시
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
            int f_bid;
            string f_bname;
            string f_bmodel;
            string f_bdate;
            int f_bamount;
            string f_ps;
            string f_bmoney;
            

            f_bid = int.Parse(tb_bid.Text.ToString());
            f_bname = tb_bname.Text.ToString();
            f_bmodel = tb_bmodel.Text.ToString();
            f_bdate = tb_bdate.Text.ToString();
            f_bamount = int.Parse(tb_bamount.Text.ToString());
            f_ps = tb_ps.Text.ToString();
            f_bmoney = tb_bmoney.Text.ToString();

                if (f_bamount > 0 )
                {
                    //쿼리문을 준비
                    String sql = "INSERT INTO bad(불량품순번,장비명,모델명,생산일자,불량수량,불량률,손실금액)VALUES(" + f_bid + ",'" + f_bname + "','" + f_bmodel + "', '" + f_bdate + "'," + f_bamount + ",'" + f_ps + "','" + f_bmoney + "' );";
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("입력 성공~");

                    String sql1 = "SELECT 불량품순번,장비명,모델명,생산일자,불량수량,불량률,손실금액 FROM bad ORDER BY 불량품순번 DESC LIMIT 1";
                    cmd.CommandText = sql1;
                    MySqlDataReader reader;
                    reader = cmd.ExecuteReader();


                    while (reader.Read())
                    {

                        str_bid = reader["불량품순번"].ToString();
                        str_bname = reader["장비명"].ToString();
                        str_bmodel = reader["모델명"].ToString();
                        str_bdate = reader["생산일자"].ToString();
                        str_bamount = reader["불량수량"].ToString();
                        str_ps = reader["불량률"].ToString();
                        str_bmoney = reader["손실금액"].ToString();

                        ListViewItem lvi = new ListViewItem(new string[] { str_bid, str_bname, str_bmodel, str_bdate, str_bamount, str_ps, str_bmoney });
                        ls_view2.Items.Add(lvi);

                        chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;

                        chart2.Series[0].LegendText = "100미만 파랑";
                        int bid = int.Parse(reader["불량품순번"].ToString());
                        int bamount = int.Parse(reader["불량수량"].ToString());
                        chart2.Series[0].Points.AddXY(bid, bamount);

                        chart2.ChartAreas[0].AxisX.Minimum = 0;


                        if (bamount < 100)
                        {
                            chart2.Series[0].Points[bid - 1].Color = Color.CornflowerBlue;
                        }
                        else
                        {
                            chart2.Series[0].Points[bid - 1].Color = Color.IndianRed;
                        }

                    }
                    reader.Close();
                }
                else
                {
                    MessageBox.Show("수량과 불량률은 0이하일 수 없습니다.");

                }
            
            }
            catch (MySqlException)
            {
                MessageBox.Show("죄송합니다. 불량품순번은 같을 수 없습니다.\n다시입력 부탁드립니다.");
            }
        }
    }
}
