using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hafizaOyunu
{
    public partial class Form1 : Form
    {
        bool tiklamayaIzinVer = false;
        PictureBox ilkTahmin = null;
        int ilkOyuncuPuan = 0;
        int ikinciOyuncuPuan = 0;
        bool ilkOyuncuSira = true;
        Random rnd = new Random();
        
        Timer tiklamaZamani = new Timer();
         int time = 5; ///////////////////////////////////////////////////////////////////////        
        
        private bool resimlerYerlestirildi = false;

        public Form1()
        {
            InitializeComponent();
            RastgeleResimleriYerlestir();
            ResimleriGizle();
            tiklamaZamani.Interval = 5000; // 5 saniyelik oyun süresi
            tiklamaZamani.Tick += ClickTimerTick;
        }

        private PictureBox[] pictureBoxes
        {
            get
            {
                return Controls.OfType<PictureBox>().ToArray();
            }
        }

        private static IEnumerable<Image> images
        {
            get
            {
                return new Image[]
                {
                    Properties.Resources.a1,
                    Properties.Resources.a2,
                    Properties.Resources.a3,
                    Properties.Resources.a4,
                    Properties.Resources.a5,
                    Properties.Resources.a6,
                    Properties.Resources.a7,
                    Properties.Resources.a8,
                    Properties.Resources.a9,
                    Properties.Resources.a10,
                    Properties.Resources.a11,
                    Properties.Resources.a12,
                    Properties.Resources.a13,
                    Properties.Resources.a14,
                    Properties.Resources.a15,
                    Properties.Resources.a16,
                    Properties.Resources.a17,
                    Properties.Resources.a18,
                    Properties.Resources.a19,
                    Properties.Resources.a20,
                };
            }
        }

        private void OyunBaslangicZamanlayici()
        {
            tiklamaZamani.Start();
            tiklamaZamani.Tick += ClickTimerTick;
           
        }

        private void ResimleriSifirla()
        {
            foreach (var pic in pictureBoxes)
            {
                pic.Tag = null;
                pic.Visible = true;
                pic.Image = Properties.Resources.a0;
            }
            time = 5;//;///////////////////////////////////////////////////////////////////////////////
        }

        private PictureBox BosYuvaGetir()
        {
            int num;
            do
            {
                num = rnd.Next(0, pictureBoxes.Count());
            } while (pictureBoxes[num].Tag != null);
            return pictureBoxes[num];
        }

        private void RastgeleResimleriYerlestir()
        {
            var availableIndexes = Enumerable.Range(0, pictureBoxes.Length).ToList();
            //Enumerable.Range fonksiyonu, belirtilen bir aralıkta ardışık bir dizi oluşturur.Enumerable.Range(0, 10)------->0, 1, 2, 3, 4, 5, 6, 7, 8, 9.
            //availableIndexes, PictureBox dizisinin indislerini içeren bir liste oluşturuyor.
            foreach (var image in images)
            {
                for (int i = 0; i < 2; i++)
                {
                    int index = availableIndexes[rnd.Next(0, availableIndexes.Count)];
                    PictureBox pic = pictureBoxes[index];
                    pic.Tag = image;
                    //Her resim için rastgele bir indis seçiliyor ve
                    //bu indis kullanılarak PictureBox'ın Tag özelliği atanıyor. 
                    //Bu, resimlerin karışık şekilde yerleştirilmesini sağlıyor.
                    availableIndexes.Remove(index);
                }
            }
        }

        private void ClickTimerTick(object sender, EventArgs e)
        {
            tiklamaZamani.Stop();

            ResimleriGizle();
            tiklamayaIzinVer = true;
            
            if (!ilkOyuncuSira)
            {
                //MessageBox.Show("Sıra birinci oyuncuda.");
                OyuncuSiraDegistir();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (!tiklamayaIzinVer) return;
            var pic = (PictureBox)sender;

            if (ilkTahmin == null)
            {
                ilkTahmin = pic;
                pic.Image = (Image)pic.Tag;
                return;
            }

            pic.Image = (Image)pic.Tag;

            if (pic.Image == ilkTahmin.Image && pic != ilkTahmin)
            {
                pic.Visible = ilkTahmin.Visible = false;
                
                if (ilkOyuncuSira)
                {
                    ilkOyuncuPuan++;
                    labelOyuncu1.Text = "Oyuncu 1: " + ilkOyuncuPuan + " puan";
                    OyuncuSiraDegistir();
                }
                else
                {
                    ikinciOyuncuPuan++;
                    labelOyuncu2.Text = "Oyuncu 2: " + ikinciOyuncuPuan + " puan";
                    OyuncuSiraDegistir();
                }
    
                ResimleriGizle();
               
            }
            else//eğer eşleşme yoksa, sadece sıra değişimi olacak.
            {
                tiklamayaIzinVer = false;
                tiklamaZamani.Start();
                OyuncuSiraDegistir();
            }

            ilkTahmin = null;
            if (!pictureBoxes.Any(p => p.Visible))
            {
                if (ilkOyuncuPuan > ikinciOyuncuPuan)
                    MessageBox.Show("Tebrikler, Oyuncu 1 kazandı!");
                else if (ikinciOyuncuPuan > ilkOyuncuPuan)
                    MessageBox.Show("Tebrikler, Oyuncu 2 kazandı!");
                else
                    MessageBox.Show("Oyun berabere!");
                ResimleriSifirla();
            }
        }

        private void OyunuBaslat(object sender, EventArgs e)
        {
            tiklamayaIzinVer = true;
            if (!resimlerYerlestirildi)
            {
                RastgeleResimleriYerlestir();
                ResimleriGizle();
                resimlerYerlestirildi = true;
            }
            OyunBaslangicZamanlayici();
            tiklamaZamani.Interval = 1000;
            tiklamaZamani.Tick += ClickTimerTick;
            button1.Enabled = false;
        }

        private void ResimleriGizle()
        {
            foreach (var pic in pictureBoxes)
            {
                if (pic.Visible)
                    pic.Image = Properties.Resources.a0;
            }
        }

        private void OyuncuSiraDegistir()
        {
            if (ilkOyuncuSira)
            {
                labelSIRA.Text = "Sıra Birinci Oyuncuda";
                ilkOyuncuSira = false;
            }
            else
            {
                labelSIRA.Text = "Sıra İkinci Oyuncuda";
                ilkOyuncuSira = true;
            }

        }
    }
}