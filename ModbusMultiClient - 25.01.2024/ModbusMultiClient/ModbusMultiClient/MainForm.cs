using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EasyModbus;
using System.Media;



namespace ModbusMultiClient
{

    public partial class MainForm : Form
    {
        ModbusClient ModClient = new ModbusClient();
        int[] vals;
        string Normal, FAULT, PlantEmergencyAIR, LinePressFaultAIR, PlantFaultAIR, DewPointFaultAIR;
        string termikLabel1, termikLabel2, termikLabel3, termikLabel4;
        string hataLabel1, hataLabel2, hataLabel3, hataLabel4;
        string arızaLabel1, arızaLabel2, arızaLabel3, arızaLabel4;
        string calisiyorLabel1, calisiyorLabel2, calisiyorLabel3, calisiyorLabel4;
        string duruyorLabel1, duruyorLabel2, duruyorLabel3, duruyorLabel4;
        string pasifLabel1, pasifLabel2, pasifLabel3, pasifLabel4;
        string faultButonDry1String, faultButonDry2String, faultButonDry3String, faultButonDry4String;
        string activeDry1String, activeDry2String, activeDry3String, activeDry4String;
        string pasifDry1String, pasifDry2String, pasifDry3String, pasifDry4String;
        string nemliDry1String, nemliDry2String, nemliDry3String, nemliDry4String;
        string nemliGifDry1String, nemliGifDry2String, nemliGifDry3String, nemliGifDry4String;
        string kaydetButonString, iptalButonString;
        string compResetButon1String, compResetButon2String, compResetButon3String, compResetButon4String;
        string type2resimString, type3resimString, type4resimString;
        string fabrikareset1String, fabrikareset2String, fabrikareset3String, fabrikareset4String;
        string nemliButon1String, nemliButon2String, nemliButon3String, nemliButon4String;
        string muteString, AlarmString;
        string hours = "hours";
        int sayac = 0;
        SoundPlayer player = new SoundPlayer();
        private Timer visibilityTimer;
        private bool waitingForInteraction;
        int language =0;
        private Image mainScreen2Comp;
        private Image mainScreen3Comp;
        private Image mainScreen4Comp;
        DataTable alarmTable = new DataTable();

        public MainForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            hastaneAdıLabel.Text = Properties.Settings.Default.KaydedilenDeger;
            mainScreen2Comp = global::ModbusMultiClient.Properties.Resources.MainScreen2Comp;
            mainScreen3Comp = global::ModbusMultiClient.Properties.Resources.MainScreen3Comp;
            mainScreen4Comp = global::ModbusMultiClient.Properties.Resources.MainScreen4Comp;
            // Timer oluşturuluyor ve ayarları yapılıyor.
            visibilityTimer = new Timer();
            visibilityTimer.Interval = 10000; // 60 saniye = 1 dakika
            visibilityTimer.Tick += VisibilityTimer_Tick;
            waitingForInteraction = false;
            // Form yüklenirken timer'ı başlatın.
            visibilityTimer.Start();

            Timer updateTimer = new Timer();
            updateTimer.Interval = 1000; // Her 1 saniyede bir güncelle
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Start();

            // İlk kez çağırarak başlangıç değerlerini ayarla
            UpdateDate();
            UpdateTime();

            ModClient = new ModbusClient(txtPort.Text);
            ModClient.Baudrate = int.Parse(cboBaudrate.Text);
            if (cboParity.Text == "None")
            {
                ModClient.Parity = System.IO.Ports.Parity.None;
            }
            else if (cboParity.Text == "Even")
            {
                ModClient.Parity = System.IO.Ports.Parity.Even;
            }
            else if (cboParity.Text == "Odd")
            {
                ModClient.Parity = System.IO.Ports.Parity.Odd;
            }

