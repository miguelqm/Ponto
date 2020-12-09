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
using System.Media;
using System.Net.Mail;
using System.Threading;
using System.Net;
using System.Runtime.InteropServices;
using System.Diagnostics;
using AvencaLib;

namespace Ponto
{
    public partial class frmMain : Form
    {
        static List<string> lstWarnings = new List<string>();

        static int CLICK_SECONDS = 14;
        static int RED = 1;
        static int GREEN = 2;
        static int ID_LENGHT = 4;
        static int TIMER_MINUTES = 10;
        int warningTick = 0;

#if DEBUG
        bool canClose = true;
#else
        bool canClose = false;
#endif

        int isBarcode = 0;
        string userBarcode = "";

        static string emailText;
        static bool backupDone = false;
        static string backupTime = "17:30";

        dbPontoDataSet dsPonto = new dbPontoDataSet();
        dbPontoDataSetTableAdapters.PontoTableAdapter taPonto = new dbPontoDataSetTableAdapters.PontoTableAdapter();
        dbPontoDataSetTableAdapters.FuncionarioTableAdapter taFuncionario = new dbPontoDataSetTableAdapters.FuncionarioTableAdapter();
        dbPontoDataSet.FuncionarioDataTable dtFuncionario = new dbPontoDataSet.FuncionarioDataTable();
        dbPontoDataSet.PontoDataTable dtPonto = new dbPontoDataSet.PontoDataTable();
        //dbPontoDataSet.PontoHojeDataTable dtPontoHoje = new dbPontoDataSet.PontoHojeDataTable();
        //dbPontoDataSetTableAdapters.PontoHojeTableAdapter taPontoHoje = new dbPontoDataSetTableAdapters.PontoHojeTableAdapter();

        private const int WM_ACTIVATEAPP = 0x001C;
        private const int WM_NCLBUTTONDBLCLK = 0x00A3;
        private const int WM_SYSCOMMAND = 0x0112;
        private const int SC_MOVE = 0xF010;
        private const int WM_QUERYENDSESSION = 0x11;

        FormWindowState curWindowState;

        int idleTimeout = 0;
        //IntPtr activeHandle;
        private const int ALT = 0xA4;
        private const int EXTENDEDKEY = 0x1;
        private const int KEYUP = 0x2;
        private const uint Restore = 9;

        //This is a replacement for Cursor.Position in WinForms
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool GetCursorPos(out Point p);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;

        private void ActivateWindow(FormWindowState state)
        {
            this.WindowState = state;// (curWindowState == FormWindowState.Minimized) ? state : curWindowState;
        }

        protected override void WndProc(ref Message m) 
        {
            switch (m.Msg)
            {
                case WM_SYSCOMMAND: 
                    int command = m.WParam.ToInt32() & 0xfff0;
                    if (command == SC_MOVE)
                        return;
                    break;
                case WM_QUERYENDSESSION:
                    canClose = true;
                    break;
                case WM_NCLBUTTONDBLCLK:
                    return;
            }
            base.WndProc(ref m);
        }
        
        public frmMain()
        {
            InitializeComponent();
            lblTime.Text = DateTime.Now.ToLongTimeString();
            lblWarning.Text = "";
            lblWarning2.Text = "";
            lblOk.Text = "";
            
            txtInput.BackColor = Color.Black;
#if DEBUG
            lblDebug.Visible = true;
            this.TopMost = false;
#else
            lblDebug.Visible = false;
            this.TopMost = true;
#endif

            posControls();
        }
        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int id;

                txtInput.Text = txtInput.Text.TrimStart('#');

