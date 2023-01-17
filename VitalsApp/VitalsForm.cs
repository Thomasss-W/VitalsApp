using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VitalsApp
{
    public partial class VitalsForm : Form
    {
        //FIELDS       
        //Define default theme colors 
        Color colorSchemeText = Color.DimGray;
        Color colorSchemeNormal = Color.Gold;
        Color colorSchemeWarning = Color.OrangeRed;
        Color colorSchemeIssue = Color.Firebrick;

        //Define constants for MAX values
        private int MaxBPSys = 180;
        private int MaxBPDia = 110;
        private int MaxGlucoseLvl = 300;



        public VitalsForm()
        {
            InitializeComponent();
        }

        private void VitalsForm_Load(object sender, EventArgs e)
        {
            //add code here
            DisplayDashboard(false);
            LoadTestData();


            ChangeApplicationResolution(); //do not remove this line of code.
        }

        private void ChangeApplicationResolution()
        {
            //Do NOT delete this method or alter this code
            int formWidth = 780;
            int formHeigth = 605;
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.Size = new System.Drawing.Size(formWidth, formHeigth);
            this.CenterToScreen();
            //this.WindowState = FormWindowState.Maximized; //maximize the screen if it's chopping off
        }



        private void btnSubmitVitals_Click(object sender, EventArgs e)
        {
            ValidateFormData();
//            if(!ValidateFormData())
//            {
//                MessageBox.Show("please enter the vaild number", "Data Error", MessageBoxButtons.OK,
//MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
//            }

            CalcHeartRate();
            CalcBMI();
            CalcGlucose();
            string sysLevelName = CalcBloodPressureSystolic();
            string diaLevelName = CalcBloodPressureDiastolic();
            SetOverallHypertensionLevel(sysLevelName, diaLevelName);
            DisplayDashboard(true);
            //GenEmergencyAlter();
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            txtAge.Text = ""; txtAge.BackColor = Color.Empty; txtAge.Focus();
            txtHeightFt.Text = ""; txtHeightFt.BackColor = Color.Empty;
            txtHeightIn.Text = ""; txtHeightIn.BackColor = Color.Empty;
            txtWeight.Text = ""; txtWeight.BackColor = Color.Empty;
            txtBPDia.Text = ""; txtBPDia.BackColor = Color.Empty;
            txtBPSys.Text = ""; txtBPSys.BackColor = Color.Empty;
            txtGlucose.Text = ""; txtGlucose.BackColor = Color.Empty;
            lblHyperNormal.ForeColor = Color.Empty;
            lblHyperPre .ForeColor = Color.Empty;
            lblHyperStage1.ForeColor = Color.Empty;
            lblHyperStage2.ForeColor = Color.Empty;


            DisplayDashboard(false);

        }

        private void picLoadTestData_Click(object sender, EventArgs e)
        {
            //bool toDisplay = false;
            DisplayDashboard(false);
            LoadTestData();
        }

        //start the rest of the methods below this line
        private void LoadTestData()
        {
            txtAge.Text = "20";
            txtHeightFt.Text = "5";
            txtHeightIn.Text = "6";
            txtWeight.Text = "170.5";
            txtBPDia.Text = "95";
            txtBPSys.Text = "110";
            txtGlucose.Text = "90";
        }

        private void DisplayDashboard(bool showPanel)
        {
            pnlBloodPressure.Visible = showPanel;
            pnlBMI.Visible = showPanel;
            pnlGlucose.Visible = showPanel;
            pnlHeartRate1.Visible = showPanel;
            pnlHeartRate2.Visible = showPanel;
        }

        private void CalcHeartRate()
        {
            const int maxHeartRate = 220;
            const int conversionRate = 6;
            double maxBPM;
            double minBPM;
            double heartBeat;
            double secBPM1;
            double secBPM2;
            double secBPMMax;


            heartBeat = maxHeartRate - Convert.ToInt32(txtAge.Text);
            minBPM = heartBeat * 0.5;
            maxBPM = heartBeat * 0.85;
            secBPM1 = minBPM / conversionRate;
            secBPM2 = maxBPM / conversionRate;
            secBPMMax = heartBeat / conversionRate;

            lblMaxHeartRate.Text = heartBeat.ToString();
            lblMinTargetZone.Text = minBPM.ToString("F0");
            lblMaxTargetZone.Text = maxBPM.ToString("F0");
            lblHeartBeats1.Text = secBPM1.ToString("F0");
            lblHeartBeats2.Text = secBPM2.ToString("F0");
            lblDoNotExceed.Text = secBPMMax.ToString("F0");
        }
        private void CalcBMI()
        {
            const int index = 703;
            const int inchConversion = 12;
            double resultBMI;
            double weight = Convert.ToDouble(txtWeight.Text);
            double inches = Convert.ToDouble(txtHeightIn.Text);
            double height = Convert.ToDouble(txtHeightFt.Text);
            double heightInches = height * inchConversion + inches;


            resultBMI = weight * index / heightInches / heightInches;


            if (resultBMI >= 40)
            {
                int anlntX = picBmiHighRisk.Location.X;
                int anlntY = picBmiHighRisk.Location.Y;
                int shiftAmt = (picBmiHighRisk.Size.Width - picIndBmi.Size.Width) / 2;
                picIndBmi.Location = new Point(anlntX + shiftAmt, anlntY + shiftAmt);
                picIndBmi.BackColor = colorSchemeIssue;
            }
            else if (resultBMI >= 30)
            {
                int anlntX = picBmiObese.Location.X;
                int anlntY = picBmiObese.Location.Y;
                int shiftAmt = (picBmiObese.Size.Width - picIndBmi.Size.Width) / 2;
                picIndBmi.Location = new Point(anlntX + shiftAmt, anlntY + shiftAmt);
                picIndBmi.BackColor = colorSchemeIssue;
            }
            else if (resultBMI >= 25)
            {
                int anlntX = picBmiOver.Location.X;
                int anlntY = picBmiOver.Location.Y;
                int shiftAmt = (picBmiOver.Size.Width - picIndBmi.Size.Width) / 2;
                picIndBmi.Location = new Point(anlntX + shiftAmt, anlntY + shiftAmt);
                picIndBmi.BackColor = colorSchemeWarning;
            }
            else if (resultBMI >= 18.5)
            {
                int anlntX = picBmiHealthy.Location.X;
                int anlntY = picBmiHealthy.Location.Y;
                int shiftAmt = (picBmiHealthy.Size.Width - picIndBmi.Size.Width) / 2;
                picIndBmi.Location = new Point(anlntX + shiftAmt, anlntY + shiftAmt);
                picIndBmi.BackColor = colorSchemeNormal;
            }
            else
            {
                int anlntX = picBmiUnder.Location.X;
                int anlntY = picBmiUnder.Location.Y;
                int shiftAmt = (picBmiUnder.Size.Width - picIndBmi.Size.Width) / 2;
                picIndBmi.Location = new Point(anlntX + shiftAmt, anlntY + shiftAmt);
                picIndBmi.BackColor = colorSchemeNormal;
            }


            lblBMI.Text = resultBMI.ToString("F1");
        }
        private void CalcGlucose()
        {
            int glucose;
            glucose = Convert.ToInt32(txtGlucose.Text);
            lblGlucose.Text = glucose.ToString();
            if (glucose >= 126)

            {
                int anIntX = picGlucoseDiabetes.Location.X;
                int anIntY = picGlucoseDiabetes.Location.Y;
                int shiftAmt = (picGlucoseDiabetes.Size.Width - picIndGlucose.Size.Width) / 2;
                picIndGlucose.Location = new Point(anIntX + shiftAmt, anIntY + shiftAmt);
                picIndGlucose.BackColor = colorSchemeIssue;
            }
            else if (glucose >= 100)
            {
                int anlntX = picGlucosePreDiabetes.Location.X;
                int anlntY = picGlucosePreDiabetes.Location.Y;
                int shiftAmt = (picGlucosePreDiabetes.Size.Width - picIndGlucose.Size.Width) / 2;
                picIndGlucose.Location = new Point(anlntX - shiftAmt, anlntY - shiftAmt);
                picIndGlucose.BackColor = colorSchemeIssue;
            }
            else if (glucose >= 70)
            {
                int anlntX = picGlucoseNormal.Location.X;
                int anlntY = picGlucoseNormal.Location.Y;
                int shiftAmt = (picGlucoseNormal.Size.Width - picIndGlucose.Size.Width) / 2;
                picIndGlucose.Location = new Point(anlntX + shiftAmt, anlntY + shiftAmt);
                picIndGlucose.BackColor = colorSchemeWarning;
            }
            else
            {
                int anlntX = picGlucoseLow.Location.X;
                int anlntY = picGlucoseLow.Location.Y;
                int shiftAmt = (picGlucoseLow.Size.Width - picIndGlucose.Size.Width) / 2;
                picIndGlucose.Location = new Point(anlntX + shiftAmt, anlntY + shiftAmt);
                picIndGlucose.BackColor = colorSchemeNormal;
            }
        }
        private string CalcBloodPressureSystolic()
        {
            int valueBPSys = Convert.ToInt32(txtBPSys.Text);

            lblBPSys.Text = txtBPSys.Text;
            prgBpSystolic.Maximum = MaxBPSys;
            prgBpSystolic.Value = valueBPSys;

            string sysLevel;
            if (valueBPSys < 120)
            {
                sysLevel = "1";
            }
            else if (valueBPSys < 140)
            {
                sysLevel = "2";
            }
            else if(valueBPSys < 160)
            {
                sysLevel = "3";
            }
            else
            {
                sysLevel = "4";
            }
            return sysLevel;

        }
        private string CalcBloodPressureDiastolic()
        {
            int valueBPDia = Convert.ToInt32(txtBPDia.Text);
            lblBPDia.Text = txtBPDia.Text;
            prgBpDiastolic.Maximum = MaxBPDia;
            prgBpDiastolic.Value = valueBPDia;

            string diaLevel;
            if (valueBPDia < 120)
            {
                diaLevel = "1";
            }
            else if (valueBPDia < 140)
            {
                diaLevel = "2";
            }
            else if (valueBPDia < 160)
            {
                diaLevel = "3";
            }
            else
            {
                diaLevel = "4";
            }
            return diaLevel;

        }
        //enum BPLevel
        //{
        //    Normal = 1, Pre, Stage1, Stage2
        //}

        private void SetOverallHypertensionLevel(string sysLevel, string diaLevel)
        {
            int levelNum1 = Convert.ToInt32(sysLevel);
            int levelNum2 = Convert.ToInt32(diaLevel);
            if(levelNum1 >= levelNum2)
            {
                if(levelNum1 == 1)
                {
                    lblHyperNormal.ForeColor = colorSchemeNormal;
                }
                else if (levelNum1 == 2)
                {
                    lblHyperPre.ForeColor = colorSchemeWarning;
                }
                else if(levelNum1 == 3)
                {
                    lblHyperStage1.ForeColor = colorSchemeIssue;
                }
                else
                {
                    lblHyperStage2.ForeColor = colorSchemeIssue;
                }
            }
            else
            {
                if (levelNum2 == 1)
                {
                    lblHyperNormal.ForeColor = colorSchemeNormal;
                }
                else if (levelNum2 == 2)
                {
                    lblHyperPre.ForeColor = colorSchemeWarning;
                }
                else if (levelNum2 == 3)
                {
                    lblHyperStage1.ForeColor = colorSchemeIssue;
                }
                else
                {
                    lblHyperStage2.ForeColor = colorSchemeIssue;
                }
            }
        }

        private void ValidateFormData()
        {
            bool isVaild;
            int age;
            int glucose;
            double weight;
            double inches;
            double height;
            double valueBPSys;
            double valueBPDia;

            if (!int.TryParse(txtAge.Text, out age) 
                || !double.TryParse(txtHeightFt.Text, out height)
                || !double.TryParse(txtHeightIn.Text, out inches)
                || !double.TryParse(txtWeight.Text, out weight)
                || !double.TryParse(txtBPSys.Text, out valueBPSys)
                || !double.TryParse(txtBPDia.Text, out valueBPDia)
                || !int.TryParse(txtGlucose.Text, out glucose))
            {
                txtAge.BackColor = Color.Pink;
                isVaild = false;
            }
        }

        private void AllTextBoxes_TextChanged(object sender, EventArgs e)
        {
            if(sender is TextBox)
            {
                TextBox tb = (TextBox)sender;
                tb.BackColor = Color.White;
            }

        }
    }
}
