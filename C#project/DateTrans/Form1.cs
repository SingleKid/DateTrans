using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DateTrans
{
    public partial class Form1 : Form
    {
        utc lc;
        utc cur;
        MJDTime mjd;
        GPSTime gpst;
        int doy;
        int time_differ;

        bool editing = false;
        public Form1()
        {
            InitializeComponent();
            editing = true;
            System.DateTime lcn = System.DateTime.Now;
            System.DateTime currentTime = System.DateTime.UtcNow;
            time_differ = (int)(lcn - currentTime).TotalHours;
            lc = new utc(lcn.Year - (lcn.Year / 100) * 100, lcn.Month, lcn.Day, lcn.Hour, lcn.Minute, lcn.Second);
            cur = new utc(currentTime.Year - (currentTime.Year / 100) * 100, currentTime.Month, currentTime.Day, currentTime.Hour, currentTime.Minute, currentTime.Second);
            mjd = new MJDTime(cur);
            gpst = new GPSTime(cur);
            get_doy();
            update();
            editing = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void get_doy()
        {
            doy = 0;
            for (int i = 1; i < cur.month; i++)
            {
                doy += JTime.date_amount_of_month(cur.year, i);
            }
            doy += cur.date;
        }
        private void on_modify_lc(object sender, EventArgs e)
        {
            if (editing) return;
            editing = true;
            try
            {
                lc = new utc(
                    int.Parse(textBox12.Text),
                    int.Parse(textBox13.Text),
                    int.Parse(textBox15.Text),
                    int.Parse(textBox14.Text),
                    int.Parse(textBox16.Text),
                    int.Parse(textBox17.Text)
                );
                cur = lc.offset_hour(-time_differ);
                mjd = new MJDTime(cur);
                gpst = new GPSTime(cur);
                get_doy();
                update();
                label6.Text = "";
            }
            catch(Exception exce)
            {
                label6.Text = "猪";
            }
            editing = false;
        }
        private void on_modify_utc(object sender, EventArgs e)
        {
            if (editing) return;
            editing = true;
            try
            {
                cur = new utc(
                    int.Parse(textBox1.Text),
                    int.Parse(textBox2.Text),
                    int.Parse(textBox3.Text),
                    int.Parse(textBox5.Text),
                    int.Parse(textBox7.Text),
                    int.Parse(textBox10.Text)
                );
                lc = cur.offset_hour(time_differ);

                mjd = new MJDTime(cur);
                gpst = new GPSTime(cur);
                get_doy();
                update();
                label6.Text = "";
            }
            catch (Exception exce)
            {
                label6.Text = "猪";
            }
            editing = false;
        }
        private void on_modify_gpst(object sender, EventArgs e)
        {
            if (editing) return;
            editing = true;
            try
            {
                gpst = new GPSTime(
                    int.Parse(textBox4.Text),
                    int.Parse(textBox6.Text)
                );
                mjd = new MJDTime(gpst);
                cur = new utc(mjd);
                lc = cur.offset_hour(time_differ);
                get_doy();
                update();
                label6.Text = "";
            }
            catch (Exception exce)
            {
                label6.Text = "猪";
            }
            editing = false;
        }
        private void on_modify_mjd(object sender, EventArgs e)
        {
            if (editing) return;
            editing = true;
            try
            {
                mjd = new MJDTime(
                    int.Parse(textBox8.Text),
                    double.Parse(textBox11.Text)
                );
                cur = new utc(mjd);
                gpst = new GPSTime(mjd);
                lc = cur.offset_hour(time_differ);
                get_doy();
                update();
                label6.Text = "";
            }
            catch (Exception exce)
            {
                label6.Text = "猪";
            }
            editing = false;
        }
        private void on_modify_doy(object sender, EventArgs e)
        {
            if (editing) return;
            editing = true;
            try
            {
                doy = int.Parse(textBox9.Text);
                cur.change_to_doy(doy);
                mjd = new MJDTime(cur);
                gpst = new GPSTime(mjd);
                lc = cur.offset_hour(time_differ);
                update();
                label6.Text = "";
            }
            catch (Exception exce)
            {
                label6.Text = "猪";
            }
            editing = false;
        }
       
        private void update()
        {
            // LC
            textBox12.Text = lc.year.ToString();
            textBox13.Text = lc.month.ToString();
            textBox15.Text = lc.date.ToString();
            textBox14.Text = lc.hour.ToString();
            textBox16.Text = lc.minute.ToString();
            textBox17.Text = lc.sec.ToString();

            // UTC
            textBox1.Text = cur.year.ToString();
            textBox2.Text = cur.month.ToString();
            textBox3.Text = cur.date.ToString();
            textBox5.Text = cur.hour.ToString();
            textBox7.Text = cur.minute.ToString();
            textBox10.Text = cur.sec.ToString();

            // GPST
            textBox4.Text = gpst.week.ToString();
            textBox6.Text = gpst.sec.ToString();

            // MJD
            textBox8.Text = mjd.days.ToString();
            textBox11.Text = String.Format("{0:N10}", mjd.frac_day);

            // DOY
            textBox9.Text = doy.ToString();
        }
    }
}
