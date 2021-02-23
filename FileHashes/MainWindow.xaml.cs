using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Cryptography;
using System.IO;

namespace FileHashes
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SHA256 sha256 = SHA256.Create();
        MD5 md5 = MD5.Create();

        string strMd5, strSha256;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void txtPath_Drop(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] f = (string[])e.Data.GetData(DataFormats.FileDrop);
                txtPath.Text = f[0];

                strMd5 = bytes2Hexstring(HashMd5(f[0]));
                md5Compare();
                strSha256 = bytes2Hexstring(HashSha256(f[0]));
                sha256Compare();

                tbMD5.Text = "MD5: " + strMd5;
                tbSHA256.Text = "SHA256: " + strSha256;
            }
        }

        private void txtPath_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }

        string bytes2Hexstring(byte[] bytes)
        {
            string output = "";
            foreach (byte b in bytes)
                output += b.ToString("x2");

            return output;
        }

        byte[] HashSha256(string filePath)
        {
            using(FileStream fs = File.OpenRead(filePath))
            {
                return sha256.ComputeHash(fs);
            }
        }

        byte[] HashMd5(string filePath)
        {
            using(FileStream fs = File.OpenRead(filePath))
            {
                return md5.ComputeHash(fs);
            }
        }

        private void txtMD5_GotFocus(object sender, RoutedEventArgs e)
        {
            if(txtMD5.Text == "paste hash to compare to here")
                txtMD5.Text = "";
        }

        private void txtMD5_TextChanged(object sender, TextChangedEventArgs e)
        {
            md5Compare();
        }

        private void txtSHA256_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSHA256.Text == "paste hash to compare to here")
                txtSHA256.Text = "";
        }

        void md5Compare()
        {
            if (txtMD5.Text == strMd5)
                txtMD5.Background = Brushes.Green;
            else
                txtMD5.Background = Brushes.IndianRed;
        }

        private void txtSHA256_TextChanged(object sender, TextChangedEventArgs e)
        {
            sha256Compare();
        }

        void sha256Compare()
        {
            if (txtSHA256.Text == strSha256)
                txtSHA256.Background = Brushes.Green;
            else
                txtSHA256.Background = Brushes.IndianRed;
        }
    }
}