            try
            {
                ModClient.Connect();
                timerPoll.Start();
                lblStatus.Text = "Connected";
                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;
                grpRW.Enabled = true;
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error! " + ex.ToString();
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            ModClient = new ModbusClient(txtPort.Text);
            ModClient.Baudrate = int.Parse(cboBaudrate.Text);
            if (cboParity.Text == "None")
            {
                ModClient.Parity = System.IO.Ports.Parity.None;
            }
            else if (cboParity.Text == "Even")
            {
                ModClient.Parity = System.IO.Ports.Parity.Even;
            }
            else if (cboParity.Text == "Odd")
            {
                ModClient.Parity = System.IO.Ports.Parity.Odd;
            }

            try
            {
                ModClient.Connect();
                timerPoll.Start();
                lblStatus.Text = "Connected";
                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;
                grpRW.Enabled = true;
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error! " + ex.ToString();
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            ModClient.Disconnect();
            timerPoll.Stop();
            lblStatus.Text = "Disconnected";
            btnConnect.Enabled = true;
            btnDisconnect.Enabled = false;
            grpRW.Enabled = false;
        }

        private void btnReadHolding_Click(object sender, EventArgs e)
        {


        }

        private void btnWriteHolding_Click(object sender, EventArgs e)
        {

            ModClient.WriteSingleRegister(int.Parse(txtReg.Text), int.Parse(TankBasıncı.Text));
        }

       
        private void btnReadAnalog_Click(object sender, EventArgs e)
        {
            int[] vals;
            vals = ModClient.ReadInputRegisters(int.Parse(txtReg.Text), 1);
            TankBasıncı.Text = vals[0].ToString();
        }


        private void btnReadCoil_Click(object sender, EventArgs e)
        {
            bool[] vals;
            vals = ModClient.ReadCoils(0, 6);
            coil.Text = vals[4].ToString();
        }

        private void btnWriteCoil_Click(object sender, EventArgs e)
        {
            ModClient.WriteSingleCoil(41, true);
        }

   
        private void btnReadDigital_Click(object sender, EventArgs e)
        {
            bool[] vals;
            vals = ModClient.ReadDiscreteInputs(int.Parse(txtReg.Text), 1);
            TankBasıncı.Text = vals[0].ToString();
        }

     

        private void MainForm_Load(object sender, EventArgs e)
        {
           

        }

  private void InitializeAlarmTable()
{
    alarmTable.Columns.Add("Date", typeof(string));
    alarmTable.Columns.Add("Time", typeof(string));
    alarmTable.Columns.Add("AlarmType", typeof(string));
}

        private void timerPoll_Tick(object sender, EventArgs e)
        {
            try
            {
                if (ModClient.Connected == true)
                {
                    ////// ID = 1 ///////
                    ModClient.UnitIdentifier = 1;
                    
                        vals = ModClient.ReadHoldingRegisters(0, 47);
                        modbusAddress.Text = vals[0].ToString();
                        modbusBaudrate.Text = vals[1].ToString();
                        TankBasıncı.Text = (vals[2] / 100.0).ToString();
                        HatBasıncı.Text = (vals[3] / 100.0).ToString();
                        StartBasıncı.Text = (vals[4] / 10.0).ToString();
                        StopBasıncı.Text = (vals[5] / 10.0).ToString();
                        StartTime2.Text = vals[6].ToString();
                        StartTime3.Text = vals[7].ToString();
                        StartTime4.Text = vals[8].ToString();
                        StartTimeAlarm.Text = vals[9].ToString();
                        StatPump.Text = vals[10].ToString();
                        Esyaslanma.Text = vals[11].ToString();
                        EsyasDuty.Text = vals[12].ToString();
                        SaatPompa1.Text = vals[13].ToString() + " " + hours;
                        DkikaPompa1.Text = vals[14].ToString();
                        SaatPompa2.Text = vals[15].ToString() + " " + hours;
                        DkikaPompa2.Text = vals[16].ToString();
                        SaatPompa3.Text = vals[17].ToString() + " " + hours;
                        DkikaPompa3.Text = vals[18].ToString();
                        SaatPompa4.Text = vals[19].ToString() + " " + hours;
                        DkikaPompa4.Text = vals[20].ToString();
                        /* StatP1.Text = vals[21].ToString();
                         StatP2.Text = vals[22].ToString();
                         StatP3.Text = vals[23].ToString();
                         StatP4.Text = vals[24].ToString();*/
                        DewPoint.Text = vals[34].ToString();
                        DutyDryTime.Text = vals[35].ToString();
                        DrayerGrup.Text = vals[36].ToString();
                        StatDrayer1.Text = vals[37].ToString();
                        StatDrayer2.Text = vals[38].ToString();
                        StatDrayer3.Text = vals[39].ToString();
                        StatDrayer4.Text = vals[40].ToString();
                        PresureFault.Text = (vals[41] / 10.0).ToString();
                        TankPerıod.Text = vals[43].ToString();
                        TankTahlıye.Text = vals[44].ToString();
                        Dryerfault.Text = vals[46].ToString();


                    saatPompa1Gizli.Text = vals[13].ToString();
                    saatPompa2Gizli.Text = vals[15].ToString();
                    saatPompa3Gizli.Text = vals[17].ToString();
                    saatPompa4Gizli.Text = vals[19].ToString();



                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    ///
                    bool[] valu;
                    valu = ModClient.ReadCoils(0, 113);
                    coil.Text = valu[4].ToString();

                  //  muteString = valu[28].ToString();
                 //   AlarmString = valu[27].ToString();


                    termikLabel1 = valu[8].ToString();
                    termikLabel2 = valu[9].ToString();
                    termikLabel3 = valu[10].ToString();
                    termikLabel4 = valu[11].ToString();

                    hataLabel1 = valu[12].ToString();
                    hataLabel2 = valu[13].ToString();
                    hataLabel3 = valu[14].ToString();
                    hataLabel4 = valu[15].ToString();

                    arızaLabel1 = valu[16].ToString();
                    arızaLabel2 = valu[17].ToString();
                    arızaLabel3 = valu[18].ToString();
                    arızaLabel4 = valu[19].ToString();

                    calisiyorLabel1 = valu[33].ToString();
                    calisiyorLabel2 = valu[34].ToString();
                    calisiyorLabel3 = valu[35].ToString();
                    calisiyorLabel4 = valu[36].ToString();

                    duruyorLabel1 = valu[37].ToString();
                    duruyorLabel2 = valu[38].ToString();
                    duruyorLabel3 = valu[39].ToString();
                    duruyorLabel4 = valu[40].ToString();

                    pasifLabel1 = valu[68].ToString();
                    pasifLabel2 = valu[69].ToString();
                    pasifLabel3 = valu[70].ToString();
                    pasifLabel4 = valu[71].ToString();

                    compResetButon1String = valu[43].ToString();
                    compResetButon2String = valu[44].ToString();
                    compResetButon3String = valu[45].ToString();
                    compResetButon4String = valu[46].ToString();

                    PlantEmergencyAIR = valu[20].ToString();
                    LinePressFaultAIR = valu[21].ToString();
                    PlantFaultAIR = valu[22].ToString();
                    DewPointFaultAIR = valu[23].ToString();

                    FAULT = valu[108].ToString();
                    Normal = valu[107].ToString();

                    nemliButon1String = valu[59].ToString();
                    nemliButon2String = valu[60].ToString();
                    nemliButon3String = valu[61].ToString();
                    nemliButon4String = valu[62].ToString();


                    activeDry1String = valu[72].ToString();
                    activeDry2String = valu[73].ToString();
                    activeDry3String = valu[74].ToString();
                    activeDry4String = valu[75].ToString();

                    pasifDry1String = valu[76].ToString();
                    pasifDry2String = valu[77].ToString();
                    pasifDry3String = valu[78].ToString();
                    pasifDry4String = valu[79].ToString();

                    nemliDry1String = valu[80].ToString();
                    nemliDry2String = valu[81].ToString();
                    nemliDry3String = valu[82].ToString();
                    nemliDry4String = valu[83].ToString();

                    faultButonDry1String = valu[84].ToString();
                    faultButonDry2String = valu[85].ToString();
                    faultButonDry3String = valu[86].ToString();
                    faultButonDry4String = valu[87].ToString();

                    nemliGifDry1String = valu[100].ToString();
                    nemliGifDry2String = valu[101].ToString();
                    nemliGifDry3String = valu[102].ToString();
                    nemliGifDry4String = valu[103].ToString();


                    kaydetButonString = valu[41].ToString();
                    iptalButonString = valu[42].ToString();

                    type2resimString = valu[104].ToString();
                    type3resimString = valu[105].ToString();
                    type4resimString = valu[106].ToString();

                    fabrikareset1String = valu[109].ToString();
                    fabrikareset2String = valu[110].ToString();
                    fabrikareset3String = valu[111].ToString();
                    fabrikareset4String = valu[112].ToString();




                    /*   ModClient.UnitIdentifier = 2;


                          /////// ID =2 ////////


                                  vals = ModClient.ReadHoldingRegisters(0, 8);
                                  compAmper1.Text = vals[0].ToString();
                                  compAmper2.Text = vals[1].ToString();
                                  compAmper3.Text = vals[2].ToString();
                                  compAmper4.Text = vals[3].ToString();

                              */

                    #region ALARM DURUMU ÇALMA VE SESSİZE ALMA

                    muteString = valu[28].ToString();
                    AlarmString = valu[27].ToString();
                    if (AlarmString == "True")
                    {
                        player.Play();
                        string path = Application.StartupPath + "/acil.wav";
                        player.SoundLocation = path;

                    }
                    else
                    {
                        player.Stop();

                    }

                    if (muteString == "True")
                    {

                    }
                    else
                    {
                        player.Play();
                        string path = Application.StartupPath + "/acil.wav";
                        player.SoundLocation = path;
                    }


                    #endregion  

                    #region POMPA DURUMLARI

                    /////// 1. KOMP ///////
                    if (arızaLabel1 == "True")   /// 0X16
                    {
                        compResetButon1.Visible = true;
                        if (language == 0) StatP1.Text = "POWER";   
                        else if (language == 1) StatP1.Text = "POWER";
                        else if (language == 2) StatP1.Text = "KOMP. ARIZA";
                        StatP1.ForeColor = Color.Red;
                        arızaPictureBox1.Visible = true;
                    }
                    else
                    {
                        arızaPictureBox1.Visible = false;
                        compResetButon1.Visible = false;
                    }
                    if (hataLabel1 == "True")   //0X12
                    {
                        if (language == 0) StatP1.Text = "FAULT";
                        else if (language == 1) StatP1.Text = "FEHLER";
                        else if (language == 2) StatP1.Text = "HATA";
                        StatP1.ForeColor = Color.Red;
                        hataPictureBox1.Visible = true;
                    }
                    else
                    {
                        hataPictureBox1.Visible = false;
                    }
                    if (termikLabel1 == "True")
                    {
                        if (language == 0) StatP1.Text = "THERMIC";
                        else if (language == 1) StatP1.Text = "THERMIK";
                        else if (language == 2) StatP1.Text = "TERMİK";
                        StatP1.ForeColor = Color.Red;
                        termikPictureBox1.Visible = true;
                    }
                    else
                    {
                        termikPictureBox1.Visible = false;
                    }
                    if (calisiyorLabel1 == "True")
                    {
                        if (language == 0) StatP1.Text = "CUT IN";
                        else if (language == 1) StatP1.Text = "AN";
                        else if (language == 2) StatP1.Text = "ÇALIŞIYOR";
                        StatP1.ForeColor = Color.Green;
                        calisiyorPictureBox1.Visible = true;
                    }
                    else
                    {
                        calisiyorPictureBox1.Visible = false;
                    }
                    if (pasifLabel1 == "True")
                    {
                        if (language == 0) StatP1.Text = "PASSIVE";
                        else if (language == 1) StatP1.Text = "PASSIV";
                        else if (language == 2) StatP1.Text = "PASİF";
                        StatP1.ForeColor = Color.DarkGray;
                        pasifPictureBox1.Visible = true;
                    }
                    else
                    {
                        pasifPictureBox1.Visible = false;
                    }
                    if (duruyorLabel1 == "True")
                    {
                        if (language == 0) StatP1.Text = "CUT OUT";
                        else if (language == 1) StatP1.Text = "AUS";
                        else if (language == 2) StatP1.Text = "DURUYOR";
                        StatP1.ForeColor = Color.Orange;
                    }
                    /////// 2. KOMP ///////
                    if (arızaLabel2 == "True")
                    {
                        compResetButon2.Visible = true;
                        if (language == 0) StatP2.Text = "POWER";
                        else if (language == 1) StatP2.Text = "POWER";
                        else if (language == 2) StatP2.Text = "KOMP. ARIZA";
                        StatP2.ForeColor = Color.Red;
                        arızaPictureBox2.Visible = true;
                    }
                    else
                    {
                        arızaPictureBox2.Visible = false;
                        compResetButon2.Visible = false;
                    }
                    if (hataLabel2 == "True")
                    {
                        if (language == 0) StatP2.Text = "FAULT";
                        else if (language == 1) StatP2.Text = "FEHLER";
                        else if (language == 2) StatP2.Text = "HATA";
                        StatP2.ForeColor = Color.Red;
                        hataPictureBox2.Visible = true;
                    }
                    else
                    {
                        hataPictureBox2.Visible = false;
                    }
                    if (termikLabel2 == "True")
                    {
                        if (language == 0) StatP2.Text = "THERMIC";
                        else if (language == 1) StatP2.Text = "THERMIK";
                        else if (language == 2) StatP2.Text = "TERMİK";
                        StatP2.ForeColor = Color.Red;
                        termikPictureBox2.Visible = true;
                    }
                    else
                    {
                        termikPictureBox2.Visible = false;
                    }
                    if (calisiyorLabel2 == "True")
                    {
                        if (language == 0) StatP2.Text = "CUT IN";
                        else if (language == 1) StatP2.Text = "AN";
                        else if (language == 2) StatP2.Text = "ÇALIŞIYOR";
                        StatP2.ForeColor = Color.Green;
                        calisiyorPictureBox2.Visible = true;
                    }
                    else
                    {
                        calisiyorPictureBox2.Visible = false;
                    }
                    if (pasifLabel2 == "True")
                    {
                        if (language == 0) StatP2.Text = "PASSIVE";
                        else if (language == 1) StatP2.Text = "PASSIV";
                        else if (language == 2) StatP2.Text = "PASİF";
                        StatP2.ForeColor = Color.DarkGray;
                        pasifPictureBox2.Visible = true;
                    }
                    else
                    {
                        pasifPictureBox2.Visible = false;
                    }
                    if (duruyorLabel2 == "True")
                    {
                        if (language == 0) StatP2.Text = "CUT OUT";
                        else if (language == 1) StatP2.Text = "AUS";
                        else if (language == 2) StatP2.Text = "DURUYOR";
                        StatP2.ForeColor = Color.Orange;
                    }
                    /////// 3. KOMP ///////
                    if (arızaLabel3 == "True")
                    {
                        compResetButon3.Visible = true;
                        if (language == 0) StatP3.Text = "POWER";
                        else if (language == 1) StatP3.Text = "POWER";
                        else if (language == 2) StatP3.Text = "KOMP. ARIZA";
                        StatP3.ForeColor = Color.Red;
                        arızaPictureBox3.Visible = true;
                    }
                    else
                    {
                        arızaPictureBox3.Visible = false;
                        compResetButon3.Visible = false;
                    }
                    if (hataLabel3 == "True")
                    {
                        if (language == 0) StatP3.Text = "FAULT";
                        else if (language == 1) StatP3.Text = "FEHLER";
                        else if (language == 2) StatP3.Text = "HATA";
                        StatP3.ForeColor = Color.Red;
                        hataPictureBox3.Visible = true;
                    }
                    else
                    {
                        hataPictureBox3.Visible = false;
                    }
                    if (termikLabel3 == "True")
                    {
                        if (language == 0) StatP3.Text = "THERMIC";
                        else if (language == 1) StatP3.Text = "THERMIK";
                        else if (language == 2) StatP3.Text = "TERMİK";
                        StatP3.ForeColor = Color.Red;
                        termikPictureBox3.Visible = true;
                    }
                    else
                    {
                        termikPictureBox3.Visible = false;
                    }
                    if (calisiyorLabel3 == "True")
                    {
                        if (language == 0) StatP3.Text = "CUT IN";
                        else if (language == 1) StatP3.Text = "AN";
                        else if (language == 2) StatP3.Text = "ÇALIŞIYOR";
                        StatP3.ForeColor = Color.Green;
                        calisiyorPictureBox3.Visible = true;
                    }
                    else
                    {
                        calisiyorPictureBox3.Visible = false;
                    }
                    if (pasifLabel3 == "True")
                    {
                        if (language == 0) StatP3.Text = "PASSIVE";
                        else if (language == 1) StatP3.Text = "PASSIV";
                        else if (language == 2) StatP3.Text = "PASİF";
                        StatP3.ForeColor = Color.DarkGray;
                        pasifPictureBox3.Visible = true;
                    }
                    else
                    {
                        pasifPictureBox3.Visible = false;
                    }
                    if (duruyorLabel3 == "True")
                    {
                        if (language == 0) StatP3.Text = "CUT OUT";
                        else if (language == 1) StatP3.Text = "AUS";
                        else if (language == 2) StatP3.Text = "DURUYOR";
                        StatP3.ForeColor = Color.Orange;
                    }
                    /////// 4. KOMP ///////
                    if (arızaLabel4 == "True")
                    {
                        compResetButon4.Visible = true;
                        if (language == 0) StatP4.Text = "POWER";
                        else if (language == 1) StatP4.Text = "POWER";
                        else if (language == 2) StatP4.Text = "KOMP. ARIZA";
                        StatP4.ForeColor = Color.Red;
                        arızaPictureBox4.Visible = true;
                    }
                    else
                    {
                        arızaPictureBox4.Visible = false;
                        compResetButon4.Visible = false;
                    }
                    if (hataLabel4 == "True")
                    {
                        if (language == 0) StatP4.Text = "FAULT";
                        else if (language == 1) StatP4.Text = "FEHLER";
                        else if (language == 2) StatP4.Text = "HATA";
                        StatP4.ForeColor = Color.Red;
                        hataPictureBox4.Visible = true;
                    }
                    else
                    {
                        hataPictureBox4.Visible = false;
                    }
                    if (termikLabel4 == "True")
                    {
                        if (language == 0) StatP4.Text = "THERMIC";
                        else if (language == 1) StatP4.Text = "THERMIK";
                        else if (language == 2) StatP4.Text = "TERMİK";
                        StatP4.ForeColor = Color.Red;
                        termikPictureBox4.Visible = true;
                    }
                    else
                    {
                        termikPictureBox4.Visible = false;
                    }
                    if (calisiyorLabel4 == "True")
                    {
                        if (language == 0) StatP4.Text = "CUT IN";
                        else if (language == 1) StatP4.Text = "AN";
                        else if (language == 2) StatP4.Text = "ÇALIŞIYOR";
                        StatP4.ForeColor = Color.Green;
                        calisiyorPictureBox4.Visible = true;
                    }
                    else
                    {
                        calisiyorPictureBox4.Visible = false;
                    }
                    if (pasifLabel4 == "True")
                    {
                        if (language == 0) StatP4.Text = "PASSIVE";
                        else if (language == 1) StatP4.Text = "PASSIV";
                        else if (language == 2) StatP4.Text = "PASİF"; 
                        StatP4.ForeColor = Color.DarkGray;
                        pasifPictureBox4.Visible = true;
                    }
                    else
                    {
                        pasifPictureBox4.Visible = false;
                    }
                    if (duruyorLabel4 == "True")
                    {
                        if (language == 0) StatP4.Text = "CUT OUT";
                        else if (language == 1) StatP4.Text = "AUS";
                        else if (language == 2) StatP4.Text = "DURUYOR";
                        StatP4.ForeColor = Color.Orange;
                    }

                    #endregion

                    #region SAĞ KUTUCUK ALARM DURUMLARI

                    if (PlantEmergencyAIR == "True")
                    {
                        if (language == 0) PlantEmergencyLabelAIR.Text = "PLANT EMERGENCY";
                        else if (language == 1) PlantEmergencyLabelAIR.Text = "NOTTFAL IM SYSTEM";
                        else if (language == 2) PlantEmergencyLabelAIR.Text = "ACİL DURUM UYARISI";
                        PlantEmergencyLabelAIR.ForeColor = Color.White;
                        PlantEmergencyLabelAIR.BackColor = Color.Red;
                        alarmListView.Items.Add(new ListViewItem(new string[] { DateTime.Now.ToString(), "Plant Emergency" }));

                    }
                    else
                    {
                        if (language == 0) PlantEmergencyLabelAIR.Text = "PLANT EMERGENCY";
                        else if (language == 1) PlantEmergencyLabelAIR.Text = "NOTTFAL IM SYSTEM";
                        else if (language == 2) PlantEmergencyLabelAIR.Text = "ACİL DURUM UYARISI";
                        PlantEmergencyLabelAIR.ForeColor = Color.LightGray;
                        PlantEmergencyLabelAIR.BackColor = Color.Transparent;
                    }
                    if (LinePressFaultAIR == "True")
                    {
                        if (language == 0) LinePressFaultLabelAIR.Text = "LINE PRESSURE FAULT";
                        else if (language == 1) LinePressFaultLabelAIR.Text = "LEITUNGSDRUCKFEHLER";
                        else if (language == 2) LinePressFaultLabelAIR.Text = "HAT BASINÇ HATASI";
                        LinePressFaultLabelAIR.ForeColor = Color.White;
                        LinePressFaultLabelAIR.BackColor = Color.Red;
                        alarmListView.Items.Add(new ListViewItem(new string[] { DateTime.Now.ToString(), "LINE PRESSURE FAULT" }));

                    }
                    else
                    {
                        if (language == 0) LinePressFaultLabelAIR.Text = "LINE PRESSURE FAULT";
                        else if (language == 1) LinePressFaultLabelAIR.Text = "LEITUNGSDRUCKFEHLER";
                        else if (language == 2) LinePressFaultLabelAIR.Text = "HAT BASINÇ HATASI";
                        LinePressFaultLabelAIR.ForeColor = Color.LightGray;
                        LinePressFaultLabelAIR.BackColor = Color.Transparent;
                    }
                    if (PlantFaultAIR == "True")
                    {
                        if (language == 0) PlantFaultLabelAIR.Text = "PLANT FAULT";
                        else if (language == 1) PlantFaultLabelAIR.Text = "BETRIEBSTÖRUNG";
                        else if (language == 2) PlantFaultLabelAIR.Text = "SİSTEM HATASI";
                        PlantFaultLabelAIR.ForeColor = Color.White;
                        PlantFaultLabelAIR.BackColor = Color.Red;
                        alarmListView.Items.Add(new ListViewItem(new string[] { DateTime.Now.ToString(), "PLANT FAULT" }));

                    }
                    else
                    {
                        if (language == 0) PlantFaultLabelAIR.Text = "PLANT FAULT";
                        else if (language == 1) PlantFaultLabelAIR.Text = "BETRIEBSTÖRUNG";
                        else if (language == 2) PlantFaultLabelAIR.Text = "SİSTEM HATASI";
                        PlantFaultLabelAIR.ForeColor = Color.LightGray;
                        PlantFaultLabelAIR.BackColor = Color.Transparent;
                    }
                    if (DewPointFaultAIR == "True")
                    {
                        if (language == 0) DewPointFaultLabelAIR.Text = "DEWPOINT FAULT";
                        else if (language == 1) DewPointFaultLabelAIR.Text = "DRUCKTAUPUNKT";
                        else if (language == 2) DewPointFaultLabelAIR.Text = "DP NOKTA HATASI";
                        DewPointFaultLabelAIR.ForeColor = Color.White;
                        DewPointFaultLabelAIR.BackColor = Color.Red;
                        alarmListView.Items.Add(new ListViewItem(new string[] { DateTime.Now.ToString(), "DEWPOINT FAULT" }));

                    }
                    else
                    {
                        if (language == 0) DewPointFaultLabelAIR.Text = "DEWPOINT FAULT";
                        else if (language == 1) DewPointFaultLabelAIR.Text = "DRUCKTAUPUNKT";
                        else if (language == 2) DewPointFaultLabelAIR.Text = "DP NOKTA HATASI";
                        DewPointFaultLabelAIR.ForeColor = Color.LightGray;
                        DewPointFaultLabelAIR.BackColor = Color.Transparent;
                    }
                    if (Normal == "True")
                    {
                        if(language == 0) NormalFaultLabel.Text = "NORMAL";
                        else if (language == 1) NormalFaultLabel.Text = "NORMAL";
                        else if (language == 2) NormalFaultLabel.Text = "NORMAL";
                        NormalFaultLabel.ForeColor = Color.Green;
                    }
                    else
                    {
                        if (language == 0) NormalFaultLabel.Text = "FAULT";
                        else if (language == 1) NormalFaultLabel.Text = "FEHLER";
                        else if (language == 2) NormalFaultLabel.Text = "ARIZA"; 
                        NormalFaultLabel.ForeColor = Color.Red;
                    }
                    #endregion

                    #region KURUTUCULAR

                    /////// 1. KURUTUCU ////////
                    if (activeDry1String == "True")
                    {
                        activeDry1.Visible = true;
                    }
                    else
                    {
                        activeDry1.Visible = false;
                    }
                    if (pasifDry1String == "True")
                    {
                        pasifDry1.Visible = true;
                    }
                    else
                    {
                        pasifDry1.Visible = false;
                    }
                    if (nemliGifDry1String == "True")
                    {
                        nemliGifDry1.Visible = true;
                    }
                    else
                    {
                        nemliGifDry1.Visible = false;
                    }
                    if (nemliDry1String == "True")
                    {
                        nemliDry1.Visible = true;
                        nemliDryLabel1.Visible = true;
                        if (language == 0)nemliDryLabel1.Text = "HUMID";
                        else if (language == 1) nemliDryLabel1.Text = "HUMID";
                        else if (language == 2) nemliDryLabel1.Text = "NEMLİ";
                        nemliButon1.Visible = true;

                    }
                    else
                    {
                        nemliDry1.Visible = false;
                        nemliDryLabel1.Visible = false;
                        nemliDryLabel1.Text = "";
                        nemliButon1.Visible = false;

                    }
                    if (faultButonDry1String == "True")
                    {
                        faultButonDry1.Visible = true;
                    }
                    else
                    {
                        faultButonDry1.Visible = false;
                    }

                    /////// 2. KURUTUCU ////////
                    if (activeDry2String == "True")
                    {
                        activeDry2.Visible = true;
                    }
                    else
                    {
                        activeDry2.Visible = false;
                    }
                    if (pasifDry2String == "True")
                    {
                        pasifDry2.Visible = true;
                    }
                    else
                    {
                        pasifDry2.Visible = false;
                    }
                    if (nemliGifDry2String == "True")
                    {
                        nemliGifDry2.Visible = true;
                    }
                    else
                    {
                        nemliGifDry2.Visible = false;
                    }
                    if (nemliDry2String == "True")
                    {
                        nemliDry2.Visible = true;
                        nemliDryLabel2.Visible = true;
                        if (language == 0) nemliDryLabel2.Text = "HUMID";
                        else if (language == 1) nemliDryLabel2.Text = "HUMID";
                        else if (language == 2) nemliDryLabel2.Text = "NEMLİ";
                        nemliButon2.Visible = true;
                    }
                    else
                    {
                        nemliDry2.Visible = false;
                        nemliDryLabel2.Visible = false;
                        nemliDryLabel2.Text = "";
                        nemliButon2.Visible = false;
                    }
                    if (faultButonDry2String == "True")
                    {
                        faultButonDry2.Visible = true;
                    }
                    else
                    {
                        faultButonDry2.Visible = false;
                    }
                    /////// 3. KURUTUCU ////////
                    if (activeDry3String == "True")
                    {
                        activeDry3.Visible = true;
                    }
                    else
                    {
                        activeDry3.Visible = false;
                    }
                    if (pasifDry3String == "True")
                    {
                        pasifDry3.Visible = true;
                    }
                    else
                    {
                        pasifDry3.Visible = false;
                    }
                    if (nemliGifDry3String == "True")
                    {
                        nemliGifDry3.Visible = true;
                    }
                    else
                    {
                        nemliGifDry3.Visible = false;
                    }
                    if (nemliDry3String == "True")
                    {
                        nemliDry3.Visible = true;
                        nemliDryLabel3.Visible = true;
                        if (language == 0) nemliDryLabel3.Text = "HUMID";
                        else if (language == 1) nemliDryLabel3.Text = "HUMID";
                        else if (language == 2) nemliDryLabel3.Text = "NEMLİ";
                        nemliButon3.Visible = true;
                    }
                    else
                    {
                        nemliDry3.Visible = false;
                        nemliDryLabel3.Visible = false;
                        nemliDryLabel3.Text = "";
                        nemliButon3.Visible = false;
                    }
                    if (faultButonDry3String == "True")
                    {
                        faultButonDry3.Visible = true;
                    }
                    else
                    {
                        faultButonDry3.Visible = false;
                    }
                    /////// 4. KURUTUCU ////////
                    if (activeDry4String == "True")
                    {
                        activeDry4.Visible = true;
                    }
                    else
                    {
                        activeDry4.Visible = false;
                    }
                    if (pasifDry4String == "True")
                    {
                        pasifDry4.Visible = true;
                    }
                    else
                    {
                        pasifDry4.Visible = false;
                    }
                    if (nemliGifDry4String == "True")
                    {
                        nemliGifDry4.Visible = true;
                    }
                    else
                    {
                        nemliGifDry4.Visible = false;
                    }
                    if (nemliDry4String == "True")
                    {
                        nemliDry4.Visible = true;
                        nemliDryLabel4.Visible = true;
                        if (language == 0) nemliDryLabel4.Text = "HUMID";
                        else if (language == 1) nemliDryLabel4.Text = "HUMID";
                        else if (language == 2) nemliDryLabel4.Text = "NEMLİ";
                        nemliButon4.Visible = true;
                    }
                    else
                    {
                        nemliDry4.Visible = false;
                        nemliDryLabel4.Visible = false;
                        nemliDryLabel4.Text = "";
                        nemliButon4.Visible = false;
                    }
                    if (faultButonDry4String == "True")
                    {
                        faultButonDry4.Visible = true;
                    }
                    else
                    {
                        faultButonDry4.Visible = false;
                    }

                    #endregion

                    #region COMP TİPİ 2-3-4 LAMBA
                    if (type2resimString == "True")
                    {
                        type2resim.Visible = true;
                        type3resim.Visible = false;
                        type4resim.Visible = false;
                        StatP4.Visible = false;
                        StatP3.Visible = false;
                        SaatPompa3.Visible = false;
                        SaatPompa4.Visible = false;
                        compAmper3.Visible = false;
                        compAmper4.Visible = false;
                        if (this.BackgroundImage != mainScreen2Comp)
                            this.BackgroundImage = mainScreen2Comp;
                    }
                    else if (type3resimString =="True")
                    {
                        type3resim.Visible = true;
                        type2resim.Visible = false;
                        type4resim.Visible = false;
                        StatP4.Visible = false;
                        SaatPompa4.Visible = false;
                        compAmper4.Visible = false;
                        StatP3.Visible = true;
                        SaatPompa3.Visible = true;
                        compAmper3.Visible = true;
                        if (this.BackgroundImage != mainScreen3Comp)
                            this.BackgroundImage = mainScreen3Comp;
                    }
                    else if (type4resimString == "True")
                    {
                        type4resim.Visible = true;
                        type3resim.Visible = false;
                        type2resim.Visible = false;
                        StatP4.Visible = true;
                        SaatPompa4.Visible = true;
                        compAmper4.Visible = true;
                        StatP3.Visible = true;
                        SaatPompa3.Visible = true;
                        compAmper3.Visible = true;
                        if (this.BackgroundImage != mainScreen4Comp)
                            this.BackgroundImage = mainScreen4Comp;
                    
                    }



                    #endregion

                
                }
            }
            catch
            { }
        }
 
        private void alarmButon_Click(object sender, EventArgs e)
        {
            ModClient.WriteSingleCoil(27, false);
            ModClient.WriteSingleCoil(28, true);
        }



        #region ANA SAYFA BUTON KULLANIMI

        private void faultButonDry1_Click(object sender, EventArgs e)
        {
            ModClient.WriteSingleCoil(47, true);
        }

        private void faultButonDry2_Click(object sender, EventArgs e)
        {
            ModClient.WriteSingleCoil(48, true);
        }

        private void faultButonDry3_Click(object sender, EventArgs e)
        {
            ModClient.WriteSingleCoil(49, true);
        }
     
        private void faultButonDry4_Click(object sender, EventArgs e)
        {
            ModClient.WriteSingleCoil(50, true);
        }
     
        private void compResetButon1_Click(object sender, EventArgs e)
        {
            ModClient.WriteSingleCoil(43, true);
        }

        private void compResetButon2_Click(object sender, EventArgs e)
        {
            ModClient.WriteSingleCoil(44, true);
        }

        private void compResetButon3_Click(object sender, EventArgs e)
        {
            ModClient.WriteSingleCoil(45, true);
        }

        private void compResetButon4_Click(object sender, EventArgs e)
        {
            ModClient.WriteSingleCoil(46, true);
        }

        private void changedryButton1_Click(object sender, EventArgs e)
        {
            ModClient.WriteSingleRegister(36, 1);
        }

        private void changedryButton2_Click(object sender, EventArgs e)
        {
            ModClient.WriteSingleRegister(36, 2);
        }

        #endregion


        #region ANA SAYFA 2. SAYFAYA GEÇMEK İÇİN YAPILAN AYARLAMALAR

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            // Her zamanlayıcı tetiklendiğinde tarih ve saati güncelle
            UpdateDate();
            UpdateTime();
        }
        private void UpdateDate()
        {
            DateTime currentDate = DateTime.Now;
            dateLabel.Text = currentDate.ToString("dd.MM.yyyy");
        }
        private void UpdateTime()
        {
            DateTime currentTime = DateTime.Now;
            timeLabel.Text = currentTime.ToString("HH:mm");
        }
        private void dateLabel_Click(object sender, EventArgs e)
        {
            UpdateDate();
        }
        private void timeLabel_Click(object sender, EventArgs e)
        {
            UpdateTime();
        }
        private void sifreGirmeButon_Click(object sender, EventArgs e)
        {
            EnterButon.Visible = true;
            sifreTextBox.Visible = true;
            dateLabel.Visible = false;
            waitingForInteraction = true;
            visibilityTimer.Stop();
            visibilityTimer.Start();
        }
        private void VisibilityTimer_Tick(object sender, EventArgs e)
        {
            if (waitingForInteraction)
            {
                EnterButon.Visible = false;
                sifreTextBox.Visible = false;
                dateLabel.Visible = true;
            }
            visibilityTimer.Stop();
            waitingForInteraction = false;
        }
        private void EnterButon_Click(object sender, EventArgs e) /////////////// 2. sayfaya geçmek için
        {
            string enteredPassword = sifreTextBox.Text;

            string correctPassword = "11";
            //  System.Diagnostics.Process.Start("osk.exe");//ekran klavyesini başlatır.
            if (!string.IsNullOrWhiteSpace(enteredPassword)) // Şifre boş değilse kontrol et
            {
                if (enteredPassword == correctPassword)
                {
                    ayarlarSayfası.Visible = true;
                    sifreTextBox.Visible = false;
                    EnterButon.Visible = false;
                }
                else
                {
                    // Şifre yanlışsa bir hata mesajı gösterebilirsiniz.
                    MessageBox.Show("Hatalı şifre! Lütfen doğru şifreyi girin.");
                    sifreTextBox.Text = "";

                }
            }

        }
        private void AnaSayafayaDön_Click(object sender, EventArgs e)   ////// 2. sayfadan ana sayfaya dönme butonu
        {
            ayarlarSayfası.Visible = false;
            sifreTextBox.Text = "";
        }

