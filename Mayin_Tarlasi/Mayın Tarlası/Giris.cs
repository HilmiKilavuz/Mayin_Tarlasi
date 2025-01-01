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
{// bu formda kullanıcının seçtiği zorluk değerini berirledim ve basılan butona göre mayın sayısını değiştirdim.

    public partial class Giris : Form
    {
        public Giris()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            Button buton = sender as Button;
            Form1 oyun = new Form1(buton.Name);

            
           
            oyun.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Button buton = sender as Button;
            Form1 oyun = new Form1(buton.Name);
  

            oyun.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Button buton = sender as Button;
            Form1 oyun = new Form1(buton.Name);

            

            oyun.Show();
            this.Hide();
        }
    }
}