                if (txtInput.Text.Length >= ID_LENGHT)
                {
                    if (Int32.TryParse(txtInput.Text, out id))
                    {
                        txtInput.Clear();

                        switch(id)
                        {
                            case 9876:
                                addWarning("Bye.");
                                canClose = true;
                                break;
                            case 3210:
                                emailText = DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToLongTimeString() + ": This is a test.";
                                addWarning("Sending e-mail");
                                Thread emailThread = new Thread(new ThreadStart(sendEmail));
                                emailThread.Start();
                                break;
                            case 6543:
                                backup();
                                break;
                            default:
                                tmrInput.Enabled = false;
                                insertPonto(id);
                                break;
                        }
                    }
                    else
                    {
                        txtInput.Clear();
                        addWarning("Entrada inválida.\n Tente de novo.");
                    }
                }
            }
            catch(Exception ex)
            {
                txtInput.Clear();
                tmrInput.Enabled = false;
                addWarning("ERRO: " + ex.Message);
            }
        }

        private object getLastPass(int id)
        {
            return taPonto.GetLastPass(DateTime.Today, id, DateTime.Today, id);
        }

        private bool insertPonto(int id)
        {
            string entradaSaida;

            if (taFuncionario.GetNomeFuncionario(id) != null)
            {
                if (isEarly(id))
                    return false;

                object horaEntrada = taPonto.GetHoraEntradaByIdFuncionario(id, DateTime.Today);
                object horaSaida = taPonto.GetHoraSaidaByIdFuncionario(id, DateTime.Today);
                string nome = dtFuncionario.Rows.Find(id)["Nome"].ToString();

                if (horaSaida != null && horaEntrada != null)
                {
                    addWarning(nome + ", você já passou seu ponto hoje duas vezes.\n Pode ir pra casa, jovem...");
                    flash(RED);
                    return false;
                }
                if(horaEntrada != null)
                {
                    if (DateTime.Now.Subtract((DateTime)getLastPass(id)) < TimeSpan.Parse("00:00:30"))
                    {
                        addWarning(nome + ", você passou seu cartão há menos de 30 segundos.");
                        flash(RED);
                        return false;
                    }
                    taPonto.UpdatePontoHoraSaida(DateTime.Now, id, DateTime.Today, (DateTime)horaEntrada);
                    addWarning("SAÍDA\nObrigado, " + nome + ". Bom descanso!");
                    entradaSaida = "SAÍDA";
                    flash(GREEN);
                }
                else
                {
                    taPonto.Insert(id, DateTime.Today, DateTime.Now);
                    addWarning("ENTRADA\nObrigado, " + nome + ". Bom trabalho.");
                    entradaSaida = "ENTRADA";
                    flash(GREEN);
                }
                
                taPonto.Update(dsPonto.Ponto);

                string text = DateTime.Now.ToShortDateString() + ": " + nome + "\n" + entradaSaida + ": " + DateTime.Now.ToShortTimeString();

                if (Properties.Settings.Default.CanPrint)
                    AvencaPrinter.PrintString(Properties.Settings.Default.PrinterName, text, 16, true);
                
                emailText = DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToLongTimeString() + " - " + nome + " - " + entradaSaida;

                Thread emailThread = new Thread(new ThreadStart(sendEmail));
                emailThread.Start();

                posControls();

                return true;
            }
            else
            {
                addWarning("Funcionário não encontrado.\n Tente de novo.");
                return false;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !canClose;
        }

        private void posControls()
        {
            this.Width = 1600;
            this.Height = 800;
            lblTime.Left = (this.Width / 2) - (lblTime.Width / 2);
            panel1.Left = (this.Width / 2) - (panel1.Width / 2);
            lblWarning.Left = (this.Width / 2) - (lblWarning.Width / 2);
            lblWarning2.Left = (this.Width / 2) - (lblWarning2.Width / 2);

            panel1.Top = (this.Height / 2) - (panel1.Height / 2);
            lblTime.Top = panel1.Top - 100;
            lblWarning.Top = panel1.Bottom + 20;
            lblWarning2.Top = lblWarning.Bottom + 30;

            txtInput.Left = this.Width;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            posControls();
        }

        private void flash(int color)
        {
            this.BackColor = (color == GREEN) ? System.Drawing.Color.DarkGreen : System.Drawing.Color.Red;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //e.SuppressKeyPress = false;        
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            tmrInput.Enabled = true;
            tmrInput.Stop();
            tmrInput.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dbPontoDataSet.Funcionario' table. You can move, or remove it, as needed.
            this.funcionarioTableAdapter.Fill(this.dbPontoDataSet.Funcionario);
            taFuncionario.Fill(dtFuncionario);
            taPonto.Fill(dtPonto);
        }

        private bool isEarly(int id)
        {
            TimeSpan horarioChegada = ((DateTime)dtFuncionario.Rows.Find(id)["HorarioEntrada"]).TimeOfDay;
            TimeSpan now = DateTime.Now.TimeOfDay; // TimeSpan.Parse("07:44");
            int i = Convert.ToInt32(horarioChegada.Subtract(now).TotalMinutes);
            if(i > 10)
            {
                addWarning(dtFuncionario.Rows.Find(id)["Nome"] + ", ainda faltam " + i.ToString() + " minutos para o seu horário de entrada.\n Tente de novo em " + (i-10).ToString() + " minutos.");
                flash(RED);
                return true;
            }
            return false;
        }

        private void addWarning(string s)
        {
            //if ((lstWarnings.Count == 0) && (lblWarning.Text == ""))
            //{
            //    curWindowState = this.WindowState;
            //    ActivateWindow(FormWindowState.Normal);
            //}
            lstWarnings.Add(s);
            SaveToLog(s);
        }

        private void SaveToLog(string text)
        {
            try
            {
                string filepath = "log_ponto.txt";

                using (StreamWriter sw = File.AppendText(@filepath))
                {
                    sw.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " ====== " + text.Replace("\n", "\n\t\t"));
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro de log", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tmrClock_Tick(object sender, EventArgs e)
        {
            //lblDebug.Text = idleTimeout.ToString();
            if (idleTimeout++ >= CLICK_SECONDS * 2)
            {
                idleTimeout = 0;
                ActivateWindow(FormWindowState.Minimized);
                LeftMouseClick(10, 30);
                //lblDebug.Text = "CLICK!";
            }
            #if DEBUG 
                else lblDebug.Text = idleTimeout.ToString();
            #endif
            
            if((lstWarnings.Count > 0) && (warningTick == 0))
            {
                lblWarning.Text = lstWarnings[0].Split('\n')[0];
                lblWarning2.Text = lstWarnings[0].Split('\n').Count() > 1 ? lstWarnings[0].Split('\n')[1] : "";
                lstWarnings.RemoveAt(0);
                warningTick = 1;
                idleTimeout = 0;
                ActivateWindow(FormWindowState.Normal);

                //lblWarning.Text = lstWarnings[0];//.Split('\n')[0];
                //lblWarning2.Text = lstWarnings[0].Split('\n').Count() > 1 ? lstWarnings[0].Split('\n')[1] : "";
                //lstWarnings.RemoveAt(0);
                //warningTick = 1;
            }
            if (warningTick > 0)
            {
                #if DEBUG 
                    lblDebug.Text = warningTick.ToString(); 
                #endif

                    if (warningTick > TIMER_MINUTES)
                    {
                        lblWarning.Text = "";
                        lblWarning2.Text = "";
                        userBarcode = "";
                        isBarcode = 0;
                        if (lstWarnings.Count == 0)
                        {
                            this.BackColor = System.Drawing.Color.FromArgb(0, 0, 0);
                            ActivateWindow(FormWindowState.Minimized);
                        }
                        warningTick = 0;

#if DEBUG
                        lblDebug.Text = warningTick.ToString();
#else                        
                        canClose = false;
#endif
                    }
                    else
                    {
                        warningTick++;
                    }
            }
            lblTime.Text = DateTime.Now.ToLongTimeString();

            if (DateTime.Now.TimeOfDay.CompareTo(TimeSpan.Parse(backupTime)) > 0)
            {
                if (!backupDone)
                    backup();
            }
            else backupDone = false;
        }

        private void tmrInput_Tick(object sender, EventArgs e)
        {
            tmrInput.Enabled = false;
            txtInput.Text = "";
            //addWarning("Tente novamente...");
        }

        private void lblWarning_TextChanged(object sender, EventArgs e)
        {
            //lblWarning.Left = (this.Width / 2) - lblWarning.Width / 2;// ((System.Windows.Forms.Label)(sender)).PreferredWidth / 2;
            //lblWarning.BackColor = this.BackColor;
        }

        private void lblWarning2_TextChanged(object sender, EventArgs e)
        {
           // lblWarning2.Left = (this.Size.Width / 2) - (lblWarning2.Width / 2);// ((System.Windows.Forms.Label)(sender)).PreferredWidth / 2;
            //lblWarning2.BackColor = this.BackColor;
        }

        private void sendEmail()
        {
            try
            {
                // Command line argument must the the SMTP host.
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 20000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = true;
                client.Credentials = new System.Net.NetworkCredential("restauranteavenca@gmail.com", "ocipjjgsuskbvpwx");

                string subject = (emailText.IndexOf("Teste") > 0) ? "Teste de Ponto" : "Folha de Ponto";

                MailMessage mm = new MailMessage("restauranteavenca@gmail.com", "gerente@restauranteavenca.com.br", subject, emailText);
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                client.Send(mm);

                client.Dispose();
#if DEBUG
                this.addWarning("E-mail sent");
#endif
            }
            catch (Exception ex)
            {
                this.addWarning(ex.Message);
            }
        }

        void backup()
        {
            backupDone = true;

            Thread backupThread = new Thread(new ThreadStart(ftpUpload));
            backupThread.Start();
        }

        private void ftpUpload()
        {
            try
            {
                // Get the object used to communicate with the server.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://avencarestaurante@ftp.avencarestaurante.hospedagemdesites.ws/Backup/dbPonto.mdb");
                request.Method = WebRequestMethods.Ftp.UploadFile;

                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential("avencarestaurante", "ftpAvenca15");
                request.UseBinary = true;
                request.KeepAlive = true;
                
                byte[] data = File.ReadAllBytes("dbPonto.mdb");
                request.ContentLength = data.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                addWarning("Upload File Complete, status: " + response.StatusDescription);

                response.Close();
            }
            catch(Exception ex)
            {
                addWarning("Upload ERROR! " + ex.Message);
                backupDone = false;
            }
        }

        private void globalEventProvider1_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '#')
            {
                isBarcode = 1;
                e.Handled = true;
            }
            if ((isBarcode > 0) && (isBarcode++ < 6))
            {
                userBarcode += e.KeyChar.ToString();
                e.Handled = true;
            }
            if (isBarcode >= 6)
            {
                txtInput.Text = userBarcode.TrimStart('#');
                userBarcode = "";
                isBarcode = 0; 
                e.Handled = true;
            }
        }

        public static void LeftMouseClick(int xpos, int ypos)
        {
            Point p;
            GetCursorPos(out p);
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
            SetCursorPos(p.X, p.Y);
        }

        //[DllImport("user32.dll")]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //private static extern bool IsIconic(IntPtr hWnd);

        //[DllImport("user32.dll")]
        //static extern bool SetForegroundWindow(IntPtr hWnd);

        //[DllImport("user32.dll")]
        //public static extern IntPtr GetForegroundWindow();

        //[DllImport("user32.dll", SetLastError = true)]
        //public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        //[DllImport("user32.dll")]
        //private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        //[DllImport("user32.dll")]
        //private static extern int ShowWindow(IntPtr hWnd, uint Msg);

        //[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name="FullTrust")]

        //private void ActivateWindow(IntPtr mainWindowHandle)
        //{
        //    //check if already has focus
        //    if (mainWindowHandle == GetForegroundWindow()) return;

        //    //check if window is minimized
        //    if (IsIconic(mainWindowHandle))
        //    {
        //        ShowWindow(mainWindowHandle, Restore);
        //    }

        //    // Simulate a key press
        //    keybd_event((byte)ALT, 0x45, EXTENDEDKEY | 0, 0);

        //    //SetForegroundWindow(mainWindowHandle);

        //    // Simulate a key release
        //    keybd_event((byte)ALT, 0x45, EXTENDEDKEY | KEYUP, 0);

        //    SetForegroundWindow(mainWindowHandle);
        //}
    }
}