        #endregion


        #region KOMPRESÖR AYARLARI

        private void compSettingsButon_Click(object sender, EventArgs e)   ///// Ayarlar sayfasından Comp. Ayarlar sayfasına geçme
        {
            compPanel.Visible = true;
        }

        private void comp2YapButon_Click(object sender, EventArgs e)
        {
            ModClient.WriteSingleRegister(10, 2);   // kaç kompresörlü olacağını yazdırıyorum.
        }

        private void comp3YapButon_Click(object sender, EventArgs e)
        {
            ModClient.WriteSingleRegister(10, 3);   // kaç kompresörlü olacağını yazdırıyorum.
        }

        private void comp4YapButon_Click(object sender, EventArgs e)
        {
            ModClient.WriteSingleRegister(10, 4);   // kaç kompresörlü olacağını yazdırıyorum.
        }

        private void StartTime2_TextChanged(object sender, EventArgs e)
        {
            ModClient.WriteSingleRegister(6, int.Parse(StartTime2.Text));   // texte yazı yazdırıyorum.
        }

 
        private void StartTime3_TextChanged(object sender, EventArgs e)
        {
            ModClient.WriteSingleRegister(7, int.Parse(StartTime3.Text));   // texte yazı yazdırıyorum.
        }

        private void StartTime4_TextChanged(object sender, EventArgs e)
        {
            ModClient.WriteSingleRegister(8, int.Parse(StartTime4.Text));   // texte yazı yazdırıyorum.
        }

