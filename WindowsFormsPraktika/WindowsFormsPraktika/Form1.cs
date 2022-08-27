using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using System.Linq;
using System.Text;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Chrome;

namespace WindowsFormsPraktika
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private double converter(string name1, double name2)
        {
            string[] mas1 = null;
            mas1 = name1.Split('.');
            string mas2 = null;
            mas2 = mas1[0] + "," + mas1[1];
            name2 = Convert.ToDouble(mas2);
            return name2;
        }

        int flag1 = 0;
        int flag2 = 0;
        int Length = 0;
        int error = 0;

        private void gMapControl1_Load(object sender, EventArgs e)
        {
            gMapControl1.Bearing = 0;
            gMapControl1.CanDragMap = true;
            gMapControl1.DragButton = MouseButtons.Left;
            gMapControl1.GrayScaleMode = true;
            gMapControl1.MarkersEnabled = true;

            gMapControl1.MaxZoom = 18;
            gMapControl1.MinZoom = 2;
            gMapControl1.MouseWheelZoomType =
                GMap.NET.MouseWheelZoomType.MousePositionAndCenter;

            gMapControl1.NegativeMode = false;
            gMapControl1.PolygonsEnabled = true;
            gMapControl1.RoutesEnabled = true;
            gMapControl1.ShowTileGridLines = false;
            gMapControl1.Zoom = 18;
            gMapControl1.MapProvider =
                GMap.NET.MapProviders.GMapProviders.GoogleMap;
            GMap.NET.GMaps.Instance.Mode =
                GMap.NET.AccessMode.ServerOnly;

            GMap.NET.MapProviders.GMapProvider.WebProxy =
                System.Net.WebRequest.GetSystemWebProxy();
            GMap.NET.MapProviders.GMapProvider.WebProxy.Credentials =
                System.Net.CredentialCache.DefaultCredentials;

            gMapControl1.Position = new GMap.NET.PointLatLng(55.75393, 37.620795);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (flag1 == 0)
            {
                string[] dataValues = null;
                string[] latitude_longitude = null;
                string[] data = null;
                try
                {
                    data = File.ReadAllLines(Application.StartupPath + @"\data-20160311T1245-structure-20160311T1245.csv");
                    error = 0;
                }
                catch (Exception)
                {
                    error = 1;
                    MessageBox.Show($"Ошибка запуска файла");
                }
                if (error != 1)
                {
                    Length = data.Length;


                    for (int i = 0; i < data.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(data[i]))
                        {
                            char[] splitchar = { '"' };
                            dataValues = data[i].Split(splitchar);
                            latitude_longitude = data[i].Split(',');
                            dataGridView1.ColumnCount = 7;
                            dataGridView1.RowCount = data.Length;
                            int z = 1;
                            for (int j = 0; j < 5; j++)
                            {
                                if (dataValues[j + z - 1] != ",,")
                                {
                                    dataGridView1[j, i].Value = dataValues[j + z];
                                }
                                else
                                {
                                    dataGridView1[j, i].Value = "нет данных";
                                    dataGridView1[j + 1, i].Value = dataValues[j + z];
                                    j = 4;
                                }
                                z++;
                            }
                            int x = 2;
                            for (int j = 5; j < 7; j++)
                            {
                                dataGridView1[j, i].Value = latitude_longitude[latitude_longitude.Length - x].Trim('"');
                                x--;
                            }
                        }
                    }
                    dataGridView1.Columns[0].Width = 200;
                    dataGridView1.Columns[1].Width = 200;
                    dataGridView1.Columns[2].Width = 200;
                    dataGridView1.Columns[4].Width = 200;
                    flag1 = 1;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (flag1 != 0)
            {
                if (flag1 != 2)
                {
                    GMap.NET.WindowsForms.GMapOverlay markersOverlay = new GMap.NET.WindowsForms.GMapOverlay(gMapControl1, "marker");
                    GMap.NET.WindowsForms.Markers.GMapMarkerGoogleRed markerG;

                    double latitude = 0;
                    double longitude = 0;
                    for (int i = 1; i < Length; i++)
                    {
                        latitude = converter(dataGridView1[5, i].Value.ToString(), latitude);
                        longitude = converter(dataGridView1[6, i].Value.ToString(), latitude);

                        markerG = new GMap.NET.WindowsForms.Markers.GMapMarkerGoogleRed(new GMap.NET.PointLatLng(latitude, longitude));
                        markerG.ToolTip = new GMap.NET.WindowsForms.ToolTips.GMapRoundedToolTip(markerG);
                        markerG.ToolTipText = dataGridView1[0, i].Value.ToString();
                        markersOverlay.Markers.Add(markerG);
                        gMapControl1.Overlays.Add(markersOverlay);
                    }
                    flag1 = 2;
                }
            }
            else
            {
                MessageBox.Show("Сначала нажмите кнопку 'Вывести данные на экран'");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (flag1 == 2)
            {
                int RowIndex = dataGridView1.CurrentCell.RowIndex;
                int ColumnIndex = dataGridView1.CurrentCell.ColumnIndex;
                if (RowIndex != 0)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        dataGridView1[i, RowIndex].Selected = true;
                    }

                    double latitude = 0;
                    double longitude = 0;

                    latitude = converter(dataGridView1[5, RowIndex].Value.ToString(), latitude);
                    longitude = converter(dataGridView1[6, RowIndex].Value.ToString(), latitude);

                    gMapControl1.Zoom = 18;
                    gMapControl1.Position = new GMap.NET.PointLatLng(latitude, longitude);

                    gMapControl1.Position = new GMap.NET.PointLatLng(latitude, longitude);

                }
            }
            else
            {
                if (flag1 == 1)
                {
                    int RowIndex = dataGridView1.CurrentCell.RowIndex;
                    int ColumnIndex = dataGridView1.CurrentCell.ColumnIndex;
                    if (RowIndex != 0)
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            dataGridView1[i, RowIndex].Selected = true;
                        }

                        double latitude = 0;
                        double longitude = 0;

                        latitude = converter(dataGridView1[5, RowIndex].Value.ToString(), latitude);
                        longitude = converter(dataGridView1[6, RowIndex].Value.ToString(), latitude);

                        gMapControl1.Zoom = 18;
                        gMapControl1.Position = new GMap.NET.PointLatLng(latitude, longitude);

                        GMap.NET.WindowsForms.GMapOverlay markersOverlay = new GMap.NET.WindowsForms.GMapOverlay(gMapControl1, "marker");
                        GMap.NET.WindowsForms.Markers.GMapMarkerGoogleRed markerG;

                        markerG = new GMap.NET.WindowsForms.Markers.GMapMarkerGoogleRed(new GMap.NET.PointLatLng(latitude, longitude));
                        markerG.ToolTip = new GMap.NET.WindowsForms.ToolTips.GMapRoundedToolTip(markerG);
                        markerG.ToolTipText = dataGridView1[0, RowIndex].Value.ToString();

                        gMapControl1.Overlays.Clear();
                        markersOverlay.Markers.Add(markerG);
                        gMapControl1.Overlays.Add(markersOverlay);
                    }
                }
                else
                {
                    MessageBox.Show("Сначала нажмите кнопку 'Вывести данные на экран'");
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (flag1 != 0)
            {
                int RowIndex = dataGridView1.CurrentCell.RowIndex;
                int ColumnIndex = dataGridView1.CurrentCell.ColumnIndex;
                if (RowIndex != 0)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        dataGridView1[i, RowIndex].Selected = true;
                    }
                    string book_title = dataGridView1[4, RowIndex].Value.ToString();
                    ChromeDriver chromeDriver = new ChromeDriver();
                    chromeDriver.Navigate().GoToUrl("https://www.google.com/");
                    IWebElement Book = chromeDriver.FindElementByCssSelector(".gLFyf.gsfi");
                    Book.Click();
                    Book.SendKeys(book_title + OpenQA.Selenium.Keys.Enter);
                }
                else
                {
                    MessageBox.Show("Выберите другую строку");
                }

            }
        }
    }
}
