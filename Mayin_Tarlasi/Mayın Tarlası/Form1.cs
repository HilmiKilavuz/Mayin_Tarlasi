using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mayın_Tarlası
{
    public partial class Form1 : Form
    {
        private string tiklananButon;//önceki formda tıklanan buton
        private int[,] mayinMatrisi;
        private int boyut = 30; // Izgara boyutu
        private int mayinSayisi ; // Mayın sayısı
        private int acilanButonSayisi = 0;

        //nceki formdan tıklanan butonun adını aldım
        public Form1(string butonAdi)
        {
            InitializeComponent();
            tiklananButon = butonAdi;
        }
        //tıklanan butona göre mayın sayısını değiştirdim.
        private void Form1_Load(object sender, EventArgs e)
        {

           
            if (tiklananButon == "btnKolay")
            {
                mayinSayisi = 100;
              
            }
            else if (tiklananButon == "btnOrta")
            {
                mayinSayisi = 300;
            }
            else if (tiklananButon == "btnZor")
            {
                mayinSayisi = 500;
            }
            OyunuBaslat();
            timer1.Enabled = true;

        }
        //mayın tarlası için 30 a 30 luk butonları oluşturmak için metod tanımladım.
        private void MayinTarlasiOlustur()
        {
            int boyut = 30; // tablanın boyutu
            int butonBoyutu = 20; // Her bir butonun boyutu
            int ustBosluk = 100; // Üst tarafta bırakılacak boşluk miktarı
            //form boyutunu ayarladım
            this.ClientSize = new Size(boyut * butonBoyutu, boyut * butonBoyutu + ustBosluk);
            //butonları oluşturdum.
            for (int i = 0; i < boyut; i++)
            {
                for (int j = 0; j < boyut; j++)
                {
                    Button buton = new Button
                    {
                        Size = new Size(butonBoyutu, butonBoyutu),
                        Location = new Point(j * butonBoyutu, i * butonBoyutu + ustBosluk),
                        Name = $"btn_{i}_{j}"
                    };

                    buton.Click += Buton_Click;
                    buton.MouseDown += Buton_MouseDown; // Tıklama olayını bağlama
                    this.Controls.Add(buton);
                }
            }
        }
       
        //Eğer tıklanan butonun etrafında mayın yok ise komuşularında da aynı durum var ise o butonları da açmayı sağladım.
        //Açılan butonları rengini değiştirip tekrar basılmaması için enable ı false olarak ayarladım.
        private void ZincirlemeAc(int x, int y)
        {
            if (x < 0 || x >= boyut || y < 0 || y >= boyut)
                return;

            Button hedefButon = this.Controls.Find($"btn_{x}_{y}", true).FirstOrDefault() as Button;

            if (hedefButon == null || !hedefButon.Enabled)
                return;

            int komsuSayisi = KomsuMayinSayisi(x, y);

            if (komsuSayisi == 0)
            {
                hedefButon.BackColor = Color.LightGreen;
                hedefButon.Enabled = false;

                acilanButonSayisi++; // Sayaç artırılıyor
                label1.Text = $"Açılan Butonlar: {acilanButonSayisi}";

                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i != 0 || j != 0)
                            ZincirlemeAc(x + i, y + j);
                    }
                }
            }
            else
            {
                hedefButon.Text = komsuSayisi.ToString();
                hedefButon.BackColor = Color.LightYellow;
                hedefButon.Enabled = false;

                acilanButonSayisi++; // Sayaç artırılıyor
                label1.Text = acilanButonSayisi.ToString();
            }
        }
        private void TumMayinlariGoster()
        {
            for (int i = 0; i < boyut; i++) // Satırları dolaş
            {
                for (int j = 0; j < boyut; j++) // Sütunları dolaş
                {
                    if (mayinMatrisi[i, j] == 1) // Eğer hücrede mayın varsa
                    {
                        // Butonu bul
                        Button mayinButonu = this.Controls.Find($"btn_{i}_{j}", true).FirstOrDefault() as Button;

                        if (mayinButonu != null)
                        {
                            // Mayını göster
                            mayinButonu.BackColor = Color.Red; // Kırmızı arka plan
                            mayinButonu.Text = "💣"; // Mayın sembolü
                        }
                    }
                }
            }
        }


        //butonlara basıldığında oyunun kazanılıp kazanıladığını kontrol edip eğer mayın varsa butonu kırmızıya boyayıp oyunu bitiriyor.
        private void Buton_Click(object sender, EventArgs e)
        {
            Button tiklananButon = sender as Button;

            // Tıklanan butonun koordinatlarını al
            string[] koordinatlar = tiklananButon.Name.Split('_');
            int x = int.Parse(koordinatlar[1]);
            int y = int.Parse(koordinatlar[2]);

            if (mayinMatrisi[x, y] == 1) // Eğer tıklanan hücrede mayın varsa
            {
                tiklananButon.BackColor = Color.Red; // Tıklanan mayını göster
                tiklananButon.Text = "💣";

                // Tüm mayınları göster
                TumMayinlariGoster();

                MessageBox.Show("Mayına bastınız! Oyun bitti.");
                OyunSonu();
            }
            else
            {
                // Normal açılma işlemi
                ZincirlemeAc(x, y);

                // Kazanma kontrolü
                if (KazandinizMi())
                {
                    MessageBox.Show("Tebrikler, kazandınız!");
                    OyunSonu();
                }
            }
        }
        private void Buton_MouseDown(object sender, MouseEventArgs e)
        {
            Button tiklananButon = sender as Button;

            if (e.Button == MouseButtons.Right) // Sağ tık
            {
                // Eğer butonda bayrak varsa kaldır
                if (tiklananButon.Text == "🚩")
                {
                    tiklananButon.Text = ""; 
                    tiklananButon.Enabled = true; // Tekrar açılabilir hale getir
                }
                else // Bayrak koy
                {
                    tiklananButon.Text = "🚩";
                   
                }
            }
        }



        //haritaya mayınları random olarak yerleştirdim.
        private void MayinlariOlustur()
        {
            mayinMatrisi = new int[boyut, boyut];
            Random rnd = new Random();

            int yerleştirilenMayin = 0;
            while (yerleştirilenMayin < mayinSayisi)
            {
                int x = rnd.Next(0, boyut);
                int y = rnd.Next(0, boyut);

                if (mayinMatrisi[x, y] == 0) // Eğer zaten mayın yoksa
                {
                    mayinMatrisi[x, y] = 1; // Mayını yerleştir
                    yerleştirilenMayin++;
                }
            }
        }
        //tıklanan butonun komşularındaki mayın sayısını hesaplayıp mayın sayısını butona yazdırdım.
        private int KomsuMayinSayisi(int x, int y)
        {
            int sayac = 0;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int yeniX = x + i;
                    int yeniY = y + j;

                    // Izgaranın dışına çıkılmasını engelle
                    if (yeniX >= 0 && yeniX < boyut && yeniY >= 0 && yeniY < boyut)
                    {
                        if (mayinMatrisi[yeniX, yeniY] == 1) // Mayın varsa sayacı artır
                        {
                            sayac++;
                        }
                    }
                }
            }

            return sayac;
        }

      

        //açılacak buton kamadığı zaman oyunu bitirip kazanmayı sağladım.
        private bool KazandinizMi()
        {
            int acilanHucresayisi = 0;

            foreach (Control kontrol in this.Controls)
            {
                if (kontrol is Button buton && buton.Enabled == false)
                {
                    acilanHucresayisi++;
                }
            }

            return acilanHucresayisi == (boyut * boyut - mayinSayisi);
        }
        //oyunu başlatan ve gerekli metodları çağıran meod tanımladım.
        private void OyunuBaslat()
        {
           
            MayinTarlasiOlustur();
            MayinlariOlustur();
        }

     //tekrar butonuna tıklandığında girişe göderdim.

        private void btnTekrar_Click(object sender, EventArgs e)
        {
            Giris giris = new Giris();
            giris.Show();
            this.Hide();

        }
        //timer ile süreyi ayarladım.
        private void timer1_Tick(object sender, EventArgs e)
        {
            int süre;
            süre = Convert.ToInt32(label2.Text) ;
            süre++;
            label2.Text = süre.ToString();
        }

      
       //kullanıcı yandığı zaman bütün butonları devre dışı bıraktım. 
        private void OyunSonu()
        {

            foreach (Control kontrol in this.Controls)
            {
                if (kontrol is Button buton && buton.Name.StartsWith("btn_"))
                {
                    buton.Enabled = false; // Tüm butonları devre dışı bırak
                }
            }
        }


    }
}