        private void Esyaslanma_TextChanged(object sender, EventArgs e)
        {
            ModClient.WriteSingleRegister(11, int.Parse(Esyaslanma.Text));   // texte yazı yazdırıyorum.
        }

        private void kaydetButon_Click(object sender, EventArgs e)
        {
            ModClient.WriteSingleCoil(41, true);
        }

        private void iptalButon_Click(object sender, EventArgs e)
        {
            ModClient.WriteSingleCoil(42, true);
        }

        private void AyarlarSayfasınaDön_Click(object sender, EventArgs e)  //// Comp. Ayarlar sayfasından normal ayarlar sayfasına geçme
        {
            compPanel.Visible = false;
            ayarlarSayfası.Visible = true;
        }

        #endregion


        #region BASINÇ AYARLARI

        private void pressureSettingsButon_Click(object sender, EventArgs e)  ///// Ayarlar sayfasından Pressure Ayarlar sayfasına geçme
        {
            pressurePanel.Visible = true;
        }

        private void StartBasıncı_TextChanged(object sender, EventArgs e)
        {
            double StartBasıncıvalue;
            if (double.TryParse(StartBasıncı.Text, out StartBasıncıvalue))
            {
                ModClient.WriteSingleRegister(4, (int)(StartBasıncıvalue * 10)); // 10 ile çarparak tam sayıya dönüştürüyoruz
            }
            else
            {
                // Hata durumu, isteğe bağlı olarak ele alınabilir
                MessageBox.Show("Geçerli bir sayı giriniz.");
            }
        }

