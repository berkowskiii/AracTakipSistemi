using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //created by berkowski -_*
        OleDbCommand komut = new OleDbCommand();
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=arac.mdb");
        string giris_saati;
        string cikis_saati;
        int id = 0;
        void goruntule()//veritabanından verileri çekip listviewe ekliyoruz sonra listviewde görüntülüyoruz
        {
            listView1.Items.Clear();
            baglanti.Open();
            komut.Connection = baglanti;
            komut.CommandText=("Select * From araclar");
            OleDbDataReader oku = komut.ExecuteReader();
            while (oku.Read())
            {
                ListViewItem ekle = new ListViewItem();
                ekle.Text = oku["Kimlik"].ToString();
                ekle.SubItems.Add(oku["ad_soyad"].ToString());
                ekle.SubItems.Add(oku["plaka"].ToString());
                ekle.SubItems.Add(oku["tarih"].ToString());
                ekle.SubItems.Add(oku["giris_saati"].ToString());
                ekle.SubItems.Add(oku["cikis_saati"].ToString());
                listView1.Items.Add(ekle);
            }  
            baglanti.Close();
        }
        void arac()//arabaların plakalarının veri tabanından comboboxa çekip ekliyoruz
        {
            comboBox1.Items.Clear();
            baglanti.Open();
            komut.Connection=baglanti;
            komut.CommandText=("select * from arabalar");
            OleDbDataReader rd = komut.ExecuteReader();
            while (rd.Read())
            {
                comboBox1.Items.Add(rd["arac_plaka"]);
            }
            baglanti.Close();
        }
        void kayitekle()//veri tabanına kayıt ekleme
        {    
            baglanti.Open();
            komut.Connection = baglanti;
            komut = new OleDbCommand("insert into araclar (ad_soyad,plaka,tarih,giris_saati,cikis_saati) values ('" + textBox1.Text + "','" + comboBox1.SelectedItem + "','" + dateTimePicker1.Text + "','" + giris_saati + "','" + maskedTextBox1.Text + "')", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            goruntule();
        }
        void guncelle(string alan,string saat)//listviewde ve veri tabanında saat güncelleme
        {
            
            baglanti.Open();
            komut.Connection = baglanti;
            komut = new OleDbCommand("UPDATE araclar SET " + alan + "='" + saat + "'" + "WHERE Kimlik=" + id, baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            goruntule();
        }
        void yeniaracekle()//arabaların plakalarının bulunduğu veri tabanına yeni araç plakası ekliyoruz
        {
            baglanti.Open();
            komut.Connection = baglanti;
            komut = new OleDbCommand("insert into arabalar(arac_plaka)values('" + textBox2.Text + "')", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
        }
        void filtre()//listviewde istediğimiz tarihi seçerek o tarihe ait kayıtları veritabanından çekiyoruz
        {
            listView1.Items.Clear();
            baglanti.Open();
            komut.Connection = baglanti;
            komut.CommandText = ("(Select * From araclar where tarih='" + dateTimePicker2.Text + "')");
            OleDbDataReader oku = komut.ExecuteReader();
            while (oku.Read())
            {
                ListViewItem ekle = new ListViewItem();
                ekle.Text = oku["Kimlik"].ToString();
                ekle.SubItems.Add(oku["ad_soyad"].ToString());
                ekle.SubItems.Add(oku["plaka"].ToString());
                ekle.SubItems.Add(oku["tarih"].ToString());
                ekle.SubItems.Add(oku["giris_saati"].ToString());
                ekle.SubItems.Add(oku["cikis_saati"].ToString());
                listView1.Items.Add(ekle);
            }
            baglanti.Close();
        }
        public string kontrol()
        {
            if (maskedTextBox1.Text.Length < 5)
            {
                MessageBox.Show("Lütfen Saati Tam Doldurunuz !", "UYARI !!");
            }
            else if (maskedTextBox1.Text.Substring(1, 1) == " ")
            {
                MessageBox.Show("Lütfen Saati Tam Doldurunuz !", "UYARI !!");
            }
            else if (Convert.ToInt32(maskedTextBox1.Text.Substring(0, 2)) >= 24)
            {
                MessageBox.Show("Lütfen Doğru Aralıkta Giriniz !", "UYARI !!");
            }
            else if (Convert.ToInt32(maskedTextBox1.Text.Substring(3, 2)) >= 60)
            {
                MessageBox.Show("Lütfen Doğru Aralıkta Giriniz !", "UYARI !!");
            }
            else if (textBox1.Text == "" && comboBox1.Text == "Araç Seç")
            {
                MessageBox.Show("Lütfen 'Ad Soyad' ve 'Araç Plaka' Bilgelerini Giriniz !", "UYARI !!");
            }
            else if (comboBox1.Text == "Araç Seç")
            {
                MessageBox.Show("Lütfen 'Araç Plaka' Bilgisini Seçiniz !", "UYARI !!");

            }
            else if (textBox1.Text == "")
            {
                MessageBox.Show("Lütfen 'Ad Soyad' Bilgelerini Giriniz !", "UYARI !!");
            }
            else
            {
                maskedTextBox1.Text = maskedTextBox1.Text.Replace(' ', '0');
                return ("işlem başarılı");
            }
            return ("başarısız");
            
            
        }
        void kayit_sil()
        {
            baglanti.Open();
            komut.Connection = baglanti;
            komut = new OleDbCommand("delete from araclar where Kimlik=" + id, baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            goruntule();
        }
        void arac_sil()
        {
            comboBox1.Items.Clear();
            baglanti.Open();
            komut.Connection = baglanti;
            komut.CommandText = ("delete * from arabalar where arac_plaka='" + comboBox1.Text+"'");
            OleDbDataReader rd = komut.ExecuteReader();          
            baglanti.Close();
            comboBox1.Text = "Araç Seç";
        }
        private void button1_Click(object sender, EventArgs e)//veri eklenirken boş bırakılmaması gerekilen yerlerin kontrolünü sağlıyoruz ve bütün kontroller sağlanırsa veri ekleniyor
        {
            if (kontrol() == "işlem başarılı")
            {
                kayitekle();
            }
        }
        private void timer1_Tick(object sender, EventArgs e)//programa bilgisayarın güncel saatini yazdırıyor
        {
            label1.Text = ("Sistem Saati : " + DateTime.Now.ToLongTimeString());
        }
        private void Form1_Load(object sender, EventArgs e)//saatin çalışması için loada start veriyoruz
        {
            timer1.Start();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }
        private void button2_Click(object sender, EventArgs e)//veritabanındaki verileri listviewde görüntülemek için çağırdığımız metot
        {
            goruntule();
        }
        private void comboBox1_Click(object sender, EventArgs e)//comboboxa tıklayarak plakaların veri tabanından veri aktarımı
        {
            arac();
        }
        private void button4_Click(object sender, EventArgs e)//giriş saati ekleme
        {
            if (kontrol() == "işlem başarılı")
            {
                if (Convert.ToInt32(cikis_saati.Substring(0, 2)) < Convert.ToInt32(maskedTextBox1.Text.Substring(0, 2)))
                {
                    guncelle("giris_saati", maskedTextBox1.Text);
                    guncelle("tarih", dateTimePicker1.Text);
                    maskedTextBox1.Clear();
                }
                else if (Convert.ToInt32(cikis_saati.Substring(0, 2)) == Convert.ToInt32(maskedTextBox1.Text.Substring(0, 2)))
                {
                    if (Convert.ToInt32(cikis_saati.Substring(3, 2)) < Convert.ToInt32(maskedTextBox1.Text.Substring(3, 2)))
                    {

                        guncelle("giris_saati", maskedTextBox1.Text);
                        guncelle("tarih", dateTimePicker1.Text);
                        maskedTextBox1.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Giriş Saati Çıkış Saatinden Küçük Olamaz ", "UYARI !!");
                    }
                }
                else
                {
                    MessageBox.Show("Giriş Saati Çıkış Saatinden Küçük Olamaz ", "UYARI !!");
                }
            }
        }
        private void button5_Click(object sender, EventArgs e)//çıkış saati ekleme
        {
            if (kontrol() == "işlem başarılı")
            {
                if (Convert.ToInt32(cikis_saati.Substring(0, 2)) > Convert.ToInt32(maskedTextBox1.Text.Substring(0, 2)))
                {
                    guncelle("giris_saati", maskedTextBox1.Text);
                    guncelle("tarih", dateTimePicker1.Text);
                    maskedTextBox1.Clear();
                }
                else if (Convert.ToInt32(cikis_saati.Substring(0, 2)) == Convert.ToInt32(maskedTextBox1.Text.Substring(0, 2)))
                {
                    if (Convert.ToInt32(cikis_saati.Substring(3, 2)) > Convert.ToInt32(maskedTextBox1.Text.Substring(3, 2)))
                    {

                        guncelle("giris_saati", maskedTextBox1.Text);
                        guncelle("tarih", dateTimePicker1.Text);
                        maskedTextBox1.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Giriş Saati Çıkış Saatinden Büyük Olamaz ", "UYARI !!");
                    }
                }
                else
                {
                    MessageBox.Show("Giriş Saati Çıkış Saatinden Büyük Olamaz ", "UYARI !!");
                }
            }
        }
        
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)//listview'e tıklayarak veri güncellemesi yapıyoruz
        {
            if (listView1.SelectedItems.Count == 1)
            {
                id = Convert.ToInt32(listView1.SelectedItems[0].SubItems[0].Text);
                textBox1.Text = listView1.SelectedItems[0].SubItems[1].Text.ToString();
                comboBox1.Text=listView1.SelectedItems[0].SubItems[2].Text.ToString();
                dateTimePicker1.Text=listView1.SelectedItems[0].SubItems[3].Text.ToString();
                giris_saati = listView1.SelectedItems[0].SubItems[4].Text.ToString();
                cikis_saati = listView1.SelectedItems[0].SubItems[5].Text.ToString();
            }          
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)//yeni araç eklemek için aktifleştiriyoruz
        {
            if (checkBox1.Checked == true)
            {
                button3.Visible = true;
                textBox2.Visible = true;
                label4.Visible=true;
            }
            else
            {
                button3.Visible = false;
                textBox2.Visible = false;
                label4.Visible = false;
            }
        }
        private void button3_Click_1(object sender, EventArgs e)//araç ekleme
        {
            if (textBox2.Text == "")
            {
                MessageBox.Show("Lütfen Aracın Plakasını Giriniz ! ", "UYARI !!");
            }
            else
            {
                yeniaracekle();
                textBox2.Text = "";
            }
        }

        private void button6_Click_1(object sender, EventArgs e)//istediğimiz tarihteki verileri görmek için olan buton
        {
            filtre();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)//tarih aralığını seçmemiz için aktifleştiriyoruz
        {
            if (checkBox2.Checked == true)
            {
                button6.Visible = true;
                dateTimePicker2.Visible = true;
            }
            else
            {
                button6.Visible=false;
                dateTimePicker2.Visible=false;
                label4.Visible=false;
            }
        }

        private void button8_Click(object sender, EventArgs e)//kayıt silme
        {
            kayit_sil();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            arac_sil();
        }
    }
}

