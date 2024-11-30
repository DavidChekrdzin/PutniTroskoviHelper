using Microsoft.EntityFrameworkCore;
using Putni_Troskovi_App.Model;
using System.Collections.Generic;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Putni_Troskovi_App
{
    public partial class Form1 : Form
    {
        private AppDbContext? _context;
        List<CheckBox> checkBoxList;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public void getSetDaysInMonth(int month, bool isPrestupna)//sets the amount of checkboxes required for each month
        {
            int daysInMonth = 0;
            if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
            {
                daysInMonth = 31;
            }
            else if (month == 4 || month == 6 || month == 9 || month == 11)
            {
                daysInMonth = 30;
            }
            else
            {
                if (!isPrestupna)
                {
                    daysInMonth = 28;
                }
                else
                {
                    daysInMonth = 29;
                }
            }

            foreach (CheckBox checkBox in checkBoxList)
            {
                checkBox.Hide();
                checkBox.Checked = false;
            }
            for (int i = 0; i < daysInMonth; i++)
            {
                checkBoxList[i].Show();
            }
        }
        public void getSetCheckedDays(DateTime date)
        {
            List<Dan> checkedDaysOfMonth = GetCheckedDays(date);
            foreach (Dan checkedDay in checkedDaysOfMonth)
            {
                foreach (CheckBox checkBox in checkBoxList.Where(x => x.TabIndex.Equals(checkedDay.Datum.Day)))
                {
                    checkBox.Checked = true;
                }
            }
        }
        private List<Dan> GetCheckedDays(DateTime date)
        {
            List<Dan> checkedDaysOfMonth = _context.Dani.AsQueryable().Where(x => x.Datum.Year == date.Year)
                .Where(x => x.Datum.Month == date.Month).ToList();
            return checkedDaysOfMonth;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this._context = new AppDbContext();

            // Uncomment the line below to start fresh with a new database.
            //this._context.Database.EnsureDeleted();
            this._context.Database.EnsureCreated();
            this._context.Dani.Load();

            this.bindingSource1.DataSource = _context.Dani.Local.ToBindingList();

            this._context = new AppDbContext();

            checkBoxList = new List<CheckBox>();

            foreach (var checkBox in groupBox1.Controls.OfType<CheckBox>())
            {
                checkBoxList.Add(checkBox);
            }

            checkBoxList = checkBoxList.AsQueryable().OrderBy(x => x.TabIndex).ToList();

            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MM/yyyy";

            dateTimePicker1.Value = DateTime.Now;

            int month = dateTimePicker1.Value.Month;
            bool isLeapYear = DateTime.IsLeapYear(dateTimePicker1.Value.Year);



            getSetDaysInMonth(month, isLeapYear);

            getSetCheckedDays(dateTimePicker1.Value);

            calculatePrice();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        private void calculatePrice()
        {
            decimal sum = 0;
            decimal pricePerDay = numericUpDown1.Value;
            foreach (CheckBox checkBox in checkBoxList)
            {
                if (checkBox.Checked)
                {
                    sum += pricePerDay;
                }
            }
            label3.Text = sum.ToString();
        }
        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox checkBox = (CheckBox)sender;
                string date = dateTimePicker1.Value.Year.ToString() + "-" + dateTimePicker1.Value.Month.ToString() + "-" + checkBox.TabIndex.ToString();
                if (!checkBox.Checked)
                {//if unchecked
                    if (_context.Dani.Any(x => x.Datum.Year.ToString() + "-" + x.Datum.Month.ToString() + "-" + x.Datum.Day.ToString() == date))//if exists in db
                    {
                        try
                        {
                            Dan dan = _context.Dani.FirstOrDefault(x => x.Datum.Year.ToString() + "-" + x.Datum.Month.ToString() + "-" + x.Datum.Day.ToString() == date);
                            _context.Dani.Remove(dan);
                            _context.SaveChanges();
                            calculatePrice();
                            MessageBox.Show("Uspesno brisanje dana");
                        }
                        catch
                        {
                            MessageBox.Show("Greska sa brisanjem dana");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Dan ne postoji u bazi");
                    }
                }
                else//if checked
                {
                    if (!_context.Dani.Any(x => x.Datum.ToString() == date))//if doesn't exists in db
                    {
                        try
                        {
                            Dan dan = new Dan();
                            dan.Datum = DateTime.Parse(date);
                            _context.Dani.Add(dan);
                            _context.SaveChanges();
                            calculatePrice();
                            MessageBox.Show("Dan uspesno dodat!");
                        }
                        catch
                        {
                            MessageBox.Show("Greska prilikom dodavanja dana u bazu!");
                        }

                    }
                }
            }
            catch
            {
                MessageBox.Show("Error");
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            calculatePrice();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            int month = dateTimePicker1.Value.Month;
            MessageBox.Show(month.ToString());
            bool isLeapYear = DateTime.IsLeapYear(dateTimePicker1.Value.Year);

            getSetDaysInMonth(month, isLeapYear);

            getSetCheckedDays(dateTimePicker1.Value);

            calculatePrice();
        }
    }
}