        private void StopBasıncı_TextChanged(object sender, EventArgs e)
        {
            double StopBasıncıvalue;
            if (double.TryParse(StopBasıncı.Text, out StopBasıncıvalue))
            {
                ModClient.WriteSingleRegister(5, (int)(StopBasıncıvalue * 10)); // 10 ile çarparak tam sayıya dönüştürüyoruz
            }
            else
            {
                // Hata durumu, isteğe bağlı olarak ele alınabilir
                MessageBox.Show("Geçerli bir sayı giriniz.");
            }
           // ModClient.WriteSingleRegister(5, int.Parse(StopBasıncı.Text));   // texte yazı yazdırıyorum.
        }

        private void StartTimeAlarm_TextChanged(object sender, EventArgs e)
        {
            ModClient.WriteSingleRegister(9, int.Parse(StartTimeAlarm.Text));   // texte yazı yazdırıyorum.
        }

        private void PresureFault_TextChanged(object sender, EventArgs e)
        {
            double PressureFaultvalue;
            if (double.TryParse(PresureFault.Text, out PressureFaultvalue))
            {
                ModClient.WriteSingleRegister(41, (int)(PressureFaultvalue * 10)); // 10 ile çarparak tam sayıya dönüştürüyoruz
            }
            else
            {
                // Hata durumu, isteğe bağlı olarak ele alınabilir
                MessageBox.Show("Geçerli bir sayı giriniz.");
            }
        }
        private void kaydetButon2_Click(object sender, EventArgs e)
        {
            ModClient.WriteSingleCoil(41, true);
        }

        private void iptalButon2_Click(object sender, EventArgs e)
        {
            ModClient.WriteSingleCoil(42, true);
        }

        private void AyarlarSayfasınaDön2_Click(object sender, EventArgs e)
        {
            pressurePanel.Visible = false;
            ayarlarSayfası.Visible = true;
        }

        #endregion


        #region SİSTEM AYARLARI


        private void sistemSettingsButon_Click(object sender, EventArgs e)    ///// Ayarlar sayfasından Sistem Ayarlar sayfasına geçme
        {
            sistemPanel.Visible = true;
        }

        private void modbusAddress_TextChanged(object sender, EventArgs e)
        {
            ModClient.WriteSingleRegister(0, int.Parse(modbusAddress.Text));   // texte yazı yazdırıyorum.
        }

        private void modbusBaudrate_TextChanged(object sender, EventArgs e)
        {
            ModClient.WriteSingleRegister(1, int.Parse(modbusBaudrate.Text));   // texte yazı yazdırıyorum.
        }

        private void fabrikaResetButon1_Click(object sender, EventArgs e)
        {
            ModClient.WriteSingleCoil(109, true);
            fabrikaResetPictureBox1.Visible = true;
        }
        private void fabrikaResetButon2_Click(object sender, EventArgs e)
        {
            ModClient.WriteSingleCoil(110, true);
            fabrikaResetPictureBox2.Visible = true;
        }

        private void fabrikaResetButon3_Click(object sender, EventArgs e)
        {
            ModClient.WriteSingleCoil(111, true);
            fabrikaResetPictureBox3.Visible = true;
        }

        private void fabrikaResetButon4_Click(object sender, EventArgs e)
        {
            ModClient.WriteSingleCoil(112, true);
            fabrikaResetPictureBox1.Visible = false;
            fabrikaResetPictureBox2.Visible = false;
            fabrikaResetPictureBox3.Visible = false;
        }

        private void kaydetButon3_Click(object sender, EventArgs e)
        {
            ModClient.WriteSingleCoil(41, true);
        }

        private void iptalButon3_Click(object sender, EventArgs e)
        {
            ModClient.WriteSingleCoil(42, true);
        }
        private void ayarlarSayfasınaDön3_Click(object sender, EventArgs e)
        {
            sistemPanel.Visible = false;
            ayarlarSayfası.Visible = true;
        }

        #endregion


        #region KURUTUCU AYARLARI 

        private void drySettingsButon_Click(object sender, EventArgs e)   ///// Ayarlar sayfasından Kurutucu Ayarlar sayfasına geçme
        {
            kurutucuPanel.Visible = true;
        }

        private void DutyDryTime_TextChanged(object sender, EventArgs e)
        {
            ModClient.WriteSingleRegister(35, int.Parse(DutyDryTime.Text));   // texte yazı yazdırıyorum.
        }

        private void Dryerfault_TextChanged(object sender, EventArgs e)
        {
            ModClient.WriteSingleRegister(46, int.Parse(Dryerfault.Text));   // texte yazı yazdırıyorum.
        }

        private void TankPerıod_TextChanged(object sender, EventArgs e)
        {
            ModClient.WriteSingleRegister(43, int.Parse(TankPerıod.Text));   // texte yazı yazdırıyorum.
        }

        private void TankTahlıye_TextChanged(object sender, EventArgs e)
        {
            ModClient.WriteSingleRegister(44, int.Parse(TankTahlıye.Text));   // texte yazı yazdırıyorum.
        }

        private void nemliButon1_Click(object sender, EventArgs e)
        {
            ModClient.WriteSingleCoil(59, true);  // kurutucular sayfasında ki nemli butonu
        }

        private void nemliButon2_Click(object sender, EventArgs e)
        {
            ModClient.WriteSingleCoil(60, true);
        }

        private void nemliButon3_Click(object sender, EventArgs e)
        {
            ModClient.WriteSingleCoil(61, true);
        }

        private void nemliButon4_Click(object sender, EventArgs e)
        {
            ModClient.WriteSingleCoil(62, true);
        }

        private void kaydetButon4_Click(object sender, EventArgs e)
        {
            ModClient.WriteSingleCoil(41, true);
        }

        private void iptalButon4_Click(object sender, EventArgs e)
        {
            ModClient.WriteSingleCoil(42, true);
        }

        private void AyarlarSayfasınaDön4_Click(object sender, EventArgs e)
        {
            kurutucuPanel.Visible = false;
            ayarlarSayfası.Visible = true;
        }




        #endregion


        #region HASTANE ADI

        private void hospitalNameButon_Click(object sender, EventArgs e)  ///// Ayarlar sayfasından hastane adı sayfasına geçme
        {
            hospitalNamePanel.Visible = true;
        }
        private void hospitalName_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.KaydedilenDeger = hospitalName.Text;
            Properties.Settings.Default.Save();
            hastaneAdıLabel.Text = hospitalName.Text;
        }
        private void AyarlarSayfasınaDön5_Click(object sender, EventArgs e)
        {
            hospitalNamePanel.Visible = false;
            ayarlarSayfası.Visible = true;
        }

        #endregion


        #region REPORTS SAYFASI


        private void reportsButon_Click(object sender, EventArgs e)  ///// Ayarlar sayfasından raporlar sayfasına geçme
        {
            reportsPanel.Visible = true;
        }

        private void exportToPDFButton_Click(object sender, EventArgs e)
        {

        }


        private void AyarlarSayfasınaDön6_Click(object sender, EventArgs e)
        {
            reportsPanel.Visible = false;
            ayarlarSayfası.Visible = true;
        }

        #endregion


        #region GİZLİ SAYFA

        private void gizliSayfaButon_Click(object sender, EventArgs e)
        {
            string enteredPassword = gizliText.Text;

            string correctPassword = "11";
            //  System.Diagnostics.Process.Start("osk.exe");//ekran klavyesini başlatır.
            if (!string.IsNullOrWhiteSpace(enteredPassword)) // Şifre boş değilse kontrol et
            {
                if (enteredPassword == correctPassword)
                {
                    gizliPanel.Visible = true;
                }
                else
                {
                    // Şifre yanlışsa bir hata mesajı gösterebilirsiniz.
                    MessageBox.Show("Hatalı şifre! Lütfen doğru şifreyi girin.");
                    gizliText.Text = "";

                }
            }
        }

        private void reportsSayfasınaDön_Click(object sender, EventArgs e)
        {
            gizliPanel.Visible = false;
            reportsPanel.Visible = true;
            gizliText.Text = "";
        }

        private void saatPompa1Gizli_TextChanged(object sender, EventArgs e)
        {
            ModClient.WriteSingleRegister(13, int.Parse(saatPompa1Gizli.Text));
        }

        private void saatPompa2Gizli_TextChanged(object sender, EventArgs e)
        {
            ModClient.WriteSingleRegister(15, int.Parse(saatPompa2Gizli.Text));
        }

        private void saatPompa3Gizli_TextChanged(object sender, EventArgs e)
        {
            ModClient.WriteSingleRegister(17, int.Parse(saatPompa3Gizli.Text));
        }

        private void saatPompa4Gizli_TextChanged(object sender, EventArgs e)
        {
            ModClient.WriteSingleRegister(19, int.Parse(saatPompa4Gizli.Text));
        }
        private void saveButon_Click(object sender, EventArgs e)
        {
            ModClient.WriteSingleCoil(41, true);
        }

        #endregion


        #region DİL DEĞİŞTİRME

        public void ENlanguageButon_Click(object sender, EventArgs e)
        {
            language = 0;
            this.LinePressure.Text = "LINE PRESSURE";
            this.TankPressure.Text = "TANK PRESSURE";
            this.dewpointLabel.Text = "DEWPOINT";
            this.EnterButon.Text = "ENTER";
            this.ayarlarBaslıkLabel.Text = "SETTİNGS";
            this.resportsLabel.Text = "Reports";
            this.hastaneLabel.Text = "Hospital Name";
            this.dryLabel.Text = "Dryer Settings";
            this.sistemLabel.Text = "System Settings";
            this.pressureLabel.Text = "Pressure Settings";
            this.compLabel.Text = "Compressor Settings";
            this.compAyarlarBaslıkLabel.Text = "COMPRESSOR SETTINGS";
            this.gecisSüresiLabel.Text = "Transition Times";
            this.compTypeLabel.Text = "Compressor Type";
            this.comp2SecLabel.Text = "Compressor 2 (second)";
            this.comp3SecLabel.Text = "Compressor 3 (second)";
            this.comp4SecLabel.Text = "Compressor 4 (second)";
            this.esYaslanmaLabel.Text = "Synchron Aging (day)";
            this.kaydetButon.Text = "SAVE";
            this.iptalButon.Text = "CANCEL";
            this.pressureAyarlarBaslıkLabel.Text = "PRESSURE SETTINGS";
            this.startLabel.Text = "START";
            this.stopLabel.Text = "STOP";
            this.alarmSuresiLabel.Text = "ALARM TIME (minute)";
            this.hatBasıncAlarmLabel.Text = "LINE PRESSURE ALARM";
            this.kaydetButon2.Text = "SAVE";
            this.iptalButon2.Text = "CANCEL";
            this.fabrikaAyarlarınaDönLabel.Text = " Return to Factory Settings";
            this.modbusBaudrateLabel.Text = "Modbus Baudrate";
            this.modbusAddressLabel.Text = "Modbus Address";
            this.sistemAyarlarıBaslıkLabel.Text = "SYSTEM SETTINGS";
            this.kaydetButon3.Text = "SAVE";
            this.iptalButon3.Text = "CANCEL";
            hours = "hours";
            this.kurutucuAyarlarıBaslıkLabel.Text = "DRYER SETTINGS";
            this.StartUpLabel.Text = "START UP";
            this.transitionTimeLabel.Text = "Transition Time (Minute)";
            this.faultReportTimeLabel.Text = "Fault Report Time (second)";
            this.tankFlushPeriodLabel.Text = "Tank Flush Period (Minute)";
            this.tankFlushTimeLabel.Text = "Tank Flush Time  (second)";
            this.kaydetButon4.Text = "SAVE";
            this.iptalButon4.Text = "CANCEL";
            this.hospitalNameLabel.Text = "Please enter the hospital name.";
            this.hastaneAdıBaslıkLabel.Text = "HOSPITAL NAME";
            this.reportsBaslıkLabel.Text = "REPORTS";

        }

        public void DElanguageButon_Click(object sender, EventArgs e)
        {
            language = 1;
            this.LinePressure.Text = "LEITUNGSDRUCK";
            this.TankPressure.Text = "BEHÂLTERDRUCK";
            this.dewpointLabel.Text = "DRUCKTAUPUNKT";
            this.EnterButon.Text = "EINGEBEN";
            this.ayarlarBaslıkLabel.Text = "EINSTELLUNGEN";
            this.resportsLabel.Text = "Berichte";
            this.hastaneLabel.Text = "Name des Krankenhauses";
            this.dryLabel.Text = "Trockner Einstellungen";
            this.sistemLabel.Text = "Systemeinstellungen";
            this.pressureLabel.Text = "Druckeinstellungen";
            this.compLabel.Text = "Kompressor Einstellungen";
            this.compAyarlarBaslıkLabel.Text = "KOMPRESSOR EINSTELLUNGEN";
            this.gecisSüresiLabel.Text = "ÜBERGANGSZEIT";
            this.compTypeLabel.Text = "Kompressorenanzahl";
            this.comp2SecLabel.Text = "Kompressor 2 (sek.)";
            this.comp3SecLabel.Text = "Kompressor 3 (sek.)";
            this.comp4SecLabel.Text = "Kompressor 4 (sek.)";
            this.esYaslanmaLabel.Text = "Umshaltintervall (tag)";
            this.kaydetButon.Text = "SPEICHERN";
            this.iptalButon.Text = "ZURÜCK";
            this.pressureAyarlarBaslıkLabel.Text = "DRUCKEINSTELLUNGEN";
            this.startLabel.Text = "START";
            this.stopLabel.Text = "STOP";
            this.alarmSuresiLabel.Text = "ALARMZEIT (Min.)";
            this.hatBasıncAlarmLabel.Text = "LEINTUGSDRUCKALARM";
            this.kaydetButon2.Text = "SPEICHERN";
            this.iptalButon2.Text = "ZURÜCK";
            this.fabrikaAyarlarınaDönLabel.Text = "WERKSEINSTELLUNGEN";
            this.modbusBaudrateLabel.Text = "Modbus Baudrate";
            this.modbusAddressLabel.Text = "Modbus Addresse";
            this.sistemAyarlarıBaslıkLabel.Text = "SYSTEMEINSTELLUNGEN";
            this.kaydetButon3.Text = "SPEICHERN";
            this.iptalButon3.Text = "ZURÜCK";
            hours = "stunde";
            this.kurutucuAyarlarıBaslıkLabel.Text = "TROCKNEREINSTELLUNGEN";
            this.StartUpLabel.Text = "STARTEN";
            this.transitionTimeLabel.Text = "Überganszeit (Min.)";
            this.faultReportTimeLabel.Text = "Störungsmeldezeit (sek.)";
            this.tankFlushPeriodLabel.Text = "Tankentleerungszeit (Min.)";
            this.tankFlushTimeLabel.Text = "Tankentleerungszeit (sek.)";
            this.kaydetButon4.Text = "SPEICHERN";
            this.iptalButon4.Text = "ZURÜCK";
            this.hospitalNameLabel.Text = "Bitte geben Sie den Namen des Krankenhauses ein";
            this.hastaneAdıBaslıkLabel.Text = "Name Des Krankenhauses";
            this.reportsBaslıkLabel.Text = "BERICHTE";

        }

        public void TRlanguageButon_Click(object sender, EventArgs e)
        {
            language = 2;
            this.LinePressure.Text = "HAT BASINCI";
            this.TankPressure.Text = "TANK BASINCI";
            this.dewpointLabel.Text = "DP NOKTASI";
            this.EnterButon.Text = "GİRİŞ";
            this.ayarlarBaslıkLabel.Text = "AYARLAR";
            this.resportsLabel.Text = "Raporlar";
            this.hastaneLabel.Text = "Hastane Adı";
            this.dryLabel.Text = "Kurutucu Ayarları";
            this.sistemLabel.Text = "Sistem Ayarları";
            this.pressureLabel.Text = "Basınç Ayarları";
            this.compLabel.Text = "Kompresör Ayarları";
            this.compAyarlarBaslıkLabel.Text = "KOMPRESÖR AYARLARI";
            this.gecisSüresiLabel.Text = "Geçiş Süresi";
            this.compTypeLabel.Text = "Kompresör Tipi";
            this.comp2SecLabel.Text = "Kompresör 2 (saniye)";
            this.comp3SecLabel.Text = "Kompresör 3 (saniye)";
            this.comp4SecLabel.Text = "Kompresör 4 (saniye)";
            this.esYaslanmaLabel.Text = "Eş Yaşlanma (gün)";
            this.kaydetButon.Text = "KAYDET";
            this.iptalButon.Text = "İPTAL";
            this.pressureAyarlarBaslıkLabel.Text = "BASINÇ AYARLARI";
            this.startLabel.Text = "BAŞLA";
            this.stopLabel.Text = "DURDUR";
            this.alarmSuresiLabel.Text = "ALARM SÜRESİ (dakika)";
            this.hatBasıncAlarmLabel.Text = "LINE PRESSURE ALARM";
            this.kaydetButon2.Text = "KAYDET";
            this.iptalButon2.Text = "İPTAL";
            this.fabrikaAyarlarınaDönLabel.Text = "Fabrika Ayarlarına Dön";
            this.modbusBaudrateLabel.Text = "Modbus Baudrate";
            this.modbusAddressLabel.Text = "Modbus Adresi";
            this.sistemAyarlarıBaslıkLabel.Text = "SİSTEM AYARLARI";
            this.kaydetButon3.Text = "KAYDET";
            this.iptalButon3.Text = "İPTAL";
            hours = "saat";
            this.kurutucuAyarlarıBaslıkLabel.Text = "KURUTUCU AYARLARI";
            this.StartUpLabel.Text = "BAŞLANGIÇ";
            this.transitionTimeLabel.Text = "Geçiş Süresi (dakika)";
            this.faultReportTimeLabel.Text = "Hata Rapor Süresi (saniye)";
            this.tankFlushPeriodLabel.Text = "Tank Tahliye Periyodu (dakika)";
            this.tankFlushTimeLabel.Text = "Tank Tahliye Süresi (saniye)";
            this.kaydetButon4.Text = "KAYDET";
            this.iptalButon4.Text = "İPTAL";
            this.hospitalNameLabel.Text = "Lütfen hastane adını giriniz.";
            this.hastaneAdıBaslıkLabel.Text = "HASTANE ADI";
            this.reportsBaslıkLabel.Text = "RAPORLAR";

        }

        #endregion

    }

}

